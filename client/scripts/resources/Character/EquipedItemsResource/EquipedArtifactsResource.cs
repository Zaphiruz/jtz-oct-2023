using Godot;

public partial class EquipedArtifactsResource : Resource
{
	EquipedArtifactsResource()
	{
		Seal1 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_SEAL);
		Seal2 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_SEAL);
		Talisman1 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_TALISMAN);
		Talisman2 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_TALISMAN);
		Amulet1 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_AMULET);
		Amulet2 = new EquipedItemResource(ITEM_TYPE.ARTIFACT_AMULET);
	}

	[Export]
	EquipedItemResource Seal1;

	[Export]
	EquipedItemResource Seal2;

	[Export]
	EquipedItemResource Talisman1;

	[Export]
	EquipedItemResource Talisman2;

	[Export]
	EquipedItemResource Amulet1;

	[Export]
	EquipedItemResource Amulet2;
}