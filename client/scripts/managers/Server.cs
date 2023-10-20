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

	public override void _Ready()
	{
		base._Ready();
		client = new ENetMultiplayerPeer();
		multiplayer = GetTree().GetMultiplayer();
		multiplayer.ConnectedToServer += _ConnectedToServer;
		multiplayer.ConnectionFailed += _ConnectionFailed;
	}

	public Error ConnectToServer()
	{
		
		Error error = client.CreateClient(ADDRESS, PORT);
		if (error != Error.Ok)
		{
			GD.PrintErr("failed to create client", error);
		} else
		{
			GD.Print("client created");
			multiplayer.MultiplayerPeer = client;
		}

		return error;
	}

	public void _ConnectedToServer()
	{
		GD.Print("connected to server");
	}

	public void _ConnectionFailed()
	{
		GD.Print("failed to connect to server");
	}
}
