using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using System.Collections;
using System.Collections.Generic;

// Class containing all the useful elements of this particular game
public class GameElements : MonoBehaviour
{
    // Singleton implementation
    protected static GameElements _self;
    public static GameElements Self
    {
        get
        {
            if (_self == null)
                _self = FindObjectOfType(typeof(GameElements)) as GameElements;
            return _self;
        }
    }

    public string buildVersion
    {
        get
        {
            return System.IO.File.ReadAllText(Application.streamingAssetsPath + "/version");
        }
        set
        {
            System.IO.File.WriteAllText(Application.streamingAssetsPath + "/version", value);
        }
    }

    void Awake()
    {
        introGUI.GetComponent<CanvasGroup>().alpha = 0;
        instructionSpider1.GetComponent<CanvasGroup>().alpha = 1;
        instructionSpider2.GetComponent<CanvasGroup>().alpha = 1;
    }

    public GUIManager GUIManager;

    [Header("GUI Canvas")]
    public UIFader introGUI;                                            
    public UIFader instructionSpider1;
    public UIFader instructionSpider2;
    public UIFader flash;
	public GameObject countdownCanvas;

    [Header("Core Mechanic Objects")]
    public DetectInputToStart inputToStartGame;
    public RagnoManager ragnoManager;

    [Header("Audio effects")]
    public AudioSource correctAnswer;
    public AudioSource wrongAnswer;
    public AudioSource roundCompleted;
                    

}