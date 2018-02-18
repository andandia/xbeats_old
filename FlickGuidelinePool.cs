using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// ノートの矢印部分のオブジェクトプール
/// </summary>
public class FlickGuidelinePool : ObjectPoolSuper
{
	[SerializeField] ObjectPoolSuper ops;

	//ObjectPoolのインスタンス
	private static FlickGuidelinePool _instance;
	// シングルトン
	public static FlickGuidelinePool instance
	{
		get
		{
			// シーン上からオブジェクトプールを取得して返す
			_instance = FindObjectOfType<FlickGuidelinePool>();
			return _instance;
		}
	}



	[SerializeField] GameObject GuidelinePrefab;
	Transform flick_way;

	Quaternion originalRotation;

	private List<GameObject> pooledGuideline;

	void Start ()
	{
		pooledGuideline = new List<GameObject>();
	}
	
	public void Make_Guideline ()
	{
		foreach (GameObject obj in pooledGuideline)
		{
			if (obj.activeInHierarchy == false)
			{
				Set_Guideline_property(obj);
				flick_way = obj.transform.Find("Flick_Way");
				flick_way.rotation = LookAt(obj , ops.note_data.flick_pos);
				obj.SetActive(true);
				Set_made_note_Property(obj.GetInstanceID());
				//Debug.Log("make GetInstanceID " + obj.GetInstanceID());
				return;
			}
		}
		GameObject newObj = Instantiate(GuidelinePrefab , ops.note_data.note_end_pos , originalRotation);
		Set_Guideline_property(newObj);
		flick_way = newObj.transform.Find("Flick_Way");
		flick_way.rotation = LookAt(newObj , ops.note_data.flick_pos);
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
	void Set_Guideline_property ( GameObject obj )
	{
		
		
		//ノートより奥に配置するためにこの形式
		obj.transform.position = new Vector3(ops.note_data.note_end_pos.x , ops.note_data.note_end_pos.y , 2);
		obj.transform.rotation = Quaternion.Euler(0 , 0 , ops.note_data.rotation);
	}


	Quaternion LookAt ( GameObject myself , Vector3 target )
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



	//オブジェクトのidとtweenをセットして作ったノートとして詰める
	void Set_made_note_Property ( int guide_line_id )
	{
		ops.made_Note.guide_line_id = guide_line_id;
	}



}