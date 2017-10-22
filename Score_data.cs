using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Score_data : MonoBehaviour
{


    public BPM_struct[] BPM_List;
    public notes_struct[] note_List;
    public double offset;

   

    // Use this for initialization
    void Start()
    {

    }




    /// <summary>
    /// ヘッダー情報の構造体
    /// </summary>
    public struct Header_struct
    {
        public string musicName, artistName, category;
        public float musicLevel;


        public Header_struct(string music, string artist, string cat, float Level)
        {
            musicName = music;
            artistName = artist;
            musicLevel = Level;
            category = cat;
        }
    }


    public void  make_BPM_List(int length)
    {
        BPM_List = new BPM_struct[length];
    }


    public struct BPM_struct
    {
        public double count, value;
        public BPM_struct(double cou, double val)
        {
            count = cou;
            value = val;
        }


    }


    public void make_notes_List(int length)
    {
        note_List = new notes_struct[length];
    }



    public struct notes_struct
    {
        public double count, endCnt, flickAngle, freeX, freeY, rotation;
        public int part, positionIndex;

        public notes_struct
            (double cou, int par, double endC , double flickA ,
            double fX , double fY , int posIndex , double rot)
        {
            count = cou;
            part = par;
            endCnt = endC;
            flickAngle = flickA;
            freeX = fX;
            freeY = fY;
            positionIndex = posIndex;
            rotation = rot;
        }


    }



}
