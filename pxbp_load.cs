
using SimpleJSON;
using System;
using UnityEngine;


public class pxbp_load : MonoBehaviour {

    [SerializeField]    Score_data Score_data;
    JSONNode jsons;

    public void Load_json(string json)
    {
        jsons = JSON.Parse(json);
    }

    public void Insert_Header()
    {
        
        Score_data.Header_struct Header_struct = new Score_data.Header_struct();
        Header_struct.musicName = jsons["musicName"].Value;
        Header_struct.artistName = jsons["artistName"].Value;
        Header_struct.musicLevel = jsons["musicLevel"].AsInt;
        Header_struct.category = jsons["category"].Value;
        //Debug.Log(Header_struct.musicName);

    }

    public void Insert_BPM_List()
    {
        Score_data.make_BPM_List(jsons["bpmList"].Count);
        for (int i = 0; i < jsons["bpmList"].Count; i++)
        {
            Score_data.BPM_List[i].count = jsons["bpmList"][i]["count"];
            Score_data.BPM_List[i].value = jsons["bpmList"][i]["value"];
        }

        /*↓デバック用。中身閲覧。
        foreach (var item in Score_data.BPM_List)
        {
            Debug.Log(item.count);
            Debug.Log(item.value);
        }
        */

    }


    public void Insert_notes_List()
    {
        Score_data.offset = (-1) * jsons["offset"].AsDouble;
        //Debug.Log(Score_data.offset);
        Score_data.make_notes_List(jsons["notes"].Count);
        for (int i = 0; i < jsons["notes"].Count; i++)
        {
            Score_data.note_List[i].count = jsons["notes"][i]["count"];
            Score_data.note_List[i].part = jsons["notes"][i]["part"];
            Score_data.note_List[i].endCnt = jsons["notes"][i]["prop"]["endCnt"];
            Score_data.note_List[i].flickAngle = jsons["notes"][i]["prop"]["flickAngle"];//ないとnull
            Score_data.note_List[i].freeX = Null_reject(i, "freeX");
            Score_data.note_List[i].freeY = Null_reject(i, "freeY");
            Score_data.note_List[i].positionIndex = jsons["notes"][i]["prop"]["positionIndex"];
            Score_data.note_List[i].rotation = Rotation_formating(i);
            //パラメーターに明示的に0を入れてもnullになる謎仕様のためpositionはNull_rejectしない
        }
        Score_data.note_List.Sort(c => c.count, c => c.part);

        //↓デバック用。中身閲覧。
        /*
        foreach (var item in Score_data.notes_List)
       {
            int view = 1;//onoffを繰り返すならここだけイジる
            if (view == 1)
            {
            Debug.Log("----------notes----------");
           Debug.Log("count " + item.count);
           Debug.Log("part " + item.part);
            Debug.Log("endCnt " + item.endCnt);
            Debug.Log("flickAngle " + item.flickAngle);
            Debug.Log("freeX " + item.freeX);
            Debug.Log("freeY " + item.freeY);
            Debug.Log("positionIndex " + item.positionIndex);
            Debug.Log("rotation " + item.rotation);

            }

        }  
      */ 
    }


    /// <summary>
    /// nullの場合-1を、そうでない場合値を返す。(そのままだとnullだと0が入り判別がつかなくなるため)
    /// </summary>
    /// <param name="i">配列の場所。Insert_notes_Listのforのiに同じ</param>
    /// <param name="key">配列のキー</param>
    /// <returns></returns>
    double Null_reject(int i, string key)
    {
        double value;

        if (jsons["notes"][i]["prop"][key] == null)
        {
            value = -1;
        }
        else
        {
            value = jsons["notes"][i]["prop"][key];
        }

        //Debug.Log(value);
        return value;
    }



    /// <summary>
    /// マイナスや360度以上の角度を0～359までに収める
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    double Rotation_formating(int i)
    {
        double rotation = jsons["notes"][i]["prop"]["rotation"];
        //Debug.Log("生 rotation " + rotation);
        if (rotation >= 360 || rotation < 0)
        {
            double remainder = rotation % 360;
          
            if (remainder < 0)
            {
               remainder += 360;
            }
            rotation = remainder;
        }
        //Debug.Log("rotation " +rotation);
        return rotation;
    }






}

