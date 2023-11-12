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
	private PackedScene TriggerResource;
	private PackedScene RockGatherableResource;

	private Dictionary<int, EntityController> entityDictionary;
	private Dictionary<string, TriggerController> triggerDictionary;
	private Dictionary<string, GatherableController> gatherableDictionary;

	[Export]
	public string mapId;

	public override void _Ready()
	{
		server = Server.GetInstance(this, SpawnController.LABEL);
		sceneManager = SceneManager.GetInstance(this);

		PlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//Player.tscn");
		OtherPlayerResource = GD.Load<PackedScene>("res://scene-objects//Entities//OtherPlayer.tscn");
		EnemiesResource = GD.Load<PackedScene>("res://scene-objects//Entities//Enemy.tscn");
		TriggerResource = GD.Load<PackedScene>("res://scene-objects/Tilesets/TileTrigger.tscn");
		RockGatherableResource = GD.Load<PackedScene>("res://scene-objects/Static-Entities/RockGatherable.tscn");

		entityDictionary = new Dictionary<int, EntityController>();
		triggerDictionary = new Dictionary<string, TriggerController>();
		gatherableDictionary = new Dictionary<string, GatherableController>();

		RequestSpawn(mapId);
		RequestStaticEntities(mapId);
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
		GetNode<Node2D>("Players").AddChild(character);
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

	public void SpawnTrigger(StaticEntity entity)
	{
		GD.Print("Trigger Data ", entity);

		TriggerController trigger;
		if (triggerDictionary.ContainsKey(entity.id))
		{
			trigger = triggerDictionary[entity.id];
		} else
		{
			trigger = (TriggerController)TriggerResource.Instantiate();
			trigger.id = entity.id;
			trigger.Triggered += MapTriggerHit;
			triggerDictionary.Add(entity.id, trigger);
			GetNode<Node2D>("Triggers").AddChild(trigger);
		}

		trigger.GlobalPosition = entity.position;
	}

	public void SpawnResource(StaticEntity entity)
	{
		GD.Print("Resource Data ", entity);
		GatherableController gatherable;
		if (gatherableDictionary.ContainsKey(entity.id))
		{
			gatherable = gatherableDictionary[entity.id];
		}
		else
		{
			gatherable = (GatherableController)RockGatherableResource.Instantiate();
			gatherable.id = entity.id;
			gatherable.ActionTriggered += GatherableActionTriggered;
			gatherableDictionary.Add(entity.id, gatherable);
			GetNode<Node2D>("Resources").AddChild(gatherable);
		}

		gatherable.GlobalPosition = entity.position;
	}

	public void RequestStaticEntities(string mapId)
	{
		server.RequestStaticEntities(mapId);
	}

	public void UpdateStaticEntities(Array<StaticEntity> staticEntities)
	{
		foreach (StaticEntity entity in staticEntities)
		{
			switch(entity.type)
			{
				case SPAWNABLES.RESOURCE_ROCK:
					SpawnResource(entity);
					break;

				case SPAWNABLES.TRIGGER:
					SpawnTrigger(entity);
					break;

				default:
					continue;
			}
		}
	}

	public void GatherableActionTriggered(string gatherableId)
	{
		GD.Print("Gatherable Action", gatherableId);
	}
}
