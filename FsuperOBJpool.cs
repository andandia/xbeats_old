using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用のオブジェクトプールのスーパークラス
/// </summary>
public class FsuperOBJpool : MonoBehaviour {

	public fakeNote_data fakeNote_data2;


	

	public void setNote_data (fakeNote_data fakeNote_Data)
	{
		fakeNote_data2 = fakeNote_Data;//スーパークラス内にセット
	}

}
