using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class TakeAScreenshot : MonoBehaviour
{

    private bool taking;

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.A) && !taking) //Debug eh
        {
            StartCoroutine(ScreenshotHappy(0));
            taking = true;
        }
    }

    public IEnumerator ScreenshotHappy(int playerCount)
    {
        //Miro dove stanno i giocatori (trucco: con 0 fa tutto lo schermo
        Vector2 imageFrom = playerCount <=1 ? new Vector2(0, 0) : new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 imageTo = playerCount == 1 ? new Vector2(Screen.width / 2, Screen.height / 2) : new Vector2(Screen.width, Screen.height);
        
        byte[] temp = GetScreenshot(CaptureMethod.RenderToTex_Synch, imageFrom, imageTo).EncodeToJPG();
        byte[] report = new byte[temp.Length];
        for (int i = 0; i < report.Length; i++)
        {
            report[i] = (byte)temp[i];
        } // for
        // create a form to send the data to the sever
        WWWForm form = new WWWForm();
        // add the necessary data to the form
        form.AddBinaryData("upload_file", report, "spinder.jpg", "image/jpeg"); ;
        // send the data via web
        WWW www2 = new WWW("http://risingpixel.azurewebsites.net/other/apps/spinder/upload.php", form);
        // wait for the post completition
        while (!www2.isDone)
        {
            Debug.Log("I'm waiting... " + www2.uploadProgress);
            yield return new WaitForEndOfFrame();
        } // while
        // print the server answer on the debug log
        Debug.Log(www2.text);
        // destroy the www
        www2.Dispose();

        yield return null;
        taking = false;

        yield return new WaitForSeconds(.2f);
    }

    #region AsyncHelpers
    //Esempio base GameHelper.Current.DelayInvoke(LoadGame, 2f);
    //Esempio inline con parametri GameHelper.Current.DelayInvoke(new Action(delegate { InitGame(slg.LoadGame()); }), 2f);
    public void DelayInvoke(Action a, float seconds)
    {
        DelayInvoke(a, seconds, null);
    }

    public void DelayInvoke(Action a, float seconds, Action onComplete)
    {
        StartCoroutine(ActionInvoke(a, seconds, onComplete));
    }

    private IEnumerator ActionInvoke(Action a, float seconds, Action onComplete)
    {
        yield return new WaitForSeconds(seconds);
        a();

        if (onComplete != null)
            onComplete();
    }
    #endregion

    public enum CaptureMethod
    {
        AppCapture_Asynch,
        AppCapture_Synch,
        ReadPixels_Asynch,
        ReadPixels_Synch,
        RenderToTex_Asynch,
        RenderToTex_Synch
    }
    private static int tempFileCount = 0;
    public Texture2D GetScreenshot(CaptureMethod method, Vector2? from = null, Vector2? to = null)
    {
        if (from == null)
        {
            from = new Vector2(0, 0);
        }

        if (to == null)
        {
            to = new Vector2(Screen.width, Screen.height);
        }

        Vector2 imageDimension = to.Value - from.Value;

        if (method == CaptureMethod.AppCapture_Synch)
        {
            string tempFilePath = System.Environment.GetEnvironmentVariable("TEMP") + "/screenshotBuffer" + tempFileCount + ".png";
            tempFileCount++;
            Application.CaptureScreenshot(tempFilePath);
            WWW www = new WWW("file://" + tempFilePath.Replace(Path.DirectorySeparatorChar.ToString(), "/"));

            Texture2D texture = new Texture2D((int)imageDimension.x, (int)imageDimension.y, TextureFormat.RGB24, false);
            while (!www.isDone) { }
            www.LoadImageIntoTexture((Texture2D)texture);
            File.Delete(tempFilePath); //Can delete now

            return texture;
        }
        else if (method == CaptureMethod.ReadPixels_Synch)
        {
            //Create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            //Put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); //Unity complains about this line's call being made "while not inside drawing frame", but it works just fine.*

            return texture;
        }


        ///GUARDA ME CHE HO MIRATO MALE
        else if (method == CaptureMethod.RenderToTex_Synch)
        {
            RenderTexture rt = new RenderTexture((int)imageDimension.x, (int)imageDimension.y, 24);
            Texture2D screenShot = new Texture2D((int)imageDimension.x, (int)imageDimension.y, TextureFormat.RGB24, false);

            //Camera.main.targetTexture = rt;
            //Camera.main.Render();

            //Render from all!
            foreach (Camera cam in Camera.allCameras)
            {
                cam.targetTexture = rt;
                cam.Render();
                cam.targetTexture = null;
            }

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(from.Value, imageDimension), (int)from.Value.x, (int)from.Value.y);
            Camera.main.targetTexture = null;
            RenderTexture.active = null; //Added to avoid errors
            Destroy(rt);

            return screenShot;
        }
        else
            return null;
    }
}
