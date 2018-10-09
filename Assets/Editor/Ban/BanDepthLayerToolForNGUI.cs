using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BanDepthLayerToolForNGUI : EditorWindow {

	static private BanDepthLayerToolForNGUI SingleWindow;

	private List<UIWidget> list_Sprite = new List<UIWidget>();

	private int targetDepth = 0;

	[MenuItem("NGUI/DepthLayerTool #&d")]
	static void ShowWindow(){
		if(SingleWindow == null){
			SingleWindow = (BanDepthLayerToolForNGUI)ScriptableObject.CreateInstance(typeof(BanDepthLayerToolForNGUI));
			SingleWindow.title = "BanDepthLayerToolForNGUI";
			SingleWindow.minSize = new Vector2(300, 420);
			SingleWindow.maxSize = new Vector2(300, 420);
			SingleWindow.position = new Rect(200, 200, 300, 420);
		}
		SingleWindow.ReadUISprite();
		SingleWindow.ShowUtility();
		SingleWindow.Focus();

	}

	void OnGUI (){
		GUILayout.BeginArea(new Rect(0, 0, this.position.width - 8, this.position.height));
		GUILayout.Space(4);

		for(int i = 0;i<list_Sprite.Count;i++){
			GUILayout.BeginHorizontal();
			GUILayout.Label("Index:"+i);
			GUILayout.Label(""+list_Sprite[i].name);
			GUILayout.Label("Depth:"+list_Sprite[i].depth);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.EndHorizontal();
		}
		if(list_Sprite.Count > 0){
			targetDepth = EditorGUILayout.IntField(targetDepth);
			if(GUILayout.Button("SetAll")){
				foreach(UIWidget aUISprite in list_Sprite){
					aUISprite.depth = targetDepth;
				}
			}
		}
		GUILayout.EndArea();
	}

	void OnDisable(){
		SingleWindow = null;
	}

	void OnFocus() {
		ReadUISprite();
	}

	void ReadUISprite(){
		list_Sprite.Clear();
		foreach(GameObject aGameObject in Selection.gameObjects){
			UIWidget uiSprite = aGameObject.GetComponent<UIWidget>();
			if(uiSprite != null){
				list_Sprite.Add(uiSprite);
			}
		}
	}

}
