using UnityEngine;
using System.Collections;

public enum Side
{
	Left, Right
}

public class LeftJumper : MonoBehaviour
{
	public Vector2 JumpAmount;
	public float MaxJumpTime;
	private float timer;
	public Side Side;
	private Rigidbody2D rig;
	private bool jumpStarted;
	void Start()
	{
		rig = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate ()
	{
		var rayLength = GetComponent<CircleCollider2D>().radius * transform.lossyScale.y + 0.01f;
		if (jumpStarted || Physics2D.Raycast(transform.position, Vector2.down, rayLength, 1 << 8).collider != null)
		{
			var player = Rewired.ReInput.players.GetPlayer(0);
			if (player.GetAxis(Side + " Jump") > 0.2f)
			{
				if (!jumpStarted)
				{
					jumpStarted = true;
					timer = 0;
				}
				timer += Time.fixedDeltaTime;
				var vel = rig.velocity;
				vel.y = JumpAmount.y * player.GetAxis(Side + " Jump");
				rig.velocity = vel;
				if (timer >= MaxJumpTime)
				{
					jumpStarted = false;
				}
			}
		}

		
		
		Debug.DrawRay(transform.position, rayLength * Vector2.down);
	}
}
