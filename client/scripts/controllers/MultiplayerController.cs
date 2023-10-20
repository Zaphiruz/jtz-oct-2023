using Godot;
using System.Collections.Generic;

public partial class MultiplayerController : Control
{
	private Server server;
	private SceneManager sceneManager;
	private DataManager dataManager;

	private MultiplayerApi multiplayer;

	public override void _Ready()
	{
		server = Server.GetInstance(this);
		multiplayer = server.multiplayer;
		sceneManager = SceneManager.GetInstance(this);
		dataManager = DataManager.GetInstance(this);

		GetNode<Button>("StartGameButton").Pressed += this.onStartGameButtonDown;
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
