using Godot;

public partial class Character : Resource
{
	Character()
	{
		Inventory = new InventoryResource();
		Skills = new SkillsProficiency();
		Stats = new StatsResource();
		EquipedItems = new EquipedItemsResource();
	}

	[Export]
	InventoryResource Inventory;

	[Export]
	SkillsProficiency Skills;

	[Export]
	StatsResource Stats;

	[Export]
	EquipedItemsResource EquipedItems;
}