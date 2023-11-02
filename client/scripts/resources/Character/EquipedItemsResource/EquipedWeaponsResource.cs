using Godot;

public partial class EquipedWeaponsResource : Resource
{
	EquipedWeaponsResource()
	{
		Weapon1 = new EquipedItemResource(ITEM_TYPE.WEAPON_1H);
		Weapon2 = new EquipedItemResource(ITEM_TYPE.WEAPON_1H);
	}

	[Export]
	EquipedItemResource Weapon1;

	[Export]
	EquipedItemResource Weapon2;
}