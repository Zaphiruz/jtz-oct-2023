using Godot;
using Godot.Collections;
using System;


public partial class SceneManager : Node
{
	public enum SCENES
	{
		MULTIPLAYER_LOBBY,
		TEST_2_PLAYER
	}

	private Dictionary<SCENES, string> sceneDict;

	public SceneManager()
	{
		sceneDict = new Dictionary<SCENES, string>()
		{
			{ SCENES.MULTIPLAYER_LOBBY, "res://scenes//Multplayer.tscn" },
			{ SCENES.TEST_2_PLAYER, "res://scenes//Test_2Player.tscn" }
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
