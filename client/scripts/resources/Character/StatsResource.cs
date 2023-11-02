using Godot;

public partial class StatsResource : Resource
{
	StatsResource()
	{
		Constitution = new StatResource();
		Strength = new StatResource();
		Dexterity = new StatResource();
		Intelligence = new StatResource();
		Wisdom = new StatResource();
		Charisma = new StatResource();
		Luck = new StatResource();
	}

	[Export]
	StatResource Constitution;

	[Export]
	StatResource Strength;

	[Export]
	StatResource Dexterity;

	[Export]
	StatResource Intelligence;

	[Export]
	StatResource Wisdom;

	[Export]
	StatResource Charisma;

	[Export]
	StatResource Luck;
}