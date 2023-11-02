using Godot;

public partial class GatheringSkillProficiency : Resource
{
	GatheringSkillProficiency()
	{
		HerbGathering = new SkillProficiency();
		OreGathering = new SkillProficiency();
		JewelGathering = new SkillProficiency();
		WoodGathering = new SkillProficiency();
		FiberGathering = new SkillProficiency();
	}

	[Export]
	SkillProficiency HerbGathering;

	[Export]
	SkillProficiency OreGathering;

	[Export]
	SkillProficiency JewelGathering;

	[Export]
	SkillProficiency WoodGathering;

	[Export]
	SkillProficiency FiberGathering;
}