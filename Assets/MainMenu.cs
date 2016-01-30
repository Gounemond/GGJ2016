using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	void Update()
	{
		foreach (var p in Rewired.ReInput.players.AllPlayers)
		{
			if (p.GetButtonDown("Start Game") || (p.GetButton("Left Jump") && p.GetButton("Right Jump")))
			{
				SceneManager.LoadScene(SRScenes.SpiderPhotoSession);
			}

			if (p.GetButtonDown("End Game"))
			{
				Application.Quit();
			}
		}
	}
}
