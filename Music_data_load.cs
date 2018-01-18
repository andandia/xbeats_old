
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
	Music_struct[] Music_list_per_page = new Music_struct[10];

	const int text_per_page = 10;
	int now_page = 0;

	string[] MusicDB = new string[0];
	

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
		Insert_Music_data_per_page(1);
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

	/// <summary>
	/// 1ページ用のMusic_list_per_pageへのデータ挿入処理
	/// </summary>
	public void Insert_Music_data_per_page (int add_page)
	{
		if (is_page_violation(add_page,now_page))
		{
			now_page += add_page;
			int start_index = Calc_start_index(now_page);
			int end_index = Calc_start_index(now_page);
			int index = start_index;

			for (int i = 0; i < 10; i++)
			{
				if (index == Music_list.Count)//全体の終わり
				{
					break;
				}
				Music_list_per_page[i] = Music_list[index];
				index++;
			}
		}
	}


	public void Insert_UI_text ()
	{
		int start_index = Calc_start_index(now_page);
		int end_index = Calc_end_index(now_page);
		
		for (int i = 0; i < 10; i++)
		{
			text[i].GetComponent<Text>().text = Music_list_per_page[i].MusicName + " / " + Music_list_per_page[i].artistName;
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



	/// <summary>
	/// 改ページの際のルールに違反していないか
	/// </summary>
	/// <param name="pattern">-1がページ戻り、1がページ進みの判定</param>
	/// <param name="now_page"></param>
	/// <returns></returns>
	private bool is_page_violation (int pattern, int now_page)
	{
		bool check = true;
		if (pattern == -1)//ページ戻りの判定
		{
			if (now_page < 0 )
			{
				check = false;
			}
		}
		else if(pattern == 1)//ページ進みの判定
		{
			if (Calc_start_index(now_page + 1) > Music_list.Count)
			{
				check = false;
			}
		}



		return check;
	}



}

