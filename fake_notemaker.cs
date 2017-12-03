using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_notemaker : MonoBehaviour {


	[SerializeField] fake_objPool fake_objPool;
	[SerializeField] fake_Dc fake_Dc;

	public Tween Tween;


	int flame = 0;



	[SerializeField] int flameLimit = 30;
	[SerializeField] float speed = 10f;
	


	
	// Update is called once per frame
	void Update () {
		flame++;
		if (flame % flameLimit == 0 && fake_Dc.GetMakeCount() != fake_Dc.tween.Length )
		{
			//ここでnull回避してないからエラー出る
			GameObject game = fake_objPool.ReturnObj(fake_Dc.GetMakeCount());
			fake_Dc.IncMakeCount();
			Tween testtween = game.transform.DOMove(new Vector3(0f, 0f, 0f), speed);
			fake_Dc.InsertTween(testtween);
		}
	}







	/*
	void stopTest()
	{

		GameObject test = Imitation_objPool.ReturnObj();
		Transform test2 = Imitation_objPool.Returntransform();
		test2.DOMove(new Vector3(0f, 0f, 0f), speed).SetEase(Ease.Linear);

		//.transform.DOMove(new Vector3(0f, 1f, 0f), 2f);

	}
	*/
}
