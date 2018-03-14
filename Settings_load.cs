
using SimpleJSON;
using System;
using UnityEngine;


public class Settings_load : MonoBehaviour
{

	[SerializeField] File_load file_Load;
	JSONNode jsons;
	Settings_struct settings_Struct;

	string[] settings = new string[0];

	
	public void To_be_load()
	{
		string filepath;
		filepath = file_Load.Filepath_decide(2 , null , "Settings.json");
		settings = file_Load.Load_file(filepath);
		Load_settings_json(settings[0]);
		Debug.Log(settings[0]);
		Insert_Sttings_data();
	}





	public void Load_settings_json ( string json )
	{
		jsons = JSON.Parse(json);
	}



	public void Insert_Sttings_data ()
	{

		settings_Struct = new Settings_struct();
		settings_Struct.startOffset = jsons["startOffset"].AsFloat;
		settings_Struct.perfectOffset = jsons["perfectOffset"].AsFloat;
		settings_Struct.Swipe_distance = jsons["Swipe_distance"].AsFloat;
		//Debug.Log(settings_Struct.startOffset);
		//Debug.Log(settings_Struct.perfectOffset);
	}
	
	public Settings_struct GetSettings ()
	{
		return settings_Struct;
	}
}

