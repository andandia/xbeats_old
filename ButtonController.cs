using UnityEngine;
using System.Collections;

public class ButtonController : BaseButtonController
{
	[SerializeField] Music_data_load Music_data_load;


	//やること:button分のメソッドを作る、メソッドからMusicdataloadに飛ばす
	protected override void OnClick ( string objectName )
	{
		// 渡されたオブジェクト名で処理を分岐
		//buttonは曲名のやつ
		if ("Button".Equals(objectName))
		{
			// Button1がクリックされたとき
			Button_0_Click();
		}
		else if ("Button (1)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_1_Click();
		}
		else if ("Button (2)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_2_Click();
		}
		else if ("Button (3)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_3_Click();
		}
		else if ("Button (4)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_4_Click();
		}
		else if ("Button (5)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_5_Click();
		}
		else if ("Button (6)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_6_Click();
		}
		else if ("Button (7)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_7_Click();
		}
		else if ("Button (8)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_8_Click();
		}
		else if ("Button (9)".Equals(objectName))
		{
			// Button2がクリックされたとき
			Button_9_Click();
		}
		/*--------------------------------------------------------------------------------------------*/
		else if ("page_plus_button".Equals(objectName))//ページ
		{
			// Button2がクリックされたとき
			page_plus_button_Click();
		}
		else if ("page_minus_button".Equals(objectName))//ページ
		{
			// Button2がクリックされたとき
			page_minus_button_Click();
		}
		/*--------------------------------------------------------------------------------------------*/
		else if ("HS_plus_button".Equals(objectName))//ページ
		{
			// Button2がクリックされたとき
			HS_plus_button_Click();
		}
		else if ("HS_minus_button".Equals(objectName))//ページ
		{
			// Button2がクリックされたとき
			HS_minus_button_Click();
		}
		else if ("debug_button".Equals(objectName))//タッチ検証シーンへ
		{
			// Button2がクリックされたとき
			debug_button_Click();
		}
		else
		{
			throw new System.Exception("Not implemented!!"); 
		}
	}

	private void Button_0_Click ()
	{
		Music_data_load.LoadDTO_insert(0);
	}

	private void Button_1_Click ()
	{
		Music_data_load.LoadDTO_insert(1);
	}

	private void Button_2_Click ()
	{
		Music_data_load.LoadDTO_insert(2);
	}

	private void Button_3_Click ()
	{
		Music_data_load.LoadDTO_insert(3);
	}

	private void Button_4_Click ()
	{
		Music_data_load.LoadDTO_insert(4);
	}

	private void Button_5_Click ()
	{
		Music_data_load.LoadDTO_insert(5);
	}

	private void Button_6_Click ()
	{
		Music_data_load.LoadDTO_insert(6);
	}

	private void Button_7_Click ()
	{
		Music_data_load.LoadDTO_insert(7);
	}

	private void Button_8_Click ()
	{
		Music_data_load.LoadDTO_insert(8);
	}

	private void Button_9_Click ()
	{
		Music_data_load.LoadDTO_insert(9);
	}


	/*--------------------------------------------------------------------------------------------*/

	private void page_plus_button_Click ()
	{
		Debug.Log("page_plus_button_Click");
		Music_data_load.Insert_Music_data_per_page(1);
		Music_data_load.Insert_UI_text();
	}

	private void page_minus_button_Click ()
	{
		Debug.Log("page_minus_button_Click Click");
		Music_data_load.Insert_Music_data_per_page(-1);
		Music_data_load.Insert_UI_text();
	}

	/*--------------------------------------------------------------------------------------------*/

	private void HS_plus_button_Click ()
	{
		Debug.Log("Button2 Click");
		Music_data_load.SetHS(0.5f);
	}

	private void HS_minus_button_Click ()
	{
		Debug.Log("Button2 Click");
		Music_data_load.SetHS(-0.5f);
	}

	private void debug_button_Click ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("touch検証");
	}


	


}