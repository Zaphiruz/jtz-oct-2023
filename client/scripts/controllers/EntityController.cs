using Godot;
using Godot.Collections;

public partial class EntityController : CharacterBody2D, ITriggerable, IAnimateable, INetworkSynced
{
	protected AnimatedSprite2D sprite;
	public MultiplayerSynchronizer multiplayerSynchronizer { get; set; }
	protected GameManager gameManager;
	public Area2D area2D { get; set; }
	public Dictionary<ENTITY_STATE, string> animationMap { get; set; }
	protected Dictionary<ENTITY_STATE, Vector2> transformMap;

	public const float speed = 250f;
	public const float lerpWeight = .5f;

	[Export]
	public ENTITY_STATE state = ENTITY_STATE.NONE;

	[Export]
	public Vector2 position;

	public override void _Ready()
	{
		base._Ready();

		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		multiplayerSynchronizer = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
		gameManager = GameManager.GetInstance(this);
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
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (!HasAuthority())
		{
			SyncState();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (HasAuthority())
		{
			ClientMove();
		}
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
		string animation;
		bool hasAnimation = animationMap.TryGetValue(state, out animation);
		if (!hasAnimation)
		{
			sprite.Stop();
		}
		else
		{
			sprite.Play(animation);
		}
	}

	public virtual void SyncState()
	{
		GlobalPosition = GlobalPosition.Lerp(position, .1f);
		Animate(state);
	}

	public virtual bool HasAuthority()
	{
		return multiplayerSynchronizer.GetMultiplayerAuthority() == gameManager.multiplayer.GetUniqueId();
	}

	public virtual void SetAuthority(int id, bool recursive = true)
	{
		multiplayerSynchronizer.SetMultiplayerAuthority(id, recursive);
	}

	public virtual void ClientMove() { }

	public virtual void OnAreaEntered(Area2D entity) { }
}
