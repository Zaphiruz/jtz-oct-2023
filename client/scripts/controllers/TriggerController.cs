using Godot;
using System;

public partial class TriggerController : Node2D, ITriggerable
{

	[Signal]
	public delegate void TriggeredEventHandler(string triggerId);

	public Area2D area2D { get; set; }

	[Export]
	public string triggerId;

	public override void _Ready()
	{
		base._Ready();

		area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += OnAreaEntered;
	}

	public virtual void OnAreaEntered(Area2D entity) {
		GD.Print("Emit Triggered", triggerId);
		EmitSignal(SignalName.Triggered, triggerId);
	}
}
