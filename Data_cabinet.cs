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
	[SerializeField] public GameObject[] cache_arrow_list = new GameObject[30];//todo 数は可変に
	[SerializeField] public Tween[] cache_tween_list = new Tween[30];//todo 数は可変に

	/// <summary>
	/// ノート作成時にNote_data_listから値を取ってくるためのindex
	/// </summary>
	private int Create_get_note_data_index;

	/// <summary>
	/// ノート作成後にNote_made_list(作ったノートIDリスト)に値を格納するためのindex
	/// </summary>
	private int Create_add_note_made_index;


	//private int Note_data_list_index;
	//private int Note_made_list_index;
	/*--------------------------------------------------------------------------*/
	//public int Get_Note_data_list_index() { return Note_data_list_index; }
	//public int Get_Note_made_list_index() { return Note_made_list_index; }
	public int Get_Create_note_data_index() { return Create_get_note_data_index; }


	/*--------------------------------------------------------------------------*/
	public void Inc_Create_note_data_index() { Create_get_note_data_index++; }



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
		

	}

}