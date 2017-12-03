using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_judge : MonoBehaviour {

	[SerializeField] fake_Dc fake_Dc;

	/*fake系設計
	 * 実証のための疑似スクリプト群
	 * 流れ
	 * notemakerで一定フレームごとにobjpoolのReturnObを呼び出す
	 * (これはオブジェクトプールで作った後を仮想的に表す)
	 * returnしたオブジェクトをnotemakerで受け取り、それに対してtweenを適用、
	 * それをfakeDcに投げる
	 * (ここの処理、notemakerで受け取るかは未確定)
	 * でfakejudgeでは見ての通りスペースキーを押したらfakeDcからtweenを受け取り止める
	 * 
	 * 
	 */

	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			int judgecount = fake_Dc.GetjudgeCount();
			if (fake_Dc.Returntween(judgecount) != null)
			/*note このチェックをしないと、連打したときにkillするtweenが作成されていないうちにkillが実行され(nullなので何も起きない)
			 * IncjudgeCountが実行されてしまう。この時点でjudgeCountと止めたいtweenがズレてしまうということになってしまう。
			 */
			{
				fake_Dc.Returntween(judgecount).Kill();
				fake_Dc.IncjudgeCount();
			}
		}

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				int judgecount = fake_Dc.GetjudgeCount();
				if (fake_Dc.Returntween(judgecount) != null)
				{
					fake_Dc.Returntween(judgecount).Kill();
					fake_Dc.IncjudgeCount();
				}
			}
		}
	}
}
