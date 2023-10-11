using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control
{
	[Export] public string ADDRESS = "127.0.0.1";
	[Export] public int PORT = 8910;
	const string INSERT_NAME = "steve";

	MultiplayerApi multiplayer;
	ENetMultiplayerPeer peer;
	GameManager gameManager;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");
		multiplayer = GetTree().GetMultiplayer();

		multiplayer.PeerConnected += _PeerConnected;
		multiplayer.PeerDisconnected += _PeerDisconnected;
		multiplayer.ConnectedToServer += _ConnectedToServer;
		multiplayer.ConnectionFailed += _ConnectionFailed;
		
		GetNode<Button>("/root/Node2D/Control/HostButton").Pressed += this.onHostButtonDown;
		GetNode<Button>("/root/Node2D/Control/JoinButton").Pressed += this.onJoinButtonDown;
		GetNode<Button>("/root/Node2D/Control/StartGameButton").Pressed += this.onStartGameButtonDown;
	}

	public override void _Process(double delta)
	{

	}

	public void _PeerConnected(long id)
	{
		GD.Print("Player Connected " + id.ToString());
	}

	public void _PeerDisconnected(long id)
	{
		GD.Print("Player Disconnected " + id.ToString());
		gameManager.removePlayer(id);
		// remove from scene
	}

	public void _ConnectedToServer()
	{
		GD.Print("connected to server");
		RpcId(1, new StringName(nameof(SendPlayerInfo)), multiplayer.GetUniqueId(), INSERT_NAME);
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void SendPlayerInfo(long id, string name)
	{
		if(!gameManager.hasPlayer(id))
		{
			gameManager.addPlayer(id, name);
		}

		if(multiplayer.IsServer())
		{
			foreach(Player player in gameManager.getPlayers())
			{
				Rpc(new StringName(nameof(SendPlayerInfo)), player.id, player.name);
			}
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void StartGame()
	{
		// load scene
		// add scene to tree
		// hide node
		GD.Print("Starting Game!");
	}

	public void HostGame()
	{
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateServer(PORT, 4);
		if (error != Error.Ok)
		{
			GD.Print("server failed to create", error.ToString());
			return;
		}
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

		multiplayer.MultiplayerPeer = peer;
		GD.Print("Waiting For Players!");
		Rpc(new StringName(nameof(SendPlayerInfo)), multiplayer.GetUniqueId(), INSERT_NAME);
	}

	public void onHostButtonDown()
	{
		HostGame();
	}

	public void JoinHost()
	{
		peer = new ENetMultiplayerPeer();
		Error error = peer.CreateClient(ADDRESS, PORT);
		if (error != Error.Ok)
		{
			GD.Print("failed to join server", error.ToString());
			return;
		}
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		multiplayer.MultiplayerPeer = peer;
	}

	public void onJoinButtonDown()
	{
		JoinHost();
	}

	public void onStartGameButtonDown()
	{
		Rpc(new StringName(nameof(StartGame)));
	}
}
