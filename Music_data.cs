using UnityEngine;


/// <summary>
/// 曲情報の構造体
/// </summary>
public struct Music_struct
{
	public string MusicName, artistName;


	public Music_struct ( string MusicName , string artistName )
	{
		this.MusicName = MusicName;
		this.artistName = artistName;
	}
}




