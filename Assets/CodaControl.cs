using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class CodaControl : MonoBehaviour
{

	private System.Random random;

	void Start()
	{
		random = new System.Random();
	}

	void Update ()
	{
		var spider = GetComponent<SpiderMain>();
		if (spider.RwPlayer.GetButtonDown("CodaControl"))
		{
			var anim = GetComponent<Animator>();
			anim.CrossFade("coda"+random.Next(1,4), 0.2f, anim.GetLayerIndex("Coda"));
		}
	}
}
