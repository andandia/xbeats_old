using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_manager : MonoBehaviour
{

	[SerializeField] AudioSource audioSource;


	/// <summary>
	/// 現在の時間
	/// </summary>
	public double now_time;


	public GameObject time_count_txt;//少数時間を表示する文字



	public float Get_time ()
	{
		//time_count_txt.GetComponent<UnityEngine.UI.Text>().text = audioSource.time.ToString();
		if (audioSource.isPlaying == true)
		{
			return audioSource.time;
		}
		return 0;
	}

}
	
