using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameplayManager : MonoBehaviour
{
    // Singleton Implementation
    protected static GameplayManager _self;
    public static GameplayManager Self
    {
        get
        {
            if (_self == null)
                _self = FindObjectOfType(typeof(GameplayManager)) as GameplayManager;
            return _self;
        }
    }

    public int turnDuration = 5;

    // Waiting bool
    private bool m_WaitClick = false;

    private bool m_playing;
    private int m_score;
    private int m_bonus;


     private IEnumerator Start()
     {
        yield return StartCoroutine(StartPhase());
        yield return StartCoroutine(PlayPhase());
        yield return StartCoroutine(EndPhase());
     }

    private IEnumerator StartPhase()
    {
        //TutorialOverlays: Wait for user input
        yield return StartCoroutine(GameElements.Self.instructionSpider1.InteruptAndFadeOut());
        yield return StartCoroutine(GameElements.Self.instructionSpider2.InteruptAndFadeOut());

        //SelectingThePose
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeIn());
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose());
        yield return StartCoroutine(GameElements.Self.introGUI.InteruptAndFadeOut());

        yield return null;
    }

    private IEnumerator PlayPhase()
    {
        // The game is now playing.
        m_playing = true;

        // Go through the rounds of the game
        yield return StartCoroutine(PlayRound(1));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(PlayRound(2));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(PlayRound(3));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(PlayRound(4));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(PlayRound(5));
    }

    private IEnumerator PlayRound(int phaseNumber)
    {
        // Enable spider
        yield return new WaitForSeconds(6);

        yield return StartCoroutine(TakeThePhoto());
    }

    public IEnumerator TakeThePhoto()
    {


        // User need to press the button to proceed
        m_WaitClick = true;
        yield return StartCoroutine(waitSomethingClicked());                                      // Wait the user input to proceed


    }

    private IEnumerator EndPhase()
    {
        yield return null;

        SceneManager.LoadScene(13);
    }

    

    private void HandleGameComplete()
    {
        // If the game is complete it is no longer playing
        m_playing = false;
    }


    // Various Methods
    public IEnumerator waitSomethingClicked()
    {
        while (m_WaitClick)
        {
            yield return null;
        }
    }

    public void startPhase()
    {
        m_WaitClick = false;
    }
}
