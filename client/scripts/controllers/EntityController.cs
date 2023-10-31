using Godot;
using Godot.Collections;

public partial class EntityController : CharacterBody2D, ITriggerable, IAnimateable
{
	protected AnimatorController animator;
	public Area2D area2D { get; set; }
	public Dictionary<ENTITY_STATE, string> animationMap { get; set; }
	protected Dictionary<ENTITY_STATE, Vector2> transformMap;

	public const float speed = 250f;
	public const float lerpWeight = .5f;

	[Export]
	public ENTITY_STATE state = ENTITY_STATE.NONE;

	[Export]
	public Vector2 position;

	protected Label nameplate;


	public override void _Ready()
	{
		base._Ready();

		animator = GetNode<AnimatorController>("AnimatorController");
		area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += OnAreaEntered;

		animationMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "Up" },
			{ ENTITY_STATE.LEFT, "Left" },
			{ ENTITY_STATE.DOWN, "Down" },
			{ ENTITY_STATE.RIGHT, "Right" },
		};

		transformMap = new Dictionary<ENTITY_STATE, Vector2>()
		{
			{ ENTITY_STATE.NONE, Vector2.Zero },
			{ ENTITY_STATE.UP, Vector2.Up },
			{ ENTITY_STATE.LEFT, Vector2.Left },
			{ ENTITY_STATE.DOWN, Vector2.Down },
			{ ENTITY_STATE.RIGHT, Vector2.Right },
		};

		position = GlobalPosition;

		nameplate = GetNode<Label>("Nameplate");
		nameplate.Text = GetMeta("Name").As<string>();
	}

	public virtual void Move(ENTITY_STATE state, float speed)
	{
		this.state = state;

		Vector2 delta = new Vector2();
		transformMap.TryGetValue(state, out delta);

		Velocity = (delta * speed);

		MoveAndSlide();

		position = GlobalPosition;

		Animate(state);
	}

	public virtual void Animate(ENTITY_STATE state)
	{
		if (state == ENTITY_STATE.NONE)
		{
			animator.Stop();
		}
		else
		{
			animator.Play(state);
		}
	}

	public virtual void SyncState(Vector2 position)
	{
		GlobalPosition = GlobalPosition.Lerp(position, .1f);
		state = DerriveState(position);
		Animate(state);
	}

	public virtual ENTITY_STATE DerriveState(Vector2 position)
	{
		Vector2 normal = position.Normalized();
		if (normal == Vector2.Up)
		{
			return ENTITY_STATE.UP;
		} else if (normal == Vector2.Right)
		{
			return ENTITY_STATE.RIGHT;
		} else if (normal == Vector2.Left)
		{
			return ENTITY_STATE.LEFT;
		} else if (normal == Vector2.Down)
		{
			return ENTITY_STATE.DOWN;
		} else if (normal ==  Vector2.Zero)
		{
			return ENTITY_STATE.NONE;
		}

		return ENTITY_STATE.NONE;
	}

	public virtual void ClientMove() { }

	public virtual void OnAreaEntered(Area2D entity) { }
}
