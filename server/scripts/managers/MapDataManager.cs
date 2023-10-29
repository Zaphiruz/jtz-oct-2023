using Godot;
using Godot.Collections;
using System;
using System.Reflection;

public enum SPAWN_POINT_TYPE
{
	PLAYER,
	ENEMY,
	RESOURCE
}

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
					points = mapData.enemySpawnPoints;
					break;
				case SPAWN_POINT_TYPE.RESOURCE:
					points = mapData.resourceSpawnPoint;
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
}
