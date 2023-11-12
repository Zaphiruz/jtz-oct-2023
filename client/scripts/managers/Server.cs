using Godot;
using Godot.Collections;

public partial class Server : Node, IGlobalInterface<Server>
{
	public static string NodePath = "/root/Server";
	public static Server GetInstance(Node context) => context.GetNode<Server>(NodePath);
	public static Server GetInstance(Node context, string Name) {
		SceneMapper.GetInstance(context, Name);
		return context.GetNode<Server>(NodePath);
	}

	[Export]
	public string ADDRESS = "127.0.0.1";
	[Export]
	public int PORT = 8910;

	SceneMapper sceneMapper;
	AuthManager authManager;
	public MultiplayerApi multiplayer { get; private set; }
	private ENetMultiplayerPeer client;

	public override void _Ready() {
		base._Ready();
		
		sceneMapper = SceneMapper.GetInstance(this);
		authManager = AuthManager.GetInstance(this);
	}

	public Error ConnectToServer()
	{
		client = new ENetMultiplayerPeer();
		
		Error error = client.CreateClient(ADDRESS, PORT);
		if (error != Error.Ok)
		{
			GD.PrintErr("failed to create client", error);
		} else
		{
			GD.Print("client created");
			multiplayer = GetTree().GetMultiplayer();
			multiplayer.ConnectedToServer +=  _ConnectedToServer;
			multiplayer.ConnectionFailed += _ConnectionFailed;
			multiplayer.ServerDisconnected += _Disconnected;
			multiplayer.MultiplayerPeer = client;
		}

		return error;
	}

	public void _ConnectedToServer()
	{
		GD.Print("connected to server");
		ClientRegisterRequest(authManager.GetAccessToken());
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
		sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.ConnectionFailed();
	}

	public void _Disconnected()
	{
		GD.Print("disconnected from server");
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.DisconnectedFromServer();
		sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.ConnectionFailed("disconnected from server");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRegisterRequest(string accessToken)
	{
		RpcId(MultiplayerPeer.TargetPeerServer, "ClientRegisterRequest", accessToken);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ClientRegisterResponse(bool isOk)
	{
		if (isOk)
		{
			sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.EnterGame();
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(string mapId)
	{
		GD.Print("RequestToSpawn", mapId);
		RpcId(MultiplayerPeer.TargetPeerServer, "RequestToSpawn", mapId);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(int id, string name, Vector2 position)
	{
		GD.Print("ResponseToSpawn", id, name, position);
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.SpawnPlayer(new Player(id, name, position));
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdatePlayerPosition(Vector2 position, int state)
	{
		RpcId(MultiplayerPeer.TargetPeerServer, "UpdatePlayerPosition", position, state);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateEntities(Dictionary<int, Array<Variant>> players)
	{
		players.Remove(multiplayer.GetUniqueId());
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.UpdateEntities(players);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void RemoveEntity(int id)
	{
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.RemoveEntity(id);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void MapTriggerHit(string mapId, string triggerId)
	{
		GD.Print("MapTriggerHit", mapId, triggerId);
		RpcId(MultiplayerPeer.TargetPeerServer, "MapTriggerHit", mapId, triggerId);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void TeleportCharacter(Vector2 destinationPos)
	{
		GD.Print("TeleportCharacter", destinationPos);
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.TeleportEntity(multiplayer.GetUniqueId(), destinationPos);
		UpdatePlayerPosition(destinationPos, (int) ENTITY_STATE.NONE);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestStaticEntities(string mapId)
	{
		RpcId(MultiplayerPeer.TargetPeerServer, "RequestStaticEntities", mapId);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateStaticEntites(Array<Array<Variant>> staticEntities)
	{
		Array<StaticEntity> staticEntries = new Array<StaticEntity>();
		foreach (Array<Variant> data in staticEntities)
		{
			staticEntries.Add(StaticEntity.From(data));
		}

		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.UpdateStaticEntities(staticEntries);
	}
}
