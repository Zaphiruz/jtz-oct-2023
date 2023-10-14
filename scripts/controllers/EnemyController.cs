using Godot;
using Godot.Collections;

public partial class EnemyController : EntityController
{
	

	public override void _Ready()
	{
		base._Ready();

		animationMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "Up" },
			{ ENTITY_STATE.LEFT, "Left" },
			{ ENTITY_STATE.DOWN, "Down" },
			{ ENTITY_STATE.RIGHT, "Right" },
			{ ENTITY_STATE.DEAD, "Die" },
		};

		transformMap = new Dictionary<ENTITY_STATE, Vector2>()
		{
			{ ENTITY_STATE.NONE, Vector2.Zero },
			{ ENTITY_STATE.UP, Vector2.Up },
			{ ENTITY_STATE.LEFT, Vector2.Left },
			{ ENTITY_STATE.DOWN, Vector2.Down },
			{ ENTITY_STATE.RIGHT, Vector2.Right },
			{ ENTITY_STATE.DEAD, Vector2.Zero },
		};

		//multiplayerSynchronizer.SetMultiplayerAuthority(1);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (HasAuthority())
		{
			ClientMove();
		}
		else
		{
			// SyncState();
		}
	}

	public override bool HasAuthority()
	{
		return gameManager.multiplayer.IsServer();
	}
}
