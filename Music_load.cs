using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_load : MonoBehaviour
{

	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip audioClip;
	[SerializeField] File_load file_Load;

	[SerializeField] string song_dir;
	[SerializeField] string song_file_name1;

	/// <summary>
	/// 音源再生を待つ時間
	/// </summary>
	[SerializeField] float wait_time;

	const string _FILE_HEADER = "file:///";


	// Use this for initialization
	//void Start ()
	//{ //todo後で消す
	//	Load_music();
	//}

	public void Load_music()
	{
		string file_path = file_Load.Filepath_decide(0 , song_dir , song_file_name1);
		
		StartCoroutine(LoadSound(file_path));
	}


	/*Source https://goo.gl/T685hK
	*/
	private IEnumerator LoadSound ( string path )
	{
		// ファイルが無かったら終わる
		if (!System.IO.File.Exists(path))
		{
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
			Debug.Log("Failed to Load!");
			yield break;
		}

		// 読み込んだAudioClicpを設定する
		audioSource.clip = audioTrack;


		yield return new WaitForSeconds(wait_time);  //10秒待つ

		// 読み込んだファイルを再生する
		audioSource.Play();

		yield return 0;
	}






	
}
