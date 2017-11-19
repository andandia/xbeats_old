using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure_calc : MonoBehaviour
{
    /*メモ　ホールドでも使えるようにしなければならない
    ホールドの場合
    傾きは一緒。全画面端との交点を計算しなければならない
    改修すべきところ
    ・Pos_decide
    ホールドでの場合わけintを追加して全交点を計算できるように拡張
    ・is_inside_area
    ホールドでの場合わけintを追加して全交点を考慮できるように拡張
    ・Note_line
    さらに2点分出現位置を追加
    */



    Display_size display_size;
    Note_line note_line;


    double disp_width_x, disp_width_y;
    double pos_unit_x, pos_unit_y;


    /*
    double[] point_position = new double {



    };
    */

    // Use this for initialization
    void Start()
    {
        Main_figure_calc(1, 22.5, -2, 2);//テスト用

    }

    //最終的にはここを呼び出すだけで座標周りの計算ができるようにする
    void Main_figure_calc(int type, double rotation, double through_x, double through_y)
    {//type=1：タッチノート 2:ホールドノート
        //画面サイズの設定
        display_size.xMax = 13.65;
        display_size.xMin = -13.65;
        display_size.yMax = 7.68;
        display_size.yMin = -7.68;
        //画面幅と1点ごとの増加量の設定
        double adjust_x = 14.76;
        double adjust_y = 3.36;
        disp_width_x = display_size.xMax - display_size.xMin - adjust_x;
        disp_width_y = display_size.yMax - display_size.yMin - adjust_y;
        pos_unit_x = pos_unit_x / 6;
        pos_unit_y = pos_unit_y / 6;
        Debug.Log("ddddd");
        Convert_worldpos(8, 0, 0);//デバック用

        Angle_calc(rotation);
        Pos_decide(rotation, through_x, through_y);
        //Equation_answer("y", display_size.xMin, display_size.yMax,-2,2,2);
        //Equation_answer("y", display_size.xMin, display_size.yMax, 0.5, 3, 2);
    }




    void Angle_calc(double rotation)
    {
        double one = rotation;//パターン1における、rotationで示されている方の角度
        double two = rotation + 90;
        //if (rotation  < 90){
        //    two = 180 - rotation - 90;
        //    one = 180 - rotation;
        //}
        //else if (rotation <180)
        //{
        //    two = 180 - rotation;
        //    one = two + 90;
        //}
        //else if (rotation < 270)
        //{
        //    double a = rotation - 180;
        //    two = 90 - a;
        //    one = two + 90;
        //}
        //else if (rotation < 360)
        //{
        //    two = rotation - 270;
        //    one = two + 90;
        //}
        //Note_line.angle_one = one;
        //Note_line.angle_two = two;
        note_line.slope_one = Tan_calc(one) * -1;
        note_line.slope_two = Tan_calc(two) * -1;
    }




    void Pos_decide(double rotation, double through_x, double through_y)
    {
        /*解決すべき点
         * 1.oneとtwoでfixed_x,yに入れるべき値が変わってしまい、長ったらしくなる。rotation180のときtwoはrotation90のやつを入れたらよかったりしないか
         * 範囲を超えたかの判断がfixed_x,yに何が入ってるかで変わってしまう。わかりやすく短く書けないか
         1：ro90のときにtwoはro180を入れたらよいことが分かったので、
         for文で2回回す。if文に回数及びrotationでの条件追加をしてif(i == 1 && rotation  < 90 || i == 2 && rotation  < 180)のようにすれば
         if1つでone two両方に対応できる
         2：判定を別メソッドへ切り出し、引数で変えるようにする。引数はfor文中のif内で代入しておく
         1.2について解決したはず
         
        */
        double fixed_x = 0;
        double fixed_y = 0;
        int cross_pattern = 0;
        for (int i = 1; i < 3; i++)
        {
            if (i == 1 && rotation < 90 || i == 2 && rotation < 360)
            {
                fixed_x = display_size.xMin;
                fixed_y = display_size.yMax;
                cross_pattern = 1;
            }
            else if (i == 1 && rotation < 180 || i == 2 && rotation < 90)
            {
                fixed_x = display_size.xMax;
                fixed_y = display_size.yMax;
                cross_pattern = 2;
            }
            else if (i == 1 && rotation < 270 || i == 2 && rotation < 180)
            {
                fixed_x = display_size.xMax;
                fixed_y = display_size.yMin;
                cross_pattern = 3;
            }
            else if (i == 1 && rotation < 360 || i == 2 && rotation < 270)
            {
                fixed_x = display_size.xMin;
                fixed_y = display_size.yMin;
                cross_pattern = 4;
            }
            string answer_side = "x";
            double temp_pos_move = Equation_answer(answer_side, fixed_x, fixed_y, note_line.slope_one, through_x, through_y);
            double temp_pos_fixed = fixed_y;
            if (is_inside_area(cross_pattern, temp_pos_fixed) == false)
            {
                answer_side = "y";
                temp_pos_move = Equation_answer(answer_side, fixed_x, fixed_y, note_line.slope_one, through_x, through_y);
                temp_pos_fixed = fixed_x;
            }
            if (i == 1 && answer_side == "x")
            {//x軸との交点が始点
                note_line.note_pos_one_x = temp_pos_move;
                note_line.note_pos_one_y = temp_pos_fixed;
            }
            else if (i == 1 && answer_side == "y")
            {
                note_line.note_pos_one_x = temp_pos_fixed;
                note_line.note_pos_one_y = temp_pos_move;
            }
            if (i == 2 && answer_side == "x")
            {//x軸との交点が始点
                note_line.note_pos_two_x = temp_pos_move;
                note_line.note_pos_two_y = temp_pos_fixed;
            }
            else if (i == 2 && answer_side == "y")
            {
                note_line.note_pos_two_x = temp_pos_fixed;
                note_line.note_pos_two_y = temp_pos_move;
            }
        }
        /*関係ないメモ
		タッチに関して、最大タッチ数(=プログラム側で受け付けるタッチの数)は
		ゲーム側想定の最大タッチ数*2でなければならない
		理由は、ロングノートを最大3点タッチしつづけて、終点と同時に3点のタッチノートが来た場合、
		3点をホールドしたまま3点をタッチするという動作をすることが考えられるからである。(できるかはともかく)
		もし3点しかプログラム側で認識しなければタッチノートの方はまったく反応もしないことになってしまう。
		*/


    }




    //方程式の代入結果を返す
    double Equation_answer(string return_hope, double fixed_x, double fixed_y, double slope, double through_x, double through_y)
    {
        //fixed_x,y = 画面端の固定のxyの値	through_x,y = 方程式が通っている点=ノートの終点
        double answer = 0;
        if (return_hope == "x")
        {
            answer = through_x + (fixed_y - through_y) / slope;
            //answer = through_x + ((fixed_y - through_y) / slope);
        }
        else if (return_hope == "y")
        {
            answer = slope * (fixed_x - through_x) + through_y;
        }
        Debug.Log("answer " + answer);
        return answer;
    }





    double Tan_calc(double angle)
    {
        double tangent = Mathf.Tan((float)angle * Mathf.Deg2Rad);//ラジアンに変換
        return tangent;
    }


    //投げられた値がノート出現範囲内か
    bool is_inside_area(int cross_pattern, double aquation_answer)
    {
        bool inside = true;
        if (cross_pattern == 1 || cross_pattern == 4)
        {
            //常にxの答えが投げられる
            if (aquation_answer < display_size.xMin)
            {
                inside = false;
            }
        }
        else if (cross_pattern == 2 || cross_pattern == 3)
        {
            if (aquation_answer > display_size.xMax)
            {
                inside = false;
            }
        }
        return inside;
    }


    void Convert_worldpos(int positionIndex, double freeX, double freeY)
    {
        double pos_x, pos_y;
        int abs_x = 0;
        int abs_y = 0; //xyの絶対位置、例えば1と8のxは同じ2になる

        if (positionIndex != -1)
        {
            /*xの出し方
             * positionIndex+1を7から順番に割って余りが出ない数
             * 
             * ↑これじゃダメ
             * その行の端は7*行-1で出せるのでそこからの距離で出せないか
             * 11だったら端は7*2-1で13になって13-11=2で7-2=5番目
             * 
             * yの出し方
             * 
             */





            for (int value = 1; value < 5; value++)
            {
                if (positionIndex > 7 * (value - 1) && positionIndex < 7 * value)
                {
                    abs_x = (positionIndex - 7 * (value - 1)) + 1;
                    abs_y = value;
                }
                Debug.Log("abs_x " + abs_x);
                Debug.Log("abs_y " + abs_y);
            }


        }
        else
        {


        }
    }











    public struct Note_line
    { //part(トラック)は配列のインデックスで判定する
        public double world_pos_x, world_pos_y, angle_one, angle_two, slope_one, slope_two,
            note_pos_one_x, note_pos_one_y, note_pos_two_x, note_pos_two_y,
            note_pos_three_x, note_pos_three_y, note_pos_four_x, note_pos_four_y;

        public Note_line
            (double wpx, double wpy, double ao, double at, double so, double st, double nonex, double noney, double ntwox, double ntwoy, double nthreex, double nthreey, double nfourx, double nfoury)
        {
            world_pos_x = wpx;
            world_pos_y = wpy;
            angle_one = ao;
            angle_two = at;
            slope_one = so;
            slope_two = st;
            note_pos_one_x = nonex;
            note_pos_one_y = noney;
            note_pos_two_x = ntwox;
            note_pos_two_y = ntwoy;
            note_pos_three_x = nthreex;
            note_pos_three_y = nthreey;
            note_pos_four_x = nfourx;
            note_pos_four_y = nfoury;
        }
    }



    public struct Display_size
    {
        public double xMax, xMin, yMax, yMin;

        public Display_size
            (double xx, double xn, double yx, double yn)
        {
            xMax = xx;
            xMin = xn;
            yMax = yx;
            yMin = yn;
        }


    }

    /*また作るのめんどから残すけど使わない
    public struct Touch_point
    {
        public double 1x, 1y, 2x, 2y,3x,3y,4x,4y,5x,5y,6x,6y,7x,7y,8x,8y,9x,9y,10x,10y,
            11x,11y,12x,12y,13x,13y,14x,14y,15x,15y,16x,16y,17x,17y,18x,18y,19x,19y,20x,20y,
            21x,21y,22x,22y,23x,23y,24x,24y,25x,25y,26x,26y,27x,27y,28x,28y,29x,;


        public Touch_point
            (double xx, double xn, double yx, double yn)
        {



        }
    }
    */



    public struct Touch_point
    {
        public double point_x, point_y;

        public Touch_point
            (double px, double py)
        {
            point_x = px;
            point_y = py;
        }
    }

}