using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// ホールドノートの矢印部分のオブジェクトプール
/// </summary>
public class HoldArrowPool : ObjectPoolSuper
{


	[SerializeField] ObjectPoolSuper ops;
	//ObjectPoolのインスタンス
	private static HoldArrowPool _instance;
	// シングルトン
	public static HoldArrowPool instance
	{
		get
		{
			// シーン上からオブジェクトプールを取得して返す
			_instance = FindObjectOfType<HoldArrowPool>();
			return _instance;
		}
	}

	[SerializeField] GameObject HoldArrowPrefab;
	SpriteRenderer Now_Sprite;
	SpriteRenderer sr;

	[SerializeField] Touch_Manager touch_Manager;

	Quaternion originalRotation;

	private List<GameObject> pooledArrow;

	void Start ()
	{
		pooledArrow = new List<GameObject>();
	}
	
	public void Make_arrow (int note_num )//note_numは将来的に矢印2つ出すときに必要
	{
		Tween tween;
		foreach (GameObject obj in pooledArrow)
		{
			if (obj.activeInHierarchy == false)
			{
				//ここでノート設定
				//オブジェクトを投げる
				Set_note_property(obj , 1);
				obj.transform.rotation = LookAt(1 , obj , ops.note_data.note_end_pos);
				tween = Set_tween(obj);
				obj.SetActive(true);
				Debug.Log("obj.transform " + obj.transform.position);
				Set_made_note_Property(obj.GetInstanceID() , tween);
				return;
			}
		}


		GameObject newObj = Instantiate(HoldArrowPrefab , ops.note_data.note_pos1 , originalRotation);
		Set_note_property(newObj , 1);
		newObj.transform.rotation = LookAt(1 , newObj , ops.note_data.note_end_pos);
		tween = Set_tween(newObj);
		pooledArrow.Add(newObj);
		Set_made_note_Property(newObj.GetInstanceID() , tween);
	}






	//これがDestroyの代わりを果たす
	public void ArrowDestoy ()
	{
		foreach (GameObject obj in pooledArrow)
		{
			//Debug.Log("des GetInstanceID " + obj.GetInstanceID());
			if (obj.GetInstanceID() == ops.made_Note.arrow_id_1)
			{
				obj.SetActive(false);
				break;
			}
		}
	}


	//tweenをセットして、そのtweenを返す
	private Tween Set_tween ( GameObject obj )
	{
		Tween testtween = obj.transform.DOMove(new Vector3(ops.note_data.note_end_pos.x, ops.note_data.note_end_pos.y , 1.5f), ops.note_data.steamTime).SetEase(Ease.Linear);
		return testtween;
	}



	Quaternion LookAt ( int direction , GameObject myself , Vector3 target )
	{
		Quaternion LookLoatation;
		Vector3 diff = ( target - new Vector3(myself.transform.position.x , myself.transform.position.y , 0) ).normalized;
		//Vector3 orientation;
		//if	(direction == 1) { orientation = Vector3.up; }
		//else if (direction == 2) { orientation = Vector3.right; }
		//else if (direction == 3) { orientation = Vector3.down; }
		//else if (direction == 4) { orientation = Vector3.left; }
		LookLoatation = Quaternion.FromToRotation(Vector3.right , diff);
		return LookLoatation;
	}



	
	//この中で位置やらをオブジェクトに与える
	void Set_note_property ( GameObject obj , int note_num )
	{
		switch (note_num)
		{
			//他の矢印より奥に配置するためにこの形式
			case 1:
				obj.transform.position = new Vector3(ops.note_data.note_pos1.x , ops.note_data.note_pos1.y , 1.5f);
				break;
			case 2:
				obj.transform.position = new Vector3(ops.note_data.note_pos2.x , ops.note_data.note_pos2.y , 1.5f);
				break;
			case 3:
				obj.transform.position = new Vector3(ops.note_data.note_pos3.x , ops.note_data.note_pos3.y , 1.5f);
				break;
			case 4:
				obj.transform.position = new Vector3(ops.note_data.note_pos4.x , ops.note_data.note_pos4.y , 1.5f);
				break;
			default:
				break;
		}
		//Debug.Log("obj.transform.position " + obj.transform.position);
		Now_Sprite = obj.GetComponent<SpriteRenderer>();
		Now_Sprite.size = Note_width_calc(note_num);
		obj.transform.rotation = LookAt(1 , obj , ops.note_data.note_end_pos);

	}


	Vector2 Note_width_calc (int note_num)
	{//ノートの出現箇所と終点の距離を取ってそれに色を付けるといい感じ
		Vector2 Arrow_size;
		float distance = 0.5f;
		float add = 1.2f;
		switch (note_num)
		{
			case 1:
				distance = Vector2.Distance(ops.note_data.note_pos1 , ops.note_data.note_end_pos);
				break;
			case 2:
				distance = Vector2.Distance(ops.note_data.note_pos2 , ops.note_data.note_end_pos);
				break;
			case 3:
				distance = Vector2.Distance(ops.note_data.note_pos3 , ops.note_data.note_end_pos);
				break;
			case 4:
				distance = Vector2.Distance(ops.note_data.note_pos4 , ops.note_data.note_end_pos);
				break;
			default:
				break;
		}

		Arrow_size = new Vector2((distance + add),1f);
	
		return Arrow_size;
	}





	public void Set_Hold_Anime (Made_note made_Note , float time , int hold_note_data_index , int line , int made_note_data_index)
	{
		foreach (GameObject obj in pooledArrow)
		{
			if (obj.GetInstanceID() == made_Note.arrow_id_1)//とりあえず1つだけ
			{
				sr = obj.GetComponent<SpriteRenderer>();
				Tweener tweener =  DOVirtual.Float(sr.size.x , 1f , time , Set_sprite_size).SetEase(Ease.Linear).OnComplete(() => {touch_Manager.Hold_complete(hold_note_data_index , line);});
				ops.Dc.Set_tweener_made_note(made_note_data_index , tweener);
				break;
			}
		}
	}
	

	void Set_sprite_size ( float value)
	{
		sr.size = new Vector2(value , 1f);
	}



	








	//オブジェクトのidとtweenをセットして作ったノートとして詰める
	void Set_made_note_Property ( int arrowID1 , Tween tween )
	{
		ops.made_Note.arrow_id_1 = arrowID1;
		ops.made_Note.tween = tween;
	}



}