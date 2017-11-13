using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure_calc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	
    void angle_calc(double rotation)
    {
        double one;//パターン1における、rotationで示されている方の角度
        double two;
        if (rotation  < 90){
            two = 180 - rotation - 90;
            one = 180 - rotation;
        }
        else if (rotation <180)
        {
            two = 180 - rotation;
            one = two + 90;
        }
        else if (rotation < 270)
        {
            double a = rotation - 180;
            two = 90 - a;
            one = two + 90;
        }
        else if (rotation < 360)
        {
            two = rotation - 270;
            one = two + 90;
        }
    }


    double tan_calc(double angle)
    {
        double tangent = Mathf.Sin((float)angle * Mathf.Deg2Rad)* Mathf.Rad2Deg;//ラジアンに変換
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



}
