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
			for (int i = 0; i < Input.touchCount; i++)
			{
				touch = Input.GetTouch(i);
				switch (touch.phase)
				{
					case TouchPhase.Began:
						TouchBegan(touch);
						break;
					default:
						break;
				}
			}
		}

#if UNITY_EDITOR
		if (Input.GetMouseButtonDown(0))
		{
			TouchBegan();
		}
#endif
	}




	void TouchBegan (Touch touch)
	{
		float touchTime = time_Manager.Get_time();//同時押し常に同じになるように
		my_Touch[touch.fingerId].fingerID = touch.fingerId;
		my_Touch[touch.fingerId].touchTime = touchTime;
		my_Touch[touch.fingerId].touchPos = GetTouchWorldPos(touch);
		Debug.Log("ID " + touch.fingerId);
		judge.Main_judge(touchTime , GetTouchWorldPos(touch));
	}



	Vector2 GetTouchWorldPos (Touch touch)
	{
		//Debug.Log("touchPos " + touch.position);
		return Camera.main.ScreenToWorldPoint(touch.position);

	}

	




#if UNITY_EDITOR
	/*---下2つはマウス用--*/
	void TouchBegan ()
	{
		judge.Main_judge(time_Manager.Get_time() , GetTouchWorldPos(Input.mousePosition));
	}

	Vector2 GetTouchWorldPos ( Vector3 mousePos )
	{
		return Camera.main.ScreenToWorldPoint(mousePos);

	}

#endif


	/// <summary>
	/// 独自のタッチ構造体
	/// </summary>
	struct My_touch
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
