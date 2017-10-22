using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_load1 : MonoBehaviour
{

    [SerializeField] Score_data Sd;
    [SerializeField] Data_cabinet Dc;

    /// <summary>
    /// 小節を何等分してるか。p×bの仕様に準拠。
    /// </summary>
    const int note_resolution = 192;
    /// <summary>
    /// 1拍のcnt。p×bの仕様に準拠すると48で変動なし
    /// </summary>
    int one_beat_cnt = 48;//4/4拍子の1小節192cntで1拍は192/4=48

    /// <summary>
    /// 現在の小節
    /// </summary>
    int bar = 1;//現在の小節
    /// <summary>
    /// 現在の小節の最大cnt(＝次の小節の頭のcntになるので＜を使うこと)
    /// </summary>
    int bar_cnt;
    ///// <summary>
    ///// その小節でBPM変動が起こっているか
    ///// </summary>
    //bool is_bar_BPMchanged;

    /// <summary>
    /// 今見るべきノートリストのインデックス
    /// </summary>
    int note_list_index = 0;
    /// <summary>
    /// 今見るべきBPMリストのインデックス
    /// </summary>
    int bpm_list_index = 0;

    double up_to_time = 0;//ここまでの時間

    /// <summary>
    /// 各小節ごとの区間の(区間の実質的な意味ははBPM変動ありなしで異なる)時間のリスト
    /// </summary>
    List<double> area_time_list = new List<double>();


    public void Load_score()
    {
        Dc.make_notes_List(Sd.note_List.Length);

        Timing_calc();

    }




    /// <summary>
    /// ノートのタイミング計算
    /// </summary>
    void Timing_calc()
    {
        // Debug.Log("near_index " + near_index(2, 96));
        Debug.Log(Bar_time_calc(2));
        

        bar_cnt = bar * note_resolution;//最初の小節のcntを出す
        for (; note_list_index < Sd.note_List.Length; note_list_index++)
        {
            if (Sd.note_List[note_list_index].count >= bar_cnt)//次の小節の移行処理
            {
                bar++;
                bar_cnt = bar * note_resolution;
                note_list_index--;//continueでforをすすめると++になるので補正

                continue;
            }
            if (Has_BPM_changed(bar))//現在小節にBPM変動あり
            {
                //ここ部分はちょっと練り直し、感覚で作ってるのでちゃんと理解する
                if (bpm_list_index == 0)
                {
                    bpm_list_index++;//最初のBPM指定の補正
                }
                for (; bpm_list_index < Sd.BPM_List.Length; bpm_list_index++)
                {
                    if (Sd.BPM_List[bpm_list_index].count > Sd.note_List[note_list_index].count)
                    {
                        Debug.Log(Sd.BPM_List[bpm_list_index].count);
                        break;
                    }
                    else if (bpm_list_index == (Sd.BPM_List.Length - 1))
                    {
                        Debug.Log(Sd.BPM_List[bpm_list_index].count);
                    }
                }

            }
            else//なし
            {
                if (Sd.note_List[note_list_index].count < bar_cnt)
                {
                    //↓どの場合でも1小節のcntは192(note_resolution)固定なので投げるのはnote_resolution
                    double area_time = Area_time_calc(note_resolution, Sd.BPM_List[bpm_list_index].value);//小節時間の算出
                    double one_cnt_time = Time_par_cnt(area_time, bar_cnt);
                    Debug.Log(Cnt_to_time(up_to_time, Sd.note_List[note_list_index].count, one_cnt_time));
                    double from_bar_cnt = Sd.note_List[note_list_index].count - ((bar-1)*note_resolution);
                    Debug.Log("from_bar_cnt " + from_bar_cnt);
                    Dc.notes_List[note_list_index].time = Cnt_to_time(up_to_time, from_bar_cnt, one_cnt_time);
                }

            }




        }
    }












    /// <summary>
    /// 1区間の時間を計算して返す
    /// </summary>
    /// <param name="area_cnt"></param>
    /// <param name="BPM"></param>
    /// <returns></returns>
    double Area_time_calc(double area_cnt, double BPM)
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
    double Time_par_cnt(double area_time, double cnt)
    {
        double one_cnt_time;
        one_cnt_time = area_time / cnt;
        return one_cnt_time;
    }


    /// <summary>
    /// cntからノートの位置する時間を返す
    /// </summary>
    /// <param name="up_to_time">ここまでの時間</param>
    /// <param name="from_bar_cnt">小節の頭からノートまでのcnt</param>
    /// <param name="one_cnt_time"></param>
    /// <returns></returns>
    double Cnt_to_time(double up_to_time, double from_bar_cnt, double one_cnt_time)
    {
        double time;
        //時間＝小節の頭からノートまでのcnt×1cntあたりの時間＋ここまでの時間
        time = (from_bar_cnt * one_cnt_time) + up_to_time;
        return time;
    }


    /// <summary>
    /// BPM変化があるか調べる
    /// </summary>
    /// <param name="bar">現在の小節</param>
    /// <returns></returns>
    bool Has_BPM_changed(int bar)
    {
        bool has_change;
        int change_count = 0;
        double area_cnt_max = bar * note_resolution;
        double area_cnt_min = (bar - 1) * note_resolution;
        for (int i = 0; i < Sd.BPM_List.Length; i++)
        {  //その小節の中にBPM変動があるか
            if (area_cnt_min <= Sd.BPM_List[i].count && Sd.BPM_List[i].count < area_cnt_max)
            {
                change_count++;
            }
        }
        if (bar == 1) { change_count -= 1; }//最初のBPM指定の補正
        if (change_count > 0)
        {
            has_change = true;
        }
        else
        {
           has_change = false;
        }
        Debug.Log("has_change " + has_change);
        return has_change;
    }

    /// <summary>
    /// このメソッド必要なさそう BPM変化が本当にそのノートが属しているBPM帯なのかを調べて返す
    /// </summary>
    /// <param name="b">呼び出し元のbに同じ</param>
    /// <param name="i">呼び出し元のiに同じ</param>
    /// <returns></returns>
    bool isNote_placed_BPM(int b , int i)
    {
        bool isPlaced = false;//属せばtrue
        //Sd.BPM_List[b].count <= Sd.notes_List[i].count;
         return isPlaced;
    }



    /// <summary>
    /// ある区間のcntを返す
    /// </summary>
    /// <param name="end_cnt"></param>
    /// <param name="bar"></param>
    /// <returns></returns>
    double Area_cnt_calc(double end_cnt, int bar)
    {
        double latest_bar_cnt = 0;
        double latest_cBPM_cnt = 0;

        latest_bar_cnt = (bar - 1) * note_resolution;
        for (int i = 0; i < Sd.BPM_List.Length; i++)
        {
            if (Sd.BPM_List[i].count > end_cnt)
            {
                latest_cBPM_cnt = Sd.BPM_List[i - 1].count;
            }
        }
        return latest_bar_cnt > latest_cBPM_cnt ? latest_bar_cnt : latest_cBPM_cnt;//大きい方を返す

    }



    /// <summary>
    /// その小節の時間を出す
    /// </summary>
    /// <returns></returns>
    double Bar_time_calc(int bar)
    {
        double up_to_time = 0;
        List<double> area_time_list = new List<double>();
        if (Has_BPM_changed(bar))//BPM変動あり
        {
            int head_index = near_index("BPM", (bar-1)*note_resolution);//その小節の頭に近いBPMのインデックス
            Debug.Log("head_index "+ head_index);
            //int foot_index = near_index("bar", bar * note_resolution);//その小節次の小節にの頭に近いBPMのインデックス
            Debug.Log("area_time " + Area_time_calc(Sd.BPM_List[head_index+1].count -((bar - 1) * note_resolution), Sd.BPM_List[head_index].value) );
            //10/22 ↑で「小節の頭から次のBPM変動」までの時間を算出できた
            //小節の頭の後のBPM変動から小節の終わりの一つ手前までのBPM変動までをforで回して時間を出す
            area_time_list.Add(Area_time_calc(Sd.BPM_List[head_index + 1].count - ((bar - 1) * note_resolution), Sd.BPM_List[head_index].value));
            for (int i = head_index+1; i < Sd.BPM_List.Length; i++)
            {
                

            }







        }
        else//BPM変動なし
        {
            double BPM = 0;
            BPM = Sd.BPM_List[near_index("BPM", bar * note_resolution)].value;
            Debug.Log("BPM " + BPM);
            up_to_time = Area_time_calc(note_resolution, BPM);
        }
        return up_to_time;
    }


   /// <summary>
   /// 引数として与えたcntに最も近いindexを返す
   /// ここでの「近い」は直前の意
   /// </summary>
   /// <param name="mode">bar or BPM(bpm)</param>
   /// <param name="cnt">cnt</param>
   /// <returns></returns>
    int near_index(string mode ,double cnt)
    {
        //modeは1→小節 2がBPM
        int index = 0;
        if (mode == "bar")//最も近い小節を返す
        {
            index = (int)(cnt / note_resolution);
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
                if (Sd.BPM_List[i].count >= cnt)//見てるBPM変動が与えられたcntを超えたら
                {
                    index = i-1;//その1つ前がインデックス
                    break;
                }
                else if(i == (Sd.BPM_List.Length-1) && Sd.BPM_List[i].count <= cnt)//ノートの前までにBPM変動が全て終わっている場合
                {
                    index = i;
                }
            }
        }
        return index;
    }












}
