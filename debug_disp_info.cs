using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デバッグ用にいろいろな情報を表示させるスクリプト
/// </summary>
public class debug_disp_info : MonoBehaviour {

	[SerializeField] GameObject disp_judge_txt;


	public void disp_text (string text)
	{
		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = text;
	}

	public void disp_judge (float lag)
	{


		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = lag.ToString();

		//switch (judge)
		//{
		//	case 1:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "Time_Judge PERFECT!!! ";
		//		break;
		//	case 2:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "Time_Judge GREAT!!! ";
		//		break;
		//	case 3:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "Time_Judge GOOD!!! ";
		//		break;
		//	case 4:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "Time_Judge POOR!!! ";
		//		break;
		//	case 5:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "Through ";
		//		break;
		//	default:
		//		disp_judge_txt.GetComponent<UnityEngine.UI.Text>().text = "";
		//		break;
		//}

	}
}
