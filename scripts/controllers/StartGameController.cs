using Godot;

public partial class StartGameController : Control
{
	SceneManager sceneManager;
	SettingsController settingsController;

	

	public override void _Ready()
	{
		sceneManager = SceneManager.GetInstance(this);

		GetNode<Button>("VSplitContainer/BottomCenterContainer/GridContainer/Play").Pressed += onStartButtonPressed;
		GetNode<Button>("VSplitContainer/BottomCenterContainer/GridContainer/Settings").Pressed += onSettingsButtonPressed;
		GetNode<Button>("VSplitContainer/BottomCenterContainer/GridContainer/Quit").Pressed += onQuitButtonPressed;
		settingsController = GetNode<SettingsController>("SettingsMenu");
		settingsController.ExitSettings += onSettingsExitSignal;
	}
	
	public void onStartButtonPressed() {
		sceneManager.ShowScene(SceneManager.SCENES.MULTIPLAYER_LOBBY, this);
	}
	
	public void onQuitButtonPressed() {
		GetTree().Quit();
	}

	public void onSettingsButtonPressed()
	{
		settingsController.Visible = true;
	}

	public void onSettingsExitSignal()
	{
		settingsController.Visible = false;
	}
}
