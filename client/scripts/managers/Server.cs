using Godot;

public partial class Server : Node, IGlobalInterface<Server>
{
	public static string NodePath = "/root/Server";
	public static Server GetInstance(Node context) => context.GetNode<Server>(NodePath);

	[Export]
	public string ADDRESS = "127.0.0.1";
	[Export]
	public int PORT = 8910;

	public MultiplayerApi multiplayer { get; private set; }
	private ENetMultiplayerPeer client;

	public Error ConnectToServer(ulong instanceId)
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
			multiplayer.ConnectedToServer += () => _ConnectedToServer(instanceId);
			multiplayer.ConnectionFailed += _ConnectionFailed;
			multiplayer.MultiplayerPeer = client;
		}

		return error;
	}

	public void _ConnectedToServer(ulong instanceId)
	{
		GD.Print("connected to server");
		((MultiplayerController) InstanceFromId(instanceId)).EnterGame();
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(Vector2 location, ulong requester)
	{
		GD.Print("RequestToSpawn", location, requester);
		RpcId(1, "RequestToSpawn", location, requester);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 location, ulong requester, int id)
	{
		GD.Print("ResponseToSpawn", location, requester, id);
		((SpawnController) InstanceFromId(requester)).SpawnPlayer(location, id);
	}
}
