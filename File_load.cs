﻿using System;
using System.IO;//ファイルを読むのに必要
using System.Text;//ファイルのエンコード指定に必要
using UnityEngine;

public class File_load : MonoBehaviour
{
	/*
	const String pxbpEx = ".pxbp";//拡張子を表す定数
	const String oggEx = ".ogg";
	*/

	/// <summary>
	/// フォルダパスを返す
	/// </summary>
	/// <param name="filetype">0,1が譜面、音源用、2が設定ファイル用</param>
	/// <param name="folder_name"></param>
	/// <param name="file_name">拡張子必須。</param>
	/// <returns></returns>
	public string Filepath_decide ( int filetype , string folder_name , string file_name )
	{
		string basic_path = Return_basic_path();
		string filepath = null;

		if (filetype == 0 || filetype == 1)//譜面、音源用
		{
			filepath = basic_path + "/Songs/" + folder_name + "/" + file_name;
		}
		else if (filetype == 2)//設定ファイル用
		{

		}

		//Debug.Log(filepath);

		return filepath;
	}
	




	/// <summary>
	/// 環境ごとの自由にアクセスできる場所のパスを返す
	/// </summary>
	string Return_basic_path ()
	{
		string filepath = null;

		switch (Application.platform)
		{
			case RuntimePlatform.OSXPlayer:
				break;
			case RuntimePlatform.WindowsPlayer:
				break;
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.WindowsEditor:
				filepath = Application.dataPath;
				break;
			case RuntimePlatform.Android:
				using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject externalFilesDir = currentActivity.Call<AndroidJavaObject>("getExternalFilesDir" , null))
						{
							filepath = externalFilesDir.Call<string>("getCanonicalPath");
							//Debug.Log("filepath " + filepath);
							//基本的に内部SD/Android/data/com.xbeats/が返ってくる
						}
					}
				}
				break;

			default:
				break;
		}

		return filepath;
	}



	/// <summary>
	/// filepathを元にファイルを読み込む
	/// </summary>
	/// <param name="filepath"></param>
	public string[] Load_file ( string filepath )
	{
		string file_content = "";//ファイルの中身(1行にまるごと)
		string[] file_separated = null;//ファイルの中身(配列1行にファイルの1行)

		FileInfo file = new FileInfo(filepath);
		try
		{

			// 一行毎読み込み
			using (StreamReader sr = new StreamReader(file.OpenRead() , Encoding.UTF8))
			{
				file_content = sr.ReadToEnd();
				file_separated = file_content.Split('\n'); //linesに1行毎にツッコむ
																									 //Debug.Log("file_content" + file_content);
																									 //Debug.Log("file_separated" + file_separated);
			}
		}
		catch (Exception)
		{
			// 改行コード
			file_content += "エラー：ファイルが読めませんでした。";
			//エラーは将来的にポップアップとか出したい
			Debug.Log("エラー ");
		}

		//↓デバック用。file_separatedの中身を見る
		//foreach (var item in file_separated)
		//{
		//    Debug.Log("行  " + item);
		//}

		return file_separated;

	}
}
