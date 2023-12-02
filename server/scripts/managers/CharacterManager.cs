using Godot;
using System;
using Godot.Collections;
using SocketIOClient;

public partial class CharacterManager : Node, IGlobalInterface<CharacterManager>
{
	public static string NodePath = "/root/CharacterManager";
	public static CharacterManager GetInstance(Node context) => context.GetNode<CharacterManager>(NodePath);

	private static string HOST_DEFAULT = "ws://localhost:3010/";
	private static string CONFIG_PATH = "res://data/CharacterManager.config.json";
	private string host;

	private SocketIOClient.SocketIO socketIoClient;
	private bool isConnected;

	public class AckMessage
	{
		public bool ok;
	}

	public override void _Ready()
	{
		base._Ready();
		host = HOST_DEFAULT;
		if (ResourceLoader.Exists(CONFIG_PATH))
		{
			Json json = ResourceLoader.Load<Json>(CONFIG_PATH);
			host = (json.Data.As<Dictionary>())["CharacterHostWs"].AsString();
		}

		GD.Print("CharacterManager WS Host ", host);
	}

	public async void ConnectWebsocket()
	{
		if (isConnected)
			return;

		socketIoClient = new SocketIOClient.SocketIO(host);
		socketIoClient.OnConnected += SocketConnected;
		socketIoClient.OnDisconnected += SocketDisconnected;
		socketIoClient.OnError += SocketError;

		GD.Print("Connecting to character websocket server", host);
		await socketIoClient.ConnectAsync();
	}

	private async void SocketConnected(object sender, object e)
	{
		isConnected = true;
		GD.Print("Connected to character websocket server", e);
		await socketIoClient.EmitAsync("identity", "server");
		GD.Print("identified");
	}
	private void SocketDisconnected(object sender, object e)
	{
		isConnected = false;
		GD.Print("Disconnected from character websocket server", e);
	}

	private void SocketError(object sender, object e)
	{
		isConnected = false;
		GD.Print("Disconnected from character websocket server", e);
	}
}
