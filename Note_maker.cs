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
				//Dc.Mark_Made_note(); //todo 要らなかったら消す
				Ops.Make_note();
				//if (Dc.Note_data_list[Dc.Get_Create_note_data_index()].syncTimes > 0)
				//{

				//}

				Debug_Note_info();//todo 後で消す
				Dc.Inc_Create_note_data_index();


				//何回も回ってしまいそうな気がする
			}
		}


		/*
		stopwatchクラスは0秒始まりなのでstartTimeがマイナスだと反応してくれない。note_list側を補正するようにするか他の時間測定を考えるか
		しないといけない
		↑deltaTime使うようにしたが精度的にこれでいいか分からない
		*/





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