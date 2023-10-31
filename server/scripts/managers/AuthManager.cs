using Godot;
using System;
using Godot.Collections;
using System.Text;

public partial class AuthManager : HttpRequest, IGlobalInterface<AuthManager>
{
	public static string NodePath = "/root/AuthManager";
	public static AuthManager GetInstance(Node context) => context.GetNode<AuthManager>(NodePath);

	private static string HOST = "http://localhost:3000/aws";

	private SceneMapper sceneMapper;

	public override void _Ready()
	{
		base._Ready();

		sceneMapper = SceneMapper.GetInstance(this);
	}

	public void VerifyToken(int playerId, string token)
	{
		string url = $"{HOST}/verifyToken";
		string json = $"{{\"token\": \"{token}\"}}";

		GD.Print("Sending", url);

		string[] headers = new string[] { "Content-Type: application/json" }; ;
		RequestCompleted += (long result, long responseCode, string[] headers, byte[] body) => { VerifyTokenResponse(result, responseCode, headers, body, playerId); };
		Request(url, headers, HttpClient.Method.Post, json);
	}

	public void VerifyTokenResponse(long result, long responseCode, string[] headers, byte[] body, int playerId)
	{
		Dictionary json = null;
		if (responseCode == 201L)
		{
			json = Json.ParseString(Encoding.UTF8.GetString(body)).AsGodotDictionary();
		}

		sceneMapper.getInstanceOf<ServerController>(ServerController.LABEL)?.ClientRegisterResponse(playerId, json);
	}
}
