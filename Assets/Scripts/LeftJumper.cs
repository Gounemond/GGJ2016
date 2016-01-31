using System;
using UnityEngine;
using System.Collections;

public enum Side
{
	Left, Right
}

public class LeftJumper : MonoBehaviour
{
	public SpiderMain SpiderRef;
	public Vector2 JumpAmount;
	public float MaxJumpTime;
	private float timer;
	public Side Side;
	private Rigidbody2D rig;
	private bool jumpStarted;
	private CircleCollider2D circ;
	void Start()
	{
		rig = GetComponent<Rigidbody2D>();
		circ = GetComponent<CircleCollider2D>();
	}

	void FixedUpdate ()
	{
		var rayLength = circ.radius * transform.lossyScale.y + 0.1f;
		if (jumpStarted || Physics2D.Raycast(transform.position, Vector2.down, rayLength, 1 << 8).collider != null)
		{
			var player = SpiderRef.RwPlayer;
			if (player.GetAxis(Side + " Jump") > 0.2f)
			{
				if (!jumpStarted)
				{
					var anim = SpiderRef.GetComponent<Animator>();
					anim.Play("Salto"+Side, anim.GetLayerIndex("Gambe"+Side), 0f);
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
