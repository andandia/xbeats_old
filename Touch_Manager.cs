using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class Touch_Manager : MonoBehaviour
{

	[SerializeField] Judge judge;
	[SerializeField] Time_manager time_Manager;
	GameObject Dc_OBJ;
	Data_cabinet Dc;


	My_touch[] my_Touch = new My_touch[3];
	int my_touch_max_count = 3;


	//[SerializeField]
	float Swipe_distance;


	[SerializeField] debug_disp_info debug_Disp_Info;

	void OnEnable ()
	{
		LeanTouch.OnFingerDown += OnFingerDown;
		LeanTouch.OnFingerSet += OnFingerSet;
		LeanTouch.OnFingerUp += OnFingerUp;
		Dc_OBJ = GameObject.FindGameObjectWithTag("Dc");
		Dc = Dc_OBJ.GetComponent<Data_cabinet>();
#if UNITY_EDITOR
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Z_PlayONLY")
		{//Z_PlayONLYはSettingsを読んでいないのでこの形
			Swipe_distance = 0.1f;
			Debug.Log("Swipe_distance " + Swipe_distance);
		}
#endif
	}

	public void Set_swipe_distance (float distance )
	{
		Swipe_distance = distance;
		if (Swipe_distance == 0)
		{
			Swipe_distance = 100f;//0ということは恐らく読めてないということなのでおかしくする
		}
	}




	void OnDisable ()
	{
		LeanTouch.OnFingerDown -= OnFingerDown;
		LeanTouch.OnFingerSet -= OnFingerSet;
		LeanTouch.OnFingerUp -= OnFingerUp;
	}

	/*----------------------------------------------------------------*/
	/*----------------------------------------------------------------*/

	void OnFingerDown ( LeanFinger finger )
	{
		int mytouch_index = -1; //-1のままならおかしい
		for (int i = 0; i < LeanTouch.Fingers.Count; i++)//同時押しでも指を見分けるため
		{
			if (finger.Index == LeanTouch.Fingers[i].Index)
			{
				mytouch_index = i;
				break;
			}
		}
		//Debug.Log("タッチ");
		Set_My_touch(time_Manager.Get_time() , finger , mytouch_index);
		judge.Main_judge(0,my_Touch[mytouch_index]);
	}



	void OnFingerSet ( LeanFinger finger )
	{
		Check_Swipe(finger);
		Check_Holding(finger);
	}


	void OnFingerUp ( LeanFinger finger )
	{
		int mytouch_index = -1; //-1のままならおかしい
		for (int i = 0; i < LeanTouch.Fingers.Count; i++)//同時押しでも指を見分けるため
		{
			if (finger.Index == LeanTouch.Fingers[i].Index)
			{
				mytouch_index = i;
				break;
			}
		}
		if (my_Touch[mytouch_index].isHolding == true)
		{
			my_Touch[mytouch_index].isHolding = false;
			my_Touch[mytouch_index].isTouching = false;

			judge.Hold_judge(my_Touch[mytouch_index] , time_Manager.Get_time());
		}
	}




	public void Set_Hold ( int fingerID , int Hold_note_data_index , int Hold_note_line )
	{
		int index = -1;
		for (int i = 0; i < my_touch_max_count; i++)
		{
			if (my_Touch[i].fingerID == fingerID)
			{
				index = i;
			}
		}

		my_Touch[index].isHolding = true;//ホールド中にする
		my_Touch[index].Hold_note_data_index = Hold_note_data_index;//ホールドのnote_dataのindexを取得
		my_Touch[index].Hold_note_line = Hold_note_line;//ホールド中のnoteのlineを取得
																										//Debug.Log(my_Touch[fingerIndex].isTouching);
																										//HoldState.GetComponent<Text>().text = "Set_Holdホールド";
	}




	/// <summary>
	/// ホールド中のチェック。
	/// </summary>
	public void Check_Holding ( LeanFinger finger )
	{
		int mytouch_index = -1; //-1のままはおかしい
		for (int i = 0; i < LeanTouch.Fingers.Count; i++)//同時押しでも指を見分けるため
		{
			if (finger.Index == LeanTouch.Fingers[i].Index)
			{
				mytouch_index = i;
				break;
			}
		}

		if (my_Touch[mytouch_index].isHolding == true)//ホールド中なら
		{
			//Debug.Log("a");
			if (judge.Is_hold_within_range(finger.GetWorldPosition(0) , my_Touch[mytouch_index]))//範囲内なら
			{
				//Debug.Log(finger.GetStartWorldPosition(0));
				//Debug.Log("ホールド範囲");
				//何もしないかホールド中のエフェクト再生

			}
			else
			{
				//Debug.Log("ホールド範囲外");
				my_Touch[mytouch_index].isHolding = false;
				judge.Hold_judge(my_Touch[mytouch_index] , time_Manager.Get_time());
			}
		}
	}


	public void Check_Swipe ( LeanFinger finger )
	{
		
		int mytouch_index = -1; //-1のままはおかしい
		for (int i = 0; i < LeanTouch.Fingers.Count; i++)//同時押しでも指を見分けるため
		{
			if (finger.Index == LeanTouch.Fingers[i].Index)
			{
				mytouch_index = i;
				break;
			}
		}

		float distance =  Get_distance(finger.GetWorldPosition(0) , finger.GetLastWorldPosition(0));
		//Debug.Log("distance  " + distance);
		if (distance >= Swipe_distance)
		{
			Set_My_touch(time_Manager.Get_time() , finger , mytouch_index);
			judge.Main_judge(1,my_Touch[mytouch_index]);
		}

	}






	float Get_distance (Vector3 Now_pos , Vector3 Start_pos)
	{
		return Vector3.Distance(Now_pos , Start_pos);
	}
	


	void Set_My_touch ( float touchTime , LeanFinger finger , int mytouch_index )
	{
		my_Touch[mytouch_index].fingerID = finger.Index;
		my_Touch[mytouch_index].touchTime = touchTime;
		my_Touch[mytouch_index].touchPos = finger.GetWorldPosition(0);
		my_Touch[mytouch_index].isTouching = true;
		my_Touch[mytouch_index].mytouch_index = mytouch_index;
		//Debug.Log("Set_My_touch " + my_Touch.touchPos);
	}



	public void Hold_complete ( int hold_note_data_index , int line )
	{

		for (int i = 0; i < my_Touch.Length; i++)
		{
			if (my_Touch[i].isHolding == true)
			{
				if (my_Touch[i].Hold_note_data_index == hold_note_data_index && my_Touch[i].Hold_note_line == line)
				{
					my_Touch[i].isHolding = false;
					//my_Touch[i].isTouching = false;
					judge.Hold_end(1 , my_Touch[i].Hold_note_line ,
						Dc.Get_any_made_note_index_by_note_data(my_Touch[i].Hold_note_line , my_Touch[i].Hold_note_data_index)
						);
				}
			}
		}
	}




	/*

#if UNITY_EDITOR || UNITY_STANDALONE

	void Update ()
	{
		//キーボードでも遊べるように
		if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.J))
		{
			judge.Main_judge(1 , time_Manager.Get_time());
		}
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.K))
		{
			judge.Main_judge(2 , time_Manager.Get_time());
		}
	}
#endif


	*/


	/// <summary>
	/// 独自のタッチ構造体
	/// </summary>
	public struct My_touch
	{
		public int fingerID,
							 Hold_note_data_index,
							 Hold_note_line,
							 mytouch_index;
		public float touchTime;
		public Vector2 touchPos;
		public bool isTouching, isHolding;

		public My_touch ( int fingerID , float touchTime , Vector2 touchPos , bool isTouching , bool isHolding ,
										 int Hold_note_data_index , int Hold_note_line , int mytouch_index  )
		{
			this.fingerID = fingerID;
			this.touchTime = touchTime;
			this.touchPos = touchPos;
			this.isTouching = isTouching;
			this.isHolding = isHolding;
			this.Hold_note_data_index = Hold_note_data_index;
			this.Hold_note_line = Hold_note_line;
			this.mytouch_index = mytouch_index;
			//this.isSwipeing = isSwipeing;
		}


	}



}
