using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_controller : MonoBehaviour {

	[SerializeField] Time_manager Time_manager;
	[SerializeField] Data_cabinet Data_cabinet;
	[SerializeField] Music_load music_Load;


	[SerializeField] double pull_time = 1.0;

	
	//後々Startにするべき
	public void Starter () {
		//Time_manager.Start_stopwatch();

		music_Load.Load_music();
	}




}
