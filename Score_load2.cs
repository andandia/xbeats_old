using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_load2 : MonoBehaviour {
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
    /// 今見るべきノートリストのインデックス
    /// </summary>
    int note_list_index = 0;
    /// <summary>
    /// 今見るべきBPMリストのインデックス
    /// </summary>
    int bpm_list_index = 0;


    double up_to_time = 0;//ここまでの時間

    public void Load_score()
    {
        Dc.make_notes_List(Sd.note_List.Length);

        Timing_calc();

    }

    void Timing_calc()
    {
        for (; bpm_list_index < Sd.BPM_List.Length; bpm_list_index++)
        {
            double area_time = 0;//この区間の時間
            double area_one_cnt_time = 0;//この区間の1cntの時間
            if (Sd.BPM_List.Length == 1)
            {
                area_time = Area_time_calc(note_resolution, Sd.BPM_List[bpm_list_index].value);
                area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                Debug.Log("area_one_cnt_time " + area_one_cnt_time);
            }
            else
            {
                double area_cnt = 0;
                if (bpm_list_index == 0)
                {
                    area_cnt =  Sd.BPM_List[bpm_list_index + 1].count;
                }
                else
                {
                    area_cnt = Sd.BPM_List[bpm_list_index].count - Sd.BPM_List[bpm_list_index - 1].count;
                }
                area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index].value);
                area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                Debug.Log("area_time " + area_time);
            }


            for (; note_list_index < Sd.note_List.Length; note_list_index++)
            {
                double debug_time;//これを最終的にノート時間として入れる
                double from_bar_cnt = Sd.note_List[note_list_index].count - Sd.BPM_List[bpm_list_index].count;
                debug_time = Cnt_to_time(up_to_time, from_bar_cnt, area_one_cnt_time);
                Debug.Log("debug_time " + debug_time);
                if (note_list_index != Sd.note_List.Length - 1 )//note_listの最後でない
                {
                    if (Sd.note_List[note_list_index + 1 ].count >= Sd.BPM_List[bpm_list_index + 1].count)//次のノートが次の区間にある
                    {
                        note_list_index++;
                        Debug.Log("break " + note_list_index);
                        break;
                    }
                }

            }




            up_to_time += area_time;//次の区間に移るときにこの区間の時間を足しておく
            Debug.Log("up_to_time " + up_to_time);
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
    /// <param name="from_area_cnt">区間の頭からノートまでのcnt</param>
    /// <param name="one_cnt_time"></param>
    /// <returns></returns>
    double Cnt_to_time(double up_to_time, double from_area_cnt, double one_cnt_time)
    {
        double time;
        //時間＝小節の頭からノートまでのcnt×1cntあたりの時間＋ここまでの時間
        time = (from_area_cnt * one_cnt_time) + up_to_time;
        return time;
    }

}
