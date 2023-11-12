using Godot;
using Godot.Collections;
using System;

public partial class MapData : Resource
{
	public MapData()
	{
		playerSpawns = new Array<Vector2>();
		enemySpawnPoints = new Array<Vector2>();
		resourceSpawnPoints = new Dictionary<string, Vector2>();
		connectedMaps = new Array<string>();
		triggers = new Dictionary<string, Array<Vector2>>();
	}

	[Export]
	public string mapName;

	[Export]
	public Array<Vector2> playerSpawns;

	[Export]
	public int enemySpawnRate;
	[Export]
	public Array<Vector2> enemySpawnPoints;

	[Export]
	public int resourceSpawnRate;

	[Export]
	public Dictionary<string, Vector2> resourceSpawnPoints;

	[Export]
	public Array<string> connectedMaps;

	[Export]
	public Dictionary<string, Array<Vector2>> triggers;
}
