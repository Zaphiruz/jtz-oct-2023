using Godot;

public partial class EquipedItemResource : Resource
{
	EquipedItemResource() { }
	EquipedItemResource(ITEM_TYPE ItemType)
	{
		this.ItemType = ItemType;
	}

	[Export]
	int ItemId;

	[Export]
	ITEM_TYPE ItemType;
}