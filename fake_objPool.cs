using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_objPool : MonoBehaviour {

	[SerializeField]	GameObject[] testObj = new GameObject[10];

	




	public GameObject ReturnObj(int index)
	{
		//エラーになるのはnotemakerのupdateでnull回避していないため
		return testObj[index];
	}


	//public Transform Returntransform(int index)
	//{
	//	return testObj[index].transform;
	//}
}
