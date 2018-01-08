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
	[SerializeField] Time_manager Time;
	[SerializeField] AudioSource audioSource;

	public Note_data[] Note_data_list;
	public Made_note[] Note_made_list;
	//[SerializeField] public GameObject[] cache_arrow_list = new GameObject[30];//todo 数は可変に
	//[SerializeField] public Tween[] cache_tween_list = new Tween[30];//todo 数は可変に

	/// <summary>
	/// ノート作成時にNote_data_listから値を取ってくるためのindex
	/// </summary>
	private int Create_get_note_data_index;

	/// <summary>
	/// ノート作成後にNote_made_list(作ったノートIDリスト)に値を格納するためのindex
	/// </summary>
	private int Create_add_note_made_index;

	/// <summary>
	/// 判定時にNote_data_listから値を取ってくるためのindex
	/// </summary>
	private int Judge_get_note_data_index;


	/// <summary>
	/// 判定時にNote_made_listから値を取ってくるためのindex
	/// </summary>
	private int Judge_get_note_made_index;

	/*--------------------------------------------------------------------------*/

	public int Get_Create_note_data_index() { return Create_get_note_data_index; }
	//Create_add_note_made_indexはこの中でしか使ってないのでGetが要らない
	//Judge_get_note_data_indexはこの中でしか使ってないのでGetが要らない

	/*--------------------------------------------------------------------------*/
	public void Inc_Create_note_data_index()			{ Create_get_note_data_index += Avoid_nullpo(1 , Create_get_note_data_index); }
	public void Inc_Create_add_note_made_index () { Create_add_note_made_index += Avoid_nullpo(2 , Create_add_note_made_index); }
	public void Inc_Judge_get_note_data_index ()	{ Judge_get_note_data_index += Avoid_nullpo(1 , Judge_get_note_data_index); }
	public void Inc_Judge_get_note_made_index ()	{ Judge_get_note_made_index += Avoid_nullpo(2 , Judge_get_note_made_index); }

	/// <summary>
	/// 各indexのインクリメント時に各listのlengthを超えてインクリメントしないようにするメソッド。
	/// </summary>
	/// <param name="listtype">どのリストに対してのインデックスか</param>
	/// <param name="Inc_value">加えるインデックス</param>
	/// <returns></returns>
	int Avoid_nullpo (int list_type, int Inc_value )//Note_data_list =1  Note_made_list = 2
	{
		int return_value = 1;
		switch (list_type)
		{
			case 1: if (Note_data_list.Length - 1 == Inc_value) { return_value = 0; } break;
			case 2: if (Note_made_list.Length - 1 == Inc_value) { return_value = 0; } break;
			default: break;
		}
		return return_value;
	}
	/*--------------------------------------------------------------------------*/

	public void ToCreate_note_data_List(int length)
	{
		Note_data_list = new Note_data[length];
	}

	public void ToCreate_note_made_List(int length)
	{
		Note_made_list = new Made_note[length];
	}

	/*--------------------------------------------------------------------------*/
	

	//tureならif続行
	/// <summary>
	/// 作るノートを探すか
	/// </summary>
	/// <returns></returns>
	public bool Is_create_note_search()
	{
		bool search = false;
		if (audioSource.isPlaying == true &&
				Create_get_note_data_index <= Note_data_list.Length - 1)
		{
			search = true;
		}
		return search;
	}


	/// <summary>
	/// ノートを作るかどうか
	/// </summary>
	/// <returns></returns>
	public bool Is_create_note()
	{
		bool create = false;
		if (audioSource.isPlaying == true &&
				Time.Get_time() >= Note_data_list[Create_get_note_data_index].startTime &&
				Note_data_list[Create_get_note_data_index].made == false)
		{
			create = true;
		}
		return create;
	}
	//Time.Get_time() >= Dc.notes_list[make_index].startTime && Dc.notes_list[make_index].alive == true)


	/// <summary>
	/// 作ったノートのフラグを折る
	/// </summary>
	public void Mark_Made_note()
	{
		Note_data_list[Create_get_note_data_index].made = true;
	}
	
	

	/// <summary>
	/// ノート作成時にnote_dataを取得する
	/// </summary>
	/// <returns></returns>
	public Note_data Get_Create_note_data()
	{
		return Note_data_list[Create_get_note_data_index];
	}

	/// <summary>
	/// Note_made_listに追加する
	/// </summary>
	/// <param name="made_Note"></param>
	public void Add_Note_made_list(Made_note made_Note)
	{
		Note_made_list[Create_add_note_made_index] = made_Note;
		Inc_Create_add_note_made_index();

	}

	/// <summary>
	/// 判定時にnote_dataを取得する
	/// </summary>
	/// <returns></returns>
	public Note_data Get_Judge_note_data ()
	{
		return Note_data_list[Judge_get_note_data_index];
	}

	/// <summary>
	/// 判定後にmade_noteを取得する
	/// </summary>
	/// <returns></returns>
	public Made_note Get_judge_made_note ()
	{
		return Note_made_list[Judge_get_note_made_index];
	}

	/// <summary>
	/// 判定後にnote_dataのjudgedをtrueにする
	/// </summary>
	public void Set_judge_note_data_judged ()
	{
		Note_data_list[Judge_get_note_data_index].judged = true;
	}


	public float Get_judge_note_parfectTime ()
	{
		return Note_data_list[Judge_get_note_data_index].parfectTime;
	}


	/// <summary>
	/// 現在判定すべきノートが判定済みかを返す
	/// </summary>
	/// <returns></returns>
	public bool Get_judge_note_is_judged ()
	{
		return Note_data_list[Judge_get_note_data_index].judged;
	}
}