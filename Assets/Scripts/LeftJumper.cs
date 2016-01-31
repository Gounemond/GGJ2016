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
	private bool grounded;
	public float stepsVolume = 0.5f;
	void Start()
	{
		rig = GetComponent<Rigidbody2D>();
		circ = GetComponent<CircleCollider2D>();
	}

	IEnumerator DestroyInSeconds(AudioSource target, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(target);
	}

	void FixedUpdate ()
	{
		var rayLength = circ.radius * transform.lossyScale.y + 0.1f;

		var newGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, 1 << 8).collider != null;
		if (newGrounded && !grounded)
		{
			var source = gameObject.AddComponent<AudioSource>();
			source.clip = SpiderRef.stepsSound;
			source.Play();
			source.volume = stepsVolume;
            StartCoroutine(DestroyInSeconds(source, source.clip.length));
		}
		grounded = newGrounded;

		if (jumpStarted || grounded)
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
