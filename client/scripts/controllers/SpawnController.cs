using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SpawnController : Node2D
{
	public static string LABEL = "Spawn";

	private Server server;

	private PackedScene PlayerResource;
	private PackedScene OtherPlayerResource;
	private PackedScene EnemiesResource;

	public override void _Ready()
	{
		server = Server.GetInstance(this, SpawnController.LABEL);
		PlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//Player.tscn");
		OtherPlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//OtherPlayer.tscn");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//Enemy.tscn");
		
		RequestSpawn();
		//SpawnEncounters();
	}

	private void RequestSpawn()
	{
		Array<Node> spawnPoints = GetTree().GetNodesInGroup("PlayerSpawn");
		server.RequestToSpawn(((Node2D) spawnPoints[0]).GlobalPosition);
	}

	public void SpawnPlayer(Vector2 location, int id)
	{
		GD.Print("Spawning ", id);
		CharacterBody2D character = (CharacterBody2D) (id == server.multiplayer.GetUniqueId() ? PlayerResource.Instantiate() : OtherPlayerResource.Instantiate());
		character.SetMeta("ID", id);
		AddChild(character);
		character.GlobalPosition = location;
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
