using Godot;

public partial class ElementalSkillsProficiency : Proficiency
{
	ElementalSkillsProficiency()
	{
		Fire = new SkillProficiency();
		Water = new SkillProficiency();
		Earth = new SkillProficiency();
		Air = new SkillProficiency();
		Neutral = new SkillProficiency();
	}

	[Export]
	SkillProficiency Fire;

	[Export]
	SkillProficiency Water;

	[Export]
	SkillProficiency Earth;

	[Export]
	SkillProficiency Air;

	[Export]
	SkillProficiency Neutral;
}