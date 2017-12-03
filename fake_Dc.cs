using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_Dc : MonoBehaviour
{

	int MakeCount = 0;

	int countlimit = 9;

	public Tween[] tween = new Tween[10];

	int TweenCount = 0;

	int judgeCount = 0;


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
}
