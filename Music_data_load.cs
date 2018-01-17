
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music_data_load : MonoBehaviour
{
	[SerializeField] GameObject[] text = new GameObject[10];
	[SerializeField] File_load file_Load;
	JSONNode jsons;

	List<Music_struct> Music_list = new List<Music_struct>();

	const int text_per_page = 10;
	int now_page = 1;

	string[] MusicDB = new string[0];
	//1ページ分の曲データの配列を作る(buttonによる曲の特定をしやすくするため)

	void Start ()
	{
		To_be_load();
	}


	void To_be_load ()
	{
		string filepath;
		filepath = file_Load.Filepath_decide(0 , null , "Music.json");
		MusicDB = file_Load.Load_file(filepath);
		Load_Music_json(MusicDB[0]);
		//Debug.Log(settings[0]);
		Insert_Music_data();
		Insert_UI_text();
	}



	public void Load_Music_json ( string json )
	{
		jsons = JSON.Parse(json);
	}

	public void Insert_Music_data ()
	{
		for (int i = 0; i < jsons["Music"].Count; i++)
		{
			Music_struct music_Struct = new Music_struct();
			music_Struct.MusicName = jsons["Music"][i]["曲名"].Value;//セル名を変えたらここも変える
			music_Struct.artistName = jsons["Music"][i]["アーティスト"].Value;
			Debug.Log(music_Struct.MusicName);
			Debug.Log(music_Struct.artistName);
			Music_list.Add(music_Struct);
		}
	}


	public void Insert_UI_text ()
	{
		int start_index = Calc_start_index(now_page);
		int end_index = Calc_end_index(now_page);
		
		for (int i = 0; i < 10; i++)
		{
			text[i].GetComponent<Text>().text = Music_list[i].MusicName + " / " + Music_list[i].artistName;
			if (i == (Music_list.Count-1) )
			{
				break;
			}
		}
	}




	private int Calc_start_index (int now_page)
	{
		int i = ( ( now_page * text_per_page )  ) - text_per_page;
		Debug.Log("start" + i);
		return i;
	}



	private int Calc_end_index(int now_page )
	{
		int i = ( now_page * text_per_page ) - 1;
		Debug.Log("end" + i);
		return i;
	}





}

