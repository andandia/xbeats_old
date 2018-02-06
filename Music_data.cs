using UnityEngine;


/// <summary>
/// 曲情報の構造体
/// </summary>
public struct Music_struct
{
	public string MusicName, artistName,filesName;


	public Music_struct ( string MusicName , string artistName, string filesName )
	{
		this.MusicName = MusicName;
		this.artistName = artistName;
		this.filesName = filesName;
	}
}




