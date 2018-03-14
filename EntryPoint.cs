using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint : MonoBehaviour {

	/*Select_Musicシーン用*/
	[SerializeField] Settings_load settings_Load;
	[SerializeField] Music_data_load music_Data_Load;

	/*Loadシーン用*/
	GameObject LoadDTO_OBJ;
	LoadDTO loadDTO;
	[SerializeField] File_load file_load;
	[SerializeField] pxbp_load pxbp_Load;
	[SerializeField] Score_load Score_load;
	[Tooltip("これをONにするとLoadシーンからPlayシーンに遷移しなくなります")]
	[SerializeField] bool Debug_not_scene_change;


	/*Playシーン用*/
	[SerializeField] Music_load music_Load;
	[SerializeField] Touch_Manager touch_Manager;

	void Start () {
		if (SceneManager.GetActiveScene().name == "Select_Music")
		{
			Select_Music();
		}
		else if (SceneManager.GetActiveScene().name == "Load")
		{
			Load();
		}
		else if (SceneManager.GetActiveScene().name == "Play")
		{
			Play();
		}
	}
	



	void Select_Music ()
	{
		settings_Load.To_be_load();
		music_Data_Load.To_be_load();
	}

	

	void Load ()
	{
		LoadDTO_OBJ = GameObject.FindGameObjectWithTag("LoadDTO");
		loadDTO = LoadDTO_OBJ.GetComponent<LoadDTO>();
		string filepath;
		filepath = file_load.Filepath_decide(0 , loadDTO.Get_filesName() , loadDTO.Get_filesName() + ".pxbp");
		string[] pxbp = file_load.Load_file(filepath);
		if (pxbp != null)
		{
			pxbp_Load.To_be_load(pxbp);
			Score_load.Main_Load_score(loadDTO.Get_HS() , loadDTO.Get_startOffset() , loadDTO.Get_perfectOffset() , loadDTO.Get_filesName() , loadDTO.Get_Swipe_distance());
			if (Debug_not_scene_change == false)
			{
				SceneManager.LoadScene("Play");
			}
		}
	}


	void Play ()
	{
		LoadDTO_OBJ = GameObject.FindGameObjectWithTag("LoadDTO");
		loadDTO = LoadDTO_OBJ.GetComponent<LoadDTO>();
		touch_Manager.Set_swipe_distance(loadDTO.Get_Swipe_distance());
		music_Load.To_be_load(loadDTO.Get_filesName());
		Destroy(LoadDTO_OBJ);
	}



	void Set_judgeTime ()
	{
		PlayerPrefs.SetFloat("perfectTime" , 0.02f);
		PlayerPrefs.SetFloat("greatTime" , 0.04f);
		PlayerPrefs.SetFloat("goodTime" , 0.15f);
		PlayerPrefs.SetFloat("poorTime" , 0.2f);
	}

}
