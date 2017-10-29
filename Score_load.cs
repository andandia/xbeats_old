using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score_load : MonoBehaviour
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
    /// 1譜面の終わりを実際の最後のノーツから何小節伸ばすか
    /// </summary>
    int bar_extend = 3;




    /// <summary>
    /// 今見るべきノートリストのインデックス
    /// </summary>
    int note_list_index = 0;
    /// <summary>
    /// 今見るべきBPMリストのインデックス
    /// </summary>
    int bpm_list_index = 0;



    public void Load_score()
    {
        Dc.make_notes_List(Sd.note_List.Length);
        Calc_setup();
        Timing_calc();

    }


    void Calc_setup()
    {
        for (int bar = 1;Break_term(bar * note_resolution);bar++)//全ての小節を見る
        {//↓この2つで1区間(小節)の範囲を指定する
            double head_cnt;
            double foot_cnt;

            if (Has_BPM_changed(bar))//小節にBPM変動がある
            {
                double area_cnt;
                double area_time = 0;//この区間の時間
                double area_one_cnt_time = 0;//この区間の1cntの時間
                for (; ; bpm_list_index++)
                {
                    if (bpm_list_index == 0)//最初の小節のBPM変動の補正
                    {//最初のBPM指定は飛ばさないとarea_timeの計算でbpm_list_index - 1でぬるぽする
                        //それ以外はbpm_list_indexが1以上あるのでぬるぽにならない
                        continue;
                    }
                    if (Witch_near(Sd.BPM_List[bpm_list_index].count) == 0)//BPM変動開始から小節が近い
                    {
                        head_cnt = Near_index("bar", Sd.BPM_List[bpm_list_index].count) * note_resolution;
                        foot_cnt =  Sd.BPM_List[bpm_list_index].count;
                        area_cnt = foot_cnt - head_cnt;
                        area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index].value);
                        //もし変拍子対応を入れ込むならここ(Area_time_calcを拡張)
                        area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                        Debug.Log("area_time " + area_time);
                    }
                    else//BPM変動開始から別のBPM変動が近い
                    {
                        head_cnt = Sd.BPM_List[Near_index("BPM", Sd.BPM_List[bpm_list_index].count)].count;
                        foot_cnt = Sd.BPM_List[bpm_list_index].count;
                        area_cnt = foot_cnt - head_cnt;
                        area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index - 1].value);
                        //もし変拍子対応を入れ込むならここ(Area_time_calcを拡張)
                        area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                        Debug.Log("area_time " + area_time);
                    }
                    //if (bpm_list_index < Sd.BPM_List.Length)変更前
                    if (bpm_list_index == (Sd.BPM_List.Length -1) || Sd.BPM_List[bpm_list_index+1].count >= bar * note_resolution)
                    {//このようにしている理由:forの条件判定はインクリメントよりも後のため、bpm_list_indexがList.Lengthより多くなった状態でforから抜けてしまい、ぬるぽの原因になる。
                      //そのためインクリメントする前にfor文を抜けさせるためにbreakを使っている
                      //条件1→その小節での最後のBPM変動がそのまま曲中最後のBPM変動だった場合
                      //条件2→今見ているBPM変動の次の変動が次の小節に存在している場合
                        break;
                    }
                    
                }
                //小節内最後のBPM変動から次の小節までの区間の計算
                Debug.Log("小節内最後のBPM変動から次の小節までの区間の計算");
                head_cnt = Sd.BPM_List[bpm_list_index].count;
                foot_cnt = bar * note_resolution;
                area_cnt = foot_cnt - head_cnt;
                area_time = Area_time_calc(area_cnt, Sd.BPM_List[bpm_list_index].value);
                area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                Debug.Log("area_time " + area_time);

                Debug.Log("bpm_list_index " + bpm_list_index);
            }
            else//BPM変動がない
            {
                head_cnt = (bar - 1) * note_resolution;
                foot_cnt = bar * note_resolution;
                double BPM;
                double area_time = 0;//この区間の時間
                double area_one_cnt_time = 0;//この区間の1cntの時間
                BPM = Sd.BPM_List[Near_index("BPM", foot_cnt)].value;
                area_time = Area_time_calc(note_resolution, Sd.BPM_List[bpm_list_index].value);
                //もし変拍子対応を入れ込むならここ(area_timeを変化させる)
                area_one_cnt_time = Time_par_cnt(area_time, note_resolution);
                Debug.Log("area_time " + area_time);

            }











        
        }






    }



    void Timing_calc()
    {

    }


    //for文の条件判定式。長過ぎるので切り出した
    bool Break_term(int foot_cnt)
    {
        bool isbreak;
        if (foot_cnt <= (Sd.note_List[Sd.note_List.Length - 1].count + note_resolution * bar_extend))
        {
            isbreak = true;
        }
        else
        {
            isbreak = false;
        }
        return isbreak;
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
    int Witch_near(double point_cnt)
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
    /// 引数として与えたcntに最も近いindexを返す
    /// ここでの「近い」は直前の意
    /// </summary>
    /// <param name="mode">bar or BPM(bpm)</param>
    /// <param name="point_cnt">point_cnt</param>
    /// <returns></returns>
    int Near_index(string mode, double point_cnt)
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

















}
