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

    [Header ("Selezione della posa")]
    public Image smartphoneInitial;
    public RectTransform scrollViewContent;
    public Image[] poseToBlink;
    public Image[] poseSuggestorSpider1;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator SelectTheSexyPose(int poseSelected = 3)
    {
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeIn());

        smartphoneInitial.transform.DOLocalMoveY(160, 1.5f);
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1f);
        Sequence mySequence = DOTween.Sequence();

        switch (poseSelected)
        {
            case 0:
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                break;
            case 1:
                scrollViewContent.DOLocalMoveY(40, 1f);
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));

                break;
            case 2:
                scrollViewContent.DOLocalMoveY(80, 1f);
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));

                break;
            case 3:
                scrollViewContent.DOLocalMoveY(120, 1f);
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                break;
            case 4:
                scrollViewContent.DOLocalMoveY(160, 1f);
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 1f));
                mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
                break;
        }
        yield return new WaitForSeconds(7);
        smartphoneInitial.transform.DOMoveY(0, 1f);
        yield return new WaitForSeconds(1f);

        poseSuggestorSpider1[poseSelected].DOColor(Color.white,1);
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeOut());         
    }
}
