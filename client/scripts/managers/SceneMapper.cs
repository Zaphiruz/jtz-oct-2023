using Godot;
using Godot.Collections;

public partial class SceneData : GodotObject
{
	public ulong instanceId;
	public string name;

	public SceneData(ulong instanceId, string name)
	{
		this.instanceId = instanceId;
		this.name = name;
	}
}

public partial class SceneMapper : Node, IGlobalInterface<SceneMapper>
{
	public static string NodePath = "/root/SceneMapper";
	public static SceneMapper GetInstance(Node context) => context.GetNode<SceneMapper>(NodePath);
	public static SceneMapper GetInstance(Node context, string Name)
	{
		SceneMapper sceneMapper = context.GetNode<SceneMapper>(NodePath);
		sceneMapper.RegisterScene(context, Name);
		return sceneMapper;
	}

	private Dictionary<string, SceneData> SceneMap;


	public override void _Ready()
	{
		SceneMap = new Dictionary<string, SceneData>();
	}

	public void RegisterScene(Node context, string Name)
	{
		if (!SceneMap.ContainsKey(Name)) SceneMap.Add(Name, new SceneData(context.GetInstanceId(), Name));
	}

	public T getInstanceOf<T>(string Name) where T : Node
	{
		SceneData data;
		if (SceneMap.TryGetValue(Name, out data))
		{
			return ((T)InstanceFromId(data.instanceId));
		}
		else
		{
			return null;
		}
	}
}
