using Godot;

public partial class SettingsController : Control
{
	[Signal]
	public delegate void ExitSettingsEventHandler();

	private DataManager dataManager;

	public override void _Ready()
	{
		dataManager = DataManager.GetInstance(this);

		GetNode<Button>("MarginContainer/VBoxContainer/Exit").Pressed += onExitPressed;
		SetProcess(false);
	}

	public void onExitPressed()
	{
		dataManager.SaveConfigToDisk();
		EmitSignal(SignalName.ExitSettings);
		SetProcess(false);
	}
}
