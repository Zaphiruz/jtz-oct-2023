using Godot;
using Godot.Collections;

public abstract partial class DataCollapsable<T> : GodotObject
{
	public abstract T FromArgs(Array<Variant> args);
	public abstract Array<Variant> ToArgs();
}

public partial interface IDataCollapsable<T> where T : DataCollapsable<T>, new()
{
	public static T From(Array<Variant> args)
	{
		return new T().FromArgs(args);
	}

	public T FromArgs(Array<Variant> args);
	public Array<Variant> ToArgs();
}
