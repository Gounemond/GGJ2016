using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathScript))]

public class CustomEditor_PathScript : Editor 
{	
	
	public override void OnInspectorGUI() 
	{		
		serializedObject.Update ();
		PathScript me = (PathScript)serializedObject.targetObject;
		SerializedProperty property = serializedObject.GetIterator();
		while( property.NextVisible( true ) )
		{
			if( property.propertyPath.StartsWith( "path.Array.data" ) )
			{
				Transform actualNode = (Transform)property.objectReferenceValue;
				int actualIndex = System.Convert.ToInt32( property.propertyPath.Replace( "path.Array.data[", "" ).Replace( "]","" ) );
				// in- Array
				EditorGUILayout.BeginHorizontal( );
				if( GUILayout.Button( "<" ) )
				{
					GameObject newNode = me.cloneNode( actualNode );
					me.path.Insert( actualIndex, newNode.transform );
				}
				property.objectReferenceValue = EditorGUILayout.ObjectField( property.objectReferenceValue, typeof(Transform),true );
				
				if( GUILayout.Button( "x" ) )
				{					
					me.path.RemoveAt( actualIndex );
					DestroyImmediate( actualNode.gameObject );
				}
				if( GUILayout.Button( ">" ) )
				{					
					GameObject newNode = me.cloneNode( actualNode );
					me.path.Insert( actualIndex+1, newNode.transform );
				}
				EditorGUILayout.EndHorizontal( );
			}
			else
        		EditorGUILayout.PropertyField ( property );
		} ;

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties ();
	}	
}