using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using VRStandardAssets.Utils;
using System.Collections;

public class GUIManager : MonoBehaviour
{ 
    // Singleton implementation
    protected static GUIManager _self;
    public static GUIManager Self
    {
        get
        {
            if (_self == null)
                _self = FindObjectOfType(typeof(GUIManager)) as GUIManager;
            return _self;
        }
    }

    public Image smartphoneInitial;

    public Image countdown3;
    public Image countdown2;
    public Image countdown1;

    void Awake()
    {
        countdown3.enabled = false;
        countdown2.enabled = false;
        countdown1.enabled = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator SelectTheSexyPose()
    {
        /*Easing.easeToPosition(smartphoneInitial.transform,
                                smartphoneInitial.transform.position,
                                new Vector3(smartphoneInitial.transform.position.x, 200, smartphoneInitial.transform.position.z),
                                2, EasingType.InCubic);
          */
        smartphoneInitial.transform.DOMoveY(260, 0.5f);
        


        yield return null;
    }

    public IEnumerator StartCountDown()
    {
        countdown3.enabled = true;
        yield return new WaitForSeconds(0.5f);
        countdown3.enabled = false;
        yield return new WaitForSeconds(0.5f);
        countdown2.enabled = true;
        yield return new WaitForSeconds(0.5f);
        countdown2.enabled = false;
        yield return new WaitForSeconds(0.5f);
        countdown1.enabled = true;
        yield return new WaitForSeconds(0.5f);
        countdown1.enabled = false;
        yield return new WaitForSeconds(0.5f);
    }
}
