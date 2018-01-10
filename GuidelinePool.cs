using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// ノートの矢印部分のオブジェクトプール
/// </summary>
public class GuidelinePool : ObjectPoolSuper
{
	[SerializeField] ObjectPoolSuper ops;

	//ObjectPoolのインスタンス
	private static GuidelinePool _instance;
	// シングルトン
	public static GuidelinePool instance
	{
		get
		{
			// シーン上からオブジェクトプールを取得して返す
			_instance = FindObjectOfType<GuidelinePool>();
			return _instance;
		}
	}


	
	public GameObject GuidelinePrefab;

	Quaternion originalRotation;//todo 使わなかったら消す

	private List<GameObject> pooledGuideline;

	void Start()
	{
		pooledGuideline = new List<GameObject>();
	}
	/*
	ノート生成流れ
	ここに処理が来る
プールの中から使えるものが無いか探す
無かったら新規生成
そのどちらかのオブジェクトを
メソッドに投げて
位置や角度の設定を行う
tweenの設定をする
作成したオブジェクトのIDを取得し、
Made_noteに格納
*/

	
		
	
	public void Make_Guideline()//note_num...ノートが角度計算時にoneで出した方かtwoで出した方か
	{
		
		//poolされたゲームオブジェクトで使用出来るもの(非アクティブ)がある場合
		//位置、角度を初期化、オブジェクトを有効化して返す
		foreach (GameObject obj in pooledGuideline)
		{
			if (obj.activeInHierarchy == false)
			{
				//ここでノート設定
				//オブジェクトを投げる
				Set_Guideline_property(obj);
				obj.SetActive(true);
				Set_made_note_Property(obj.GetInstanceID());
				break;
			}
		}

		//使用できるものが無かった場合
		//新たに生成して、リストに追加して返す
		GameObject newObj = Instantiate(GuidelinePrefab, ops.note_data.note_end_pos, originalRotation);
		//ここでノート設定
		//オブジェクトを投げる
		Set_Guideline_property(newObj);
	
		
		//リストに追加
		pooledGuideline.Add(newObj);
		Set_made_note_Property(newObj.GetInstanceID());

	}






	//これがDestroyの代わりを果たすMake
	public void releaseBall(GameObject obj)
	{
		obj.SetActive(false);
	}


	
	




	//作るべきメソッド
	//	・ノーツ生成時の初期化(位置、角度)決定メソッド
	//これをオブジェクトを引数に取るようにする
	//この中で位置やらをオブジェクトに与える
	void Set_Guideline_property(GameObject obj)
	{
		//ノートより奥に配置するためにこの形式
		obj.transform.position = new Vector3(ops.note_data.note_end_pos.x, ops.note_data.note_end_pos.y,2);
		obj.transform.rotation = Quaternion.Euler(0 , 0 , ops.note_data.rotation);
		//todo note_data格納段階でfloatにするのと、意図した角度になっていないのを直す
	}

	//オブジェクトのidとtweenをセットして作ったノートとして詰める
	//todo 実態に即した形にする
	void Set_made_note_Property(int guide_line_id )
	{
		ops.made_Note.guide_line_id = guide_line_id;
	}
		
	

}