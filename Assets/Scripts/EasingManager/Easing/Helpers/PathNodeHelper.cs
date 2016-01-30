using UnityEngine;
using System.Collections;

public class PathNodeHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void OnDrawGizmos( )
	{
		Gizmos.DrawIcon ( transform.position, "KeyPoint.png", false);
	}
}

