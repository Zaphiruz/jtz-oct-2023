using Godot;
using Godot.Collections;

public class InputCacher
{
	private Dictionary<StringName, bool> pressCache;

	public InputCacher()
	{
		pressCache = new Dictionary<StringName, bool>();
	}

	public void clearCache()
	{
		pressCache.Clear();
	}

	public bool checkInputPress(StringName action)
	{
		bool value;
		bool success = pressCache.TryGetValue(action, out value);
		if (success)
			return value;

		value = Input.IsActionPressed(action);
		pressCache.Add(action, value);

		return value;
	}
}
