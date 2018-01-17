using UnityEngine;
using System.Collections;

public class ButtonController : BaseButtonController
{

	//やること:button分のメソッドを作る、メソッドからMusicdataloadに飛ばす
	protected override void OnClick ( string objectName )
	{
		// 渡されたオブジェクト名で処理を分岐
		//（オブジェクト名はどこかで一括管理した方がいいかも）
		if ("Button".Equals(objectName))
		{
			// Button1がクリックされたとき
			ButtonClick();
		}
		else if ("Button (1)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_1Click();
		}
		else
		{
			throw new System.Exception("Not implemented!!");
		}
	}

	private void ButtonClick ()
	{
		Debug.Log("Button1 Click");
	}

	private void Button_1Click ()
	{
		Debug.Log("Button2 Click");
	}
}