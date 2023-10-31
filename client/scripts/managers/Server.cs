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
	public MultiplayerApi multiplayer { get; private set; }
	private ENetMultiplayerPeer client;

	public override void _Ready() {
		base._Ready();
		
		sceneMapper = SceneMapper.GetInstance(this);
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

		sceneMapper.getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.EnterGame();
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

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(string mapId)
	{
		GD.Print("RequestToSpawn", mapId);
		RpcId(MultiplayerPeer.TargetPeerServer, "RequestToSpawn", mapId);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 location, int id)
	{
		GD.Print("ResponseToSpawn", location, id);
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.SpawnPlayer(location, id);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdatePlayerPosition(Vector2 position)
	{
		RpcId(MultiplayerPeer.TargetPeerServer, "UpdatePlayerPosition", position);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateEntities(Dictionary<int, Vector2> players)
	{
		players.Remove(multiplayer.GetUniqueId());
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.UpdateEntities(players);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void RemoveEntity(int id)
	{
		sceneMapper.getInstanceOf<SpawnController>(SpawnController.LABEL)?.RemoveEntity(id);
	}
}
