using System;
using UnityEngine;
using System.Collections;

public class RagnoManager : MonoBehaviour
{
    public GameObject spider1;
    public GameObject spider2;

	private System.Random random;

    private Vector3 m_spider1Position;
    private Vector3 m_spider2Position;

	private string[] skins = new[]
	{
		"ragno-blu",
		"ragno-verde2",
		"ragno-viola"
	};

	void SetupSpiders()
	{
		spider1.GetComponent<SpiderMain>().playerID = 0;
		spider2.GetComponent<SpiderMain>().playerID = 1;

		/*
		int skin_no = random.Next(0,3);
		var sa1 = spider1.GetComponent<SkeletonAnimator>();
		var sa2 = spider2.GetComponent<SkeletonAnimator>();
		sa1.Skeleton.Skin = sa1.skeleton.data.Skins.Find( s => s.Name == skins[skin_no]);
		sa2.Skeleton.Skin = sa2.skeleton.data.Skins.Find(s => s.Name == skins[(skin_no + 1) % 3]);
		*/

		spider1.transform.position = m_spider1Position;
		spider2.transform.position = m_spider2Position;
	}

	// Use this for initialization
	void Start ()
    {
		random = new System.Random();
		m_spider1Position = spider1.transform.position;
        m_spider2Position = spider2.transform.position;
		SetupSpiders();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	public void FreezeSpiders()
	{
		spider1.GetComponent<Animator>().speed = 0;
		spider2.GetComponent<Animator>().speed = 0;
		foreach (var b in spider1.GetComponentsInChildren<Rigidbody2D>())
		{
			b.simulated = false;
		}
		foreach (var b in spider2.GetComponentsInChildren<Rigidbody2D>())
		{
			b.simulated = false;
		}
	}

	public void UnfreezeSpiders()
	{
		spider1.GetComponent<Animator>().speed = 1;
		spider2.GetComponent<Animator>().speed = 1;
		foreach (var b in spider1.GetComponentsInChildren<Rigidbody2D>())
		{
			b.simulated = true;
		}
		foreach (var b in spider2.GetComponentsInChildren<Rigidbody2D>())
		{
			b.simulated = true;
		}
	}

	public void ResetSpiderPositions()
    {
        Destroy(spider1);
        Destroy(spider2);
        spider1 = Instantiate(SRResources.Spider.Load());
        spider2 = Instantiate(SRResources.Spider.Load());
		SetupSpiders();
    }
}
