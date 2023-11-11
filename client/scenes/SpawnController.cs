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

	private Dictionary<int, EntityController> entityDictionary;

	[Export]
	public string mapId;

	public override void _Ready()
	{
		server = Server.GetInstance(this, SpawnController.LABEL);
		sceneManager = SceneManager.GetInstance(this);

		PlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//Player.tscn");
		OtherPlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//OtherPlayer.tscn");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//Enemy.tscn");
		
		entityDictionary = new Dictionary<int, EntityController>();

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

	public void SpawnPlayer(Player playerData)
	{
		GD.Print("Spawning ", playerData.id);

		if (entityDictionary.ContainsKey(playerData.id)) return;
		
		EntityController character = (EntityController) (playerData.id == server.multiplayer.GetUniqueId() ? PlayerResource.Instantiate() : OtherPlayerResource.Instantiate());
		AddChild(character);
		character.SyncState(playerData);

		entityDictionary.Add(playerData.id, character);
	}

	public void UpdateEntities(Dictionary<int, Array<Variant>> otherPlayers)
	{
		foreach (Array<Variant> data in otherPlayers.Values)
		{
			Player playerData = Player.From(data);
			UpdateEntity(playerData);
		}
	}

	public void UpdateEntity(Player playerData)
	{
		EntityController match = FindEntity(playerData.id);
			if (match != null)
			{
				match.SyncState(playerData);
			} else
			{
				SpawnPlayer(playerData);
			}
	}

	public void RemoveEntity(int id)
	{
		EntityController character = FindEntity(id);
		character?.QueueFree();
		entityDictionary.Remove(id);
	}

	public void RemoveEntity(Player playerData)
	{
		RemoveEntity(playerData.id);
	}

	public EntityController FindEntity(int id)
	{
		EntityController character = null;
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
		EntityController character = FindEntity(id);
		character.GlobalPosition = newPosition;
	}
}
