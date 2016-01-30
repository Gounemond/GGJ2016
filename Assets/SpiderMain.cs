using UnityEngine;
using System.Collections;

public class SpiderMain : MonoBehaviour
{
	public int playerID;
	public Rewired.Player RwPlayer { get { return Rewired.ReInput.players.GetPlayer(playerID); } }
	public bool IsInputEnabled = true;
}
