using System.Collections.Generic;
using UnityEngine;

public class Score_load : MonoBehaviour
{
	[SerializeField] private Score_data Sd;
	[SerializeField] private Data_cabinet Dc;
	[SerializeField] private Figure_calc Fc;


	/// <summary>
	/// 小節を何等分してるか。p×bの仕様に準拠。
	/// </summary>
	private const int note_resolution = 192;

	/// <summary>
	/// 許容する同時押しの数
	/// </summary>
	const int max_sync_notes = 3;

	/// <summary>
	/// 1拍のcnt。p×bの仕様に準拠すると48で変動なし
	/// </summary>
	private int one_beat_cnt = 48;//4/4拍子の1小節192cntで1拍は192/4=48

	/// <summary>
	/// 1譜面の終わりを実際の最後のノーツから何小節伸ばすか
	/// </summary>
	private int bar_extend = 3;

	/// <summary>
	/// 今見るべきノートリストのインデックス
	/// </summary>
	private int note_list_index = 0;

	/// <summary>
	/// 今見るべきノートリストのインデックスのインクリメント
	/// </summary>
	private void Add_note_list_index () { note_list_index++; }

	/// <summary>
	/// 今見るべきBPMリストのインデックス
	/// </summary>
	private int bpm_list_index = 0;

	private double up_to_time = 0;

	private int sync_notes_limiter = 0;

	private double note_steam_time = 0;

	/// <summary>
	/// long_strict.temp_idとして使う値。重複したくないので-1スタートのカウンターの形を取る
	/// </summary>
	private int long_id_counter = -1;

	/* 選曲画面から渡される値 */
	double HiSpeed = 0;

	float startOffset;
	float perfectOffset;
	string MusicfilesName;

	//以下はノート構造体格納用の一次保管変数
	private double temp_note_time;

	private double[] temp_note_pos;

	private double temp_start_time;

	private double temp_holdend_time;

	private int temp_sync_notes;

	public Long_struct[] long_list;

	/// <summary>
	/// ここでのみ使う計算結果のnote_data_listを保存するための配列。
	/// </summary>
	List<Note_data> temp_note_data_list_line1 = new List<Note_data>();
	List<Note_data> temp_note_data_list_line2 = new List<Note_data>();

	/// <summary>
	/// ロード時に最初に呼ばれる窓口になるメソッド。
	/// </summary>
	public void Main_Load_score(float HS , float startOffset ,float perfectOffset,string MusicfilesName )
	{
		HiSpeed = HS;
		this.startOffset = startOffset;
		this.perfectOffset = perfectOffset;
		this.MusicfilesName = MusicfilesName;
		temp_note_pos = new double[12];
		long_list = new Long_struct[3];//本来はレーンの数にするべき
		Area_pointer();//ここを呼ぶと計算が行われ、temp_note_data_list群に全て結果が入る。
		Transfer_temp_note_data_list();
	}


	/// <summary>
	/// 今見るべき範囲を確定し、Area_pointer_common_calcへ飛ばす
	/// </summary>
	private void Area_pointer()
	{
		for (int bar = 1; Break_term(bar * note_resolution); bar++)//全ての小節を見る
		{

			if (Has_BPM_changed(bar))//小節にBPM変動がある
			{
				
				for (; ; bpm_list_index++)
				{
					if (bpm_list_index == 0)//最初の小節のBPM変動の補正
					{//最初のBPM指定は飛ばさないとarea_timeの計算でbpm_list_index - 1でぬるぽする
					 //それ以外はbpm_list_indexが1以上あるのでぬるぽにならない
						continue;
					}
					if (Witch_near(Sd.BPM_List[bpm_list_index].count) == 0)//BPM変動開始から小節が近い																						 
					{
						Area_pointer_common_calc(1,bar);//パターン1
					}
					else//BPM変動開始から別のBPM変動が近い
					{
						Area_pointer_common_calc(2, bar);//パターン2
					}
					//if (bpm_list_index < Sd.BPM_List.Length)変更前
					if (bpm_list_index == (Sd.BPM_List.Length - 1) || Sd.BPM_List[bpm_list_index + 1].count >= bar * note_resolution)
					{//このようにしている理由:forの条件判定はインクリメントよりも後のため、bpm_list_indexがList.Lengthより多くなった状態でforから抜けてしまい、ぬるぽの原因になる。
					 //そのためインクリメントする前にfor文を抜けさせるためにbreakを使っている
					 //条件1→その小節での最後のBPM変動がそのまま曲中最後のBPM変動だった場合
					 //条件2→今見ているBPM変動の次の変動が次の小節に存在している場合
						break;
					}
				}
				//小節内最後のBPM変動から次の小節までの区間の計算
				Area_pointer_common_calc(3, bar);//パターン3
			}
			else//BPM変動がない
			{
				Area_pointer_common_calc(4, bar);//パターン4
			}
		}
	}





