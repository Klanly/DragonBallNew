using UnityEngine;
using System.Collections;
using UnityEditor;

public class DisconnPrefab : MonoBehaviour {

	[MenuItem("Prefab Tools/Disconnect %1")]
	static void Disconnect() {
		foreach(Transform trans in Selection.transforms) {
			PrefabUtility.DisconnectPrefabInstance(trans);
		}

	}

}
