using Godot;
using Godot.Collections;

public partial class Player : DataCollapsable<Player>, IDataCollapsable<Player>
{
	public static Player From(Array<Variant> args)
	{
		return new Player().FromArgs(args);
	}

	public int id { get; set; }
	public string name { get; set; }
	public Vector2 position { get; set; }
	public double lastTeleportTime { get; set; }
	public int state { get; set; } = -1; // ENTITY_STATE.NONE

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
	public Player(int id, string name, Vector2 position, int state)
	{
		this.id = id;
		this.name = name;
		this.position = position;
		this.state = state;
	}

	public override Array<Variant> ToArgs()
	{
		Array<Variant> args = new Array<Variant>();
		args.Add(id);
		args.Add(name);
		args.Add(position);
		args.Add(state);
		return args;
	}

	public override Player FromArgs(Array<Variant> args)
	{
		this.id = args[0].As<int>();
		this.name = args[1].As<string>();
		this.position = args[2].As<Vector2>();
		this.state = args[3].As<int>();

		return this;
	}
}
