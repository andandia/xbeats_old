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


	[SerializeField] GameObject GuidelinePrefab;
	[SerializeField] Sprite Guideline_Normal;
	[SerializeField] Sprite Guideline_Hold;
	SpriteRenderer Now_Sprite;

	Quaternion originalRotation;

	private List<GameObject> pooledGuideline;

	void Start ()
	{
		pooledGuideline = new List<GameObject>();
	}
	
	public void Make_Guideline (int noteType)
	{
		foreach (GameObject obj in pooledGuideline)
		{
			if (obj.activeInHierarchy == false)
			{
				Set_Guideline_property(noteType,obj);
				obj.SetActive(true);
				Set_made_note_Property(obj.GetInstanceID());
				//Debug.Log("make GetInstanceID " + obj.GetInstanceID());
				return;
			}
		}
		GameObject newObj = Instantiate(GuidelinePrefab , ops.note_data.note_end_pos , originalRotation);
		Set_Guideline_property(noteType,newObj);
		pooledGuideline.Add(newObj);
		Set_made_note_Property(newObj.GetInstanceID());
	}

	

	//これがDestroyの代わり
	public void GuideDestoy ()
	{
		foreach (GameObject obj in pooledGuideline)
		{
			//Debug.Log("des GetInstanceID " + obj.GetInstanceID());
			if (obj.GetInstanceID() == ops.made_Note.guide_line_id)
			{
				obj.SetActive(false);
				break;
			}
		}
	}
	


	//この中で位置やらをオブジェクトに与える
	void Set_Guideline_property ( int noteType, GameObject obj )
	{
		Now_Sprite = obj.GetComponent<SpriteRenderer>();
		float z_value = 2;//z軸座標
		if (noteType == 0)//タッチ
		{
			Now_Sprite.sprite = Guideline_Normal;
		}
		else if (noteType == 2)//ホールド
		{
			Now_Sprite.sprite = Guideline_Hold;
			z_value = 3;
		}
		//ノートより奥に配置するためにこの形式
		obj.transform.position = new Vector3(ops.note_data.note_end_pos.x , ops.note_data.note_end_pos.y , z_value);
		obj.transform.rotation = Quaternion.Euler(0 , 0 , ops.note_data.rotation);
	}



	//オブジェクトのidとtweenをセットして作ったノートとして詰める
	void Set_made_note_Property ( int guide_line_id )
	{
		ops.made_Note.guide_line_id = guide_line_id;
	}



}