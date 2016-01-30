using UnityEngine;
using UnityEditor;

public class PathToolbox: EditorWindow 
{

    [MenuItem ("AracnoTeam/Easing/Path Toolbox")]
    static void Init () 
	{
		PathToolbox win = EditorWindow.GetWindow<PathToolbox>();
		EditorApplication.hierarchyWindowItemOnGUI += win.hierarchyWindowItemOnGUI;
    } // Init()
	
	int selectedIndex;
    void OnGUI () 
	{

    }
	
	void OnDestroy( )
	{
		EditorApplication.hierarchyWindowItemOnGUI -= hierarchyWindowItemOnGUI;
	}
	
	public void hierarchyWindowItemOnGUI(int instanceID, Rect selectionRect ) 
	{
		GameObject obj =  (GameObject)EditorUtility.InstanceIDToObject( instanceID );
		PathNodeHelper node = obj.GetComponent<PathNodeHelper>();
		if( node != null )
		{
			PathScript path = node.transform.parent.gameObject.GetComponent<PathScript>();
			
			Rect before = 	new Rect( selectionRect.x - 36, selectionRect.y, 17, selectionRect.height );
			Rect after = 	new Rect( selectionRect.x - 19, selectionRect.y, 17, selectionRect.height );
			if( GUI.Button( before, "<" ) )
			{
				GameObject newNode = path.cloneNode( node.transform );
				int actualIndex = path.path.IndexOf( node.transform );
				Debug.Log( actualIndex );
				path.path.Insert( actualIndex, newNode.transform );
			}
			if( GUI.Button( after, ">" ) )
			{
				GameObject newNode = path.cloneNode( node.transform );
				int actualIndex = path.path.IndexOf( node.transform );
				Debug.Log( actualIndex );
				path.path.Insert( actualIndex+1, newNode.transform );
			}
			
		}
	}

}