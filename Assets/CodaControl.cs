using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class CodaControl : MonoBehaviour
{

	private System.Random random;
	private SpiderMain spider;
	void Start()
	{
		random = new System.Random();
		spider = GetComponent<SpiderMain>();
	}

	void Update ()
	{
		if (spider.RwPlayer.GetButtonDown("CodaControl"))
		{
			var anim = GetComponent<Animator>();
			anim.CrossFade("coda"+random.Next(1,4), 0.2f, anim.GetLayerIndex("Coda"));
		}
	}
}
