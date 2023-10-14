using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SpawnController : Node2D
{
	private GameManager gameManager;

	private PackedScene CharacterResource;
	private PackedScene EnemiesResource;

	public override void _Ready()
	{
		gameManager = GetNode<GameManager>(GameManager.NodePath);
		CharacterResource = GD.Load<PackedScene>("res://scene-objects//Entities//character.res");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//enemy.res");
		SpawnPlayers();
		SpawnEncounters();
	}

	public void SpawnPlayers()
	{
		Array<Node> spawnPoints = GetTree().GetNodesInGroup("PlayerSpawn");
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

	public void SpawnEncounters()
	{
		Array<Node> spawnPoints = GetTree().GetNodesInGroup("EncounterSpawn");
		Node2D spawnPoint = (Node2D) spawnPoints.First();

		CharacterBody2D enemy = (CharacterBody2D) EnemiesResource.Instantiate();
		AddChild(enemy);
		enemy.GlobalPosition = spawnPoint.GlobalPosition;
	}
}
