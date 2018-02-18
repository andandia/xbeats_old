using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// ノートの矢印部分のオブジェクトプール
/// </summary>
public class ArrowPool : ObjectPoolSuper
{


	[SerializeField] ObjectPoolSuper ops;
	//ObjectPoolのインスタンス
	private static ArrowPool _instance;
	// シングルトン
	public static ArrowPool instance
	{
		get
		{
			// シーン上からオブジェクトプールを取得して返す
			_instance = FindObjectOfType<ArrowPool>();
			return _instance;
		}
	}

	public GameObject ArrowPrefab;

	Quaternion originalRotation;//todo 使わなかったら消す

	private List<GameObject> pooledArrow;

	void Start ()
	{
		pooledArrow = new List<GameObject>();
	}
	




	public void Make_arrow ( int note_num )//note_num...ノートが角度計算時にoneで出した方かtwoで出した方か
	{

		//poolされたゲームオブジェクトで使用出来るもの(非アクティブ)がある場合
		//位置、角度を初期化、オブジェクトを有効化して返す
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
				//Debug.Log("obj.transform " + obj.transform.position);
				Set_made_note_Property(obj.GetInstanceID() , tween);
				
				return;
			}
		}

		//使用できるものが無かった場合
		//新たに生成して、リストに追加して返す
		GameObject newObj = Instantiate(ArrowPrefab , ops.note_data.note_pos1 , originalRotation);
		//ここでノート設定
		//オブジェクトを投げる
		Set_note_property(newObj , 1);
		newObj.transform.rotation = LookAt(1 , newObj , ops.note_data.note_end_pos);
		tween = Set_tween(newObj);
		//リストに追加
		pooledArrow.Add(newObj);
		//Debug.Log(newObj.GetInstanceID() +" " + ops.note_data.note_end_pos);
		Set_made_note_Property(newObj.GetInstanceID() , tween);
	}






	//これがDestroyの代わりを果たすMake
	public void ArrowDestoy ()
	{
		foreach (GameObject obj in pooledArrow)
		{
			if (obj.GetInstanceID() == ops.made_Note.arrow_id_1)
			{
			//Debug.Log("des GetInstanceID " + obj.GetInstanceID());
				obj.SetActive(false);
				break;
			}
		}
	}


	//tweenをセットして、そのtweenを返す
	private Tween Set_tween ( GameObject obj )
	{
		Tween testtween = obj.transform.DOMove(ops.note_data.note_end_pos , ops.note_data.steamTime).SetEase(Ease.Linear);
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
		LookLoatation = Quaternion.FromToRotation(Vector3.right , diff);//right固定で良いのか？(spriteの向きと関係ありそう)
		return LookLoatation;
	}





	//作るべきメソッド
	//	・ノーツ生成時の初期化(位置、角度)決定メソッド
	//これをオブジェクトを引数に取るようにする
	//この中で位置やらをオブジェクトに与える
	void Set_note_property ( GameObject obj , int note_num )
	{
		switch (note_num)
		{
			case 1:
				obj.transform.position = ops.note_data.note_pos1;
				break;
			case 2:
				obj.transform.position = ops.note_data.note_pos2;
				break;
			case 3:
				obj.transform.position = ops.note_data.note_pos3;
				break;
			case 4:
				obj.transform.position = ops.note_data.note_pos4;
				break;
			default:
				break;
		}
		//Debug.Log("obj.transform.position " + obj.transform.position);
		obj.transform.rotation = LookAt(1 , obj , ops.note_data.note_end_pos);

	}

	//オブジェクトのidとtweenをセットして作ったノートとして詰める
	void Set_made_note_Property ( int arrowID1 , Tween tween )
	{
		ops.made_Note.arrow_id_1 = arrowID1;
		ops.made_Note.tween = tween;
	}



}