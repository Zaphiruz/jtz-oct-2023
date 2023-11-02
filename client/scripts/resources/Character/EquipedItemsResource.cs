using Godot;

public partial class EquipedItemsResource : Resource
{
	EquipedItemsResource()
	{
		Jewelry = new EquipedJewelryResource();
		Artifacts = new EquipedArtifactsResource();
		Armor = new EquipedArmorResource();
		Weapons = new EquipedWeaponsResource();
	}

	[Export]
	EquipedJewelryResource Jewelry;

	[Export]
	EquipedArtifactsResource Artifacts;

	[Export]
	EquipedArmorResource Armor;

	[Export]
	EquipedWeaponsResource Weapons;
}