using Godot;
using System;

public partial class GatherableController : StaticBody2D
{
	[ExportCategory("Node2D Settings")]
	[Export]
	public string id;

	[ExportCategory("Sprite2D Settings")]
	[Export]
	private Texture2D texture;

	[Export(PropertyHint.Range, "1,25")]
	private int vFrames = 1;

	[Export(PropertyHint.Range, "1,25")]
	private int hFrames = 1;

	private Sprite2D sprite;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Texture = texture;
		sprite.Vframes = vFrames;
		sprite.Hframes = hFrames;
	}

	public override void _Process(double delta)
	{
	}

	public virtual void GoToFrame(int index)
	{
		sprite.Frame = index;
	}
}
