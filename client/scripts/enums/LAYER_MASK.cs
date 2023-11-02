using System;

[Flags]
public enum LAYER_MASK
{
	PLAYERS = 1,
	OTHER_PLAYER = 2,
	MONSTERS = 4,
	WALLS = 8,
	TRIGGERS = 16,
}
