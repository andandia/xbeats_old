﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Load_controller : MonoBehaviour
{



	/*---------------------デバック用変数等記述場所----------------------*/
	[SerializeField] bool _isGati = false;//デバック用の読み込みファイル変更
	[SerializeField] string song_file_name2;
	[SerializeField] Play_controller Play_controller;//最終的にロードシーン→プレイシーンとしたときには必要なくなる
	/*------------------------------------------------------------------*/


	[SerializeField]
	File_load File_load;
	[SerializeField] pxbp_load pxbp_Load;
	[SerializeField] Score_load Score_load;

	[SerializeField] string song_dir;
	[SerializeField] string song_file_name1;





	string[] pxbp;


	void Start()
	{
		To_be_load();
		pxbp_Load.Load_json(pxbp[0]);
		pxbp_Load.Insert_Header();
		pxbp_Load.Insert_BPM_List();
		pxbp_Load.Insert_notes_List();
		Score_load.Main_Load_score();
		Play_controller.Starter();//最終的には要らない
	}


	void To_be_load()
	{
		string filepath;
		if (_isGati == false)//ここは後々_isGatiを消して可変で読みに行かせるようにする
		{
			filepath = File_load.Filepath_decide(0, song_dir, song_file_name1);
		}
		else
		{
			filepath = File_load.Filepath_decide(0, song_dir, song_file_name2);
		}
		pxbp = File_load.Load_file(filepath);
	}

	




}
