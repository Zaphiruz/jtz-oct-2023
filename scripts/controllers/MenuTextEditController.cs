using Godot;
using System;

public partial class MenuTextEditController : Control
{
	[Export]
	public string key = "";
	[Export]
	public string value = "";
	[Export]
	public string placeholder = "";


	public override void _Ready()
	{
		base._Ready();

		GetNode<Label>("HBoxContainer/Label").Text = key;
		GetNode<TextEdit>("HBoxContainer/TextEdit").PlaceholderText = placeholder;
		GetNode<TextEdit>("HBoxContainer/TextEdit").Text = value;
	}


}
