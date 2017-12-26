using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_objPool : MonoBehaviour {

	[SerializeField]	GameObject[] testObj = new GameObject[10];
	[SerializeField] fake_Dc fake_Dc;





	public GameObject ReturnObj(int index)
	{
		//エラーになるのはnotemakerのupdateでnull回避していないため3
		fake_Dc.InsertObjID(testObj[index].GetInstanceID());
		return testObj[index];
	}


	//public Transform Returntransform(int index)
	//{
	//	return testObj[index].transform;
	//}

	public void Objfalse(int ObjID)
	{
		ObjfalseSearch(ObjID).SetActive(false);
	}

	GameObject ObjfalseSearch(int ObjID)
	{
		foreach (var item in testObj)
		{
			if (item.GetInstanceID() == ObjID)
			{
				return item;
			}
		}
		return null;
	}
}
