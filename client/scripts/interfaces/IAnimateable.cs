using Godot;
using Godot.Collections;

public partial interface IAnimateable
{
	Dictionary<ENTITY_STATE, string> animationMap { get; set; }

	public void Animate(ENTITY_STATE state);
}
