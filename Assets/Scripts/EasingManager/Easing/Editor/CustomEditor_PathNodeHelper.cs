using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathNodeHelper))]

public class CustomEditor_PathNodeHelper : Editor 
{	
	public void OnSceneGUI( )
	{
		PathNodeHelper pnh=(PathNodeHelper)target;
		
		Vector2 pos = HandleUtility.WorldToGUIPoint( pnh.transform.position );
		PathScript path = pnh.transform.parent.parent.gameObject.GetComponent<PathScript>();
		Handles.BeginGUI( );
		Rect before = new Rect( pos.x-41, pos.y-17, 30, 30 );
		Rect after 	= new Rect( pos.x+13, pos.y-17, 30, 30 );
		Rect delete	= new Rect( after.x+after.width, pos.y-17, 30, 30 );
		Rect dist	= new Rect( after.x+after.width, pos.y-17-30, 30, 30 );
		if( GUI.Button( before,"<" ) )
		{
			Selection.activeGameObject=path.addBefore( Selection.activeTransform );
		}
		if( GUI.Button( after, ">" ) )
		{
			Selection.activeGameObject=path.addAfter( Selection.activeTransform );
		}
		if( GUI.Button( delete, "x" ) )
		{
			path.removeNode( Selection.activeTransform );
		}
		if( GUI.Button( dist, ":" ) )
		{
			path.DistributeNodes( );
		}
		Handles.EndGUI();
	}
}
