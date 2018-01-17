using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour {

	[SerializeField] Data_cabinet Dc;
	[SerializeField] ObjectPoolSuper ops;
	[SerializeField] Time_manager time;
	[SerializeField] SE_player SE_Player;
	Note_data note_Data_line1;
	Note_data note_Data_line2;

	[SerializeField] float perfectTime;
	[SerializeField] float greatTime;
	[SerializeField] float goodTime;
	[SerializeField] float poorTime;

	[SerializeField] debug_disp_info debug_Disp_Info;


	/// <summary>
	/// タッチの範囲の許容量。(ワールド座標基準)
	/// </summary>
	[SerializeField] float touchwidth = 2;

	/// <summary>
	/// タッチした指がどのlineのノートの範囲内にあるか。is_touch_within_rangeで判定。
	/// </summary>
	int within_range_line;

	void LateUpdate ()
	{
		Through_chack();
	}



	/// <summary>
	/// 判定時に最初に呼ばれる
	/// </summary>
	/// <param name="touchtime"></param>
	/// <param name="touchPos"></param>
	public void Main_judge (int touchCount , Touch_Manager.My_touch my_Touch)
	{
		note_Data_line1 = Dc.Get_Judge_note_data(1);
		if (Dc.Note_data_list_line2.Length != 0)
		{
			note_Data_line2 = Dc.Get_Judge_note_data(2);
		}
		//Dc.Inc_Judge_get_note_data_index();ここにあるのはおかしいはず

		if (Is_touch_within_range(my_Touch.touchPos))
		{
			Time_judge(within_range_line,my_Touch.touchTime);
		}
	}


	/// <summary>
	/// タッチがノートの範囲内か
	/// </summary>
	/// <returns></returns>
	bool Is_touch_within_range ( Vector2 touchpos)
	{
		if (touchpos.x <= note_Data_line1.note_end_pos.x + touchwidth
			&& touchpos.x >= note_Data_line1.note_end_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line1.note_end_pos.y + touchwidth
			&& touchpos.y >= note_Data_line1.note_end_pos.y - touchwidth)
			{
				//Dc.Inc_Judge_get_note_data_index(1);ここじゃない
				within_range_line = 1;
				
				return true;
			}
		}
		if (touchpos.x <= note_Data_line2.note_end_pos.x + touchwidth
			&& touchpos.x >= note_Data_line2.note_end_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line2.note_end_pos.y + touchwidth
			&& touchpos.y >= note_Data_line2.note_end_pos.y - touchwidth)
			{
				//Dc.Inc_Judge_get_note_data_index(2);
				within_range_line = 2;
				
				return true;
			}
		}
		return false;
	}



	/// <summary>
	/// 時間の判定
	/// </summary>
	void Time_judge (int line, float touchtime)
	{
		float lag = 10;
		switch (line)
		{
			case 1:
				lag = Mathf.Abs(note_Data_line1.parfectTime - touchtime);
				break;
			case 2:
				lag = Mathf.Abs(note_Data_line2.parfectTime - touchtime);
				break;
			default:
				break;
		}
		
		//Debug.Log("note_Data.parfectTime " + note_Data.parfectTime + " touchtime " + touchtime);
		if (lag <= perfectTime)
		{
			
			Debug.Log("Time_Judge PERFECT!!! " + line);
			//debug_Disp_Info.disp_judge(1);
			SE_Player.Play_touch_sound(1);
			ops.DestroyNote(line);
		}
		else if (lag <= greatTime)
		{
			
			Debug.Log("Time_Judge GREAT!! " + line);
			//debug_Disp_Info.disp_judge(2);
			SE_Player.Play_touch_sound(2);
			ops.DestroyNote(line);
		}
		else if (lag <= goodTime)
		{
			
			Debug.Log("Time_Judge GREAT!! " + line);
			//debug_Disp_Info.disp_judge(3);
			SE_Player.Play_touch_sound(3);
			ops.DestroyNote(line);
		}
		else if (lag <= poorTime)
		{
			
			Debug.Log("Time_Judge BAD… " + line);
			//debug_Disp_Info.disp_judge(4);
			SE_Player.Play_touch_sound(4);
			ops.DestroyNote(line);

		}
		else//判定時間外
		{
			return;
			//Debug.Log("None…");
			//debug_Disp_Info.disp_judge(0);
		}
	}






	/// <summary>
	/// 見逃し判定チェック
	/// </summary>
	void Through_chack ()
	{
		float now_time = time.Get_time();
		if (Dc.Note_data_list_line1.Length != 0)
		{
			if (now_time > ( Dc.Get_judge_note_parfectTime(1) + poorTime))
			{
				if (Dc.Get_judge_note_is_judged(1) == false)
				{
					Debug.Log("Through… " + 1);
					//debug_Disp_Info.disp_judge(5);
					ops.DestroyNote(1);
					

				}
			}
		}
		if (Dc.Note_data_list_line2.Length != 0)
		{
			if (now_time > ( Dc.Get_judge_note_parfectTime(2) + poorTime))
			{
				if (Dc.Get_judge_note_is_judged(2) == false)
				{
					Debug.Log("Through… " + 2);
					//debug_Disp_Info.disp_judge(5);
					ops.DestroyNote(2);
				}
			}
		}

	}
}
