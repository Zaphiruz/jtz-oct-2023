using Godot;
using Godot.Collections;
using System;

public partial class PlayerDataOverlayController : Node
{
	private GameManager gameManager;
	private TextEdit textEdit;

	public override void _Ready()
	{
		base._Ready();

		gameManager = GameManager.GetInstance(this);

		textEdit = GetNode<TextEdit>("MarginContainer/VBoxContainer/TextEdit");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		textEdit.Text = "";
		
		if (gameManager.playerCount() == 0) return;

		Dictionary<int, Player> players = gameManager.duplicatePlayers();
		foreach (Player playerData in players.Values)
		{
			textEdit.Text += string.Format("Player:: id: {0}, name: {1}, position:(x: {2}, y: {3})\n\r", playerData.id, playerData.name, playerData.position[0], playerData.position[1]);
		}
	}
}
