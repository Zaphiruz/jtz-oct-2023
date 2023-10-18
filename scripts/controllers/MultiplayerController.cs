using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control
{
	[Export]
	public string ADDRESS = "127.0.0.1";
	[Export]
	public int PORT = 8910;

	MultiplayerApi multiplayer;
	ENetMultiplayerPeer peer;
	GameManager gameManager;
	SceneManager sceneManager;
	DataManager dataManager;

	public override void _Ready()
	{
		gameManager = GameManager.GetInstance(this);
		multiplayer = gameManager.multiplayer;
		sceneManager = SceneManager.GetInstance(this);
		dataManager = DataManager.GetInstance(this);

		multiplayer.PeerConnected += _PeerConnected;
		multiplayer.PeerDisconnected += _PeerDisconnected;
		multiplayer.ConnectedToServer += _ConnectedToServer;
		multiplayer.ConnectionFailed += _ConnectionFailed;
		
		GetNode<Button>("VBoxContainer/HBoxContainer/HostButton").Pressed += this.onHostButtonDown;
		GetNode<Button>("VBoxContainer/HBoxContainer/JoinButton").Pressed += this.onJoinButtonDown;
		GetNode<Button>("VBoxContainer/StartGameButton").Pressed += this.onStartGameButtonDown;
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
		RpcId(1, new StringName(nameof(SendPlayerInfo)), multiplayer.GetUniqueId(), dataManager.GetConfigValue<string>("PlayerName"));
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void SendPlayerInfo(long id, string name)
	{
		GD.Print("tryadd ", id);
		if (!gameManager.hasPlayer(id))
		{
			gameManager.addPlayer(id, name);
		}

		if(multiplayer.IsServer())
		{
			GD.Print("spread the wealth");
			foreach(Player player in gameManager.getPlayers())
			{
				GD.Print("send ", player.id);
				Rpc(new StringName(nameof(SendPlayerInfo)), player.id, player.name);
			}
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void StartGame()
	{
		if (peer == null) return;

		GD.Print("Starting Game!");
		sceneManager.ShowScene(SceneManager.SCENES.TEST_2_PLAYER, this);
	}

	public void HostGame()
	{
		if (peer != null) return;

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

		long id = multiplayer.GetUniqueId();
		string name = dataManager.GetConfigValue<string>("PlayerName");
		gameManager.addPlayer(id, name);
		Rpc(new StringName(nameof(SendPlayerInfo)), id, name);
	}

	public void onHostButtonDown()
	{
		HostGame();
	}

	public void JoinHost()
	{
		if (peer != null) return;

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
