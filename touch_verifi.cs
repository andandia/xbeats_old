using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Android実機での同時押し検証スクリプト
public class touch_verifi : MonoBehaviour {
	Touch touch;

	[SerializeField] Text targetText;

	// Update is called once per frame
	void Update () {
		touchTest();
	}

	void touchTest ()
	{
		if (Input.touchCount > 0)
		{

			for (int i = 0; i < Input.touchCount; i++)
			{
				touch = Input.GetTouch(i);
				switch (touch.phase)
				{
					case TouchPhase.Began:
						if (Input.touchCount == 1)
						{
							OneFinger();
						}
						else
						{
							TwoFInger();
						}
						//Debug.Log("TouchPhase.Began " + touch.fingerId);
						break;
					default:
						//Debug.Log("touch.phase " + touch.phase + " " + touch.fingerId);
						break;
				}
			}
		}
	}



	void OneFinger ()
	{
		targetText.text = "one " + Input.touches[0].phase;
		Debug.Log("one " + Input.touches[0].phase);
	}



	void TwoFInger ()
	{
		targetText.text = "two " + Input.touches[0].phase + " " + Input.touches[1].phase;
		Debug.Log("two "  + Input.touches[0].phase + " " + Input.touches[1].phase);
	}
}
