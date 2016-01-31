using UnityEngine;
using System;
using System.Collections;

public class SpiderTrigger : MonoBehaviour {

    public event Action onCollisionEnter;              // Called when a Tangram Object enters the collider
    public event Action onCollisionExit;               // Called when a Tangram Object enters the collider

    public int spiderNumber = 1;

    private int objectsInCollider = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (objectsInCollider == 0)
        {
            GameplayManager.Self.EnteredInThePose(spiderNumber);
            if (onCollisionEnter != null)
            {
                onCollisionEnter();
            }
        }
        objectsInCollider++;
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (objectsInCollider == 1)
        {
            GameplayManager.Self.ExitedFromThePose(spiderNumber);
            if (onCollisionExit != null)
            {
                onCollisionExit();
            }
            objectsInCollider--;
        }
    }
}
