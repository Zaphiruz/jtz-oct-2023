using Godot;
using Godot.Collections;

public partial class ServerController : Node
{
	[Export]
	public const int PORT = 8910;
	[Export]
	public const int MAX_CLIENTS = 64;

	private ENetMultiplayerPeer network;
	private MultiplayerApi multiplayer;

	public override void _Ready()
	{
		base._Ready();

		StartServer();
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
	}

	public void ClientDisconencted(long id)
	{
		GD.Print(id, " disconnected");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void RequestToSpawn(Vector2 location)
	{
		GD.Print("RequestToSpawn", location);
		ResponseToSpawn(location, multiplayer.GetRemoteSenderId());
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void ResponseToSpawn(Vector2 location, int id)
	{
		GD.Print("ResponseToSpawn", location, id);
		RpcId(0, "ResponseToSpawn", location, id);
	}
}
