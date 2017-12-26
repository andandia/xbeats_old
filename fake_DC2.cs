using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fake_DC2 : MonoBehaviour {

	[SerializeField] fakeNote_data fakeNote_Data;
	[SerializeField] FsuperOBJpool FsuperOBJpool;
	[SerializeField] FabsOBJpool FabsOBJpool;



	//抽象クラスのテスト用
	// Use this for initialization
	void Start () {
		fakeNote_Data = new fakeNote_data(10);
		FsuperOBJpool.setNote_data(fakeNote_Data);
		FabsOBJpool.view();

	}
	
	

}
