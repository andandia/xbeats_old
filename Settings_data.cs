using UnityEngine;

/// <summary>
/// 設定情報の構造体
/// </summary>
public struct Settings_struct
{
	public float startOffset, perfectOffset;


	public Settings_struct ( float startOffset , float perfectOffset )
	{
		this.startOffset = startOffset;
		this.perfectOffset = perfectOffset;
	}
}




