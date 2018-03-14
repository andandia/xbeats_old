using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{

	GameObject Dc_OBJ;
	Data_cabinet Dc;
	[SerializeField] ObjectPoolSuper ops;
	[SerializeField] HoldArrowPool holdArrowPool;
	[SerializeField] Time_manager time;
	[SerializeField] SE_player SE_Player;
	[SerializeField] Effect_Manager effect_Manager;
	[SerializeField] Touch_Manager touch_Manager;
	Note_data note_Data_line1;
	Note_data note_Data_line2;

	//Set_judgeTimeで定義されている
	float perfectTime;//初期 0.02
	float greatTime;// 0.04
	float goodTime;// 0.1
	float poorTime;// 0.15

	[SerializeField] debug_disp_info debug_Disp_Info;




	/// <summary>
	/// タッチの範囲の許容量。(ワールド座標基準)
	/// </summary>
	[SerializeField] float touchwidth;

	/// <summary>
	/// タッチした指がどのlineのノートの範囲内にあるか。is_touch_within_rangeで判定。
	/// </summary>
	int within_range_line;

	void Start ()
	{
		Dc_OBJ = GameObject.FindGameObjectWithTag("Dc");
		Dc = Dc_OBJ.GetComponent<Data_cabinet>();
		Dc.Set_Time_Script(time);
		ops.Set_Dc_Script(Dc);
		CriAtom.SetBusAnalyzer(false);
		perfectTime = PlayerPrefs.GetFloat("perfectTime");
		greatTime = PlayerPrefs.GetFloat("greatTime");
		goodTime = PlayerPrefs.GetFloat("goodTime");
		poorTime = PlayerPrefs.GetFloat("poorTime");

	}


	void LateUpdate ()
	{
		Through_chack();
	}



	/// <summary>
	/// 判定時に最初に呼ばれる処理
	/// </summary>
	/// <param name="touchType">0がタッチorホールドの最初、1がフリック</param>
	/// <param name="my_Touch"></param>
	public void Main_judge ( int touchType , Touch_Manager.My_touch my_Touch )
	{
		//Debug.Log("judege ");
		note_Data_line1 = Dc.Get_Judge_note_data(1);
		if (Dc.Note_data_list_line2.Length != 0)
		{
			note_Data_line2 = Dc.Get_Judge_note_data(2);
		}

		if (touchType == 0)//タッチorホールド始点のとき
		{
			if (Is_touch_within_range(my_Touch.touchPos))
			{
				int hold_note_data_index = 0;
				Note_data note_Data = new Note_data();
				switch (within_range_line)
				{
					case 1:
						hold_note_data_index = Dc.judge_Index.get_note_data_line1;
						note_Data = note_Data_line1;
						break;
					case 2:
						hold_note_data_index = Dc.judge_Index.get_note_data_line2;
						note_Data = note_Data_line2;
						break;
					default: break;
				}
				if (note_Data.judged == false)//まだ未判定
				{
					if (note_Data.noteType == 0 || note_Data.noteType == 2)
					{
						Time_judge(note_Data , within_range_line , hold_note_data_index , my_Touch.touchTime , my_Touch.fingerID);
					}
				}
				else
				{
					return;
				}
			}
		}
		else if (touchType == 1)//フリックのとき
		{
			if (Is_Swipe_within_range(my_Touch.touchPos))
			{
				Note_data note_Data = new Note_data();
				switch (within_range_line)
				{
					case 1:
						//hold_note_data_index = Dc.judge_Index.get_note_data_line1;//note_data.made
						note_Data = note_Data_line1;
						break;
					case 2:
						//hold_note_data_index = Dc.judge_Index.get_note_data_line2;
						note_Data = note_Data_line2;
						break;
					default: break;
				}
				if (note_Data.judged == false)//まだ未判定
				{
					Time_judge(note_Data , within_range_line , -1 , my_Touch.touchTime , my_Touch.fingerID);
				}
				else
				{
					return;
				}
			}
		}


	}


	/// <summary>
	/// タッチがノートの範囲内か
	/// </summary>
	/// <returns></returns>
	bool Is_touch_within_range ( Vector2 touchpos )
	{
		if (touchpos.x <= note_Data_line1.note_end_pos.x + touchwidth
			&& touchpos.x >= note_Data_line1.note_end_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line1.note_end_pos.y + touchwidth
			&& touchpos.y >= note_Data_line1.note_end_pos.y - touchwidth)
			{
				within_range_line = 1;
				return true;
			}
		}
		if (note_Data_line2 != null &&
				 touchpos.x <= note_Data_line2.note_end_pos.x + touchwidth
			&& touchpos.x >= note_Data_line2.note_end_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line2.note_end_pos.y + touchwidth
			&& touchpos.y >= note_Data_line2.note_end_pos.y - touchwidth)
			{
				within_range_line = 2;
				return true;
			}
		}
		return false;
	}





	public bool Is_hold_within_range ( Vector2 touchpos  , Touch_Manager.My_touch my_Touch )
	{
		Note_data note_Data = Dc.Get_any_note_data(my_Touch.Hold_note_line , my_Touch.Hold_note_data_index);
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



	bool Is_Swipe_within_range ( Vector2 touchpos )
	{
		if (touchpos.x <= note_Data_line1.flick_pos.x + touchwidth
			&& touchpos.x >= note_Data_line1.flick_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line1.flick_pos.y + touchwidth
			&& touchpos.y >= note_Data_line1.flick_pos.y - touchwidth)
			{
				within_range_line = 1;
				return true;
			}
		}
		if (touchpos.x <= note_Data_line2.flick_pos.x + touchwidth
			&& touchpos.x >= note_Data_line2.flick_pos.x - touchwidth)
		{
			if (touchpos.y <= note_Data_line2.flick_pos.y + touchwidth
			&& touchpos.y >= note_Data_line2.flick_pos.y - touchwidth)
			{
				within_range_line = 2;
				return true;
			}
		}
		return false;
	}






	/// <summary>
	/// 時間の判定
	/// </summary>
	void Time_judge (Note_data note_Data ,  int line , int hold_note_data_index, float touchtime , int fingerID )
	{
		float lag = Mathf.Abs(note_Data.parfectTime - touchtime);
		//raw_lag = note_Data_line1.parfectTime - touchtime;
		float hold_time = 0;
		/*
		ホールドすべき秒数とは
		ホールドノートのホールド終了時刻-タッチした絶対時刻
		*/
		if (note_Data.noteType == 2)//ホールドなら
		{
			hold_time = (float)note_Data.hold_end_time - touchtime;
		}

		//Debug.Log("note_Data.parfectTime " + note_Data.parfectTime + " touchtime " + touchtime);
		if (lag <= perfectTime)
		{
			//Debug.Log("Time_Judge PERFECT!!! " + line);
			//debug_Disp_Info.disp_judge(raw_lag);
			After_judge(1 , line , fingerID , hold_note_data_index , hold_time, note_Data);

		}
		else if (lag <= greatTime)
		{
			//Debug.Log("Time_Judge GREAT!! " + line);
			//debug_Disp_Info.disp_judge(raw_lag);
			After_judge(2 , line , fingerID , hold_note_data_index , hold_time , note_Data);
		}
		else if (lag <= goodTime)
		{
			//Debug.Log("Time_Judge GREAT!! " + line);
			//debug_Disp_Info.disp_judge(raw_lag);
			After_judge(3 , line , fingerID , hold_note_data_index , hold_time , note_Data);
		}
		else if (lag <= poorTime)
		{
			//Debug.Log("Time_Judge BAD… " + line);
			After_judge(4 , line , fingerID , hold_note_data_index , hold_time , note_Data);

		}
		else//判定時間外
		{
			return;
			//Debug.Log("None…");
			//debug_Disp_Info.disp_judge(0);
		}
	}


	/// <summary>
	/// 判定した後の共通処理
	/// </summary>
	void After_judge ( int judgetype ,  int line , int fingerID , int hold_note_data_index ,float hold_time ,Note_data note_Data)
	{
		SE_Player.Play_touch_sound(judgetype);
		Dc.Set_judge_note_data_judged(line);
		effect_Manager.Play_Particle(judgetype , note_Data.note_end_pos);

		if (note_Data.noteType == 2 && judgetype != 4)//ホールドかつpoorでないなら
		{
			touch_Manager.Set_Hold(fingerID , hold_note_data_index , within_range_line);
			Hold_process(line , hold_note_data_index , hold_time);
			ops.DestroyNote(line , note_Data.made_note_list_index , true, false);
			Debug.Log("line "  + line);
			Debug.Log("hold_end_time " + note_Data.hold_end_time);
		}
		else if (note_Data.noteType == 0 || note_Data.noteType == 1)
		{
			ops.DestroyNote(line, note_Data.made_note_list_index , false , false);
			//Debug.Log("破壊 " + note_Data.made_note_list_index);
		}

	}


	public void Hold_judge ( Touch_Manager.My_touch my_Touch , float hold_end_time )
	{
		Note_data note_Data = Dc.Get_any_note_data(my_Touch.Hold_note_line , my_Touch.Hold_note_data_index);
		float lag = Mathf.Abs((float)note_Data.hold_end_time - hold_end_time);
		int judgetype = -1;
		if (lag <= perfectTime)
		{
			//Debug.Log("Hold_Judge PERFECT!!! ");
			//debug_Disp_Info.disp_judge(raw_lag);
			judgetype = 1;
		}
		else if (lag <= greatTime)
		{
			//Debug.Log("Hold_Judge GREAT!! ");
			//debug_Disp_Info.disp_judge(raw_lag);
			judgetype = 2;
		}
		else if (lag <= goodTime)
		{
			//Debug.Log("Hold_Judge GREAT!! ");
			//debug_Disp_Info.disp_judge(raw_lag);
			judgetype = 3;
		}
		else if (lag <= poorTime)
		{
			//Debug.Log("Hold_Judge BAD… ");
			judgetype = 4;
		}
		else
		{
			judgetype = 4;
		}
		Hold_end(judgetype , my_Touch.Hold_note_line , my_Touch.Hold_note_data_index);
	}


	
	public void Hold_end (int judgetype , int Hold_note_line  , int made_note_list_index )
	{
		SE_Player.Play_touch_sound(judgetype);
		ops.DestroyNote(Hold_note_line, made_note_list_index , false , true);
	}










	/// <summary>
	/// 見逃し判定チェック
	/// </summary>
	void Through_chack ()
	{
		//float add =2.0f;
		float now_time = time.Get_time();
		if (Dc.Note_data_list_line1.Length != 0)
		{
			if (now_time > ( Dc.Get_judge_note_parfectTime(1) + poorTime ))
			{
				if (Dc.Get_judge_note_is_judged(1) == false)
				{
					Debug.Log("Through… " + 1);
					//debug_Disp_Info.disp_judge(5);
					Dc.Set_judge_note_data_judged(1);
					ops.DestroyNote(1, Dc.Get_judge_made_note_list_index(1), false , false);
				}
			}
		}
		if (Dc.Note_data_list_line2.Length != 0)
		{
			if (now_time > ( Dc.Get_judge_note_parfectTime(2) + poorTime ))
			{
				if (Dc.Get_judge_note_is_judged(2) == false)
				{
					Debug.Log("Through… " + 2);
					//debug_Disp_Info.disp_judge(5);
					Dc.Set_judge_note_data_judged(2);
					ops.DestroyNote(2, Dc.Get_judge_made_note_list_index(2), false , false);
				}
			}
		}

	}


	/// <summary>
	/// ホールドの最初のタッチ判定した後の処理
	/// </summary>
	void Hold_process (int line, int hold_note_data_index ,float hold_time)
	{
		//Debug.Log("hold_time");
		holdArrowPool.Set_Hold_Anime(Dc.Get_any_made_note_by_note_data(line , hold_note_data_index) , 
																 hold_time , hold_note_data_index , line,
																 Dc.Get_any_made_note_index_by_note_data(line , hold_note_data_index));
	}


	

	//エディタ用デバッグメソッド
#if UNITY_EDITOR || UNITY_STANDALONE




	//キーボードで無理やり判定を通す
	public void Main_judge ( int line , float touchTime )
	{
		note_Data_line1 = Dc.Get_Judge_note_data(1);
		if (Dc.Note_data_list_line2.Length != 0)
		{
			note_Data_line2 = Dc.Get_Judge_note_data(2);
		}


		//Time_judge(line , touchTime , 0);

	}

#endif

}
