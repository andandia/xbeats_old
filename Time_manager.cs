using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_manager : MonoBehaviour
{
	
	/// <summary>
	/// 現在の時間
	/// </summary>
	public double now_time;

	
	
	public GameObject time_count_txt;//少数時間を表示する文字



	System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

	//void Start()
	//{
	//	Start_stopwatch();
	//}

	// Update is called once per frame
	void Update()
	{
		//now_time = stopwatch.Elapsed.TotalSeconds;
		now_time += Time.deltaTime;
		time_count_txt.GetComponent<UnityEngine.UI.Text>().text = now_time.ToString();
	}


	public double Get_time()
	{
		return now_time;
	}



	public void Set_adjustTime(double Time)
	{
		now_time = Time;
	}



	public void Start_stopwatch()
	{
		stopwatch.Start();
	}



	public void Stop_stopwatch()
	{
		stopwatch.Start();
	}
}
