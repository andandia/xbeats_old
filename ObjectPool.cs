using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ObjectPool : MonoBehaviour
{
	[SerializeField] Data_cabinet Dc;

	notes_struct note;


	//ObjectPoolのインスタンス
	private static ObjectPool _instance;
	// シングルトン
	public static ObjectPool instance
	{
		get
		{
			// シーン上からオブジェクトプールを取得して返す
			_instance = FindObjectOfType<ObjectPool>();
			return _instance;
		}
	}


	//玉のprefab
	public GameObject NotePrefab;

		//玉の初期角度
	Quaternion originalRotation;


	private List<GameObject> pooledGameObjects;

	void Start()
	{
		note = new notes_struct();
		pooledGameObjects = new List<GameObject>();
	}






	//これがInstantiateの代わりを果たす
	public GameObject Make_note(int note_num)//note_num...ノートが角度計算時にoneで出した方かtwoで出した方か
	{
		//poolされたゲームオブジェクトで使用出来るもの(非アクティブ)がある場合
		//位置、角度を初期化、オブジェクトを有効化して返す
		Set_note_property(note_num);

		
		foreach (GameObject obj in pooledGameObjects)
		{
			if (obj.activeInHierarchy == false)
			{

				//位置を初期化
				obj.transform.position = note.note_pos;
				//角度を設定
				obj.transform.rotation = LookAt(1,obj, note.note_end_pos);
				//アクティブにする
				Dc.Add_cache_arrow(obj);
				obj.SetActive(true);
				Set_tween(obj);
				//オブジェクトを返す
				return obj;

			}
		}

		//使用できるものが無かった場合
		//新たに生成して、リストに追加して返す
		GameObject newObj = Instantiate(NotePrefab, note.note_pos, originalRotation);
		Dc.Add_cache_arrow(newObj);
		newObj.transform.rotation = LookAt(1,newObj, note.note_end_pos);
		//リストに追加
		pooledGameObjects.Add(newObj);
		Set_tween(newObj);
		//オブジェクトを返す
		return newObj;
	}






	//これがDestroyの代わりを果たすMake
	public void releaseBall(GameObject obj)
	{
		obj.SetActive(false);
	}



	void Set_tween(GameObject obj)
	{
		Tween testtween = obj.transform.DOMove(note.note_end_pos, note.steamTime).SetEase(Ease.Linear);
	}



	Quaternion LookAt(int direction, GameObject myself , Vector3 target)
	{
		Quaternion LookLoatation;
		Vector3 diff = (target - new Vector3(myself.transform.position.x, myself.transform.position.y,0)).normalized;
		//Vector3 orientation;
		//if			(direction == 1) { orientation = Vector3.up; }
		//else if (direction == 2) { orientation = Vector3.right; }
		//else if (direction == 3) { orientation = Vector3.down; }
		//else if (direction == 4) { orientation = Vector3.left; }
		LookLoatation = Quaternion.FromToRotation(Vector3.right, diff);//right固定で良いのか？(spriteの向きと関係ありそう)
		return LookLoatation;
	}

	



		//作るべきメソッド
		//	・ノーツ生成時の初期化(位置、角度)決定メソッド

		void Set_note_property(int note_num)
	{
		
		int index = Dc.Get_make_note_index();
		note.noteType     = Dc.notes_List[index].noteType;
		note.steamTime    = Dc.notes_List[index].steamTime;
		note.note_end_pos = Dc.notes_List[index].note_end_pos;
		switch (note_num)
		{
			case 1:
				note.note_pos = Dc.notes_List[index].note_pos1;
				break;
			case 2:
				note.note_pos = Dc.notes_List[index].note_pos2;
				break;
			case 3:
				note.note_pos = Dc.notes_List[index].note_pos3;
				break;
			case 4:
				note.note_pos = Dc.notes_List[index].note_pos4;
				break;
			default:
				break;
		}
		note.rotation     = Dc.notes_List[index].rotation;
		note.syncTimes    = Dc.notes_List[index].syncTimes;
	}








	public struct notes_struct
	{
		//note vector3 どっかで参照元が無くなってコピーしても中身の値0とかなってたりしそう、バグがあったらそれを疑え
		public double noteType, rotation;
		public float steamTime;
		public Vector3 note_end_pos, note_pos;
		public int syncTimes;

		public notes_struct
				(double Ty, float steTi,
				//float nepX, float nepY,
				//float x1, float y1,
				Vector3 nep, Vector3 np,
				double rot, int syTi
				)
		{
			noteType = Ty;
			steamTime = steTi;
			note_end_pos = nep;
			note_pos = np;
			//endCnt = endC;
			rotation = rot;
			//flickAngle = flickA;
			syncTimes = syTi;
		}


	}
}