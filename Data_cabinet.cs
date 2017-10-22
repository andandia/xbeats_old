using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_cabinet : MonoBehaviour {

    public notes_struct[] notes_List;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void make_notes_List(int length)
    {
        notes_List = new notes_struct[length];
    }

    public struct notes_struct
    {
        public double time, endCnt, flickAngle, freeX, freeY, rotation;
        public int part, positionIndex;

        public notes_struct
            (double ti, int par, double endC, double flickA,
            double fX, double fY, int posIndex, double rot)
        {
            time = ti;
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
