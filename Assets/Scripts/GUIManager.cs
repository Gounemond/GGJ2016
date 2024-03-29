﻿using UnityEngine;
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

    public Image spiderWinning;
    public Image spiderLosing;
    public Sprite spiderWebOnScreen;

    public IEnumerator SelectTheSexyPose(int poseSelected = 3)
    {
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeIn());

        AudioManager.Self.SmartPhoneAppears();
		yield return smartphoneInitial.transform.DOLocalMoveY(200, 1.5f).WaitForCompletion();

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
            case 5:
                yield return scrollViewContent.DOLocalMoveY(200, 1f).WaitForCompletion();
                break;
            case 6:
                yield return scrollViewContent.DOLocalMoveY(240, 1f).WaitForCompletion();
                break;
        }
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(poseToBlink[poseSelected].DOColor(new Color(1,0.78f,0.76f), 0.5f));
		mySequence.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
		yield return mySequence.WaitForCompletion();

		Sequence mySequence2 = DOTween.Sequence();
		mySequence2.Append(poseToBlink[poseSelected].DOColor(new Color(1, 0.78f, 0.76f), 0.5f));
		mySequence2.Append(poseToBlink[poseSelected].DOColor(Color.white, 0.5f));
		yield return mySequence2.WaitForCompletion();
		     
        AudioManager.Self.SmartPhoneAppears();
		yield return smartphoneInitial.transform.DOMoveY(0, 1f).WaitForCompletion();

        AudioManager.Self.PoseSelected();
        yield return poseSuggestorSpider1[poseSelected].DOColor(Color.white,1).WaitForCompletion();
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeOut());         
    }

    public IEnumerator WinningScreenTime(bool hasPlayer1Won)
    {

        if (!hasPlayer1Won)
        {
            Debug.Log("Il secondo ha vinto");
            yield return spiderWinning.transform.DOLocalMoveX(0, 0.1f).WaitForCompletion();
            yield return spiderLosing.transform.DOLocalMoveX(0, 0.1f).WaitForCompletion();
            //spiderWinning.transform.position = new Vector3(400, spiderWinning.transform.position.y, spiderWinning.transform.position.z);
            //spiderLosing.transform.position = new Vector3(-400, spiderLosing.transform.position.y, spiderLosing.transform.position.z);
        }

        yield return StartCoroutine(GameElements.Self.finalEndScreen.InteruptAndFadeIn());

        spiderWinning.transform.DOLocalMoveY(0, 2);
        yield return spiderLosing.transform.DOLocalMoveY(0, 2).WaitForCompletion();

        yield return new WaitForSeconds(2);
        spiderLosing.sprite = spiderWebOnScreen;
        yield return new WaitForSeconds(2); 

    }
}
