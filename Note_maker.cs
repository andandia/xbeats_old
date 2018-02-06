using System.Collections.Generic;
using UnityEngine;

/*
時間を取ってくる
現在の時間>= 今作るべきノートの時間
ノートを生成
同時押しがあれば
さらにその分生成

*/



public class Note_maker : MonoBehaviour
{
	[SerializeField] Time_manager Time;
	[SerializeField] ObjectPoolSuper Ops;
	GameObject Dc_OBJ;
	Data_cabinet Dc;


	void Start ()
	{
		Dc_OBJ = GameObject.FindGameObjectWithTag("Dc");
		Dc = Dc_OBJ.GetComponent<Data_cabinet>();
	}



	void Update ()
	{
		Note_make();
	}



	void Note_make ()
	{
		if (Dc.Is_create_note_search(1))
		{
			if (Dc.Is_create_note_line1())
			{
				Dc.Mark_Made_note(1);
				Ops.Make_note(1);
				//Debug_Note_info(1);
				Dc.Inc_Create_note_data_index(1);
			}
		}
		if (Dc.Is_create_note_search(2))
		{
			if (Dc.Is_create_note_line2())
			{
				Dc.Mark_Made_note(2);
				Ops.Make_note(2);
				Dc.Inc_Create_note_data_index(2);
			}
		}
	}


	
	/// <summary>
	/// デバッグ用ノート情報表示
	/// </summary>
	void Debug_Note_info (int line)
	{
		Note_data note_data = Dc.Get_Create_note_data(line);//デバッグする時なおして
		//Debug.Log("/*-------------------*/");
		Debug.Log("line " + line);
		Debug.Log("startTime " + note_data.startTime);
		Debug.Log("steamTime " + note_data.steamTime);
		Debug.Log("parfectTime " + note_data.parfectTime);
		Debug.Log("time " + Time.Get_time());
		Debug.Log("生成 ");
		Debug.Log("/*-------------------*/");
	}

}