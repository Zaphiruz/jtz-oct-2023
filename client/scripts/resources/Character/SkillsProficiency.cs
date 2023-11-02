using Godot;

public partial class SkillsProficiency : Resource
{
	SkillsProficiency()
	{
		CraftingSkills = new CraftingSkillsProficiency();
		GatheringSkills = new GatheringSkillProficiency();
		RefinementSkills = new RefinementSkillsProficiency();
		ElementalSkills = new ElementalSkillsProficiency();
		WeaponSkills = new WeaponSkillsProficiency();
		ArmorSkills = new ArmorSkillsProficiency();
		ArtifactSkills = new ArtifactSkillsProficiency();
		JewelrySkills = new JewelrySkillsProficiency();
	}

	[Export]
	CraftingSkillsProficiency CraftingSkills;

	[Export]
	GatheringSkillProficiency GatheringSkills;

	[Export]
	RefinementSkillsProficiency RefinementSkills;

	[Export]
	ElementalSkillsProficiency ElementalSkills;
	
	[Export]
	WeaponSkillsProficiency WeaponSkills;

	[Export]
	ArmorSkillsProficiency ArmorSkills;

	[Export]
	ArtifactSkillsProficiency ArtifactSkills;

	[Export]
	JewelrySkillsProficiency JewelrySkills;
}