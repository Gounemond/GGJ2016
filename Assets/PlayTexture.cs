using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayTexture : MonoBehaviour
{

	private float length;

	// Use this for initialization
	void Start ()
	{
		var movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
		movie.Play();
		length = movie.duration;
		StartCoroutine(MovieCoroutine());
	}

	IEnumerator MovieCoroutine()
	{
		yield return new WaitForSeconds(length);

		SceneManager.LoadScene(SRScenes.SpiderPhotoSession);
	}
}
