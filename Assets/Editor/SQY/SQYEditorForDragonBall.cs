using UnityEngine;
using System.Collections;
using UnityEditor;
public class SQYEditorForDragonBall{

	[MenuItem("GameObject/Create Select Empty &n")]
	static void CreatEmptyGOWithSelect()
	{
		GameObject go = new GameObject("GameObject");
		if(Selection.transforms.Length>0)
		{
			go.transform.parent = Selection.transforms[0];
			go.layer = Selection.transforms[0].gameObject.layer;
		}
		
		go.transform.localPosition = new Vector3(0,0,0);
		go.transform.localScale = Vector3.one;
		Selection.activeObject = go;
	}
}
