using DG.Tweening;
using UnityEngine;

public class Data_cabinet : MonoBehaviour
{
	/*
	 * 命名ルール
	 * リスト→make_(対象)_list
	 * キャッシュ配列→Add_(対象)_list
	 index系命名規則目的(～する)_動作(Get、Add)_配列名_index
	 * indexの取得、増加→Get_(対象)、Inc_(対象)
	*/
	Time_manager Time;
	AudioSource BGM_Play;

	public Note_data[] Note_data_list_line1;
	public Note_data[] Note_data_list_line2;
	public Made_note[] Note_made_list;

	public Create_index create_Index;
	public Judge_index judge_Index;
	//[SerializeField] public GameObject[] cache_arrow_list = new GameObject[30];//todo 数は可変に
	//[SerializeField] public Tween[] cache_tween_list = new Tween[30];//todo 数は可変に

	private string MusicfileName;


	void Start ()
	{
		DontDestroyOnLoad(this);
	}



	/*--------------------------------------------------------------------------*/
	//Create_add_note_made_indexはこの中でしか使ってないのでGetが要らない
	//Judge_get_note_data_indexはこの中でしか使ってないのでGetが要らない

	public int Get_Create_note_data_index ( int line )
	{
		switch (line)
		{
			case 1:
				return create_Index.get_note_data_line1;
			case 2:
				return create_Index.get_note_data_line1;
			default:
				return -1;
		}
	}

	/*--------------------------------------------------------------------------*/
	public void Inc_Create_note_data_index ( int line )
	{
		switch (line)
		{
			case 1:
				create_Index.get_note_data_line1 += Avoid_nullpo(10 , create_Index.get_note_data_line1);
				return;
			case 2:
				create_Index.get_note_data_line2 += Avoid_nullpo(11 , create_Index.get_note_data_line2);
				return;
			default:
				return;
		}
	}

	public void Inc_Create_add_note_made_index () { create_Index.add_note_made += Avoid_nullpo(20 , create_Index.add_note_made); }

	public void Inc_Judge_get_note_data_index ( int line )
	{
		switch (line)
		{
			case 1:
				judge_Index.get_note_data_line1 += Avoid_nullpo(10 , judge_Index.get_note_data_line1);
				return;
			case 2:
				judge_Index.get_note_data_line2 += Avoid_nullpo(11 , judge_Index.get_note_data_line2);
				return;
			default:
				return;
		}
	}
	
	public void Inc_Judge_get_note_made_index () { judge_Index.get_note_made += Avoid_nullpo(20 , judge_Index.get_note_made); }

	/// <summary>
	/// 各indexのインクリメント時に各listのlengthを超えてインクリメントしないようにするメソッド。
	/// </summary>
	/// <param name="listtype">どのリストに対してのインデックスか</param>
	/// <param name="Inc_value">加えるインデックス</param>
	/// <returns></returns>
	int Avoid_nullpo ( int list_type , int Inc_value )
	{//Note_data_list_line1 =10 Note_data_list_line2 =11  Note_made_list = 20
	 //2桁なのは増やしてもmade側を対応しなくてよくするため
		int return_value = 1;
		switch (list_type)
		{
			case 10: if (Note_data_list_line1.Length - 1 == Inc_value) { return_value = 0; } break;
			case 11: if (Note_data_list_line2.Length - 1 == Inc_value) { return_value = 0; } break;
			case 20: if (Note_made_list.Length - 1 == Inc_value) { return_value = 0; } break;
			default: break;
		}
		return return_value;
	}
	/*--------------------------------------------------------------------------*/
	/// <summary>
	/// Note_data_list群を作る
	/// </summary>
	/// <param name="Line1Length"></param>
	/// <param name="Line2Length"></param>
	public void ToCreate_Note_data_list ( int Line1Length , int Line2Length )
	{
		Note_data_list_line1 = new Note_data[Line1Length];
		Note_data_list_line2 = new Note_data[Line2Length];
	}
	/// <summary>
	/// Note_made_listを作る
	/// </summary>
	/// <param name="length"></param>
	public void ToCreate_note_made_List ( int length )
	{
		Note_made_list = new Made_note[length];
	}


	/*--------------------------------------------------------------------------*/



/// <summary>
/// 作るノートを探すか
/// </summary>
/// <param name="line">どのlineで探すかどうか返答するか</param>
/// <returns></returns>
	public bool Is_create_note_search (int line)
	{
		bool search = false;
		if (BGM_Play.isPlaying == true)
		{
			switch (line)
			{
				case 1:
					if (create_Index.get_note_data_line1 <= Note_data_list_line1.Length - 1)
					{
						search = true;
					}
					return search;
				case 2:
					if (create_Index.get_note_data_line2 <= Note_data_list_line2.Length - 1)
					{
						search = true;
					}
					return search;
				default:
					return false;
			}
		}
		return search;
	}



	/// <summary>
	/// line1からノートを作るかどうか
	/// </summary>
	/// <returns></returns>
	public bool Is_create_note_line1 ()
	{
		bool create = false;
		if (BGM_Play.isPlaying == true)
		{
			if (Time.Get_time() >= Note_data_list_line1[create_Index.get_note_data_line1].startTime &&
					Note_data_list_line1[create_Index.get_note_data_line1].made == false)
			{
				create = true;
				//Debug.Log("line1");
			}
		}
		return create;
	}

	/// <summary>
	/// line1からノートを作るかどうか
	/// </summary>
	/// <returns></returns>
	public bool Is_create_note_line2 ()
	{
		bool create = false;
		if (BGM_Play.isPlaying == true)
		{ 
			if (Time.Get_time() >= Note_data_list_line2[create_Index.get_note_data_line2].startTime &&
								Note_data_list_line2[create_Index.get_note_data_line2].made == false)
			{
				create = true;
				//Debug.Log("line2");
			}
		}
		return create;
	}




