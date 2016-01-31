using UnityEngine;
using System.Collections;

public class rotationalSpring : MonoBehaviour
{
	public float KappaFactor;
	/// <summary>
	/// ve
	/// </summary>
	void FixedUpdate ()
	{
		var rb = GetComponent<Rigidbody2D>();
		rb.AddTorque(-rb.rotation*KappaFactor);
	}
}
