﻿using UnityEngine;
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
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(3));

        yield return null;
    }

    private IEnumerator PlayPhase()
    {
        // The game is now playing.
        m_playing = true;

        // Go through the rounds of the game
        yield return StartCoroutine(PlayRound(1));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(4));
        yield return StartCoroutine(PlayRound(2));
        yield return StartCoroutine(TakeThePhoto());
        yield return StartCoroutine(GameElements.Self.GUIManager.SelectTheSexyPose(2));
        yield return StartCoroutine(PlayRound(3));
        yield return StartCoroutine(TakeThePhoto());
    }

    private IEnumerator PlayRound(int phaseNumber)
    {
        // Enable spider
        yield return new WaitForSeconds(6);

        yield return StartCoroutine(TakeThePhoto());
    }

    public IEnumerator TakeThePhoto()
    {
        //GameElements.Self.ragnoManager.spider1.SetActive(false);
        //GameElements.Self.ragnoManager.spider2.SetActive(false);

        // Lampeggio, audio e cose varie
        yield return new WaitForSeconds(0.2f);
        // Screenshot By Tato

        //GameElements.Self.ragnoManager.ResetSpiderPositions();
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
