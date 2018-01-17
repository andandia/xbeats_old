
using SimpleJSON;
using System;
using UnityEngine;


public class Settings_load : MonoBehaviour
{

	[SerializeField] File_load file_Load;
	JSONNode jsons;

	string[] settings = new string[0];

	void Start ()
	{
		To_be_load();
	}

	void To_be_load ()
	{
		string filepath;
		filepath = file_Load.Filepath_decide(0 , null , "Settings.json");
		settings = file_Load.Load_file(filepath);
		Load_settings_json(settings[0]);
		//Debug.Log(settings[0]);
		Insert_Sttings_data();
	}





	public void Load_settings_json ( string json )
	{
		jsons = JSON.Parse(json);
	}

	public void Insert_Sttings_data ()
	{

		Settings_struct settings_Struct = new Settings_struct();
		settings_Struct.startOffset = jsons["startOffset"].AsFloat;
		settings_Struct.perfectOffset = jsons["perfectOffset"].AsFloat;
		Debug.Log(settings_Struct.startOffset);
		Debug.Log(settings_Struct.perfectOffset);
	}
	

}

