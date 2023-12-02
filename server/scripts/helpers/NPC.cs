using Godot;
using Godot.Collections;

public partial class NPC : DataCollapsable<NPC>, IDataCollapsable<NPC>
{
    public static NPC From(Array<Variant> args)
    {
        return new NPC().FromArgs(args);
    }

    public string id { get; set; }
    public Vector2 position { get; set; }
    public NPCS type { get; set; }

    public NPC() { }

    public NPC(string id, Vector2 position, NPCS type)
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

    public override NPC FromArgs(Array<Variant> args)
    {
        id = args[0].As<string>();
        position = args[1].As<Vector2>();
        type = (NPCS)args[2].As<int>();

        return this;
    }
}
