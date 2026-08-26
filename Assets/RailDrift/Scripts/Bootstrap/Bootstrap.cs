using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private Train _train;
	[SerializeField] private Track _track;
	[SerializeField] private RailWayGenerator _railWayGenerator;

	private void Start()
	{
		_track.Initializing();
		_train.Initializing();
		_railWayGenerator.Initializing();
	}
}
