using UnityEngine;
using System.Collections;

public class rotationalSpring : MonoBehaviour
{
	public float MaxRotation;
	/// <summary>
	/// ve
	/// </summary>
	void FixedUpdate ()
	{
		var rb = GetComponent<Rigidbody2D>();
		if (rb.rotation > MaxRotation) rb.MoveRotation(MaxRotation);
		if (rb.rotation < -MaxRotation) rb.MoveRotation(-MaxRotation);
	}
}
