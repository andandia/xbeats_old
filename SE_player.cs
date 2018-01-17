using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 名の通りノート判定時などのSEを再生するスクリプト
/// </summary>
public class SE_player : MonoBehaviour {

	[SerializeField] CriAtomSource clap_se;
	[SerializeField] CriAtomSource miss_se;

	/// <summary>
	/// タッチ音の再生スクリプト。
	/// </summary>
	/// <param name="judge">判定。PERFECT＝1</param>
	public void Play_touch_sound (int judge)
	{
		switch (judge)
		{
			case 1:
				clap_se.Play("clap");
				return;
			case 2:
				clap_se.Play("clap");
				return;
			case 3:
				clap_se.Play("clap");
				return;
			case 4:
				miss_se.Play("miss");
				return;
			default:
				break;
		}
	}

}
