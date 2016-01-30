using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class CountdownAnimation : MonoBehaviour
{
	public float enlargement = 1.4f;
	public float springbackTime = 0.6f;
	public Text Text;

	public void Start()
	{
		if (Text == null) Text = GetComponent<Text>();
		var col = Color.white;
		col.a = 0f;
		Text.color = col;
	}

	public void CountdownUpdate(int number)
	{
		Text.DOFade(1, 0.1f);
		Text.transform.localScale = Vector3.one*enlargement;
		Text.transform.DOScale(Vector3.one, 0.6f);
		Text.text = number.ToString();
	}

	public void CountdownStop()
	{
		Text.DOFade(0, 0.1f);
	}
}
