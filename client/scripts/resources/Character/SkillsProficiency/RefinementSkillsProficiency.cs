using Godot;

public partial class RefinementSkillsProficiency : Proficiency
{
	RefinementSkillsProficiency()
	{
		HerbProcessing = new SkillProficiency();
		OreProcessing = new SkillProficiency();
		JewelProcessing = new SkillProficiency();
		WoodProcessing = new SkillProficiency();
		FiberProcessing = new SkillProficiency();
	}

	[Export]
	SkillProficiency HerbProcessing;

	[Export]
	SkillProficiency OreProcessing;

	[Export]
	SkillProficiency JewelProcessing;

	[Export]
	SkillProficiency WoodProcessing;

	[Export]
	SkillProficiency FiberProcessing;
}