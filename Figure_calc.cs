using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure_calc : MonoBehaviour {

    Display_size display_size;
    Note_line note_line;

    // Use this for initialization
    void Start () {
        //Main_figure_calc(5);//テスト用

    }
	
	void Main_figure_calc(double rotation, double through_x, double through_y)
    {
        display_size.xMax = 13.65;
        display_size.xMin = -13.65;
        display_size.yMax = 7.68;
        display_size.yMin = -7.68;
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
        note_line.slope_one = Tan_calc(one) * -1 ;
        note_line.slope_two = Tan_calc(two) * -1;
    }




	void Pos_decide(double rotation, double through_x, double through_y)
    {
        /*解決すべき点
         * oneとtwoでfixed_x,yに入れるべき値が変わってしまい、長ったらしくなる。rotation180のときtwoはrotation90のやつを入れたらよかったりしないか
         * 範囲を超えたかの判断がfixed_x,yに何が入ってるかで変わってしまう。わかりやすく短く書けないか
        */
        double fixed_x = 0;
        double fixed_y = 0;
		//oneの位置決定
		if (rotation  < 90){
            fixed_x = display_size.xMin;
            fixed_y = display_size.yMax;
        }
        else if (rotation <180)
        {
            fixed_x = display_size.xMax;
            fixed_y = display_size.yMax;
        }
        else if (rotation < 270)
        {
            fixed_x = display_size.xMax;
            fixed_y = display_size.yMin;
        }
        else if (rotation < 360)
        {
            fixed_x = display_size.xMin;
            fixed_y = display_size.yMin;
        }
        double temp_pos = Equation_answer("x", fixed_x, fixed_y, note_line.slope_one, through_x, through_y);
        if (temp_pos > fixed_y)
        {

        }


    }
	
	
	
	
	//方程式の代入結果を返す
	double Equation_answer(string return_hope,double fixed_x,double fixed_y,double slope, double through_x , double through_y){
		//fixed_x,y = 画面端の固定のxyの値	through_x,y = 方程式が通っている点=ノートの終点
		double answer = 0;
		if(return_hope == "x")
        {
			answer = through_x + (fixed_y-through_y)/slope;
		}else if(return_hope == "y")
		{	
			answer = slope *(fixed_x-through_x)+through_y;
		}
        Debug.Log("answer " + answer);
        return answer;
	}
	




    double Tan_calc(double angle)
    {
        //double tangent = Mathf.Sin((float)angle * Mathf.Deg2Rad)* Mathf.Rad2Deg;//ラジアンに変換
        //↑はtanの角度が出てるのでは?(必要なのは傾きの値)
        double tangent = Mathf.Sin((float)angle * Mathf.Deg2Rad);//ラジアンに変換
        return tangent;
    }


    public struct Note_line
    { //part(トラック)は配列のインデックスで判定する
        public double angle_one, angle_two, slope_one, slope_two,
            note_pos_one_x, note_pos_one_y, note_pos_two_x, note_pos_two_y;
       
        public Note_line
            (double ao, double at, double so, double st, double npox, double npoy, double nptx, double npty)
        {
            angle_one = ao;
            angle_two = at;
            slope_one = so;
            slope_two = st;
            note_pos_one_x = npox;
            note_pos_one_y = npoy;
            note_pos_two_x = nptx;
            note_pos_two_y = npty;
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


}
