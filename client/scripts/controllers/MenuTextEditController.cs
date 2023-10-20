using Godot;
using System;

public partial class MenuTextEditController : Control
{
	[Export]
	public string key = "";
	[Export]
	public string placeholder = "";
	[Export]
	public string label = "";

	private DataManager dataManager;

	private TextEdit textEdit;

	public override void _Ready()
	{
		base._Ready();

		dataManager = DataManager.GetInstance(this);

		GetNode<Label>("HBoxContainer/Label").Text = label;
		this.textEdit = GetNode<TextEdit>("HBoxContainer/TextEdit");
		this.textEdit.PlaceholderText = placeholder;
		this.textEdit.Text = dataManager.GetConfigValue<string>(key);
		this.textEdit.TextChanged += onTextChanged;
	}

	public void onTextChanged()
	{
		dataManager.SetConfigValue<string>(key, this.textEdit.Text);
	}
}
