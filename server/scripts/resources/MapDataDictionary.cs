using Godot;
using Godot.Collections;
using System;

public partial class MapDataDictionary : Resource
{
	public MapDataDictionary()
	{
		MappedData = new Dictionary<string, MapData>();
	}

	[Export]
	public Dictionary<string, MapData> MappedData;
}
