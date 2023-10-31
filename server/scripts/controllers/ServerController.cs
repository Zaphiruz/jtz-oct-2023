using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ServerController : Node, IInstanceMappable
{
	public static string LABEL = "Server";

	[Export]
	public const int PORT = 8910;
	[Export]
	public const int MAX_CLIENTS = 64;

	private ENetMultiplayerPeer network;
	private MultiplayerApi multiplayer;
	private GameManager gameManager;
	private MapDataManager mapDataManager;
	private SceneMapper sceneMapper;
	private AuthManager authManager;

	public override void _Ready()
	{
		base._Ready();

		sceneMapper = SceneMapper. GetInstance(this, ServerController.LABEL);
		gameManager = GameManager.GetInstance(this);
		mapDataManager = MapDataManager.GetInstance(this);
		authManager = AuthManager.GetInstance(this);

		StartServer();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (gameManager.playerCount() == 0) return;

		Dictionary<int, Array<Variant>> DTO = new Dictionary<int, Array<Variant>>();
		Dictionary<int, Player> players = gameManager.duplicatePlayers();
		foreach(Player player in players.Values)
		{
			DTO.Add(player.id, player.ToArgs());
		}
		UpdateEntities(DTO);
	}

	public Error StartServer()
	{
		network = new ENetMultiplayerPeer();
		Error error = network.CreateServer(PORT, MAX_CLIENTS);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to start server", error);
		} else
		{
			multiplayer = GetTree().GetMultiplayer();
			multiplayer.MultiplayerPeer = network;
			multiplayer.PeerConnected += ClientConnected;
			multiplayer.PeerDisconnected += ClientDisconencted;

			GD.Print("Server Started");
		}

		return error;
	}

	public void ClientConnected(long id)
	{
		GD.Print(id, " connected");
		gameManager.addPlayer(Convert.ToInt32(id));
	}

	public void ClientDisconencted(long _id)
	{
		int id = Convert.ToInt32(_id);
		GD.Print(id, " disconnected");
		gameManager.removePlayer(id);
		RemoveEntity(id);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRegisterRequest(string accessToken)
	{
		int playerId = multiplayer.GetRemoteSenderId();
		authManager.VerifyToken(playerId, accessToken);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRegisterResponse(int playerId, Dictionary json)
	{
		if (json == null)
		{
			GD.Print("Invalid token", playerId);
			RpcId(playerId, "ClientRegisterResponse", false);
			multiplayer.MultiplayerPeer.DisconnectPeer(playerId, true);
			ClientDisconencted((long) playerId);
		} else
		{
			GD.Print("Valid token", playerId);
			gameManager.updatePlayer(playerId, json["username"].As<string>());
			RpcId(playerId, "ClientRegisterResponse", true);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(string mapId)
	{
		int playerId = multiplayer.GetRemoteSenderId();
		GD.Print("RequestToSpawn", playerId, mapId);
		Vector2 position = mapDataManager.GetMapSpawnLocation(mapId, SPAWN_POINT_TYPE.PLAYER);
		ResponseToSpawn(position, playerId);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 position, int id)
	{
		Player player = gameManager.getPlayer(id);
		player.position = position;

		GD.Print("ResponseToSpawn", position, id);
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "ResponseToSpawn", position, id, player.name);
		gameManager.updatePlayer(player);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdatePlayerPosition(Vector2 position)
	{
		gameManager.updatePlayer(multiplayer.GetRemoteSenderId(), position);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateEntities(Dictionary<int, Array<Variant>> players)
	{
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "UpdateEntities", players);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void RemoveEntity(int id)
	{
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "RemoveEntity", id);
	}
}
