using Godot;
using System;
using Godot.Collections;
using System.Text;

public partial class CharacterManager : Node, IGlobalInterface<CharacterManager>
{
	public static string NodePath = "/root/CharacterManager";
	public static CharacterManager GetInstance(Node context) => context.GetNode<CharacterManager>(NodePath);

	private static string HOST_DEFAULT = "ws://localhost:3010/";
	private static string CONFIG_PATH = "res://data/AuthManager.config.json";

	public override void _Ready()
	{
		base._Ready();
		host = HOST_DEFAULT;
		if (ResourceLoader.Exists(CONFIG_PATH))
		{
			Json json = ResourceLoader.Load<Json>(CONFIG_PATH);
			host = (json.Data.As<Dictionary>())["CharacterHostWs"].AsString();
		}
		GD.Print("CharacterManager WS Host ", host);
	}
}