	/// <summary>
	/// 区間のパターンに応じて1区間の時間と区間の1cntあたりの時間を出してNote_search、Long_calcへ飛ばす
	/// </summary>
	/// <param name="pattern">区間パターン</param>
	/// <param name="bar">現在小節</param>
	void Area_pointer_common_calc (int pattern, int bar)
	{
		//↓この2つで1区間(小節)の範囲を指定する
		double head_cnt = 0;
		double foot_cnt = 0;
		double area_cnt = 0;
		double area_time = 0;//この区間の時間
		double area_one_cnt_time = 0;//この区間の1cntの時間

		switch (pattern)
		{
			case 1:
				head_cnt = Near_index("bar", Sd.BPM_List[bpm_list_index].count) * note_resolution;
				break;
			case 2:
				head_cnt = Sd.BPM_List[Near_index("BPM", Sd.BPM_List[bpm_list_index].count)].count;
				break;
			case 3:
				head_cnt = Sd.BPM_List[bpm_list_index].count;
				break;
			case 4:
				head_cnt = (bar - 1) * note_resolution;
				break;
			default:
				break;
		}

		if (pattern == 1 || pattern == 2)
		{
			foot_cnt = Sd.BPM_List[bpm_list_index].count;
		}
		else if (pattern == 3 || pattern == 4)
		{
			foot_cnt = bar * note_resolution;
		}



		if (pattern == 1 || pattern == 2 || pattern == 3)
		{
			area_cnt = foot_cnt - head_cnt;
		}
		else if (pattern == 4)
		{
			//使ってないのでは？
	    double BPM;
			BPM = Sd.BPM_List[Near_index("BPM", foot_cnt)].value;
		}


		switch (pattern)
		{
			case 1:
			case 3:
				area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index].value);
				break;
			case 2:
				area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index - 1].value);
				break;
			case 4:
				area_time = Area_time_calc(note_resolution, Sd.BPM_List[bpm_list_index].value);
				break;
			default:
				break;
		}

		if (pattern == 1 || pattern == 2)
		{
			area_one_cnt_time = Time_par_cnt(area_time, area_cnt);
		}
		else if (pattern == 3 || pattern == 4)
		{
			area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
		}


		Note_search(head_cnt, foot_cnt, area_one_cnt_time);
		Long_calc(2, head_cnt, foot_cnt, area_one_cnt_time);
		up_to_time += area_time;


	}


	/// <summary>
	/// 区間のノートを全て走査する
	/// </summary>
	/// <param name="head_cnt"></param>
	/// <param name="foot_cnt"></param>
	/// <param name="area_one_cnt_time"></param>
	private void Note_search(double head_cnt, double foot_cnt, double area_one_cnt_time)
	{
		//↓区間にノートが存在しているか
		if (head_cnt <= Sd.note_List[note_list_index].count && Sd.note_List[note_list_index].count < foot_cnt)
		{
			for (; ; Add_note_list_index())
			{
				int type = 0;
				if (Sd.note_List[note_list_index].flickAngle != 0 )
				{
					type = 1;//フリック
				}
				if (Sd.note_List[note_list_index].endCnt != 0)
				{
					type = 2;//ホールド
				}
				temp_note_time = Timing_calc(head_cnt, foot_cnt, area_one_cnt_time);
				Long_calc(1, head_cnt, foot_cnt, area_one_cnt_time);
				Fc.Main_figure_calc(type , Sd.note_List[note_list_index].rotation ,//位置角度計算の呼び出し
														Sd.note_List[note_list_index].positionIndex ,
														Sd.note_List[note_list_index].freeX ,
														Sd.note_List[note_list_index].freeY ,
														Sd.note_List[note_list_index].flickAngle
														);
				temp_note_pos = Fc.Get_Note_pos_result();//figure_calcの計算結果を取得
				Sync_note_search();
				temp_start_time = Note_startTime_calc(Note_steamTime_calc());
				Note_data_add();//全ての計算を終えて格納
				if (note_list_index == (Sd.note_List.Length - 1))//今見ているノートが全体の最後
				{
					break;
					//Debug.Log("ff");
				}
				else if (Sd.note_List[note_list_index + 1].count >= foot_cnt)//次のノートは次の区間になっている
				{
					Add_note_list_index();
					break;
				}
			}
		}
	}



	/// <summary>
	/// ノートのタイミングを計算する
	/// </summary>
	/// <param name="head_cnt"></param>
	/// <param name="foot_cnt"></param>
	/// <param name="area_one_cnt_time"></param>
	private double Timing_calc(double head_cnt, double foot_cnt, double area_one_cnt_time)
	{
		double temp_note_time;
		double distance;//区間の頭からノーツまでのcnt距離
		distance = Sd.note_List[note_list_index].count - head_cnt;
		//Debug.Log("range " + distance);
		temp_note_time = distance * area_one_cnt_time + up_to_time;
		//Debug.Log("temp_note_time " + temp_note_time + " " + note_list_index);//これがノーツ位置、これとup_to_time
		return temp_note_time;
	}


	/// <summary>
	/// ロングノートの計算を行う。
	/// </summary>
	/// <param name="mode">ノーツ処理中に呼び出す(1)か、区間を抜けるときに呼び出すか(2)</param>
	/// <param name="head_cnt"></param>
	/// <param name="foot_cnt">区間最後のcnt</param>
	/// <param name="area_one_cnt_time">区間の1cntあたりの時間</param>
	private void Long_calc ( int mode , double head_cnt , double foot_cnt , double area_one_cnt_time )
	{
		if (mode == 1)//ノーツ処理中に呼び出す
		{
			if (Sd.note_List[note_list_index].endCnt != 0)//ホールドである
			{
				if (Sd.note_List[note_list_index].endCnt > head_cnt && Sd.note_List[note_list_index].endCnt <= foot_cnt)//区間内
				{
					double distance = Sd.note_List[note_list_index].endCnt - head_cnt;//区間先頭からホールド終了までの距離
					temp_holdend_time = up_to_time + ( distance * area_one_cnt_time );//区間先頭の時間＋ホールド終了の時間
					Debug.Log(temp_holdend_time);
				}
				else
				{
					long_list[Sd.note_List[note_list_index].part].state = 1;
					long_list[Sd.note_List[note_list_index].part].note_list_index = note_list_index;
					long_list[Sd.note_List[note_list_index].part].end_cnt = Sd.note_List[note_list_index].endCnt;
					long_list[Sd.note_List[note_list_index].part].temp_id = long_id_counter;
					temp_holdend_time = long_id_counter;
					long_id_counter--;
				}
			}
		}
		else if (mode == 2)//こっちが呼ばれないっぽい
		{
			for (int i = 0; i < long_list.Length; i++)
			{
				if (long_list[i].state == 1)//ホールドがある区間で呼ばれた場合
				{
					long_list[i].state = 2;
				}
				else if (long_list[i].state == 2)
				{
					if (long_list[i].end_cnt > head_cnt && long_list[i].end_cnt <= foot_cnt)//この区間でホールド終了
					{
						double distance = long_list[i].end_cnt - head_cnt;//区間先頭からホールド終了までの距離
						double temp_holdend_time = up_to_time + ( distance * area_one_cnt_time );//区間先頭の時間＋ホールド終了の時間
						Debug.Log(temp_holdend_time);
						//このtemp_holdend_timeをlong_list[i].temp_id とtemp_note_data_list_line1/2.hold_end_timeが一致するかで探して格納する
						for (int j = 0; j < temp_note_data_list_line1.Count; j++)
						{
							if (long_list[i].temp_id == temp_note_data_list_line1[j].hold_end_time)
							{
								temp_note_data_list_line1[j].hold_end_time = temp_holdend_time + Sd.offset + startOffset;
								Debug.Log("hold_end_time "  + temp_note_data_list_line1[j].hold_end_time);
								Debug.Log("parfectTime " + temp_note_data_list_line1[j].parfectTime);
							}
						}







					}
				}
			}
		}
	}



	/// <summary>
	/// 同時押しノートを探す
	/// </summary>
	void Sync_note_search()
	{
		temp_sync_notes = 0;
		if (note_list_index != 0)//最初のノート以外
		{
			if (Sd.note_List[note_list_index].count == Sd.note_List[note_list_index - 1].count)
			{
				temp_sync_notes = 1;
			}
		}
	}

	/*-------------------------------以下補助使用メソッド---------------------------------------*/


	/// <summary>
	/// Area_pointer for文の条件判定式。長過ぎるので切り出した
	/// </summary>
	/// <param name="foot_cnt"></param>
	/// <returns></returns>
	private bool Break_term(int foot_cnt)
	{
		bool continues;
		if (foot_cnt <= (Sd.note_List[Sd.note_List.Length - 1].count + note_resolution * bar_extend))
		{
			continues = true;
		}
		else
		{
			continues = false;
		}
		return continues;
	}


	/// <summary>
	/// BPM変化があるか調べる
	/// </summary>
	/// <param name="bar">現在の小節</param>
	/// <returns></returns>
	private bool Has_BPM_changed(int bar)
	{
		bool has_change;
		int change_count = 0;
		double head_area_cnt = (bar - 1) * note_resolution;
		double foot_area_cnt = bar * note_resolution;
		for (int i = 0; i < Sd.BPM_List.Length; i++)
		{  //その小節の中にBPM変動があるか
			if (head_area_cnt <= Sd.BPM_List[i].count && Sd.BPM_List[i].count < foot_area_cnt)
			{
				change_count++;
			}
		}
		if (bar == 1)//最初のBPM指定をBPM変化とみなさない補正
		{
			change_count -= 1;
		}
		if (change_count > 0)
		{
			has_change = true;
		}
		else
		{
			has_change = false;
		}
		//Debug.Log("has_change " + has_change);
		return has_change;
	}

	/// <summary>
	/// cntから近いのが小節なのか別のBPM変動なのかを調べる。0は小節1はBPM、同じならBPMが返る
	/// </summary>
	/// <param name="point_cnt">point_cnt</param>
	/// <returns></returns>
	private int Witch_near(double point_cnt)
	{
		int answer = 0;//0は小節1はBPM
		double point_bar_cnt;
		double point_BPM_cnt;
		point_bar_cnt = note_resolution * Near_index("bar", point_cnt);
		int BPM_index = Near_index("BPM", point_cnt);
		//Debug.Log("BPM_index " + BPM_index);
		point_BPM_cnt = Sd.BPM_List[BPM_index].count;
		if (point_bar_cnt > point_BPM_cnt)//数が大きい方が近い
		{
			answer = 0;
		}
		else
		{
			answer = 1;
		}
		return answer;
	}

	/// <summary>
	/// 1区間の時間を計算して返す
	/// </summary>
	/// <param name="area_cnt"></param>
	/// <param name="BPM"></param>
	/// <returns></returns>
	private double Area_time_calc(double area_cnt, double BPM)
	{
		double area_time;
		double beat = 4;//拍子。pxbpは変拍子非対応なのでとりあえずは4固定。
										//60*4*{ (その区間のcnt/48=拍数)/4(拍子) }/その区間のBPM
		area_time = 60 * beat * ((area_cnt / one_beat_cnt) / 4) / BPM;

		return area_time;
	}

	/// <summary>
	/// その区間の1cntあたりの時間を返す
	/// </summary>
	/// <param name="area_time"></param>
	/// <param name="cnt"></param>
	/// <returns></returns>
	private double Time_par_cnt(double area_time, double cnt)
	{
		double one_cnt_time;
		one_cnt_time = area_time / cnt;
		return one_cnt_time;
	}

	/// <summary>
	/// 引数として与えたcntに最も近いindexを返す
	/// ここでの「近い」は直前の意
	/// </summary>
	/// <param name="mode">bar or BPM(bpm)</param>
	/// <param name="point_cnt">point_cnt</param>
	/// <returns></returns>
	private int Near_index(string mode, double point_cnt)
	{
		//modeは1→小節 2がBPM
		int index = 0;
		if (mode == "bar")//最も近い小節を返す
		{
			index = (int)(point_cnt / note_resolution);
		}
		else if (mode == "bpm" || mode == "BPM")//最も近いBPM帯を返す
		{
			for (int i = 0; i < Sd.BPM_List.Length; i++)
			{
				if (Sd.BPM_List.Length == 1)//曲中でBPM変動が一切ない
				{
					index = 0;
					break;
				}
				if (Sd.BPM_List[i].count >= point_cnt)//見てるBPM変動が与えられたcntを超えたら
				{
					if (i == 0)
					{
						index = 0;
					}
					else
					{
						index = i - 1;//その1つ前がインデックス
					}
					break;
				}
				else if (i == (Sd.BPM_List.Length - 1) && Sd.BPM_List[i].count <= point_cnt)//ノートの前までにBPM変動が全て終わっている場合
				{
					index = i;
				}
			}
		}
		return index;
	}


	/// <summary>
	/// 音符を流すのにかかる時間を計算
	/// </summary>
	/// <returns></returns>
	double Note_steamTime_calc()
	{
		double Now_BPM = Sd.BPM_List[bpm_list_index].value;
		double base_steam_time = 3;
		double x; //基準bpmからの倍率
		x = 100 / Now_BPM;
		note_steam_time = base_steam_time * x / HiSpeed; //bpm100,HS1のとき3秒かけて流れる
		return note_steam_time;
	}


	/// <summary>
	/// 音符を流し始める時間を計算
	/// </summary>
	/// <param name="steam_time"></param>
	/// <returns></returns>
	double Note_startTime_calc(double steam_time)
	{
		double temp_start_time;
			temp_start_time = temp_note_time - steam_time;
		return temp_start_time;
	}

	


	private void Note_data_add()
	{
		Note_data note_Data = new Note_data();
		int noteType = 0;//タッチ
		if (Sd.note_List[note_list_index].flickAngle != 0)
		{
			noteType = 1;//フリック
		}
		else if (Sd.note_List[note_list_index].endCnt != 0)
		{
			noteType = 2;//ホールド
		}
		note_Data.noteType       = noteType;
		note_Data.startTime      = temp_start_time + Sd.offset + startOffset;   //temp_start_time + Sd.offset
		note_Data.steamTime      = (float)note_steam_time;
		note_Data.parfectTime    = (float)( temp_note_time + Sd.offset + perfectOffset );
		note_Data.note_end_pos.x = (float)temp_note_pos[0];
		note_Data.note_end_pos.y = (float)temp_note_pos[1];
		note_Data.note_pos1.x    = (float)temp_note_pos[2];
		note_Data.note_pos1.y    = (float)temp_note_pos[3];
		note_Data.note_pos2.x    = (float)temp_note_pos[4];
		note_Data.note_pos2.y    = (float)temp_note_pos[5];


		if (noteType == 2)
		{
			note_Data.note_pos3.x  = (float)temp_note_pos[6];
			note_Data.note_pos3.y  = (float)temp_note_pos[7];
			note_Data.note_pos4.x  = (float)temp_note_pos[8];
			note_Data.note_pos4.y  = (float)temp_note_pos[9];
		}
		if (noteType == 1)//フリック
		{
			note_Data.flick_pos.x = (float)temp_note_pos[10];
			note_Data.flick_pos.y = (float)temp_note_pos[11];
			//Debug.Log("note_list_index " + note_list_index);
			//Debug.Log("flick_x " + note_Data.flick_pos.x);
			//Debug.Log("flick_y " + note_Data.flick_pos.y);
		}
		if (temp_holdend_time < 0)//temp_idと揃えるためにマイナスの値が入っている場合
		{
			note_Data.hold_end_time = temp_holdend_time;
		}
		else
		{
			note_Data.hold_end_time = temp_holdend_time + Sd.offset + startOffset;
		}
		note_Data.rotation       = ( -1 ) * (float)Sd.note_List[note_list_index].rotation;
		note_Data.syncTimes      = temp_sync_notes;
		note_Data.judged         = false;
		note_Data.made           = false;


		switch (temp_sync_notes)//同時押しによって挿入先を変える
		{
			case 0:
				temp_note_data_list_line1.Add(note_Data);
				//Debug_note_data_show(1 , note_Data);
				break;
			case 1:
				temp_note_data_list_line2.Add(note_Data);
				//Debug_note_data_show(2 , note_Data);
				break;
			case 3:
				break;
			default:
				break;
		}
		
	}

	

	/// <summary>
	/// 譜面中のノートの計算終了後、Dcへとnote_data_listを受け渡すためのメソッド
	/// </summary>
	private void Transfer_temp_note_data_list ()
	{
		Dc.ToCreate_Note_data_list(temp_note_data_list_line1.Count , temp_note_data_list_line2.Count);
		Dc.ToCreate_note_made_List(Sd.note_List.Length);//note_made_listのサイズは今のところ譜面中の全ノーツの数にしておく

		for (int i = 0; i < temp_note_data_list_line1.Count; i++)
		{
			Dc.Note_data_list_line1[i] = temp_note_data_list_line1[i];
			//Debug.Log(i);
			//Debug_note_data_show(1 , temp_note_data_list_line1[i]);
		}

		for (int i = 0; i < temp_note_data_list_line2.Count; i++)
		{
			Dc.Note_data_list_line2[i] = temp_note_data_list_line2[i];
			//Debug.Log(i);
			//Debug_note_data_show(2 , temp_note_data_list_line2[i]);
		}
		Dc.Set_MusicfileName(MusicfilesName);
	}


	/// <summary>
	/// ロングノーツ計算のための構造体
	/// </summary>
	public struct Long_struct
	{ //part(トラック)は配列のインデックスで判定する
		public int state, note_list_index, temp_id;
		public double end_cnt;

		public Long_struct
			(int note_list_index , int state ,int temp_id , double end_cnt )
		{
			this.note_list_index = note_list_index;
			this.state = state;
			this.end_cnt = end_cnt;
			this.temp_id = temp_id;
			//↑ロングの開始と終了が同区間でない場合終了秒にこのIDを突っ込み、後から挿入する際のキーとする
		}
	}

	
	void Debug_note_data_show (int line , Note_data note_Data)
	{
		Debug.Log("insert line " + line);
		//Debug.Log("startTime " +  note_Data.startTime);
		//Debug.Log("steamTime " + note_Data.steamTime);
		Debug.Log("parfectTime " + note_Data.parfectTime);
		Debug.Log("note_end_pos.x " + note_Data.note_end_pos.x);
		Debug.Log("note_end_pos.y " + note_Data.note_end_pos.y);
		//Debug.Log("note_pos1.x " + note_Data.note_pos1.x);
		//Debug.Log("note_pos1.y " + note_Data.note_pos1.y);
		//Debug.Log("note_pos2.x " + note_Data.note_pos2.x);
		//Debug.Log("note_pos2.y " + note_Data.note_pos2.y);
		//Debug.Log("note_pos3.x " + note_Data.note_pos3.x);
		//Debug.Log("note_pos3.y " + note_Data.note_pos3.y);
		//Debug.Log("note_pos4.x " + note_Data.note_pos4.x);
		//Debug.Log("note_pos4.y " + note_Data.note_pos4.y);
		//Debug.Log("endCnt " + note_Data.endCnt);
		//Debug.Log("rotation " + note_Data.rotation);
		//Debug.Log("flickAngle " + note_Data.flickAngle);
		//Debug.Log("startTime " + note_Data.startTime);
		//Debug.Log("syncTimes " + note_Data.syncTimes);
		//Debug.Log("alive " + note_Data.judged);
		//Debug.Log("made " + note_Data.made);
		Debug.Log("-------------------------------------");
	}
	
}