using Godot;

public partial class JewelrySkillsProficiency : Proficiency
{
	JewelrySkillsProficiency()
	{
		Rings = new SkillProficiency();
		Necklaces = new SkillProficiency();
		Bracelets = new SkillProficiency();
	}

	[Export]
	SkillProficiency Rings;

	[Export]
	SkillProficiency Necklaces;

	[Export]
	SkillProficiency Bracelets;
}
