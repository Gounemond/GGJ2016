using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Image fadeImage;
    public AudioSource clickToPlay;
	private AudioSource source;
	public bool goToMovieScene;
	void Start()
	{
		source = GetComponent<AudioSource>();
		fadeImage.DOFade(0, 0.6f);
	}

	void Update()
	{
		foreach (var p in Rewired.ReInput.players.AllPlayers)
		{
			if (p.GetButtonDown("Start Game") || (p.GetButton("Left Jump") && p.GetButton("Right Jump")))
			{
                clickToPlay.Play();
                source.DOFade(0, 1);
                fadeImage.DOFade(1, 1).OnComplete(() =>
                {
	                if (goToMovieScene) SceneManager.LoadScene(SRScenes.movieScene);
	                else SceneManager.LoadScene(SRScenes.SpiderPhotoSession);
                }); 
			}

			if (p.GetButtonDown("End Game"))
			{
				source.DOFade(0, 1);
				fadeImage.DOFade(1, 1).OnComplete(() =>
				{
					Application.Quit();
				});
			}
		}
	}
}
