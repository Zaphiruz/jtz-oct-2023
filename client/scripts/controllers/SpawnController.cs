using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SpawnController : Node2D, IInstanceMappable
{
	public static string LABEL = "Spawn";

	private Server server;

	private PackedScene PlayerResource;
	private PackedScene OtherPlayerResource;
	private PackedScene EnemiesResource;

	private Dictionary<int, CharacterBody2D> entityDictionary;
	private string mapId;

	public override void _Ready()
	{
		server = Server.GetInstance(this, SpawnController.LABEL);
		PlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//Player.tscn");
		OtherPlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//OtherPlayer.tscn");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//Enemy.tscn");
		
		entityDictionary = new Dictionary<int, CharacterBody2D>();
		mapId = GetMeta("ID").As<string>();

		RequestSpawn(mapId);
		//SpawnEncounters();
	}

	private void RequestSpawn(string mapId)
	{
		server.RequestToSpawn(mapId);
	}

	public void SpawnPlayer(Vector2 location, int id)
	{
		GD.Print("Spawning ", id);

		if (entityDictionary.ContainsKey(id)) return;
		
		CharacterBody2D character = (CharacterBody2D) (id == server.multiplayer.GetUniqueId() ? PlayerResource.Instantiate() : OtherPlayerResource.Instantiate());
		character.SetMeta("ID", id);
		AddChild(character);
		character.GlobalPosition = location;

		entityDictionary.Add(id, character);
	}

	public void UpdateEntities(Dictionary<int, Vector2> otherPlayers)
	{
		foreach (System.Collections.Generic.KeyValuePair<int, Vector2> data in otherPlayers)
		{
			UpdateEntity(data.Key, data.Value);
		}
	}

	public void UpdateEntity(int id, Vector2 pos)
	{
		CharacterBody2D match = FindEntity(id);
			if (match != null)
			{
				((EntityController) match).SyncState(pos);
			} else
			{
				SpawnPlayer(pos, id);
			}
	}

	public void RemoveEntity(int id)
	{
		CharacterBody2D character = FindEntity(id);
		character?.QueueFree();
		entityDictionary.Remove(id);
	}

	public CharacterBody2D FindEntity(int id)
	{
		CharacterBody2D character = null;
		entityDictionary.TryGetValue(id, out character);
		return character;
	}
}
