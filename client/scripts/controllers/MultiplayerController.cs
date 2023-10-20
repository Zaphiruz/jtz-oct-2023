using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control
{
	private Server server;
	private GameManager gameManager;
	private SceneManager sceneManager;
	private DataManager dataManager;

	private MultiplayerApi multiplayer;

	public override void _Ready()
	{
		server = Server.GetInstance(this);
		multiplayer = server.multiplayer;
		gameManager = GameManager.GetInstance(this);
		sceneManager = SceneManager.GetInstance(this);
		dataManager = DataManager.GetInstance(this);

		GetNode<Button>("StartGameButton").Pressed += this.onStartGameButtonDown;
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

	public void StartGame()
	{
		Error error = server.ConnectToServer();
		if (error == Error.Ok)
		{
			GD.Print("Starting Game!");
			sceneManager.ShowScene(SceneManager.SCENES.TEST_2_PLAYER, this);
		}
	}

	public void onStartGameButtonDown()
	{
		StartGame();
	}
}
