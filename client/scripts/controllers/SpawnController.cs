using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class SpawnController : Node2D, IInstanceMappable
{
	public static string LABEL = "Spawn";

	private Server server;
	private SceneManager sceneManager;

	private PackedScene PlayerResource;
	private PackedScene OtherPlayerResource;
	private PackedScene EnemiesResource;

	private Dictionary<int, CharacterBody2D> entityDictionary;

	[Export]
	public string mapId;

	public override void _Ready()
	{
		server = Server.GetInstance(this, SpawnController.LABEL);
		sceneManager = SceneManager.GetInstance(this);

		PlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//Player.tscn");
		OtherPlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//OtherPlayer.tscn");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//Enemy.tscn");
		
		entityDictionary = new Dictionary<int, CharacterBody2D>();

		// set trigger maps
		foreach (Node node in GetTree().GetNodesInGroup("Trigger"))
		{
			((TriggerController) node).Triggered += MapTriggerHit;
		}
		

		RequestSpawn(mapId);
		//SpawnEncounters();
	}

	public void DisconnectedFromServer()
	{
		sceneManager.ShowScene(SceneManager.SCENES.MULTIPLAYER_LOBBY, this);
	}

	private void RequestSpawn(string mapId)
	{
		server.RequestToSpawn(mapId);
	}

	public void SpawnPlayer(Vector2 location, int id, string name)
	{
		GD.Print("Spawning ", id);

		if (entityDictionary.ContainsKey(id)) return;
		
		CharacterBody2D character = (CharacterBody2D) (id == server.multiplayer.GetUniqueId() ? PlayerResource.Instantiate() : OtherPlayerResource.Instantiate());
		character.SetMeta("ID", id);
		character.SetMeta("Name", name ?? "???");
		AddChild(character);
		character.GlobalPosition = location;

		entityDictionary.Add(id, character);
	}

	public void UpdateEntities(Dictionary<int, Array<Variant>> otherPlayers)
	{
		foreach (System.Collections.Generic.KeyValuePair<int, Array<Variant>> data in otherPlayers)
		{

			UpdateEntity(data.Key, Player.From(data.Value));
		}
	}

	public void UpdateEntity(int id, Player playerData)
	{
		CharacterBody2D match = FindEntity(id);
			if (match != null)
			{
				((EntityController) match).SyncState(playerData.position);
			} else
			{
				SpawnPlayer(playerData.position, id, playerData.name);
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

	public void MapTriggerHit(string triggerId)
	{
		GD.Print("Catch Triggered", triggerId);
		server.MapTriggerHit(mapId, triggerId);
	}

	public void TeleportEntity(int id, Vector2 newPosition)
	{
		GD.Print("Zoomin", newPosition);
		CharacterBody2D character = FindEntity(id);
		character.GlobalPosition = newPosition;
	}
}
