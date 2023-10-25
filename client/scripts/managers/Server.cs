using Godot;
using Godot.Collections;

public partial class SceneData : GodotObject {
	public ulong instanceId;
	public string name;

	public SceneData(ulong instanceId, string name)
	{
		this.instanceId = instanceId;
		this.name = name;
	}
}

public partial class Server : Node, IGlobalInterface<Server>
{
	public static string NodePath = "/root/Server";
	public static Server GetInstance(Node context, string Name) {
		Server server = context.GetNode<Server>(NodePath);
		if (Name != null) server.SceneMap.Add(Name, new SceneData(context.GetInstanceId(), Name));
		return server;
	}

	[Export]
	public string ADDRESS = "127.0.0.1";
	[Export]
	public int PORT = 8910;

	public MultiplayerApi multiplayer { get; private set; }
	private ENetMultiplayerPeer client;

	// jank?
	private Dictionary<string, SceneData> SceneMap;
	
	public override void _Ready()
	{
		SceneMap = new Dictionary<string, SceneData>();
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
			multiplayer.MultiplayerPeer = client;
		}

		return error;
	}

	public T getInstanceOf<T>(string Name) where T : Node
	{
		SceneData data;
		if (SceneMap.TryGetValue(Name, out data))
		{
			return ((T) InstanceFromId(data.instanceId));
		} else
		{
			return null;
		}
	}

	public void _ConnectedToServer()
	{
		GD.Print("connected to server");
		getInstanceOf<MultiplayerController>(MultiplayerController.LABEL)?.EnterGame();
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(Vector2 location)
	{
		GD.Print("RequestToSpawn", location);
		RpcId(1, "RequestToSpawn", location);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 location, int id)
	{
		GD.Print("ResponseToSpawn", location, id);
		getInstanceOf<SpawnController>(SpawnController.LABEL)?.SpawnPlayer(location, id);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void UpdatePlayerPosition(Vector2 position)
	{
		RpcId(1, "UpdatePlayerPosition", position);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void UpdateEntities(Dictionary<int, Vector2> players)
	{
		players.Remove(multiplayer.GetUniqueId());
		getInstanceOf<SpawnController>(SpawnController.LABEL)?.UpdateEntities(players);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void RemoveEntity(int id)
	{
		getInstanceOf<SpawnController>(SpawnController.LABEL)?.RemoveEntity(id);
	}
}
