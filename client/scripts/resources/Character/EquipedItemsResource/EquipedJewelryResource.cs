using Godot;

public partial class EquipedJewelryResource : Resource
{
	EquipedJewelryResource()
	{
		Ring1 = new EquipedItemResource(ITEM_TYPE.JEWELRY_RING);
		Ring2 = new EquipedItemResource(ITEM_TYPE.JEWELRY_RING);
		Ring3 = new EquipedItemResource(ITEM_TYPE.JEWELRY_RING);
		Ring4 = new EquipedItemResource(ITEM_TYPE.JEWELRY_RING);
		Necklace1 = new EquipedItemResource(ITEM_TYPE.JEWELRY_NECKLACE);
		Bracelet1 = new EquipedItemResource(ITEM_TYPE.JEWELRY_BRACELET);
		Bracelet2 = new EquipedItemResource(ITEM_TYPE.JEWELRY_BRACELET);
	}

	[Export]
	EquipedItemResource Ring1;

	[Export]
	EquipedItemResource Ring2;

	[Export]
	EquipedItemResource Ring3;

	[Export]
	EquipedItemResource Ring4;

	[Export]
	EquipedItemResource Necklace1;

	[Export]
	EquipedItemResource Bracelet1;

	[Export]
	EquipedItemResource Bracelet2;
}