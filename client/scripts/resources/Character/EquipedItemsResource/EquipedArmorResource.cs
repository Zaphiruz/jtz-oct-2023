using Godot;

public partial class EquipedArmorResource : Resource
{
	EquipedArmorResource()
	{
		Helmet1 = new EquipedItemResource(ITEM_TYPE.ARMOR_HELMET);
		UpperBody1 = new EquipedItemResource(ITEM_TYPE.ARMOR_UPPER_BODY);
		LowerBody1 = new EquipedItemResource(ITEM_TYPE.ARMOR_LOWER_BODY);
		Boots1 = new EquipedItemResource(ITEM_TYPE.ARMOR_BOOTS);
		Gloves1 = new EquipedItemResource(ITEM_TYPE.ARMOR_GLOVES);
		Bracers1 = new EquipedItemResource(ITEM_TYPE.ARMOR_BRACERS);
	}

	[Export]
	EquipedItemResource Helmet1;

	[Export]
	EquipedItemResource UpperBody1;

	[Export]
	EquipedItemResource LowerBody1;

	[Export]
	EquipedItemResource Boots1;

	[Export]
	EquipedItemResource Gloves1;

	[Export]
	EquipedItemResource Bracers1;
}