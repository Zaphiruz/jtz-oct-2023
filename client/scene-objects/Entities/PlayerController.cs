using Godot;
using Godot.Collections;
using System;
using Kryz.Tweening;

public partial class PlayerController : EntityController, IInstanceMappable
{
	public static string LABEL = "player";

	protected Dictionary<ENTITY_STATE, string> actionMap;
	protected SceneManager sceneManager;
	protected Server server;
	protected InputCacher inputCache;
	protected System.Collections.Generic.Stack<ENTITY_STATE> inputStack;

	protected bool isDashing = false;
	protected double lastDash = 0d;
	protected const float dashScalar = 1.3f;
	protected const float baseSpeed = 250f;
	protected const double dashDuration = 250;
	protected const double dashCooldown = 1000d;

	public override void _Ready()
	{
		base._Ready();

		sceneManager = SceneManager.GetInstance(this);
		server = Server.GetInstance(this, PlayerController.LABEL);

		actionMap = new Dictionary<ENTITY_STATE, string>()
		{
			{ ENTITY_STATE.UP, "UP" },
			{ ENTITY_STATE.LEFT, "LEFT" },
			{ ENTITY_STATE.DOWN, "DOWN" },
			{ ENTITY_STATE.RIGHT, "RIGHT" },
		};

		inputCache = new InputCacher();
		inputStack = new System.Collections.Generic.Stack<ENTITY_STATE>();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		ClientMove();
		inputCache.clearCache();

		server.UpdatePlayerPosition(GlobalPosition);
	}

	public override void ClientMove() {
		foreach (System.Collections.Generic.KeyValuePair<ENTITY_STATE, string> keyValuePair in actionMap)
		{
			if (Input.IsActionJustPressed(keyValuePair.Value))
			{
				inputStack.Push(keyValuePair.Key);
			}
		}

		float speed = TryDash();

		while(inputStack.Count > 0)
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
		// GD.Print("OnAreaEntered", entity.CollisionLayer);
		// if (entity.CollisionLayer == LAYER_MASK.MONSTERS)
		// {
		// 	
		// }
	}

	public float TryDash()
	{
		double now = DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
		if (!isDashing && lastDash + dashCooldown < now && Input.IsActionJustPressed("DASH"))
		{
			GD.Print("DASH");
			lastDash = now;
			isDashing = true;
			return baseSpeed;
		}
		
		if(isDashing && lastDash + dashDuration < now)
		{
			GD.Print("Slow...");
			isDashing = false;
			return baseSpeed;
		} else if (isDashing) {
			float scalar = EasingFunctions.InOutCubic((float) ((now - lastDash) / dashDuration));
			return baseSpeed * (dashScalar * scalar + 1);
		}

		return baseSpeed;
	}
}
