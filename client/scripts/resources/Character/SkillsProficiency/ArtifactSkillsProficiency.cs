using Godot;

public partial class ArtifactSkillsProficiency : Proficiency
{
	ArtifactSkillsProficiency()
	{
		Amulets = new SkillProficiency();
		Talismans = new SkillProficiency();
		Seals = new SkillProficiency();
	}

	[Export]
	SkillProficiency Amulets;

	[Export]
	SkillProficiency Talismans;

	[Export]
	SkillProficiency Seals;
}