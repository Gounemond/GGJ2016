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

    private bool player1Ready = false;
    private bool player2Ready = false;

    private int m_likesSpider1;
    private int m_likesSpider2;
    private int m_totalLikesSpider1;
    private int m_totalLikesSpider2;

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
        yield return StartCoroutine(WaitBothPlayersReady());

        //SelectingThePose
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(0));

        yield return null;
    }

    private IEnumerator PlayPhase()
    {
        // The game is now playing.
        m_playing = true;

        // Go through the rounds of the game
        yield return StartCoroutine(PlayRound(1));
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(4));
        yield return StartCoroutine(PlayRound(2));
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(2));
        yield return StartCoroutine(PlayRound(3));
    }

	private IEnumerator PlayRound(int phaseNumber)
	{
		var cd = GameElements.Self.countdownCanvas.GetComponentInChildren<CountdownAnimation>();
	    for (int i = 6; i > 0; i--)
	    {
			cd.CountdownUpdate(i);
			yield return new WaitForSeconds(1);
	    }
		cd.CountdownStop();
        yield return StartCoroutine(TakeThePhoto());
    }

    public IEnumerator TakeThePhoto()
    {
        for (int i = 0; i < GameElements.Self.GUIManager.poseSuggestorSpider1.Length; i++)
        {
            GameElements.Self.GUIManager.poseSuggestorSpider1[i].color = Color.clear;
        }

        yield return StartCoroutine(GameElements.Self.flash.FadeIn());

        // Lampeggio, audio e cose varie
		FindObjectOfType<RagnoManager>().FreezeSpiders();

        yield return StartCoroutine(GameElements.Self.flash.FadeOut());

        // Screenshot By Tato

        yield return StartCoroutine(GameElements.Self.tinderSwipeManager.TimeToPickUpChicks(7, 4));

        // Lampeggio, audio e cose varie
        FindObjectOfType<RagnoManager>().UnfreezeSpiders();
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

    public void startPhase()
    {
        m_WaitClick = false;
    }

    public IEnumerator WaitBothPlayersReady()
    {
        GameElements.Self.inputToStartGame.enabled = true;
        GameElements.Self.inputToStartGame.player1Ready += SetPlayer1Ready;
        GameElements.Self.inputToStartGame.player2Ready += SetPlayer2Ready;

        while (!player1Ready || !player2Ready)
        {
            yield return null;
        }

        GameElements.Self.inputToStartGame.player1Ready -= SetPlayer1Ready;
        GameElements.Self.inputToStartGame.player2Ready -= SetPlayer2Ready;
        GameElements.Self.inputToStartGame.enabled = false;
    }

    public void SetPlayer1Ready()
    {
        if (!player1Ready)
        {
            StartCoroutine(GameElements.Self.instructionSpider1.FadeOut());
        }
        player1Ready = true;
    }

    public void SetPlayer2Ready()
    {
        if (!player2Ready)
        {
            StartCoroutine(GameElements.Self.instructionSpider2.FadeOut());
        }
        player2Ready = true;
    }

}
