using UnityEngine;
using System;
using System.Collections;

public class DetectInputToStart : MonoBehaviour {


    public event Action player1Ready;
    public event Action player2Ready; 

	// Use this for initialization
	void Start ()
    {
        var playerInput1 = Rewired.ReInput.players.GetPlayer(0);
        var playerInput2 = Rewired.ReInput.players.GetPlayer(1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Rewired.ReInput.players.GetPlayer(0).GetAnyButton())
        {
            if (player1Ready != null)
            {
                player1Ready();
            }
        }

        if (Rewired.ReInput.players.GetPlayer(1).GetAnyButton())
        {
            if (player2Ready != null)
            {
                player2Ready();
            }
        }

    }
}
