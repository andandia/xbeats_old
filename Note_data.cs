using UnityEngine;


public class Note_data
{
	public double
		startTime,
		hold_end_time;

	public float steamTime, rotation, parfectTime;

	public Vector3
		note_end_pos,
		note_pos1,
		note_pos2,
		note_pos3,
		note_pos4,
		flick_pos;

	public int
		noteType,
		syncTimes,
		made_note_list_index;

	public bool
		judged,
		made;


	/// <summary>
	/// 
	/// </summary>
	/// <param name="noteType"></param>
	/// <param name="startTime"></param>
	/// <param name="steamTime"></param>
	/// <param name="parfectTime"></param>
	/// <param name="nepX"></param>
	/// <param name="nepY"></param>
	/// <param name="x1"></param>
	/// <param name="y1"></param>
	/// <param name="x2"></param>
	/// <param name="y2"></param>
	/// <param name="x3"></param>
	/// <param name="y3"></param>
	/// <param name="x4"></param>
	/// <param name="y4"></param>
	/// <param name="endCnt"></param>
	/// <param name="rotation"></param>
	/// <param name="flickAngle"></param>
	/// <param name="syncTimes"></param>
	/// <param name="judged"></param>
	/// <param name="made">ノートを作ったか。trueで作成済み</param>
	/* クラスなら要らない(デフォルトコンストラクタがあるので)
	public Note_data
			( double noteType , double startTime , float steamTime , float parfectTime ,
			float nepX , float nepY ,
			float x1 , float y1 , float x2 , float y2 , float x3 , float y3 , float x4 , float y4 ,
			double hold_end_time , float rotation , int flickAngle , int syncTimes ,
			bool judged , bool made
			)
	{
		this.noteType = noteType;
		this.startTime = startTime;
		this.steamTime = steamTime;
		this.parfectTime = parfectTime;
		note_end_pos = new Vector3(nepX , nepX);
		note_pos1 = new Vector3(x1 , y1);
		note_pos2 = new Vector3(x2 , y2);
		note_pos3 = new Vector3(x3 , y3);
		note_pos4 = new Vector3(x4 , y4);
		this.hold_end_time = hold_end_time;
		this.rotation = rotation;
		this.flickAngle = flickAngle;
		this.syncTimes = syncTimes;
		this.judged = judged;
		this.made = made;
		this.made_note_list_index = 0;
	}
	*/
}

/*
 * 必要なもの(+は既に出せてる
 * ノートタイプ +
 * スタート時間+
 * 流すのにかける時間+
 * パーフェクト時間+
 * ノート始点位置xy*4+ (変数名の先頭に数字は使えないのでこの形)
 * ノート終点位置xy +
 * ロング終了時間+
 * rotation(unityに突っ込むには*-1の必要あり)+
 * フリック角度+(とりあえず8方向数値)
 * 同時押し有無(このノートを起点としていくつあるかで表現)
 */





