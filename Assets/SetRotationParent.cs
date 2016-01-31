using UnityEngine;
using System.Collections;

public class SetRotationParent : MonoBehaviour
{

	public Rigidbody2D follow;
	public Vector2 offset;
	private TargetJoint2D target;
	public float SpringFrequency;
	public float SpringDamping;
	private void Start()
	{
		target = gameObject.AddComponent<TargetJoint2D>();
		target.frequency = SpringFrequency;
		target.dampingRatio = SpringDamping;
	}

	private void FixedUpdate()
	{
		target.frequency = SpringFrequency;
		target.dampingRatio = SpringDamping;
		var rb = GetComponent<Rigidbody2D>();
		rb.rotation = Mathf.LerpAngle(rb.rotation, follow.rotation, 1);
		target.anchor = Vector2.down;
		target.target = follow.position + (Vector2) follow.transform.TransformVector(offset);
		//rb.position = (Vector2.Lerp(rb.position, follow.position + (Vector2) follow.transform.TransformVector(offset), Mathf.Pow(0.1f, Time.fixedDeltaTime)));
	}
}