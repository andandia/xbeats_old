using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class tweentest : MonoBehaviour {

	[SerializeField]
	GameObject one;
	//Sprite Sprite;
	[SerializeField]
	SpriteRenderer image;

	// 透明tweenのテスト
	void Start () {
		image.color = new Color(1, 1, 1, 0);//透明にする
		//var image = this.gameObject.GetComponent<Image>();
		DOTween.ToAlpha(//透明から不透明へ
		() => image.color,
		color => image.color = color,
		1f,                                // 最終的なalpha値
		8f	//時間
);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
