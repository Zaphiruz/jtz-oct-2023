using Godot;
using Godot.Collections;
using System.Linq;

public partial class SpawnManager : Node2D
{
	private GameManager gameManager;

	private PackedScene CharacterResource; 

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>("/root/GameManager");

		CharacterResource = GD.Load<PackedScene>("res://scene-objects//character.res");

		Array<Node> spawnPoints = GetTree().GetNodesInGroup("SpawnLocations");
		int index = 0;
		foreach (Player player in gameManager.getPlayers())
		{
			GD.Print("Spawning ", player.id);
			CharacterBody2D character = (CharacterBody2D) CharacterResource.Instantiate();
			character.SetMeta("ID", player.id);
			AddChild(character);

			Node2D spawnPoint = (Node2D) spawnPoints.ElementAt(index);
			character.GlobalPosition = spawnPoint.GlobalPosition;

			index++;
		}
	}
}
