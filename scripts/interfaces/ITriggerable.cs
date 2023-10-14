
using Godot;

public partial interface ITriggerable
{
	Area2D area2D { get; set; }

	public void OnAreaEntered(Area2D entered);
}