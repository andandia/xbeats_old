using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBack : MonoBehaviour {

	[SerializeField] Time_manager time_Manager;

	GameObject Dc_OBJ;

	float first_touch_time;


	public void push_button ()
	{
		if (( time_Manager.Get_time() - first_touch_time ) > 0 &&  (time_Manager.Get_time() - first_touch_time) <= 1.0f )
		{
			Dc_OBJ = GameObject.FindGameObjectWithTag("Dc");
			Destroy(Dc_OBJ);
			UnityEngine.SceneManagement.SceneManager.LoadScene("Select_Music");
		}
		else
		{

			first_touch_time = time_Manager.Get_time();
		}
	}
}
