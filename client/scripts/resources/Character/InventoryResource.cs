using Godot;
using System;
using Godot.Collections;

public partial class InventoryResource : Resource
{
	InventoryResource()
	{
		slots = new Array<Dictionary<int, int>>();
	}

	[Export]
	int maxSlots;

	[Export]
	Array<Dictionary<int, int>> slots;
}