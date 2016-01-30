using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PulseAlpha : MonoBehaviour
{

	public Image PulseImage;
	public float lowerAlpha;
	public float pulseTimer;
	public float pauseTime;
	void Start ()
	{
		StartCoroutine(PulseCoroutine());
	}

	IEnumerator PulseCoroutine()
	{
		while (true)
		{
			var sequence = DOTween.Sequence();
			yield return PulseImage.DOFade(lowerAlpha, pulseTimer).WaitForCompletion();
			//yield return new WaitForSeconds(pauseTime);
			yield return PulseImage.DOFade(1, pulseTimer).WaitForCompletion();
			yield return new WaitForSeconds(pauseTime);
		}
	}
}
