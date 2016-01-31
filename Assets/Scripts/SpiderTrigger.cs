using UnityEngine;
using System;
using System.Collections;

public class SpiderTrigger : MonoBehaviour {

    public event Action onCollisionEnter;              // Called when a Tangram Object enters the collider
    public event Action onCollisionExit;               // Called when a Tangram Object enters the collider

    private int objectsInCollider = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spider")
        {
            if (objectsInCollider == 0)
            {
                if (onCollisionEnter != null)
                {
                    onCollisionEnter();
                }
            }
            objectsInCollider++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Spider")
        {
            if (objectsInCollider == 1)
            {
                if (onCollisionExit != null)
                {
                    onCollisionExit();
                }
                objectsInCollider--;
            }
        }
    }
}
