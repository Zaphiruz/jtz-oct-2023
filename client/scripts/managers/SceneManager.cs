using Godot;
using Godot.Collections;
using System;


public partial class SceneManager : Node, IGlobalInterface<SceneManager>
{
	public static string NodePath = "/root/SceneManager";
	public static SceneManager GetInstance(Node context) => context.GetNode<SceneManager>(NodePath);

	private SpawnManager spawnManager;

	public enum SCENES
	{
		START_GAME,
		MULTIPLAYER_LOBBY,
		GAME
	}

	private Dictionary<SCENES, string> sceneDict = new Dictionary<SCENES, string>()
	{
		{ SCENES.START_GAME, "res://scenes//StartGame.tscn" },
		{ SCENES.MULTIPLAYER_LOBBY, "res://scenes//Multiplayer.tscn" },
		{ SCENES.GAME, "res://scenes/game-maps/" }
	};

	public override void _Ready()
	{
		base._Ready();

		spawnManager = SpawnManager.GetInstance(this);
	}

	public Node LoadScene(SCENES scene, string mapId)
	{
		if (scene != SCENES.GAME)
		{
			return LoadScene(scene);
		}

		string scenePath = $"{sceneDict[scene]}{mapId}.tscn";

		Node activeMap = GD.Load<PackedScene>(scenePath).Instantiate();
		spawnManager.setActiveMap(activeMap, mapId);

		return activeMap;
	}

	public Node LoadScene(SCENES scene)
	{
		if (scene == SCENES.GAME)
		{
			throw new Exception("Game scene needs mapId");
		}
		
		string scenePath = sceneDict[scene];

		return GD.Load<PackedScene>(scenePath).Instantiate();
	}

	public void ShowScene(SCENES scene, Node context, string mapId)
	{
		ShowScene(LoadScene(scene, mapId), context);
	}

	public void ShowScene(SCENES scene, Node context)
	{
		ShowScene(LoadScene(scene), context);
	}

	public void ShowScene(Node scene, Node context)
	{
		AddChild(scene);
		context.QueueFree();
	}
}
