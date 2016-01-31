using UnityEngine;
using System.Collections;

public class SpiderMain : MonoBehaviour
{
	public int playerID;
	public Rewired.Player RwPlayer { get { return Rewired.ReInput.players.GetPlayer(playerID); } }
	public bool IsInputEnabled = true;
	public AudioClip stepsSound;
	public void Update()
	{
		foreach (var c in GetComponentsInChildren<LeftJumper>())
		{
			c.enabled = IsInputEnabled;
		}

		foreach (var c in GetComponentsInChildren<ArmControlTest>())
		{
			c.enabled = IsInputEnabled;
		}
	}
}
