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
	[SerializeField]	Time_manager Time;
	[SerializeField]	Data_cabinet Dc;
	[SerializeField] ObjectPool Op;

	void Start()
	{

	}



	void Update()
	{
		Note_make();
	}



	void Note_make()
	{
		int make_index = Dc.Get_make_note_index();
		if (make_index <= Dc.notes_List.Length-1)
		{
			if (Time.Get_time() >= Dc.notes_List[make_index].startTime && Dc.notes_List[make_index].alive == true)
			{
				Dc.notes_List[make_index].alive = false;
				Op.Make_note(1);
				if (Dc.notes_List[make_index].syncTimes > 0)
				{
						
				}
				Debug.Log("startTime " + Dc.notes_List[make_index].startTime);
				Dc.Add_make_note_index();
				Debug.Log("time " + Time.Get_time());
				Debug.Log("生成 ");
				//何回も回ってしまいそうな気がする
			}
		}


/*
stopwatchクラスは0秒始まりなのでstartTimeがマイナスだと反応してくれない。note_list側を補正するようにするか他の時間測定を考えるか
しないといけない
*/





	}

	


}