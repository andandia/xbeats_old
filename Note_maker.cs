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
	[SerializeField] Data_cabinet Dc;
	[SerializeField] ObjectPoolSuper Ops;

	void Start ()
	{

	}



	void Update ()
	{
		Note_make();
	}



	void Note_make ()
	{
		if (Dc.Is_create_note_search())
		{
			if (Dc.Is_create_note())
			{
				Dc.Mark_Made_note();
				Ops.Make_note();
				//int syncTimes = Dc.Note_data_list[Dc.Get_Create_note_data_index()].syncTimes;
				//if (syncTimes > 0)
				//{
				//	for (int i = 0; i < syncTimes; i++)
				//	{
				//		Dc.Mark_Made_note();
				//		Ops.Make_note();
				//	}
				//}

				//Debug_Note_info();//todo 後で消す
				Dc.Inc_Create_note_data_index();


				
			}
		}





	}

	/// <summary>
	/// デバッグ用ノート情報表示
	/// </summary>
	void Debug_Note_info ()
	{
		Note_data note_data = Dc.Get_Create_note_data();
		Debug.Log("/*-------------------*/");
		Debug.Log("startTime " + note_data.startTime);
		Debug.Log("steamTime " + note_data.steamTime);
		Debug.Log("parfectTime " + note_data.parfectTime);
		Debug.Log("time " + Time.Get_time());
		Debug.Log("生成 ");
		//Debug.Log("/*-------------------*/");
	}

}