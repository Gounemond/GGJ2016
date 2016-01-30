using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class MenuEasingEditor : MonoBehaviour 
{
    // Add menu named "Do Something" to the main menu
    [MenuItem ("StudioEvil/Easing/Create Path Helper")]
    static void createPathHelper () 
	{
    	GameObject go = new GameObject("NewPath");
		PathScript ps = go.AddComponent<PathScript>();
		ps.easingTime = 1;
		ps.delay = 0;
		ps.pathColor = Color.magenta;
		ps.path = new List<UnityEngine.Transform>();
		for( int i = 0; i < 4; i++ )
		{
			string name = "Control_"+i ;
			if( i==0 )
				name = "0-Start";
			if( i==3 )
				name = "z-End";
			GameObject placeHolder = new GameObject( name);
			placeHolder.AddComponent<PathNodeHelper>();
			placeHolder.transform.parent = go.transform;
			placeHolder.transform.localPosition = new Vector3( 10f*i, 0, 0 );
			ps.path.Add( placeHolder.transform );
		}
		
    }
	
	[MenuItem ("StudioEvil/Easing/Create Path Controller", true)]
    static bool ValidateSelectedPath () 
	{
        int selectedCount = Selection.gameObjects.Length;
		if( selectedCount == 0 )
			return true;
		
		int pathCount = 0;
		
		for( int i=0; i<selectedCount; i++ )
			if( Selection.gameObjects[i].GetComponent<PathScript>() != null ) pathCount++;
		
		return pathCount == selectedCount;
    }
	
	
	[MenuItem ("StudioEvil/Easing/Create Path Controller")]
    static void createControllerHelper () 
	{		
    	GameObject go = new GameObject("NewController");
		PathController pc = go.AddComponent<PathController>();
		pc.masterHandle = 0;
		
		int selectedCount = Selection.gameObjects.Length;
		if( selectedCount > 0 )
		{
			pc.paths = new PathScript[ selectedCount ];
			for( int i=0; i<selectedCount; i++ )
				pc.paths[i] = Selection.gameObjects[i].GetComponent<PathScript>();
		}
		
    }
	
	
}