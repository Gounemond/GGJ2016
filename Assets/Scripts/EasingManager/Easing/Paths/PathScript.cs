using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PathScript : MonoBehaviour
{
	
	public Color pathColor;
	public float scale=1f;
	public float easingTime=1;
	public float delay;
	public float handle;
	public bool alignWithSpeed;
	public bool freeRotate;
	public Transform easingTarget;
	public List<Transform> path;
	public int smooth=10;
	public bool _____DEBUG_____;
	public float splineLenght;
	public float[] splineLengths;
	public bool round=false;

	private Material diffuse;
	
	// Use this for initialization
	public void Start () 
	{
	}
	
	//#if UNITY_EDITOR
	// Update is called once per frame
	void Update () 
	{
	}
	//#endif
	
	public Vector3[] getPositions()
	{
		int offset=0;
		Vector3[] positions;
		if(!round)
			positions=new Vector3[ path.Count ];
		else
		{
			offset=1;
			positions=new Vector3[ path.Count+3 ];
			positions[0]=path[path.Count-1].position;
			positions[positions.Length-2]=path[0].position;
			positions[positions.Length-1]=path[1].position;
		}
			
		
		for(int i=0; i<path.Count; i++)
		{
			if( path[i] != null )
				positions[i+offset] = path[i].position;
		}
		return positions;
	}	
	public Vector3[] getRotations()
	{
		int offset=0;
		Vector3[] roations;
		if(!round)
			roations=new Vector3[ path.Count ];
		else
		{
			offset=1;
			roations=new Vector3[ path.Count+2 ];
			roations[0]=path[path.Count-1].eulerAngles;
			roations[roations.Length-1]=path[0].eulerAngles;
		}
		
		
		for(int i=0; i<path.Count; i++)
		{
			if( path[i] != null )
				roations[i+offset] = path[i].eulerAngles;
		}
		return roations;
	}			
	
	void drawTratteggio(Vector3 start, Vector3 end, int countPerUnit, Color color)
	{
		int count=Mathf.CeilToInt((start-end).magnitude*countPerUnit*2);
		
		bool t=true;
		for(int c=0;c<count;c++)
		{
			float i=1f/(float)count*(float)c;
			float i2=1f/(float)count*(float)(c+1);
			
			if(t)
				Debug.DrawLine(Vector3.Lerp(start,end, i), Vector3.Lerp(start, end, i2), color);
			t=!t;
		}
	}
	
	public void drawAxis(Vector3 position, Quaternion rotation )
	{
		/*
		Vector3 Yaxis = rotation * (Vector3.up* scale) ;
		Vector3 Zaxis = rotation * (Vector3.forward* scale) ;
		Vector3 Xaxis = rotation * (Vector3.right* scale) ;
		Debug.DrawLine( position, position+Yaxis, Color.green );
		Debug.DrawLine( position, position+Zaxis, Color.blue );
		Debug.DrawLine( position, position+Xaxis, Color.red );
        */
	}
	
	public GameObject cloneNode( Transform node )
	{
		GameObject placeHolder = (GameObject)Instantiate(Resources.Load("Objects/SplineNode"),node.position,node.rotation); //(GameObject)Instantiate(node.gameObject,node.position,node.rotation);
		return placeHolder;
	}
	
	public void DistributeNodes()
	{
		int numNodes = path.Count;
		//if (numNodes<5) return;
		
		Vector3[] newpositions = new Vector3[numNodes];		
		Vector3[] positions = new Vector3[numNodes];		
		
		for(int i=0; i<positions.Length; i++)
		{
			if( path[i] != null )
				positions[i] = path[i].position;
		}
		
		CRSpline posSpline = new CRSpline( positions );		
			
		for (int a=1; a<positions.Length-1; a++)
		{
			float p = (1.0f / ((float)numNodes-3.0f)) * (a-1);
			//Debug.Log(p+","+getT( splineLenght*p ));
			newpositions[a] = posSpline.Interp(getT( splineLenght*p ));			
			
		}
		
		for (int a=1; a<positions.Length-1; a++)
			path[a].position = newpositions[a];
		
	}

	public GameObject addBefore(Transform target)
	{
		GameObject newNode = cloneNode( target );
		newNode.transform.parent=transform.FindChild("Nodes");
		int actualIndex = path.IndexOf( target );
		path.Insert( actualIndex, newNode.transform );
		return newNode;
	}
	public GameObject addAfter(Transform target)
	{
		GameObject newNode = cloneNode( target );
		newNode.transform.parent=transform.FindChild("Nodes");
		int actualIndex = path.IndexOf( target );
		path.Insert( actualIndex+1, newNode.transform );
		return newNode;
	}
	public void removeNode(Transform target)
	{
		int actualIndex = path.IndexOf( target );
		path.RemoveAt( actualIndex );
		DestroyImmediate( target.gameObject );
	}

	public float getT(float vertexCursor)
	{
		int numSection=0;
		while( numSection+1<=path.Count-3 && vertexCursor>cumulativeLen(numSection+1))
			numSection++;	
		
		float t=1.1f;
		if(numSection<path.Count-2)
			t=((1.0f/(path.Count-3))*numSection) +((vertexCursor-cumulativeLen(numSection))/splineLengths[numSection]*(1.0f/(path.Count-3)) );
		//float t=vertexCursor/path.splineLenght;
		return Mathf.Clamp01(t);
	} 
	float cumulativeLen(int section)
	{
		float ret=0;
		for(int i=0;i<section;i++)
			ret+=splineLengths[i];
		return ret;
	}
	
}
