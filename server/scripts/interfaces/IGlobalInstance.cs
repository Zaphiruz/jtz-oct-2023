using Godot;

public partial interface IGlobalInterface<T> where T : Node
{
	public static string NodePath;
	public static T GetInstance(Node context)
	{
		return context.GetNode<T>(NodePath);
	}
}