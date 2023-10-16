using Godot;
using System;

public partial class StartGameController : Control
{
	
	Button PlayButton;
	Button QuitButton;
	SceneManager sceneManager;
	
	public override void _Ready()
	{
		sceneManager = SceneManager.GetInstance(this);

		PlayButton = GetNode<Button>("VSplitContainer/BottomCenterContainer/GridContainer/Play");
		PlayButton.Pressed += onStartButtonPressed;
		QuitButton = GetNode<Button>("VSplitContainer/BottomCenterContainer/GridContainer/Quit");
		QuitButton.Pressed += onQuitButtonPressed;
	}
	
	public void onStartButtonPressed() {
		sceneManager.ShowScene(SceneManager.SCENES.MULTIPLAYER_LOBBY, this);
	}
	
	public void onQuitButtonPressed() {
		GetTree().Quit();
	}
}
