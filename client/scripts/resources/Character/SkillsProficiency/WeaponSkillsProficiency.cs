using Godot;

public partial class WeaponSkillsProficiency : Proficiency
{
	WeaponSkillsProficiency()
	{
		Fire = new SkillProficiency();
		Water = new SkillProficiency();
		Earth = new SkillProficiency();
		Air = new SkillProficiency();
		Neutral = new SkillProficiency();
	}

	[Export]
	SkillProficiency Swords;

	[Export]
	SkillProficiency Sabers;

	[Export]
	SkillProficiency Lances;

	[Export]
	SkillProficiency Axes;

	[Export]
	SkillProficiency Staves;

	[Export]
	SkillProficiency Wands;

	[Export]
	SkillProficiency Bows;

	[Export]
	SkillProficiency Staves;
}