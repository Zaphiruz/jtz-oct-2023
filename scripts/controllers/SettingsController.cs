using Godot;

public partial class SettingsController : Control
{
	[Signal]
	public delegate void ExitSettingsEventHandler();

	public override void _Ready()
	{
		GetNode<Button>("MarginContainer/VBoxContainer/Exit").Pressed += onExitPressed;
		SetProcess(false);
	}

	public void onExitPressed()
	{
		EmitSignal(SignalName.ExitSettings);
		SetProcess(false);
	}
}
