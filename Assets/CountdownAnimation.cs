using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class CountdownAnimation : MonoBehaviour
{
	public float enlargement = 1.4f;
	public float springbackTime = 0.6f;
	public Image im;
	public Sprite[] Sprites;

	public void Start()
	{
		if (im == null) im = GetComponent<Image>();
		var col = Color.white;
		col.a = 0f;
		im.color = col;
	}

	public void CountdownUpdate(int number)
	{
		im.DOFade(1, 0.1f);
		im.transform.localScale = Vector3.one*enlargement;
		im.transform.DOScale(Vector3.one, 0.6f);
		if (number > 0 && number <= Sprites.Length) im.sprite = Sprites[number-1];
	}

	public void CountdownStop()
	{
		im.DOFade(0, 0.1f);
	}
}
