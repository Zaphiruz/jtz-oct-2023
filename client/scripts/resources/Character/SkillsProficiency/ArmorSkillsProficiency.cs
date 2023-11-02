using Godot;

public partial class ArmorSkillsProficiency : Proficiency
{
	ArmorSkillsProficiency()
	{
		Cloth = new SkillProficiency();
		Leather = new SkillProficiency();
		Mail = new SkillProficiency();
		Plate = new SkillProficiency();
	}

	[Export]
	SkillProficiency Cloth;

	[Export]
	SkillProficiency Leather;

	[Export]
	SkillProficiency Mail;

	[Export]
	SkillProficiency Plate;
}