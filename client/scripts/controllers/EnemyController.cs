using Godot;
using Godot.Collections;

public partial class EnemyController : EntityController
{
	public override void _Ready()
	{
		base._Ready();

		transformMap = new Dictionary<ENTITY_STATE, Vector2>()
		{
			{ ENTITY_STATE.NONE, Vector2.Zero },
			{ ENTITY_STATE.UP, Vector2.Up },
			{ ENTITY_STATE.LEFT, Vector2.Left },
			{ ENTITY_STATE.DOWN, Vector2.Down },
			{ ENTITY_STATE.RIGHT, Vector2.Right },
			{ ENTITY_STATE.DEAD, Vector2.Zero },
		};
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		SyncState();
	}
}
