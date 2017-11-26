using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_cabinet : MonoBehaviour
{

	public notes_struct[] notes_List;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}



	public void make_notes_List(int length)
	{
		notes_List = new notes_struct[length];
	}

	public struct notes_struct
	{
		public double noteType, startTime, steamTime, parfectTime,
				note_end_posX, note_end_posY,
				note_pos1X, note_pos1Y, note_pos2X, note_pos2Y, note_pos3X, note_pos3Y, note_pos4X, note_pos4Y,
				endCnt, rotation;
		public int flickAngle, syncTimes;

		public notes_struct
				(double Ty, double starTi, double steTi, double pTi,
				double nepX, double nepY,
				double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4,
				double endC, double rot, int flickA, int syTi
				)
		{
			noteType = Ty;
			startTime = starTi;
			steamTime = steTi;
			parfectTime = pTi;
			note_end_posX = nepX;
			note_end_posY = nepY;
			note_pos1X = x1;
			note_pos1Y = y1;
			note_pos2X = x2;
			note_pos2Y = y2;
			note_pos3X = x3;
			note_pos3Y = y3;
			note_pos4X = x4;
			note_pos4Y = y4;
			endCnt = endC;
			rotation = rot;
			flickAngle = flickA;
			syncTimes = syTi;
		}


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


}
