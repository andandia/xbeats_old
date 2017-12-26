using DG.Tweening;

public struct Made_note
{
	public int
		arrow_id_1,
		arrow_id_2,
		arrow_id_3,
		arrow_id_4,
		guide_line_id;

	public Tween tween;


	public Made_note(int arrow_id_1, int arrow_id_2, int arrow_id_3, int arrow_id_4 ,int guide_line_id , Tween tween)
	{
		this.arrow_id_1 = arrow_id_1;
		this.arrow_id_2 = arrow_id_2;
		this.arrow_id_3 = arrow_id_3;
		this.arrow_id_4 = arrow_id_4;
		this.guide_line_id = guide_line_id;
		this.tween = tween;
	}
		

}