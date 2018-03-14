using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//デバッグ用にPlayシーンだけで遊べるようにするスクリプト
public class Debug_OnlyPlay : MonoBehaviour {

	/// <summary>
	/// 内部的な(=フォルダ名やpxbpファイル名)名前。曲json生成.xlsmのファイル名に同じ
	/// </summary>
	[SerializeField] string Music_name;
	[SerializeField] float HS;


	[SerializeField] File_load file_Load;
	[SerializeField] pxbp_load pxbp_Load;
	[SerializeField] Score_load score_Load;
	[SerializeField] Music_load music_Load;
	[SerializeField] Data_cabinet Dc;
	[SerializeField] AudioSource audioSource;

	JSONNode jsons;
	Settings_struct settings_Struct;

	string[] pxbp;


#if UNITY_EDITOR
	void Start () {
		Emu_select_scene();
		Emu_load_scene();
		Dc.Set_BGM_Play(audioSource);
		music_Load.Load_music(Music_name);
	}
#endif 







	//曲選択シーンの再現
	void Emu_select_scene ()
	{
		//settings読み込み～格納
		string filepath;
		filepath = file_Load.Filepath_decide(2 , null , "Settings.json");
		string[] settings = new string[0];
		settings = file_Load.Load_file(filepath);
		jsons = JSON.Parse(settings[0]);
	}


	void Emu_load_scene ()
	{
		string filepath;
		filepath = file_Load.Filepath_decide(0 , Music_name , Music_name + ".pxbp");
		pxbp = file_Load.Load_file(filepath);
		if (pxbp != null)
		{
			pxbp_Load.Load_json(pxbp[0]);
			pxbp_Load.Insert_Header();
			pxbp_Load.Insert_BPM_List();
			pxbp_Load.Insert_notes_List();
			score_Load.Main_Load_score(HS , jsons["startOffset"].AsFloat , jsons["perfectOffset"].AsFloat , Music_name, jsons["Swipe_distance"].AsFloat);

		}
	}

	

}
