using UnityEngine;

/// <summary>
/// ノートの位置、角度計算スクリプト
/// </summary>
public class Figure_calc : MonoBehaviour
{
	//transform.SetPositionAndRotation(Vector3, Quaternion);
	//位置と向きを同時に指定したいならこれを使おう

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
	is_inside_areaは改修必要なし。他は対応済み
	*/
	/*freeは0.02刻みで1まで
	0-6
	7-13
	14-20
	21-27
	28-34
	*/

	private Display_size display_size;
	private Note_line note_line;

	/// <summary>
	/// フリックとみなす座標をnote_end_posからどれだけの距離に置くか
	/// </summary>
	double flick_distance = 2;


	/// <summary>
	/// メインメソッド。ここに投げると計算してくれる。
	/// </summary>
	/// <param name="notetype">0はタッチ、1はフリック、2はホールド</param>
	/// <param name="rotation"></param>
	/// <param name="positionIndex"></param>
	/// <param name="freeX"></param>
	/// <param name="freeY"></param>
	public void Main_figure_calc ( int notetype , double rotation , int positionIndex , double freeX , double freeY , double flickAngle )
	{
		Screen_width_setup();
		Convert_worldpos(positionIndex , freeX , freeY);
		if (rotation % 90 == 0)//直角なら
		{
			Lpos_decide(notetype , rotation);
		}
		else
		{
			Angle_calc(rotation);
			Pos_decide(notetype , rotation);


		}
		if (notetype == 1)
		{
			Flick_pos_decide(flickAngle);
		}
		else
		{
			note_line.flick_pos_x = 100;
			note_line.flick_pos_y = 100;
		}
		/*実際の流れ
		Screen_width_setup();で画面サイズ類を定義
		Convert_worldposで譜面記述の座標がunityワールド座標になって構造体に入る
		Angle_calcで譜面のrotationから傾きが構造体に入る
		Pos_decideで2点のxy座標が構造体に入る
		後は2点のxy座標をscore_load側からgetすればよい
		*/
		/*rotationが90で割れる値だったら
		 * 
		 */
	}

	private void Screen_width_setup ()
	{
		display_size.xMax = 13.65;
		display_size.xMin = -13.65;
		display_size.yMax = 7.68;
		display_size.yMin = -7.68;
		display_size.note_pos_xMin = -6.27;//ノートが置かれるxの最小点
		display_size.note_pos_yMax = 4;
		display_size.note_pos_xMin = display_size.note_pos_xMin + ( display_size.note_pos_xMin / 3 );//幅と絶対座標をあわせるために1つ分増やす
		display_size.note_pos_yMax = display_size.note_pos_yMax + ( display_size.note_pos_yMax / 2 );
		display_size.note_disp_width_x = display_size.note_pos_xMin * 2;
		display_size.note_disp_width_y = display_size.note_pos_yMax * 2;
		display_size.pos_unit_x = display_size.note_disp_width_x / ( 7 + 1 );//pos_unit_xyから修正
		display_size.pos_unit_y = display_size.note_disp_width_y / ( 5 + 1 );
		display_size.free_width_x = display_size.note_disp_width_x - ( display_size.pos_unit_x * 0.8 );
		display_size.free_width_y = display_size.note_disp_width_y - ( display_size.pos_unit_y * 0.8 );
		display_size.free_unit_x = display_size.free_width_x / ( 1 / 0.02 );
		display_size.free_unit_y = display_size.free_width_y / ( 1 / 0.02 );
	}

