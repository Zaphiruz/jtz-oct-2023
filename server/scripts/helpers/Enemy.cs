using Godot;
using Godot.Collections;

public partial class Enemy : DataCollapsable<Enemy>, IDataCollapsable<Enemy>
{
	public static Enemy From(Array<Variant> args)
	{
		return new Enemy().FromArgs(args);
	}

	public string id { get; set; }
	public Vector2 position { get; set; }
	public ENEMIES type { get; set; }

	public Enemy() { }

	public Enemy(string id, Vector2 position, ENEMIES type)
	{
		this.id = id;
		this.position = position;
		this.type = type;
	}

	public override Array<Variant> ToArgs()
	{
		Array<Variant> args = new Array<Variant>();
		args.Add(id);
		args.Add(position);
		args.Add((int)type);
		return args;
	}

	public override Enemy FromArgs(Array<Variant> args)
	{
		id = args[0].As<string>();
		position = args[1].As<Vector2>();
		type = (ENEMIES)args[2].As<int>();

		return this;
	}
}
