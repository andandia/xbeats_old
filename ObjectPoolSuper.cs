using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// オブジェクトプールのスーパークラス
/// </summary>
public class ObjectPoolSuper : MonoBehaviour
{


	[SerializeField] public Data_cabinet Dc;
	public Note_data note_data;
	public Made_note made_Note;

	[SerializeField] GuidelinePool guidelinePool;
	[SerializeField] FlickGuidelinePool flickguidelinePool;
	[SerializeField] ArrowPool arrowPool;
	[SerializeField] HoldArrowPool HoldarrowPool;

	public void Set_Dc_Script ( Data_cabinet Dc )
	{
		this.Dc = Dc;
	}


	public void Make_note ( int line )
	{
		made_Note = new Made_note();
		get_Note_data(line);
		made_Note.noteType = note_data.noteType;//ノートタイプを格納
		if (note_data.noteType == 0 || note_data.noteType == 2)//タッチorホールド
		{
			guidelinePool.Make_Guideline(note_data.noteType);
		}
		else if (note_data.noteType == 1)
		{
			flickguidelinePool.Make_Guideline();
		}

		if (note_data.noteType == 0 || note_data.noteType == 1)//タッチorフリック
		{
			arrowPool.Make_arrow(1);
		}
		else if (note_data.noteType == 2)//ホールド
		{
			HoldarrowPool.Make_arrow(1);
		}

		Add_Note_made_list(line);
	}



	void get_Note_data ( int line )
	{
		note_data = Dc.Get_Create_note_data(line);
		//Debug.Log("flick_x " + note_data.flick_pos.x);
		//Debug.Log("flick_y " + note_data.flick_pos.y);
	}


	void Add_Note_made_list ( int line )
	{
		Dc.Add_Note_made_list(line , made_Note);
	}

	/// <summary>
	/// ノートを破壊するときに呼ぶ
	/// </summary>
	/// <param name="line"></param>
	/// <param name="made_note_index"></param>
	/// <param name="isHolding">ホールド中に呼んでいるか</param>
	/// <param name="isHoldEnd">ホールド終了時に呼んでいるか</param>
	public void DestroyNote ( int line , int made_note_index , bool isHolding , bool isHoldEnd )
	{
		if (isHolding == false)
			//ホールド開始時じゃない(=タッチかフリック、もしくはホールド終了時。)ホールド開始時にここを呼ぶとノートが消えてしまう
		{
			made_Note = Dc.Get_any_made_note(made_note_index);
			made_Note.tween.Kill();
			made_Note.tweener.Kill();
			if (made_Note.noteType == 0 || made_Note.noteType == 2)//タッチorホールド
			{
				guidelinePool.GuideDestoy();
			}
			else if (made_Note.noteType == 1)//フリック
			{ 
				flickguidelinePool.GuideDestoy();
			}
			if (made_Note.noteType == 0 || made_Note.noteType == 1)//タッチorフリック
			{
				arrowPool.ArrowDestoy();
			}
			else if (made_Note.noteType == 2)//ホールド
			{
				HoldarrowPool.ArrowDestoy();
			}
		}
		
		if (isHoldEnd == false)//ホールド終了時じゃない
		{
			//Dc.Set_judge_note_data_judged(line);//todo 問題なければここから削除、移転先→Judge.After_judge及びThrough_chack
			Dc.Inc_Judge_get_note_data_index(line);
			Dc.Inc_Judge_get_note_made_index();
		}
	}


}
