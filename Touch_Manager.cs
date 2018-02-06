using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Touch_Manager : MonoBehaviour {

	[SerializeField] Judge judge;
	[SerializeField] Time_manager time_Manager;


	My_touch my_Touch = new My_touch();


	[SerializeField] debug_disp_info debug_Disp_Info;

	void OnEnable ()
	{
		LeanTouch.OnFingerDown += TouchBegan;
	}

	void OnDisable ()
	{
		LeanTouch.OnFingerDown -= TouchBegan;
		
	}


	void TouchBegan ( LeanFinger finger )
	{
		//Debug.Log("b " + LeanTouch.Fingers.Count);
		//for (int i = 0; i < LeanTouch.Fingers.Count; i++)
		//{
		//	Set_My_touch(time_Manager.Get_time() , finger);
		//}
		Set_My_touch(time_Manager.Get_time() , finger);
		judge.Main_judge(1 , my_Touch);
	}


	void Set_My_touch ( float touchTime , LeanFinger finger )
	{
		my_Touch.fingerID = finger.Index;
		my_Touch.touchTime = touchTime;
		my_Touch.touchPos = finger.GetStartWorldPosition(0);
		//Debug.Log("Set_My_touch " + my_Touch.touchPos);
	}


#if UNITY_EDITOR || UNITY_STANDALONE


	void Update ()
	{//ここにブレークポイントを張れる
	 //if (Input.GetKeyDown(KeyCode.Space))
	 //{
	 //	Time.timeScale = 0f;
	 //}
	 //キーボードでも遊べるように
		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
		{
			judge.Main_judge(1, time_Manager.Get_time());
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
		{
			judge.Main_judge(2 , time_Manager.Get_time());
		}
	}

#endif



	/// <summary>
	/// 独自のタッチ構造体
	/// </summary>
	public struct My_touch
	{
		public int fingerID;
		public float touchTime;
		public Vector2 touchPos;

		public My_touch ( int fingerID , float touchTime, Vector2 touchPos)
		{
			this.fingerID = fingerID;
			this.touchTime = touchTime;
			this.touchPos = touchPos;
		}


	}



}
