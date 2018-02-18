using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Manager : MonoBehaviour {

	[SerializeField] private ParticleSystem fast_slow;


	public void Play_Particle(int judgeType ,Vector2 end_pos)
	{
		fast_slow.transform.position = end_pos;
		fast_slow.Play();
	}
}
