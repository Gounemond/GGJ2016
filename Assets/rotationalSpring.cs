using UnityEngine;
using System.Collections;

public class rotationalSpring : MonoBehaviour
{
	public float KappaFactor;

	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// ve
	/// </summary>
	void FixedUpdate ()
	{
		rb.AddTorque(-rb.rotation*KappaFactor);
	}
}
