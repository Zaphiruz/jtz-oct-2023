using Godot;
using Godot.Collections;

public partial class GameManager : Node
{
	private Dictionary<long, Player> players;

	public GameManager()
	{
		players = new Dictionary<long, Player>();
	}

	public bool removePlayer(long id)
	{
		return players.Remove(id);
	}

	public void addPlayer(Player player)
	{
		players.Add(player.id, player);
	}

	public void addPlayer(long id, string name)
	{
		addPlayer(new Player(id, name));
	}

	public bool hasPlayer(long id)
	{
		return players.ContainsKey(id);
	}

	public System.Collections.Generic.ICollection<Player> getPlayers()
	{
		return players.Values;
	}
}

public partial class Player : GodotObject
{
	public long id { get; set; }
	public string name { get; set; }

	public Player() { }

	public Player(long id, string name)
	{
		this.id = id;
		this.name = name;
	}
}
