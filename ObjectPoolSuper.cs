using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトプールのスーパークラス
/// </summary>
public class ObjectPoolSuper : MonoBehaviour {

	
	[SerializeField] public Data_cabinet Dc;
	public Note_data note_data;
	public Made_note made_Note;

	[SerializeField] GuidelinePool guidelinePool;
	[SerializeField] ArrowPool arrowPool;

	public void Make_note ()
	{
		made_Note = new Made_note();
		get_Note_data();
		guidelinePool.Make_Guideline();
		arrowPool.Make_arrow(1);

		Add_Note_made_list();
	}



	void get_Note_data ()
	{
		note_data = Dc.Get_Create_note_data();
	}

		
	void Add_Note_made_list ()
	{
		Dc.Add_Note_made_list(made_Note);
	}


	public void DestroyNote ()
	{
		made_Note = Dc.Get_judge_made_note();
		guidelinePool.GuideDestoy();
		arrowPool.ArrowDestoy();
		Dc.Set_judge_note_data_judged();
		Dc.Inc_Judge_get_note_made_index();
	}

}
