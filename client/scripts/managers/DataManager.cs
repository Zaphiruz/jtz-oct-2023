using Godot;

public partial class DataManager : Node, IGlobalInterface<DataManager>
{
	public static string NodePath = "/root/DataManager";
	public static DataManager GetInstance(Node context) => context.GetNode<DataManager>(NodePath);

	public const string CONFIG_FILENAME = "user://config.tres";

	private Config playerData;

	public override void _Ready()
	{
		base._Ready();

		Config saved = LoadConfigFromDisk();
		if (saved != null)
		{
			playerData = saved;
		}
		else
		{
			playerData = new Config();

		}
	}

	public void SaveConfigToDisk()
	{
		ResourceSaver.Save(playerData, CONFIG_FILENAME);
	}

	public Config LoadConfigFromDisk()
	{
		if (ResourceLoader.Exists(CONFIG_FILENAME)) {
			return ResourceLoader.Load<Config>(CONFIG_FILENAME);
		} else
		{
			return null;
		}
	}

	public T GetConfigValue<T>(string key)
	{
		return (T) typeof(Config).GetField(key).GetValue(playerData);
	}

	public void SetConfigValue<T>(string key, T value)
	{
		typeof(Config).GetField(key).SetValue(playerData, value);
	}
}