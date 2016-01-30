using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PulseAlpha : MonoBehaviour
{

	public Image PulseImage;
	public float lowerAlpha;
	public float pulseTimer;

	void Start ()
	{
		StartCoroutine(PulseCoroutine());
	}

	IEnumerator PulseCoroutine()
	{
		while (true)
		{
			var sequence = DOTween.Sequence();
			sequence.Append(PulseImage.DOFade(lowerAlpha, pulseTimer));
			sequence.Append(PulseImage.DOFade(1, pulseTimer));
			yield return sequence.WaitForCompletion();
		}
	}
}
