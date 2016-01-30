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

		yield return smartphoneInitial.transform.DOLocalMoveY(160, 1.5f).WaitForCompletion();

        yield return new WaitForSeconds(1f);
		switch (poseSelected)
        {
            case 0:
                break;
            case 1:
				yield return scrollViewContent.DOLocalMoveY(40, 1f).WaitForCompletion();
                break;
            case 2:
				yield return scrollViewContent.DOLocalMoveY(80, 1f).WaitForCompletion();
                break;
            case 3:
				yield return scrollViewContent.DOLocalMoveY(120, 1f).WaitForCompletion();
				break;
            case 4:
				yield return scrollViewContent.DOLocalMoveY(160, 1f).WaitForCompletion();
				break;
        }
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(poseToBlink[poseSelected].DOColor(Color.green, 0.5f));
		mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
		yield return mySequence.WaitForCompletion();

		Sequence mySequence2 = DOTween.Sequence();
		mySequence2.Append(poseToBlink[poseSelected].DOColor(Color.green, 0.5f));
		mySequence2.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
		yield return mySequence2.WaitForCompletion();

		yield return smartphoneInitial.transform.DOMoveY(0, 1f).WaitForCompletion();

		yield return poseSuggestorSpider1[poseSelected].DOColor(Color.white,1).WaitForCompletion();
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeOut());         
    }
}
