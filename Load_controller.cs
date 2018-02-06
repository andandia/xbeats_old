using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load_controller : MonoBehaviour
{

	[SerializeField] File_load File_load;
	[SerializeField] pxbp_load pxbp_Load;
	[SerializeField] Score_load Score_load;

	GameObject LoadDTO_OBJ;
	LoadDTO loadDTO;

	string[] pxbp;


	void Start()
	{
		To_be_load();
	}
	
	void To_be_load()
	{
		LoadDTO_OBJ = GameObject.FindGameObjectWithTag("LoadDTO");
		loadDTO = LoadDTO_OBJ.GetComponent<LoadDTO>();
		string filepath;
		filepath = File_load.Filepath_decide(0, loadDTO.Get_filesName(), loadDTO.Get_filesName() + ".pxbp");
		pxbp = File_load.Load_file(filepath);
		if (pxbp != null)
		{
			pxbp_Load.Load_json(pxbp[0]);
			pxbp_Load.Insert_Header();
			pxbp_Load.Insert_BPM_List();
			pxbp_Load.Insert_notes_List();
			Score_load.Main_Load_score(loadDTO.Get_HS() , loadDTO.Get_startOffset() , loadDTO.Get_perfectOffset() , loadDTO.Get_filesName());
			Destroy(LoadDTO_OBJ);
			UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
		}

	}
}
