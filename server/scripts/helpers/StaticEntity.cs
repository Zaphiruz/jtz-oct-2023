using Godot;
using Godot.Collections;

public partial class StaticEntity : DataCollapsable<StaticEntity>, IDataCollapsable<StaticEntity>
{
	public static StaticEntity From(Array<Variant> args)
	{
		return new StaticEntity().FromArgs(args);
	}

	public string id { get; set; }
	public Vector2 position { get; set; }
	public SPAWNABLES type { get; set; }

	public StaticEntity() { }

	public StaticEntity(string id, Vector2 position, SPAWNABLES type)
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
		args.Add((int) type);
		return args;
	}

	public override StaticEntity FromArgs(Array<Variant> args)
	{
		id = args[0].As<string>();
		position = args[1].As<Vector2>();
		type = (SPAWNABLES) args[2].As<int>();

		return this;
	}
}
