using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// こいつを追加するとログを画面上に出してくれる
/// </summary>
/// http://www.urablog.xyz/entry/2017/04/25/195351
public class LogOnGUI : MonoBehaviour {

	[SerializeField] GameObject m_textUI;
	

	private void OnLogMessage ( string i_logText , string i_stackTrace , LogType i_type )
	{
		if (string.IsNullOrEmpty(i_logText))
		{
			return;
		}

		switch (i_type)
		{
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				i_logText = string.Format("<color=red>{0}</color>" , i_logText);
				break;
			case LogType.Warning:
				i_logText = string.Format("<color=yellow>{0}</color>" , i_logText);
				break;
			default:
				break;
		}

		m_textUI.GetComponent<Text>().text += i_logText + System.Environment.NewLine;
		
	}

	/*
	Canvas→Scroll Viewを追加
	Scroll View内のViewport→ContentにuiのtextとContentSizeFitterを追加
	ContentSizeFitterの設定は両方preseffed(textはoverflow)
	*/




	void OnEnable ()
	{
		//Application.logMessageReceived += OnLogMessage;
	}


	void OnDisable ()
	{
		//Application.logMessageReceived -= OnLogMessage;
	}


}
