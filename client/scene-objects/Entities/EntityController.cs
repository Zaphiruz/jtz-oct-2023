using Godot;
using Godot.Collections;

public partial class EntityController : CharacterBody2D, ITriggerable, IAnimateable
{
	protected AnimatorController animator;
	public Area2D area2D { get; set; }
	public Dictionary<ENTITY_STATE, string> animationMap { get; set; }
	protected Dictionary<ENTITY_STATE, Vector2> transformMap;

	public const float lerpWeight = .1f;

	[Export]
	public ENTITY_STATE state = ENTITY_STATE.NONE;

	[Export]
	public Vector2 position;

	[Export]
	public string Username;

	[Export]
	public int Id;

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
	}

	public virtual void Move(ENTITY_STATE state, float speed)
	{
		this.state = state;
		Vector2 delta = transformMap[state];
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

	public virtual void SyncState(Player player)
	{
		Id = player.id;
		nameplate.Text = Username = player.name;
		GlobalPosition = GlobalPosition.Lerp(player.position, lerpWeight);
		state = (ENTITY_STATE) player.state;
		Animate(state);
	}

	public virtual void ClientMove() { }

	public virtual void OnAreaEntered(Area2D entity) { }
}
