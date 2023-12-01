using Godot;
using System;
using Godot.Collections;
using H.Socket.IO;

public partial class CharacterManager : Node, IGlobalInterface<CharacterManager>
{
	public static string NodePath = "/root/CharacterManager";
	public static CharacterManager GetInstance(Node context) => context.GetNode<CharacterManager>(NodePath);

	private static string HOST_DEFAULT = "ws://localhost:3010/";
	private static string CONFIG_PATH = "res://data/CharacterManager.config.json";
	private string host;

	private SocketIoClient socketIoClient;
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

		socketIoClient = new SocketIoClient();
		socketIoClient.Connected += SocketConnected;
		socketIoClient.Disconnected += SocketDisconnected;
		socketIoClient.ErrorReceived += SocketError;

		await socketIoClient.ConnectAsync(new Uri(host));
	}

	private async void SocketConnected(object sender, SocketIoClient.ConnectedEventArgs args)
	{
		isConnected = true;
		GD.Print("Connected to character websocket server", args);
		await socketIoClient.Emit("identity", "server");
		GD.Print("identified");
	}
	private void SocketDisconnected(object sender, SocketIoClient.DisconnectedEventArgs args)
	{
		isConnected = false;
		GD.Print("Disconnected from character websocket server", args);
	}

	private void SocketError(object sender, SocketIoClient.ErrorReceivedEventArgs args)
	{
		GD.Print("Error from character websocket server", args);
	}

	private void SocketException(object sender, SocketIoClient.ExceptionOccurredEventArgs args)
	{
		GD.Print("Exception from character websocket server", args);
	}
}
