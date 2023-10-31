using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class ServerController : Node
{
	[Export]
	public const int PORT = 8910;
	[Export]
	public const int MAX_CLIENTS = 64;

	private ENetMultiplayerPeer network;
	private MultiplayerApi multiplayer;
	private GameManager gameManager;
	private MapDataManager mapDataManager;

	public override void _Ready()
	{
		base._Ready();

		gameManager = GameManager.GetInstance(this);
		mapDataManager = MapDataManager.GetInstance(this);

		StartServer();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (gameManager.playerCount() == 0) return;

		Dictionary<int, Vector2> DTO = new Dictionary<int, Vector2>();
		Dictionary<int, Player> players = gameManager.duplicatePlayers();
		foreach(Player player in players.Values)
		{
			DTO.Add(player.id, player.position);
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(string mapId)
	{
		int playerId = multiplayer.GetRemoteSenderId();
		GD.Print("RequestToSpawn", playerId, mapId);
		Vector2 location = mapDataManager.GetMapSpawnLocation(mapId, SPAWN_POINT_TYPE.PLAYER);
		ResponseToSpawn(location, playerId);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 location, int id)
	{
		GD.Print("ResponseToSpawn", location, id);
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "ResponseToSpawn", location, id);
		gameManager.updatePlayer(id, location);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdatePlayerPosition(Vector2 position)
	{
		gameManager.updatePlayer(multiplayer.GetRemoteSenderId(), position);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateEntities(Dictionary<int, Vector2> players)
	{
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "UpdateEntities", players);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void RemoveEntity(int id)
	{
		RpcId(MultiplayerPeer.TargetPeerBroadcast, "RemoveEntity", id);
	}
}
