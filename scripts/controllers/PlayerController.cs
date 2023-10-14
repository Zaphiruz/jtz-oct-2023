using Godot;
using Godot.Collections;

public partial class PlayerController : EntityController
{
	protected Dictionary<ENTITY_STATE, string> actionMap;
	protected SceneManager sceneManager;

	public override void _Ready()
	{
		base._Ready();

		sceneManager = GetNode<SceneManager>("/root/SceneManager");

		actionMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "UP" },
			{ ENTITY_STATE.LEFT, "LEFT" },
			{ ENTITY_STATE.DOWN, "DOWN" },
			{ ENTITY_STATE.RIGHT, "RIGHT" },
		};

		multiplayerSynchronizer.SetMultiplayerAuthority((int) GetMeta("ID"));
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (HasAuthority())
		{
			ClientMove();
		} else
		{
			SyncState();
		}
	}

	public override void ClientMove() {
		ENTITY_STATE stateTo = ENTITY_STATE.NONE;
		string currentInput;
		bool foundAction = actionMap.TryGetValue(state, out currentInput);
		if (foundAction && Input.IsActionPressed(currentInput))
		{
			stateTo = state;
		}


		foreach (System.Collections.Generic.KeyValuePair<ENTITY_STATE, string> keyValuePair in actionMap)
		{
			if (Input.IsActionJustPressed(keyValuePair.Value))
			{
				Move(keyValuePair.Key, speed);
				return;
			}
		}

		Move(stateTo, speed);
	}

	public override void OnAreaEntered(Area2D entity)
	{
		if (HasAuthority())
		{
			if (entity.CollisionLayer == 2)
			{
				Rpc(new StringName(nameof(StartBattle)));
			}
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	public void StartBattle()
	{
		GD.Print("Starting Battle!");
		sceneManager.ShowScene(SceneManager.SCENES.TEST_BATTLE, this);
	}
}
