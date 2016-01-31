using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour
{

	public Transform target;

	public void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().MovePosition(target.position);
	}
}
