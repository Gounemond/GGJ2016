using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Image fadeImage;

	void Start()
	{
		fadeImage.DOFade(0, 0.6f);
	}

	void Update()
	{
		foreach (var p in Rewired.ReInput.players.AllPlayers)
		{
			if (p.GetButtonDown("Start Game") || (p.GetButton("Left Jump") && p.GetButton("Right Jump")))
			{
				fadeImage.DOFade(1, 1).OnComplete(() =>
				{
					SceneManager.LoadScene(SRScenes.SpiderPhotoSession);
				}); 
			}

			if (p.GetButtonDown("End Game"))
			{
				fadeImage.DOFade(1, 1).OnComplete(() =>
				{
					Application.Quit();
				});
			}
		}
	}
}
