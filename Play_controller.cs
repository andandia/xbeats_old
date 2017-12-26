using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_controller : MonoBehaviour {

	[SerializeField] Time_manager Time_manager;
	[SerializeField] Data_cabinet Data_cabinet;


	[SerializeField] double pull_time = 1.0;

	// Use this for initialization
	//後々Startにするべき
	public void Starter () {
		Time_manager.Start_stopwatch();
	}




	//仕方なくここでやってるけどもっと適切なクラスがあればそこでやるべき
	void Time_adjust()
	{
		double time = Data_cabinet.Note_data_list[0].startTime - pull_time;
		Time_manager.Set_adjustTime(time);
	}
}