	/// <summary>
	/// 作ったノートのフラグを折る
	/// </summary>
	public void Mark_Made_note ( int line )
	{
		switch (line)
		{
			case 1:
				Note_data_list_line1[create_Index.get_note_data_line1].made = true;
				return;
			case 2:
				Note_data_list_line2[create_Index.get_note_data_line2].made = true;
				return;
			default:
				return;
		}
	}



	/// <summary>
	/// ノート作成時にnote_dataを取得する
	/// </summary>
	/// <returns></returns>
	public Note_data Get_Create_note_data ( int line )
	{
		switch (line)
		{
			case 1:
				return Note_data_list_line1[create_Index.get_note_data_line1];
			case 2:
				return Note_data_list_line2[create_Index.get_note_data_line2];
			default:
				return Note_data_list_line1[-1];//これが返ってる時はおかしい(nullが返せないのでこの形)
		}		
	}

	/// <summary>
	/// Note_made_listに追加する
	/// </summary>
	/// <param name="made_Note"></param>
	public void Add_Note_made_list ( int line , Made_note made_Note )
	{
		if (line == 1)
		{
			Note_data_list_line1[create_Index.get_note_data_line1].made_note_list_index = create_Index.add_note_made;
		}
		else if (line == 2)
		{
			Note_data_list_line2[create_Index.get_note_data_line2].made_note_list_index = create_Index.add_note_made;
		}
		//Debug.Log("add note " + Note_data_list_line1[Create_get_note_data_index].made_note_list_index);
		Note_made_list[create_Index.add_note_made] = made_Note;
		Inc_Create_add_note_made_index();

	}

	/// <summary>
	/// 判定時にnote_dataを取得する
	/// </summary>
	/// <returns></returns>
	public Note_data Get_Judge_note_data (int line)
	{
		switch (line)
		{
			case 1:
				return Note_data_list_line1[judge_Index.get_note_data_line1];
			case 2:
				return Note_data_list_line2[judge_Index.get_note_data_line2];
			default:
				return Note_data_list_line1[-1];//この値が帰ることはない
		}
	}


	/// <summary>
	/// 判定後にmade_noteを取得する
	/// </summary>
	/// <returns></returns>
	public Made_note Get_judge_made_note ()
	{
		return Note_made_list[judge_Index.get_note_made];
	}

	/// <summary>
	/// 判定後にnote_dataのjudgedをtrueにする
	/// </summary>
	public void Set_judge_note_data_judged (int line)
	{
		switch (line)
		{
			case 1:
				Note_data_list_line1[judge_Index.get_note_data_line1].judged = true;
				return;
			case 2:
				Note_data_list_line2[judge_Index.get_note_data_line2].judged = true;
				return;
			default:
				return;
		}
	}


	public float Get_judge_note_parfectTime (int line)
	{
		switch (line)
		{
			case 1:
				return Note_data_list_line1[judge_Index.get_note_data_line1].parfectTime;
			case 2:
				return Note_data_list_line2[judge_Index.get_note_data_line2].parfectTime;
			default:
				return 0;
		}
	}


	/// <summary>
	/// 現在判定すべきノートが判定済みかを返す
	/// </summary>
	/// <returns></returns>
	public bool Get_judge_note_is_judged (int line)
	{
		switch (line)
		{
			case 1:
				return Note_data_list_line1[judge_Index.get_note_data_line1].judged;
			case 2:
				return Note_data_list_line2[judge_Index.get_note_data_line2].judged;
			default:
				return false;
		}
	}




	/// <summary>
	/// 作成時に使用するインデックスを管理するためのスクリプト
	/// </summary>
	public struct Create_index
	{
		/// <summary>
		/// ノート作成時にNote_data_list_line1から値を取ってくるためのindex
		/// </summary>
		public int get_note_data_line1;

		/// <summary>
		/// ノート作成時にNote_data_list_line2から値を取ってくるためのindex
		/// </summary>
		public int get_note_data_line2;

		/// <summary>
		/// ノート作成後にNote_made_list(作ったノートIDリスト)に値を格納するためのindex
		/// </summary>
		public int add_note_made;

		public Create_index ( int get_note_data_line1 , int add_note_made ,
													int get_note_data_line2 )
		{
			this.get_note_data_line1 = get_note_data_line1;
			this.get_note_data_line2 = get_note_data_line2;
			this.add_note_made = add_note_made;
		}
	}



	/// <summary>
	/// 判定時に使用するインデックスを管理するためのスクリプト
	/// </summary>
	public struct Judge_index
	{
		/// <summary>
		/// 判定時にNote_data_list_line1から値を取ってくるためのindex
		/// </summary>
		public int get_note_data_line1;

		/// <summary>
		/// 判定時にNote_data_list_line2から値を取ってくるためのindex
		/// </summary>
		public int get_note_data_line2;


		/// <summary>
		/// 判定時にNote_made_listから値を取ってくるためのindex
		/// </summary>
		public int get_note_made;


		public Judge_index ( int get_note_data_line1 , int get_note_made ,
												int get_note_data_line2 )
		{
			this.get_note_data_line1 = get_note_data_line1;
			this.get_note_data_line2 = get_note_data_line2;
			this.get_note_made = get_note_made;
		}
	}




	public void Set_BGM_Play (AudioSource BGM)
	{
		BGM_Play = BGM;
	}

	public void Set_Time_Script(Time_manager Time )
	{
		this.Time = Time;
	}

	public void Set_MusicfileName(string MusicfileName )
	{
		this.MusicfileName = MusicfileName;
	}


	public string Get_MusicfileName ()
	{
		return MusicfileName;
	}

}
