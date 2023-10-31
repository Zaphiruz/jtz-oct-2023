using Godot;
using System;
using Godot.Collections;
using System.Text;

public partial class AuthManager : HttpRequest, IGlobalInterface<AuthManager>
{
	public static string NodePath = "/root/AuthManager";
	public static AuthManager GetInstance(Node context) => context.GetNode<AuthManager>(NodePath);
	public static AuthManager GetInstance(Node context, string Name)
	{
		SceneMapper.GetInstance(context, Name);
		return context.GetNode<AuthManager>(NodePath);
	}

	private static string HOST = "http://localhost:3000/aws";

	private SceneMapper sceneMapper;

	private string username;
	private string session;
	private string challengeName;
	private string accessToken;

	public override void _Ready()
	{
		base._Ready();

		sceneMapper = SceneMapper.GetInstance(this);
	}

	public string GetAccessToken()
	{
		return accessToken;
	}

	public void SignIn(string username, string password)
	{
		this.username = username;

		string url = $"{HOST}/authenticateUser";
		string json = $"{{\"username\": \"{username}\", \"password\": \"{password}\"}}";

		GD.Print("Sending", url);

		string[] headers = new string[] { "Content-Type: application/json" };;
		RequestCompleted += SignInResponce;
		Request(url, headers, HttpClient.Method.Post, json);
	}

	private void SignInResponce(long result, long responseCode, string[] headers, byte[] body)
	{
		RequestCompleted -= SignInResponce;

		Dictionary json = Json.ParseString(Encoding.UTF8.GetString(body)).AsGodotDictionary();

		if(parseErrors(json))
		{
			sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.SignInError(json["name"].As<string>());
			return;
		}

		this.session = json["Session"].As<string>();

		if (!parseChallenges(json))
		{
			parseAuthenticationResult(json);
		}
	}


	public void AnswerChallenge(string authenticatorToken)
	{
		if (session == null  || session == string.Empty)
		{
			return;
		}

		string url = $"{HOST}/challengeUser";
		string json = $"{{\"challengeName\": \"{this.challengeName}\", \"challengeResponses\": {{ \"username\": \"{this.username}\", \"authenticator_code\": \"{authenticatorToken}\", \"session\": \"{this.session}\" }} }}";

		GD.Print("Sending", url);

		string[] headers = new string[] { "Content-Type: application/json" };
		RequestCompleted += AnswerChallengeResponse;
		Request(url, headers, HttpClient.Method.Post, json);
	}

	private void AnswerChallengeResponse(long result, long responseCode, string[] headers, byte[] body)
	{
		RequestCompleted -= AnswerChallengeResponse;

		Dictionary json = Json.ParseString(Encoding.UTF8.GetString(body)).AsGodotDictionary();

		if (parseErrors(json))
		{
			sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.MfaError(json["name"].As<string>());
			return;
		}

		if (!parseChallenges(json))
		{
			parseAuthenticationResult(json);
		}
	}

	public bool parseErrors(Dictionary json)
	{
		return json.ContainsKey("$fault");
	}

	public bool parseChallenges(Dictionary json)
	{
		if (!json.ContainsKey("ChallengeName"))
		{
			return false;
		}

		string ChallengeName = json["ChallengeName"].As<string>();
		Dictionary ChallengeParameters = json["ChallengeParameters"].As<Dictionary>();

		this.challengeName = ChallengeName;
		switch (ChallengeName)
		{
			case "SOFTWARE_TOKEN_MFA":
				sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.MfaChallenge();
				return true;

			default:
				return false;
		}
	}

	public void parseAuthenticationResult(Dictionary json)
	{
		Dictionary AuthenticationResult = json["AuthenticationResult"].As<Dictionary>();

		this.accessToken = AuthenticationResult["AccessToken"].As<string>();
		int expiresIn = AuthenticationResult["ExpiresIn"].As<int>();
		string IdToken = AuthenticationResult["IdToken"].As<string>();
		string RefreshToken = AuthenticationResult["RefreshToken"].As<string>();
		string TokenType = AuthenticationResult["TokenType"].As<string>();

		sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.FullyAuthenticated();
	}
}
