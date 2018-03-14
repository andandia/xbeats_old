using UnityEngine;

/// <summary>
/// 設定情報の構造体
/// </summary>
public struct Settings_struct
{
	public float startOffset, perfectOffset , Swipe_distance;


	public Settings_struct ( float startOffset , float perfectOffset ,float Swipe_distance )
	{
		this.startOffset = startOffset;
		this.perfectOffset = perfectOffset;
		this.Swipe_distance = Swipe_distance;
	}
}




