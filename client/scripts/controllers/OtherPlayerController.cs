using Godot;
using Godot.Collections;

public partial class OtherPlayerController : EntityController
{
	protected Dictionary<ENTITY_STATE, string> actionMap;
	protected SceneManager sceneManager;
	protected InputCacher inputCache;
	protected System.Collections.Generic.Stack<ENTITY_STATE> inputStack;
	public override void _Ready()
	{
		base._Ready();

		sceneManager = SceneManager.GetInstance(this);

		actionMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "UP" },
			{ ENTITY_STATE.LEFT, "LEFT" },
			{ ENTITY_STATE.DOWN, "DOWN" },
			{ ENTITY_STATE.RIGHT, "RIGHT" },
		};

		inputCache = new InputCacher();
		inputStack = new System.Collections.Generic.Stack<ENTITY_STATE>();

		SetAuthority((int)GetMeta("ID"));
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		inputCache.clearCache();
	}

	public override void ClientMove()
	{
		foreach (System.Collections.Generic.KeyValuePair<ENTITY_STATE, string> keyValuePair in actionMap)
		{
			if (Input.IsActionJustPressed(keyValuePair.Value))
			{
				inputStack.Push(keyValuePair.Key);
			}
		}

		while (inputStack.Count > 0)
		{
			ENTITY_STATE stateTo = inputStack.Peek();

			string currentInput;
			bool foundAction = actionMap.TryGetValue(stateTo, out currentInput);
			if (foundAction && inputCache.checkInputPress(currentInput))
			{
				Move(stateTo, speed);
				return;
			}

			inputStack.Pop();
		}

		Move(ENTITY_STATE.NONE, speed);
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
