using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_Dc : MonoBehaviour
{

	int MakeCount = 0;

	int countlimit = 9;

	public Tween[] tween = new Tween[10];

	public List<int> ObjID = new List<int>();//めんどいのでlist
	//public int[] ObjID = new int[10];

	int TweenCount = 0;

	int judgeCount = 0;


	int objIDcount = 0;


	public void IncMakeCount()
	{
		if (MakeCount < countlimit)
		{
			MakeCount++;
		}

	}


	public int GetMakeCount()
	{
		return MakeCount;
	}



	public void IncjudgeCount()
	{
		judgeCount++;

	}



	public int GetobjIDcount()
	{
		return objIDcount;
	}



	public void IncobjIDcount()
	{
		objIDcount++;
	}

	public int GetjudgeCount()
	{
		Debug.Log("judgeCount " + judgeCount);
		return judgeCount;
	}


	public void InsertTween(Tween Insert)
	{

		tween[TweenCount] = Insert;
		if (TweenCount < tween.Length - 1)
		{
			TweenCount++;
		}

	}


	public Tween Returntween(int judgeCount)
	{
		Tween returntween = tween[judgeCount];
		Debug.Log("returntween " + returntween);
		return returntween;
	}


	public void InsertObjID(int InsertID)
	{
		ObjID.Add(InsertID);

	}
}
