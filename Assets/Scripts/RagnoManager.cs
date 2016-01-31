using UnityEngine;
using System.Collections;

public class RagnoManager : MonoBehaviour
{
    public GameObject spider1;
    public GameObject spider2;

    private Vector3 m_spider1Position;
    private Vector3 m_spider2Position;


	// Use this for initialization
	void Start ()
    {
        m_spider1Position = spider1.transform.position;
        m_spider2Position = spider2.transform.position;
		spider1.GetComponent<SpiderMain>().playerID = 0;
		spider2.GetComponent<SpiderMain>().playerID = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	public void FreezeSpiders()
	{
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
		spider1.GetComponent<SpiderMain>().playerID = 0;
		spider2.GetComponent<SpiderMain>().playerID = 1;
		spider1.transform.position = m_spider1Position;
        spider2.transform.position = m_spider2Position;
    }
}