	private void Convert_worldpos ( int positionIndex , double freeX , double freeY )
	{
		double pos_x = 0;
		double pos_y = 0;
		int abs_x = 0;
		int abs_y = 0; //xyの絶対位置、例えば1と8のxは同じ値を示す
		positionIndex = positionIndex + 1;//下記参照。

		if (freeX == -1 && freeY == -1)//free以外のとき(freeXYはNull_rejectによって-1にしている)
		{
			/*
			まず考え方としてPxbpのPOSITIONを1から35までにする。
			こうすることで各行最初のpositionは(行*7-6)で求められるようになる。
			そしてそれに合わせるためにpositionIndexを+1している。
			1行(横)を単位として見る(for文)
			positionIndexがその行の範囲内だったら
			positionIndexが7の倍数の場合は右端として7を、
			そうでない場合positionIndexを7で割ったあまりを
			指定している。
			 */
			for (int line = 1; line < 6; line++)
			{
				if (positionIndex >= ( line * 7 - 6 ) && positionIndex < ( ( line + 1 ) * 7 - 6 ))
				{
					if (positionIndex % 7 == 0)
					{
						abs_x = 7;
					}
					else
					{
						abs_x = positionIndex % 7;
					}
					abs_y = line;
				}
			}
			//Debug.Log("positionIndex " + positionIndex + " abs_x " + abs_x);
			//Debug.Log("positionIndex " + positionIndex + " abs_y " + abs_y);
			pos_x = display_size.note_pos_xMin - ( display_size.pos_unit_x * abs_x );
			pos_y = display_size.note_pos_yMax - ( display_size.pos_unit_y * abs_y );
		}
		else//freeのとき
		{

			pos_x = ( display_size.free_width_x / 2 ) - display_size.free_unit_x * ( freeX / 0.02 );
			pos_y = ( display_size.free_width_y / 2 ) - display_size.free_unit_y * ( freeY / 0.02 );
		}

		note_line.note_end_pos_x = pos_x;
		note_line.note_end_pos_y = pos_y;
	}

	private void Angle_calc ( double rotation )
	{
		double one = rotation;//パターン1における、rotationで示されている方の角度
		double two = rotation + 90;
		note_line.slope_one = Tan_calc(one) * -1;
		note_line.slope_two = Tan_calc(two) * -1;
	}

