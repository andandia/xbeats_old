using GodTouches;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Gestures.TransformGestures;
using UnityEngine;

public class Touch_Manager : MonoBehaviour {

	[SerializeField] Judge judge;
	[SerializeField] Time_manager time_Manager;

	Touch touch;
	My_touch[] my_Touch = new My_touch[3];

	void Update ()
	{
		Main_touch_process();
	}


	void Main_touch_process ()
	{
		
		if (Input.touchCount > 0)
		{
			float touchTime = 0;
			for (int i = 0; i < Input.touchCount; i++)
			{
				touch = Input.GetTouch(i);
				switch (touch.phase)
				{
					
					case TouchPhase.Began:
						Debug.Log("TouchPhase.Began " + touch.fingerId);
						if (i == 0)//初回のみ
						{
							touchTime = time_Manager.Get_time();//同時押し常に同じになるように
						}
						Set_My_touch(touchTime , touch);
						break;
					default:
						Debug.Log("touch.phase " + touch.phase + " " + touch.fingerId);
						break;
				}
			}
			if (touchTime != 0)
			//note 条件の目的:TouchPhase.Began以外の場合では判定に飛ばさないようにしたかった
			//条件の理由:TouchPhase.Beganを通っていれば必ずtouchTimeは0以外になるため
			{
				TouchBegan(Input.touchCount);
			}
			Debug.Log("---------------------------");
		}

#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			TouchBegan(1);
		}
#endif
	}

	void Set_My_touch ( float touchTime , Touch touch )
	{
		my_Touch[touch.fingerId].fingerID = touch.fingerId;
		my_Touch[touch.fingerId].touchTime = touchTime;
		my_Touch[touch.fingerId].touchPos = GetTouchWorldPos(touch);
	}


	void TouchBegan (int touchCount)
	{
		
		judge.Main_judge(touchCount , my_Touch);
	}






	Vector2 GetTouchWorldPos (Touch touch)
	{
		//Debug.Log("touchPos " + touch.position);
		return Camera.main.ScreenToWorldPoint(touch.position);

	}

	




#if UNITY_EDITOR
	/*---下2つはマウス用--*/
	void TouchBegan (long i)//引数はオーバーライドを適当にごまかすための要らない引数
	{
		//judge.Main_judge(time_Manager.Get_time() , GetTouchWorldPos(Input.mousePosition));
	}

	Vector2 GetTouchWorldPos ( Vector3 mousePos )
	{
		return Camera.main.ScreenToWorldPoint(mousePos);

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
