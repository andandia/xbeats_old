using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music_load : MonoBehaviour
{

	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip audioClip;
	[SerializeField] File_load file_Load;

	GameObject Dc_OBJ;
	Data_cabinet Dc;

	[SerializeField] GameObject error;

	/// <summary>
	/// 音源再生を待つ時間
	/// </summary>
	[SerializeField] float wait_time;

	const string _FILE_HEADER = "file:///";

	/*曲選択に戻るときに破壊するゲームオブジェクト*/
	/*dc
	loaddto
	criware
	dotween
	*/
	GameObject LoadDTO_OBJ;




	
	 public void To_be_load (string MusicfileName )
	{
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Play")
		{
			Dc_OBJ = GameObject.FindGameObjectWithTag("Dc");
			Dc = Dc_OBJ.GetComponent<Data_cabinet>();
			Dc.Set_BGM_Play(audioSource);
			Load_music(MusicfileName);
		}
		
	}

	public void Load_music(string MusicfileName )
	{
		string file_path = file_Load.Filepath_decide(0 , MusicfileName , MusicfileName + ".wav");
		Debug.Log("file_path " + file_path);
		StartCoroutine(LoadSound(file_path));
	}


	/*Source https://goo.gl/T685hK 曲をあるフォルダからロード
	*/
	private IEnumerator LoadSound ( string path )
	{
		// ファイルが無かったら終わる
		if (!System.IO.File.Exists(path))
		{
			error.GetComponent<Text>().text = "ファイルがありません:" + path;
			Debug.Log("File does NOT exist!! file path = " + path);
			yield break;
		}

		// 指定したファイルをロードする
		WWW request = new WWW(_FILE_HEADER + path);

		// ロードが終わるまで待つ
		while (!request.isDone)
		{
			yield return new WaitForEndOfFrame();
		}

		// 読み込んだファイルからAudioClipを取り出す
		AudioClip audioTrack = request.GetAudioClip(false , false);
		while (audioTrack.loadState == AudioDataLoadState.Loading)
		{
			// ロードが終わるまで待つ
			yield return new WaitForEndOfFrame();
		}

		if (audioTrack.loadState != AudioDataLoadState.Loaded)
		{
			// 読み込み失敗
			error.GetComponent<Text>().text = "読み込み失敗";
			Debug.Log("Failed to Load!");
			yield break;
		}

		// 読み込んだAudioClicpを設定する
		audioSource.clip = audioTrack;
		Invoke("End_BGM",(audioSource.clip.length + wait_time + 2));
		if (error != null)
		{
			error.SetActive(false);
		}
		yield return new WaitForSeconds(wait_time);  //n秒待つ

		// 読み込んだファイルを再生する
		audioSource.Play();

		yield return 0;
	}

	void End_BGM ()
	{
		LoadDTO_OBJ = GameObject.FindGameObjectWithTag("LoadDTO");
		Destroy(Dc_OBJ);
		Destroy(LoadDTO_OBJ);
		UnityEngine.SceneManagement.SceneManager.LoadScene("Select_Music");
	}




	
}