	private void Pos_decide ( int type , double rotation )
	{
		/*解決すべき点
		 * 1.oneとtwoでfixed_x,yに入れるべき値が変わってしまい、長ったらしくなる。rotation180のときtwoはrotation90のやつを入れたらよかったりしないか
		 * 範囲を超えたかの判断がfixed_x,yに何が入ってるかで変わってしまう。わかりやすく短く書けないか
		 1：ro90のときにtwoはro180を入れたらよいことが分かったので、
		 for文で2回回す。if文に回数及びrotationでの条件追加をしてif(i == 1 && rotation  < 90 || i == 2 && rotation  < 180)のようにすれば
		 if1つでone two両方に対応できる
		 2：判定を別メソッドへ切り出し、引数で変えるようにする。引数はfor文中のif内で代入しておく
		 1.2について解決したはず
		 3.ホールド対応(4点のxyを出す)
		 3.も対応済み
		*/
		double through_x = note_line.note_end_pos_x;
		double through_y = note_line.note_end_pos_y;
		double fixed_x = 0;//そのパターンでのxy最大/最小値
		double fixed_y = 0;
		int cross_pattern = 0;
		int times = 3;
		if (type == 2)//ホールドだったら
		{
			times = 5;
		}
		int quadrant = Quadrant_answer(rotation);
		for (int i = 1; i < times; i++)
		{
			if (i == 1 && quadrant == 2 || i == 2 && quadrant == 3 || i == 3 && quadrant == 4 || i == 4 && quadrant == 1)
			{
				fixed_x = display_size.xMin;
				fixed_y = display_size.yMax;
				cross_pattern = 1;
			}
			else if (i == 1 && quadrant == 1 || i == 2 && quadrant == 2 || i == 3 && quadrant == 3 || i == 4 && quadrant == 4)
			{
				fixed_x = display_size.xMax;
				fixed_y = display_size.yMax;
				cross_pattern = 2;
			}
			else if (i == 1 && quadrant == 4 || i == 2 && quadrant == 1 || i == 3 && quadrant == 2 || i == 4 && quadrant == 3)
			{
				fixed_x = display_size.xMax;
				fixed_y = display_size.yMin;
				cross_pattern = 3;
			}
			else if (i == 1 && quadrant == 3 || i == 2 && quadrant == 4 || i == 3 && quadrant == 1 || i == 4 && quadrant == 2)
			{
				fixed_x = display_size.xMin;
				fixed_y = display_size.yMin;
				cross_pattern = 4;
			}
			string answer_side = "x";
			double slope = 0;
			if (i == 1 || i == 3)
			{
				slope = note_line.slope_one;
			}
			else if (i == 2 || i == 4)
			{
				slope = note_line.slope_two;
			}
			double temp_pos_move = Equation_answer(answer_side , fixed_x , fixed_y , slope , through_x , through_y);
			double temp_pos_fixed = fixed_y;
			if (is_inside_area(cross_pattern , temp_pos_move) == false)
			{
				answer_side = "y";
				temp_pos_move = Equation_answer(answer_side , fixed_x , fixed_y , slope , through_x , through_y);
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
			if (i == 3 && answer_side == "x")
			{//x軸との交点が始点
				note_line.note_pos_three_x = temp_pos_move;
				note_line.note_pos_three_y = temp_pos_fixed;
			}
			else if (i == 3 && answer_side == "y")
			{
				note_line.note_pos_three_x = temp_pos_fixed;
				note_line.note_pos_three_y = temp_pos_move;
			}
			if (i == 4 && answer_side == "x")
			{//x軸との交点が始点
				note_line.note_pos_four_x = temp_pos_move;
				note_line.note_pos_four_y = temp_pos_fixed;
			}
			else if (i == 4 && answer_side == "y")
			{
				note_line.note_pos_four_x = temp_pos_fixed;
				note_line.note_pos_four_y = temp_pos_move;
			}
		}
	}


	void Flick_pos_decide ( double flickAngle )
	{
		double fix_Angle = 360 - flickAngle;//pxbpの角度からunityの角度へ
		note_line.flick_pos_x = note_line.note_end_pos_x + flick_distance * ( Mathf.Cos((float)fix_Angle * ( Mathf.PI / 180 )) );
		note_line.flick_pos_y = note_line.note_end_pos_y + flick_distance * ( Mathf.Sin((float)fix_Angle * ( Mathf.PI / 180 )) );
	}



	//方程式の代入結果を返す
	private double Equation_answer ( string return_hope , double fixed_x , double fixed_y , double slope , double through_x , double through_y )
	{
		//fixed_x,y = 画面端の固定のxyの値	through_x,y = 方程式が通っている点=ノートの終点
		double answer = 0;
		if (return_hope == "x")
		{
			answer = through_x + ( fixed_y - through_y ) / slope;
		}
		else if (return_hope == "y")
		{
			answer = slope * ( fixed_x - through_x ) + through_y;
		}
		//Debug.Log("answer " + answer);
		return answer;
	}

	private double Tan_calc ( double angle )
	{
		double tangent = Mathf.Tan((float)angle * Mathf.Deg2Rad);//ラジアンに変換
		return tangent;
	}


	int Quadrant_answer ( double rotation )
	{
		int quadrant = 0;
		if (0 < rotation && rotation < 90)
		{
			quadrant = 2;//左上
		}
		else if (90 < rotation && rotation < 180)
		{
			quadrant = 1;//右上
		}
		else if (180 < rotation && rotation < 270)
		{
			quadrant = 4;//右下
		}
		else if (270 < rotation && rotation < 360)
		{
			quadrant = 3;//左下
		}
		return quadrant;
	}

	//投げられた値がノート出現範囲内か
	private bool is_inside_area ( int cross_pattern , double aquation_answer )
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

	/*------------------直角時-----------------------*/
	void Lpos_decide ( int notetype , double rotation )
	{
		/*タイプで分ける
		* ・通常の場合
		* 角度で場合わけ
		* x=nの線(横線だけを見ると
		* 0、270→左端
		* 90、180→右端
		* y=nの線(縦線だけを見ると
		* 0、90→上
		* 180、270→下
		* ・ホールドの場合
		* 上下左右全て
		*/
		if (notetype == 0 || notetype == 1)
		{
			if (rotation == 0)
			{
				note_line.note_pos_one_x = display_size.xMin;
				note_line.note_pos_one_y = note_line.note_end_pos_y;
				note_line.note_pos_two_x = note_line.note_end_pos_x;
				note_line.note_pos_two_y = display_size.yMax;
			}
			else if (rotation == 90)
			{
				note_line.note_pos_one_x = note_line.note_end_pos_x;
				note_line.note_pos_one_y = display_size.yMax;
				note_line.note_pos_two_x = display_size.xMax;
				note_line.note_pos_two_y = note_line.note_end_pos_y;
			}
			else if (rotation == 180)
			{
				note_line.note_pos_one_x = display_size.xMax;
				note_line.note_pos_one_y = note_line.note_end_pos_y;
				note_line.note_pos_two_x = note_line.note_end_pos_x;
				note_line.note_pos_two_y = display_size.yMin;
			}
			else if (rotation == 270)
			{
				note_line.note_pos_one_x = display_size.xMin;
				note_line.note_pos_one_y = note_line.note_end_pos_y;
				note_line.note_pos_two_x = note_line.note_end_pos_x;
				note_line.note_pos_two_y = display_size.yMin;
			}
		}
		else
		{
			note_line.note_pos_one_x = display_size.xMin;
			note_line.note_pos_one_y = note_line.note_end_pos_y;
			note_line.note_pos_two_x = note_line.note_end_pos_x;
			note_line.note_pos_two_y = display_size.yMax;
			note_line.note_pos_three_x = display_size.xMax;
			note_line.note_pos_three_y = note_line.note_end_pos_y;
			note_line.note_pos_four_x = note_line.note_end_pos_x;
			note_line.note_pos_four_y = display_size.yMin;
		}
	}
	/*------------------直角時-----------------------*/


	public double[] Get_Note_pos_result ()
	{
		double[] note_pos = new double[12];
		note_pos[0] = note_line.note_end_pos_x;
		note_pos[1] = note_line.note_end_pos_y;
		note_pos[2] = note_line.note_pos_one_x;
		note_pos[3] = note_line.note_pos_one_y;
		note_pos[4] = note_line.note_pos_two_x;
		note_pos[5] = note_line.note_pos_two_y;
		note_pos[6] = note_line.note_pos_three_x;
		note_pos[7] = note_line.note_pos_three_y;
		note_pos[8] = note_line.note_pos_four_x;
		note_pos[9] = note_line.note_pos_four_y;
		note_pos[10] = note_line.flick_pos_x;
		note_pos[11] = note_line.flick_pos_y;
		//Debug.Log("flick_pos_x " + note_pos[10]);
		//Debug.Log("flick_pos_y " + note_pos[11]);
		return note_pos;
	}



	public struct Note_line
	{
		public double angle_one, angle_two, slope_one, slope_two,
				note_end_pos_x, note_end_pos_y,
				note_pos_one_x, note_pos_one_y, note_pos_two_x, note_pos_two_y,
				note_pos_three_x, note_pos_three_y, note_pos_four_x, note_pos_four_y,
				flick_pos_x, flick_pos_y;

		public Note_line
				( double angle_one , double angle_two , double slope_one , double slope_two ,
				 double note_end_pos_x , double note_end_pos_y ,
				 double note_pos_one_x , double note_pos_one_y , double note_pos_two_x , double note_pos_two_y ,
				 double note_pos_three_x , double note_pos_three_y , double note_pos_four_x , double note_pos_four_y ,
				 double flick_pos_x , double flick_pos_y )
		{
			this.angle_one = angle_one;
			this.angle_two = angle_two;
			this.slope_one = slope_one;
			this.slope_two = slope_two;
			this.note_end_pos_x = note_end_pos_x;
			this.note_end_pos_y = note_end_pos_y;
			this.note_pos_one_x = note_pos_one_x;
			this.note_pos_one_y = note_pos_one_y;
			this.note_pos_two_x = note_pos_two_x;
			this.note_pos_two_y = note_pos_two_y;
			this.note_pos_three_x = note_pos_three_x;
			this.note_pos_three_y = note_pos_three_y;
			this.note_pos_four_x = note_pos_four_x;
			this.note_pos_four_y = note_pos_four_y;
			this.flick_pos_x = flick_pos_x;
			this.flick_pos_y = flick_pos_y;
		}
	}

	public struct Display_size
	{
		public double xMax, xMin, yMax, yMin,
				note_disp_width_x, note_disp_width_y,
				free_width_x, free_width_y,
				note_pos_xMin, note_pos_yMax,
				free_pos_xMin, free_pos_yMax,
				pos_unit_x, pos_unit_y,
				free_unit_x, free_unit_y;

		public Display_size
				( double xx , double xn , double yx , double yn ,
				double ndwx , double ndwy ,
				double fwx , double fwy ,
				double npxM , double npyM ,
				double fpxM , double fpyM ,
				double pux , double puy ,
				double fux , double fuy )
		{
			xMax = xx;
			xMin = xn;
			yMax = yx;
			yMin = yn;
			note_pos_xMin = npxM;
			note_pos_yMax = npyM;
			free_pos_xMin = fpxM;
			free_pos_yMax = fpyM;
			note_disp_width_x = ndwx;
			note_disp_width_y = ndwy;
			pos_unit_x = pux;
			pos_unit_y = puy;
			free_width_x = fwx;
			free_width_y = fwy;
			free_unit_x = fux;
			free_unit_y = fuy;
		}
	}



	/*また作るのめんどいから残すけど使わない
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
}