using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {

	// Use this for initialization
	public void ReloadScene () {
		SceneManager.LoadScene(0);
	}
	
}
