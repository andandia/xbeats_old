using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		//vector3は予め作っておくべきでは
		Vector3 position = new Vector3(note.note_posX, note.note_posY);
		Vector3 lookpos = new Vector3(note.note_end_posX, note.note_end_posY, 0);

		foreach (GameObject obj in pooledGameObjects)
		{
			if (obj.activeInHierarchy == false)
			{

				//位置を初期化
				obj.transform.position = position;
				//角度を設定
				obj.transform.LookAt(lookpos);
				//アクティブにする



				obj.SetActive(true);
				//オブジェクトを返す
				return obj;

			}
		}

		//使用できるものが無かった場合
		//新たに生成して、リストに追加して返す
		GameObject newObj = (GameObject)Instantiate(NotePrefab, position, originalRotation);
		newObj.transform.LookAt(lookpos);
		//リストに追加
		pooledGameObjects.Add(newObj);
		//オブジェクトを返す
		return newObj;
	}






	//これがDestroyの代わりを果たすMake
	public void releaseBall(GameObject obj)
	{
		obj.SetActive(false);
	}







	//作るべきメソッド
	//	・ノーツ生成時の初期化(位置、角度)決定メソッド

		void Set_note_property(int note_num)
	{
		
		int index = Dc.Get_make_note_index();
		note.noteType = Dc.notes_List[index].noteType;
		note.note_end_posX = Dc.notes_List[index].note_end_posX;
		note.note_end_posY = Dc.notes_List[index].note_end_posY;
		switch (note_num)
		{
			case 1:
				note.note_posX = Dc.notes_List[index].note_pos1X;
				note.note_posY = Dc.notes_List[index].note_pos1Y;
				break;
			case 2:
				note.note_posX = Dc.notes_List[index].note_pos2X;
				note.note_posY = Dc.notes_List[index].note_pos2Y;
				break;
			case 3:
				note.note_posX = Dc.notes_List[index].note_pos3X;
				note.note_posY = Dc.notes_List[index].note_pos3Y;
				break;
			case 4:
				note.note_posX = Dc.notes_List[index].note_pos4X;
				note.note_posY = Dc.notes_List[index].note_pos4Y;
				break;
			default:
				break;
		}
		note.rotation = Dc.notes_List[index].rotation;
		note.syncTimes = Dc.notes_List[index].syncTimes;
	}








	public struct notes_struct
	{
		public double noteType, steamTime,
				rotation;
		public float note_end_posX, note_end_posY, note_posX, note_posY;
		public int syncTimes;

		public notes_struct
				(double Ty, double steTi,
				float nepX, float nepY,
				float x1, float y1,
				double rot, int syTi
				)
		{
			noteType = Ty;
			steamTime = steTi;
			note_end_posX = nepX;
			note_end_posY = nepY;
			note_posX = x1;
			note_posY = y1;
			//endCnt = endC;
			rotation = rot;
			//flickAngle = flickA;
			syncTimes = syTi;
		}


	}
}