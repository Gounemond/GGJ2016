using UnityEngine;
using System.Collections;
using System.IO;

public class TakeAScreenshot : MonoBehaviour {

	private bool taking;

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.F1) && !taking){
			StartCoroutine(ScreenShot());
			taking = true;
		}
	}

	public IEnumerator ScreenShot(){
		transform.position = new Vector3(.5f,.5f,0f);
		transform.eulerAngles = new Vector3(0,0,0);
		transform.localScale = new Vector3(0,0,1);
		
		string filename = System.DateTime.Now.ToString("hh.mm.ss") + "." + System.DateTime.Now.ToString("MM.dd.yyyy") + ".png"; 
		string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures) + "//The Sound of Silence//Screenshots";

		GetComponent<GUITexture>().pixelInset = new Rect(-Screen.width/2,-Screen.height/2,Screen.width,Screen.height);
		GetComponent<GUITexture>().enabled = true;
		
		
		if(!Directory.Exists(path))
			Directory.CreateDirectory(path);

		yield return new WaitForSeconds (.2f);
		Application.CaptureScreenshot(path + "//" + filename, 3);
		yield return new WaitForSeconds (.2f);

		GetComponent<GUITexture>().enabled = false;
		

		GetComponent<AudioSource>().Play ();
		//ScreenFader.StartAlphaFade (Color.white, true, 2);

		taking = false;
		print("Screenshot taked: " + path + "//" + filename);
	}
}
