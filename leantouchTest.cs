using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class leantouchTest : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		LeanTouch.OnFingerDown += OnFingerDown;
	}

	void OnDisable ()
	{
		LeanTouch.OnFingerDown -= OnFingerDown;
	}


	void OnFingerDown ( LeanFinger finger )
	{
		Debug.Log("finger " + finger.Index + " " + finger.GetStartWorldPosition(0));
	}
}
