using Godot;
using Godot.Collections;
using System;


public partial class SceneManager : Node, IGlobalInterface<SceneManager>
{
	public static string NodePath = "/root/SceneManager";
	public static SceneManager GetInstance(Node context) => context.GetNode<SceneManager>(NodePath);

	public enum SCENES
	{
		START_GAME,
		MULTIPLAYER_LOBBY,
		TEST_2_PLAYER,
		TEST_BATTLE
	}

	private Dictionary<SCENES, string> sceneDict;

	public override void _Ready()
	{
		base._Ready();

		sceneDict = new Dictionary<SCENES, string>()
		{
			{ SCENES.START_GAME, "res://scenes//StartGame.tscn" },
			{ SCENES.MULTIPLAYER_LOBBY, "res://scenes//Multiplayer.tscn" },
			{ SCENES.TEST_2_PLAYER, "res://scenes//Test_2Player.tscn" },
			{ SCENES.TEST_BATTLE, "res://scenes//Test_Battle.tscn" },

		};
	}

	public Node LoadScene(SCENES scene)
	{
		
		string scenePath;
		bool keyInDict = sceneDict.TryGetValue(scene, out scenePath);

		if (!keyInDict)
		{
			throw new Exception("NO KEY IN SCENE DICT");
		}

		return GD.Load<PackedScene>(scenePath).Instantiate();
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
