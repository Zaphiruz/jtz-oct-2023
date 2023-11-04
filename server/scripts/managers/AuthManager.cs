using Godot;
using System;
using Godot.Collections;
using System.Text;

public partial class AuthManager : Node, IGlobalInterface<AuthManager>
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
		
		string[] headers = new string[] { "Content-Type: application/json" };
		HttpRequest request = new HttpRequest();
		AddChild(request);
		request.RequestCompleted += (long result, long responseCode, string[] headers, byte[] body) => { VerifyTokenResponse(request, result, responseCode, headers, body, playerId); };
		request.Request(url, headers, HttpClient.Method.Post, json);
	}

	public void VerifyTokenResponse(HttpRequest request, long result, long responseCode, string[] headers, byte[] body, int playerId)
	{
		request.QueueFree();

		Dictionary json = null;
		if (responseCode == 201L)
		{
			json = Json.ParseString(Encoding.UTF8.GetString(body)).AsGodotDictionary();
		}

		sceneMapper.getInstanceOf<ServerController>(ServerController.LABEL)?.ClientRegisterResponse(playerId, json);
	}
}
