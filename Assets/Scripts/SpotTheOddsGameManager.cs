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
        // Get testData for the current playingGame

        // Setting bonus and score to 0;
        m_score = 0;
        m_bonus = 1000;


        // Show welcome message

        yield return StartCoroutine(WaitToStartNextRound());
    }

    private IEnumerator PlayPhase()
    {
        // The game is now playing.
        m_playing = true;

        // Go through the rounds of the game
        yield return StartCoroutine(PlayRound(1));
        yield return StartCoroutine(WaitToStartNextRound());
        yield return StartCoroutine(PlayRound(2));
        yield return StartCoroutine(WaitToStartNextRound());
        yield return StartCoroutine(PlayRound(3));
        yield return StartCoroutine(WaitToStartNextRound());
        yield return StartCoroutine(PlayRound(4));
        yield return StartCoroutine(WaitToStartNextRound());
        yield return StartCoroutine(PlayRound(5));
    }

    private IEnumerator PlayRound(int phaseNumber)
    {
        yield return null;
    }

    

    private IEnumerator EndPhase()
    {
        yield return null;

        SceneManager.LoadScene(13);
    }

    public IEnumerator WaitToStartNextRound()
    {
      

        // User need to press the button to proceed
        m_WaitClick = true;
        yield return StartCoroutine(waitSomethingClicked());                                      // Wait the user input to proceed


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
