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
        yield return StartCoroutine(PlayRound(0));
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(1));
        yield return StartCoroutine(PlayRound(1));
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(2));
        yield return StartCoroutine(PlayRound(2));
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(3));
        yield return StartCoroutine(PlayRound(3));
    }

	private IEnumerator PlayRound(int poseNumber)
	{
        // Reset the points
        m_likesSpider1 = 0;
        m_likesSpider2 = 0;
        GameElements.Self.spiderOneTriggerPoses[poseNumber].SetActive(true);
        GameElements.Self.spiderTwoTriggerPoses[poseNumber].SetActive(true);


        // Do the countdown while playing
        var cd = GameElements.Self.countdownCanvas.GetComponentInChildren<CountdownAnimation>();
	    for (int i = 6; i > 0; i--)
	    {
			cd.CountdownUpdate(i);
			yield return new WaitForSeconds(1);
	    }
        // Countdown finished
		cd.CountdownStop();

        // Smile! Photo time!
        GameElements.Self.spiderOneTriggerPoses[poseNumber].SetActive(false);
        GameElements.Self.spiderTwoTriggerPoses[poseNumber].SetActive(false);
        yield return StartCoroutine(TakeThePhoto());
    }

    public IEnumerator TakeThePhoto()
    {
        // Clear all the suggestors, so they don't get in the screenshot
        for (int i = 0; i < GameElements.Self.GUIManager.poseSuggestorSpider1.Length; i++)
        {
            GameElements.Self.GUIManager.poseSuggestorSpider1[i].color = Color.clear;
        }

        // FLASH PHOTOGRAPH!
        yield return StartCoroutine(GameElements.Self.flash.FadeIn());

        // Spiders in pose for the photo!
		FindObjectOfType<RagnoManager>().FreezeSpiders();

        // FLASH FINISHED
        yield return StartCoroutine(GameElements.Self.flash.FadeOut());

        // Screenshot By Tato
        if (m_likesSpider1 >= m_likesSpider2)
        {
            //StartCoroutine(GameElements.Self.screenshotCamera.ScreenshotHappy(1));
        }
        else
        {
            //StartCoroutine(GameElements.Self.screenshotCamera.ScreenshotHappy(2));
        }

        yield return new WaitForSeconds(1);

        // Play the tinder swiping games 
        yield return StartCoroutine(GameElements.Self.tinderSwipeManager.TimeToPickUpChicks(m_likesSpider1, m_likesSpider2));

        m_totalLikesSpider1 += m_likesSpider1;
        m_totalLikesSpider2 += m_likesSpider2;

        // Spiders can move again, ready to get another shot!
        FindObjectOfType<RagnoManager>().UnfreezeSpiders();
    }

    public IEnumerator EndPhase()
    {
        if (m_totalLikesSpider1 >= m_totalLikesSpider2)
        {
            yield return StartCoroutine(GameElements.Self.GUIManager.WinningScreenTime(true));
        }
        else
        {
            yield return StartCoroutine(GameElements.Self.GUIManager.WinningScreenTime(false));
        }

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

    public void EnteredInThePose(int player)
    {
        if (player ==1)
        {
            m_likesSpider1++;
        }
        else
        {
            m_likesSpider2++;
        }
        Debug.Log(m_likesSpider1);
    }

    public void ExitedFromThePose(int player)
    {
        if (player == 1)
        {
            m_likesSpider1--;
        }
        else
        {
            m_likesSpider2--;
        }
    }

}
