using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PathController : MonoBehaviour 
{	

	public PathScript[] paths;
	
	public float masterHandle;
	
	// Update is called once per frame
	void Update () 
	{
		if( Application.isPlaying )
			return;
		
		clampHandle();
		
		if( paths != null)
			foreach( PathScript path in paths )
				if( path != null )
					path.handle = masterHandle;
		
	}
	
	void clampHandle()
	{
		float maxHandle=0;
		if( paths != null)
			foreach( PathScript path in paths )
				if( path != null )
					maxHandle = Mathf.Max( maxHandle, path.easingTime+path.delay);
		
		masterHandle = Mathf.Clamp( masterHandle, 0, maxHandle );
	}
}
