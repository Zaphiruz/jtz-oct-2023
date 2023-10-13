using Godot;
using Godot.Collections;

public partial class PlayerController : CharacterBody2D
{
	public const float speed = 30.0f;
	private AnimatedSprite2D sprite;
	private DIRECTIONS direction = DIRECTIONS.NONE;

	private Dictionary<DIRECTIONS, string> actionMap;
	private Dictionary<DIRECTIONS, string> animationMap;
	private Dictionary<DIRECTIONS, Vector2> transformMap;

	public override void _Ready()
	{
		base._Ready();
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		actionMap = new Dictionary<DIRECTIONS, string>()
		{
			{ DIRECTIONS.UP, "UP" },
			{ DIRECTIONS.LEFT, "LEFT" },
			{ DIRECTIONS.DOWN, "DOWN" },
			{ DIRECTIONS.RIGHT, "RIGHT" },
		};

		animationMap = new Dictionary<DIRECTIONS, string>()
		{
			{ DIRECTIONS.UP, "Up" },
			{ DIRECTIONS.LEFT, "Left" },
			{ DIRECTIONS.DOWN, "Down" },
			{ DIRECTIONS.RIGHT, "Right" },
		};

		transformMap = new Dictionary<DIRECTIONS, Vector2>()
		{
			{ DIRECTIONS.NONE, Vector2.Zero },
			{ DIRECTIONS.UP, Vector2.Up },
			{ DIRECTIONS.LEFT, Vector2.Left },
			{ DIRECTIONS.DOWN, Vector2.Down },
			{ DIRECTIONS.RIGHT, Vector2.Right },
		};
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		DIRECTIONS moveTo = DIRECTIONS.NONE;
		string currentInput;
		bool foundAction = actionMap.TryGetValue(direction, out currentInput);
		if (foundAction && Input.IsActionPressed(currentInput))
		{
			moveTo = direction;
		}
		

		foreach (System.Collections.Generic.KeyValuePair<DIRECTIONS, string> keyValuePair in actionMap)
		{
			if (Input.IsActionJustPressed(keyValuePair.Value))
			{
				Move(keyValuePair.Key, speed);
				return;
			}
		}

		Move(moveTo, speed);
		return;
	}

	public void Move(DIRECTIONS direction, float speed)
	{
		this.direction = direction;

		Vector2 delta = new Vector2();
		transformMap.TryGetValue(direction, out delta);
		Translate(delta);

		Animate(direction);
	}

	public void Animate(DIRECTIONS direction)
	{
		string animation;
		bool hasAnimation = animationMap.TryGetValue(direction, out animation);
		if (!hasAnimation)
		{
			sprite.Stop();
		}
		else
		{
			sprite.Play(animation);
		}
	}
}
