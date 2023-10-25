using Godot;
using Godot.Collections;

public partial class Player : GodotObject
{
	public int id { get; set; }
	public string name { get; set; }
	public Vector2 position { get; set; }

	public Player() { }

	public Player(int id)
	{
		this.id = id;
	}

	public Player(int id, string name)
	{
		this.id = id;
		this.name = name;
	}

	public Player(int id, string name, Vector2 position)
	{
		this.id = id;
		this.name = name;
		this.position = position;
	}
}
