using Godot;
using System;

public partial class GatherableController : StaticBody2D, IActionZone
{
	[Export]
	public string id;

	[Signal]
	public delegate void EnterActionZoneEventHandler(string triggerId);
	[Signal]
	public delegate void ExitActionZoneEventHandler(string triggerId);
	[Signal]
	public delegate void ActionTriggeredEventHandler(string triggerId);

	private Sprite2D sprite;
	public Area2D area2D { get; set; }
	public bool playerInZone { get; set; }

	public override void _Ready()
	{
		base._Ready();

		area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += OnAreaEntered;
		area2D.AreaExited += OnAreaExited;

		sprite = GetNode<Sprite2D>("Sprite2D");
	}

	public virtual void OnAreaEntered(Area2D entity)
	{
		GD.Print("Zone Entered", id);
		playerInZone = true;
		EmitSignal(SignalName.EnterActionZone, id);
	}

	public virtual void OnAreaExited(Area2D entity)
	{
		GD.Print("Zone Exited", id);
		playerInZone = false;
		EmitSignal(SignalName.ExitActionZone, id);
	}

	public virtual void OnActionTriggered()
	{
		GD.Print("Action Trigggered", id);
		EmitSignal(SignalName.ActionTriggered, id);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (!playerInZone) return;

		if (Input.IsActionJustPressed("INTERACT")) OnActionTriggered();
	}

	public virtual void GoToFrame(int index)
	{
		sprite.Frame = index;
	}
}
