using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour {

	[SerializeField] Data_cabinet Dc;
	[SerializeField] ObjectPoolSuper ops;
	[SerializeField] Time_manager time;
	Note_data note_Data;

	[SerializeField] float perfectTime;
	[SerializeField] float greatTime;
	[SerializeField] float goodTime;
	[SerializeField] float poorTime;

	/// <summary>
	/// タッチの範囲の許容量。(ワールド座標基準)
	/// </summary>
	[SerializeField] float touchwidth = 1;



	void LateUpdate ()
	{
		Through_chack();
	}



	/// <summary>
	/// 判定時に最初に呼ばれる
	/// </summary>
	/// <param name="touchtime"></param>
	/// <param name="touchPos"></param>
	public void Main_judge (float touchtime, Vector2 touchPos )
	{
		Debug.Log("touchPos " + touchPos);
		note_Data = Dc.Get_Judge_note_data();
		Dc.Inc_Judge_get_note_data_index();
		if (is_touch_within_range(touchPos))
		{
			Time_judge(touchtime);
		}

	}


	/// <summary>
	/// タッチがノートの範囲内か
	/// </summary>
	/// <returns></returns>
	bool is_touch_within_range ( Vector2 touchpos)
	{
		if (touchpos.x <= note_Data.note_end_pos.x + touchwidth
			&& touchpos.x >= note_Data.note_end_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data.note_end_pos.y + touchwidth
			&& touchpos.y >= note_Data.note_end_pos.y - touchwidth)
			{
				return true;
			}
		}
		return false;
	}



	/// <summary>
	/// 時間の判定
	/// </summary>
	void Time_judge (float touchtime)
	{
		float lag = Mathf.Abs(note_Data.parfectTime - touchtime);
		Debug.Log("note_Data.parfectTime " + note_Data.parfectTime + " touchtime " + touchtime);
		if (lag <= perfectTime)
		{
			
			Debug.Log("Time_Judge PERFECT!!!");
			ops.DestroyNote();
		}
		else if (lag <= greatTime)
		{
			
			Debug.Log("Time_Judge GREAT!!");
			ops.DestroyNote();
		}
		else if (lag <= goodTime)
		{
			
			Debug.Log("Time_Judge GREAT!!");
			ops.DestroyNote();
		}
		else if (lag <= poorTime)
		{
			
			Debug.Log("Time_Judge BAD…");
			ops.DestroyNote();

		}
		else//判定時間外
		{
			Debug.Log("None…");
			ops.DestroyNote();
		}
	}






	/// <summary>
	/// 見逃し判定チェック
	/// </summary>
	void Through_chack ()
	{
		if (time.Get_time() > (Dc.Get_judge_note_parfectTime() + poorTime))
		{
			if (Dc.Get_judge_note_is_judged() == false)
			{
				Debug.Log("Through…");
				ops.DestroyNote();
				Dc.Inc_Judge_get_note_data_index();

			}
		}
	}
}
