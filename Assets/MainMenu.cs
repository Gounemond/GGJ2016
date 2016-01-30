using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	void Update()
	{
		var system = Rewired.ReInput.players.GetSystemPlayer();
		if (system.GetButtonDown("Start Game") || (system.GetButton("Left Jump") && system.GetButton("Right Jump")))
		{
			SceneManager.LoadScene(SRScenes.SpiderPhotoSession);
		}

		if (system.GetButtonDown("End Game"))
		{
			Application.Quit();
		}
	}
}
