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

    [Header("GUI Canvas")]
    public UIFader pressToStart;                                            // UI Element with the button to start a round
    public UIFader gameGUI;                                                 // UI Element with the grid

    [Header("Core Mechanic Objects")]

    [Header("Audio effects")]
    public AudioSource correctAnswer;
    public AudioSource wrongAnswer;
    public AudioSource roundCompleted;
                    

}