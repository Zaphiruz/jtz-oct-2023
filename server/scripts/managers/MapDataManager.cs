using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Reflection;


public partial class MapDataManager : Node, IGlobalInterface<GameManager>
{
	public static string NodePath = "/root/MapDataManager";
	public static MapDataManager GetInstance(Node context) => context.GetNode<MapDataManager>(NodePath);

	private const string MAP_DATA = "res://data//mapData.tres";

	private MapDataDictionary mapDataDict;
	private Random random;

	public override void _Ready()
	{
		base._Ready();

		random = new Random();

		if (ResourceLoader.Exists(MAP_DATA))
		{
			mapDataDict = ResourceLoader.Load<MapDataDictionary>(MAP_DATA);
		} else
		{
			mapDataDict = new MapDataDictionary();
			ResourceSaver.Save(mapDataDict, MAP_DATA);
		}
	}

	public Vector2 GetMapSpawnLocation(string mapId, SPAWN_POINT_TYPE type)
	{
		MapData mapData;
		bool success = mapDataDict.MappedData.TryGetValue(mapId, out mapData);
		if (success)
		{
			Array<Vector2> points;

			switch (type)
			{
				case SPAWN_POINT_TYPE.PLAYER:
					points = mapData.playerSpawns;
					break;
				case SPAWN_POINT_TYPE.ENEMY:
					points = mapData.enemySpawnPoints[0];	// FIX THIS!
					break;
				default:
					points = new Array<Vector2>();
					break;
			}
			int index = random.Next(points.Count);
			return points[index];
		} else
		{
			mapDataDict.MappedData.Add(mapId, new MapData());
			ResourceSaver.Save(mapDataDict, MAP_DATA);
		}

		return default(Vector2);
	}

	public Array<Array<Variant>> GetEnemies(string mapId)
	{
		Array<Array<Variant>> ret = new Array<Array<Variant>>();

		MapData mapData;
		bool success = mapDataDict.MappedData.TryGetValue(mapId, out mapData);
		if (success)
		{
			foreach (System.Collections.Generic.KeyValuePair<int, Array<Vector2>> data in mapData.enemySpawnPoints)
			{
				foreach(Vector2 point in data.Value)
				{
					Enemy thing = new Enemy();
					thing.id = data.Key.ToString(); // Get unique ID
					thing.position = point;
					thing.type = (ENEMIES)data.Key;

					ret.Add(thing.ToArgs());
				}
			}
		}

		return ret;
	}

	public Array<Array<Variant>> GetResources(string mapId)
	{
		Array<Array<Variant>> ret = new Array<Array<Variant>>();

		MapData mapData;
		bool success = mapDataDict.MappedData.TryGetValue(mapId, out mapData);
		if (success)
		{
			foreach(System.Collections.Generic.KeyValuePair<string, Vector2> data in mapData.resourceSpawnPoints)
			{
				StaticEntity thing = new StaticEntity();
				thing.id = data.Key;
				thing.position = data.Value;
				thing.type = SPAWNABLES.RESOURCE_ROCK;

				ret.Add(thing.ToArgs());
			}
		}

		return ret;
	}

	public Array<Array<Variant>> GetTriggers(string mapId)
	{
		Array<Array<Variant>> ret = new Array<Array<Variant>>();

		MapData mapData;
		bool success = mapDataDict.MappedData.TryGetValue(mapId, out mapData);
		if (success)
		{
			foreach(System.Collections.Generic.KeyValuePair<string, Array<Vector2>> data in mapData.triggers)
			{
				StaticEntity thing = new StaticEntity();
				thing.id = data.Key;
				thing.position = data.Value[0];
				thing.type = SPAWNABLES.TRIGGER;

				ret.Add(thing.ToArgs());
			}
		}
		return ret;
	}

	public Vector2 ValidateTrigger(string mapId, string triggerId, Player player, double when)
	{
		if (player == null)
		{
			return Vector2.Zero;
		}
		
		MapData mapData = mapDataDict.MappedData[mapId];
		Array<Vector2> trigggerData = mapData.triggers[triggerId];
		Vector2 validationPos = trigggerData[0];
		Vector2 destinationPos = trigggerData[1];

		float distance = validationPos.DistanceTo(player.position);
		double delta = when - player.lastTeleportTime;
		GD.Print("ValidateTrigger", distance, delta);
		if (distance < 32f && delta > 1000) // arbitrary, 32 and 1 second(s)
		{
			return destinationPos;
		} else
		{
			return Vector2.Zero;
		}
	}
}
