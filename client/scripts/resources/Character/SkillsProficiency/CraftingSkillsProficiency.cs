using Godot;

public partial class CraftingSkillsProficiency : Resource
{
	CraftingSkillsProficiency()
	{
		Artifactsmith = new SkillProficiency();
		Blacksmith = new SkillProficiency();
		Woodsmith = new SkillProficiency();
		Jewelsmith = new SkillProficiency();
		Wordsmith = new SkillProficiency();
		Soulsmith = new SkillProficiency();
		Clothesmith = new SkillProficiency();
	}

	[Export]
	SkillProficiency Artifactsmith;

	[Export]
	SkillProficiency Blacksmith;

	[Export]
	SkillProficiency Woodsmith;

	[Export]
	SkillProficiency Jewelsmith;

	[Export]
	SkillProficiency Wordsmith;

	[Export]
	SkillProficiency Soulsmith;

	[Export]
	SkillProficiency Clothesmith;
}