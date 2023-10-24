using Godot;
using Godot.Collections;

public partial class OtherPlayerController : EntityController
{
	protected Dictionary<ENTITY_STATE, string> actionMap;
	public override void _Ready()
	{
		base._Ready();

		actionMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "UP" },
			{ ENTITY_STATE.LEFT, "LEFT" },
			{ ENTITY_STATE.DOWN, "DOWN" },
			{ ENTITY_STATE.RIGHT, "RIGHT" },
		};
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		SyncState();
	}
}
