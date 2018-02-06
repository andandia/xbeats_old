using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選曲シーンからloadに値を受け渡すためのスクリプト
/// </summary>
public class LoadDTO : MonoBehaviour {

	private string MusicName, artistName , filesName;
	private float startOffset, perfectOffset;
	private float HS;

	void Start ()
	{
		DontDestroyOnLoad(this);
	}

	public string Get_MusicName ()
	{
		return MusicName;
	}

	public void Set_MusicName (string MusicName)
	{
		this.MusicName = MusicName;
	}




	public string Get_artistName ()
	{
		return artistName;
	}

	public void Set_artistName ( string artistName )
	{
		this.artistName = artistName;
	}


	public string Get_filesName ()
	{
		return filesName;
	}

	public void Set_filesName ( string filesName )
	{
		this.filesName = filesName;
	}





	public float Get_startOffset ()
	{
		return startOffset;
	}

	public void Set_startOffset ( float startOffset )
	{
		this.startOffset = startOffset;
	}




	public float Get_perfectOffset ()
	{
		return perfectOffset;
	}

	public void Set_perfectOffset ( float perfectOffset )
	{
		this.perfectOffset = perfectOffset;
	}


	public float Get_HS ()
	{
		return HS;
	}

	
	public void Set_HS ( float HS )
	{
		this.HS = HS;
	}
}
