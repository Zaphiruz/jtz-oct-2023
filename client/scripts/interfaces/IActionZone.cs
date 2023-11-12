using Godot;

public partial interface IActionZone
{
	Area2D area2D { get; set; }
	bool playerInZone { get; set; }

	public void OnAreaEntered(Area2D entered);
	public void OnAreaExited(Area2D entered);
	public void OnActionTriggered();
	public delegate void EnterActionZoneEventHandler(string triggerId);
	public delegate void ExitActionZoneEventHandler(string triggerId);
	public delegate void ActionTriggeredEventHandler(string triggerId);
}