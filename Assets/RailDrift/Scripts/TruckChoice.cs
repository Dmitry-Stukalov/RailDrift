using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TrackChoice
{
	public string Name;
	public List<TurnDirection> Directions;
	public List<TurnDirection> ResultDirections;
}
