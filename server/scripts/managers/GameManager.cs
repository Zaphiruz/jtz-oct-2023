using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node, IGlobalInterface<GameManager>
{
	public static string NodePath = "/root/GameManager";
	public static GameManager GetInstance(Node context) => context.GetNode<GameManager>(NodePath);

	private Dictionary<int, Player> players;

	public override void _Ready()
	{
		base._Ready();
		players = new Dictionary<int, Player>();
	}

	public bool removePlayer(int id)
	{
		return players.Remove(id);
	}

	public bool removePlayer(Player player)
	{
		return removePlayer(player.id);
	}

	public void addPlayer(Player player)
	{
		players.Add(player.id, player);
	}

	public void addPlayer(int id, string name)
	{
		addPlayer(new Player(id, name));
	}

	public void addPlayer(int id)
	{
		addPlayer(new Player(id));
	}

	public bool hasPlayer(int id)
	{
		return players.ContainsKey(id);
	}

	public bool hasPlayer(Player player)
	{
		return hasPlayer(player.id);
	}

	public int playerCount()
	{
		return players.Count;
	}

	public Dictionary<int, Player> duplicatePlayers()
	{
		return new Dictionary<int, Player>(players);
	}

	public void updatePlayer(int id, Vector2 position)
	{
		Player player;
		bool success = players.TryGetValue(id, out player);
		if (success)
		{
			player.position = position;
			players[id] = player;
		}
	}

	public void updatePlayer(int id, string name)
	{
		Player player;
		bool success = players.TryGetValue(id, out player);
		if (success)
		{
			player.name = name;
			players[id] = player;
		}
	}

	public void updatePlayer(int id, string name, Vector2 position)
	{
		Player player;
		bool success = players.TryGetValue(id, out player);
		if (success)
		{
			player.name = name;
			player.position = position;
			players[id] = player;
		}
	}

	public void updatePlayer(Player playerData)
	{
		Player player;
		bool success = players.TryGetValue(playerData.id, out player);
		if (success)
		{
			players[playerData.id] = playerData;
		}
	}

	public Player getPlayer(int id)
	{
		Player player;
		bool success = players.TryGetValue(id, out player);
		if (success)
		{
			return player;
		} else
		{
			return null;
		}
	}

	public void TeleportPlayer(int id, Vector2 position, double when)
	{
		Player player;
		bool success = players.TryGetValue(id, out player);
		if (success)
		{
			TeleportPlayer(player, position, when);
		}
	}

	public void TeleportPlayer(Player playerData, Vector2 position, double when)
	{
		
		playerData.position = position;
		playerData.lastTeleportTime = when;
		players[playerData.id] = playerData;
	}

	public System.Collections.Generic.ICollection<Player> getPlayers()
	{
		return players.Values;
	}
}
