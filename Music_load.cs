using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_load : MonoBehaviour {

	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip audioClip;
	[SerializeField] File_load file_Load;

	[SerializeField] string song_dir;
	[SerializeField] string song_file_name1;

	const string _FILE_HEADER = "file:///";


	// Use this for initialization
	void Start () { //todo後で消す
		Load_music();
	}
	
	void Load_music ()
	{
		string file_path = file_Load.Filepath_decide(0 , song_dir , song_file_name1);

		//LoadSound(file_path);

		/*
		WWW www = new WWW(file_path);
		audioClip = www.GetAudioClip(true,true);
		audioSource.clip = audioClip;
		audioSource.Play();
		
		Debug.Log(file_path);
		*/


		StartCoroutine(StreamPlayAudioFile(file_path));
		//StreamPlayAudioFile(file_path);
	}


	IEnumerator StreamPlayAudioFile ( string file_path )
	{
		//ソース指定し音楽流す
		//音楽ファイルロード
		using (WWW www = new WWW("file:///" + file_path))
		//using (WWW www = new WWW(file_path))
		{
			//読み込み完了まで待機
			yield return www;

			audioSource.clip = www.GetAudioClip(true , true);

			audioSource.Play();
		}
	}



	/*
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
		AudioClip audioTrack = request.GetAudioClip(false , true);
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

		// AudioSourceを生成し、gameobjectに追加する
		AudioSource source = gameObject.AddComponent<AudioSource>();

		// 生成したsourceに読み込んだAudioClicpを設定する
		source.clip = audioTrack;

		// 読み込んだファイルを再生する
		source.Play();

		yield return 0;
	}
	*/
}
