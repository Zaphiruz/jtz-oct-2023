using Godot;
using Godot.Collections;

public partial class AnimatorController : Node2D
{
	[Export]
	private Texture2D texture;

	private Sprite2D sprite;
	private AnimationPlayer animationPlayer;

	[Export]
	private Dictionary<ENTITY_STATE, string> animationMap;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Texture = texture;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public void Play(ENTITY_STATE state)
	{
		if (state == ENTITY_STATE.NONE)
		{
			Stop();
			return;
		}

		string animation;
		bool hasAnimation = animationMap.TryGetValue(state, out animation);
		if (hasAnimation)
			animationPlayer.Play(animation);
	}

	public void Stop()
	{
		animationPlayer.Stop();
	}
}
