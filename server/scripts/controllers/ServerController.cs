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

		network = new ENetMultiplayerPeer();
		multiplayer = GetTree().GetMultiplayer();
		multiplayer.PeerConnected += ClientConnected;
		multiplayer.PeerDisconnected += ClientDisconencted;

		StartServer();
	}

	public void StartServer()
	{
		Error error = network.CreateServer(PORT, MAX_CLIENTS);
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to start server", error);
		} else
		{
			multiplayer.MultiplayerPeer = network;

			GD.Print("Server Started");
		}
	}

	public void ClientConnected(long id)
	{
		GD.Print(id, " connected");
	}

	public void ClientDisconencted(long id)
	{
		GD.Print(id, " disconnected");
	}
}
