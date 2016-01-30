using UnityEngine;
using System.Collections;

public class RagnoManager : MonoBehaviour
{
    public GameObject spider1;
    public GameObject spider2;

    private Vector3 m_spider1Position;
    //private Vector3 m_spider2Position;


	// Use this for initialization
	void Start ()
    {
        m_spider1Position = spider1.transform.position;
        //m_spider2Position = spider2.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void ResetSpiderPositions()
    {
        Destroy(spider1);
        //Destroy(spider2);
        spider1 = (GameObject)Instantiate(Resources.Load("Spider"));
        //spider2 = (GameObject)Instantiate(Resources.Load("Spider"));
        spider1.transform.position = m_spider1Position;
        //spider2.transform.position = m_spider2Position;
    }
}
