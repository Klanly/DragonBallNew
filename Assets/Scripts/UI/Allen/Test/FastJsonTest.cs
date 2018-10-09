using System;
using UnityEngine;
using fastJSON;
using System.Collections.Generic;

using SObject = System.Object;
public class FastJsonTest : MonoBehaviour
{

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 250, 100), "Fast Json Serialize and Deserialize.")) {
			FastJson ();
		}
	}

	public void FastJson ()
	{
		Tdata instance = new Tdata();
		instance.m = @"2334";
		instance.dic = new Dictionary<string, List<float>>();
		List<float> ins = new List<float>();
		ins.Add( 3.5f );

		instance.dic.Add("123", ins);

		string json = JSON.Instance.ToJSON(instance);
		ConsoleEx.DebugLog(json);

		Tdata newone = JSON.Instance.ToObject<Tdata>(json);
		ConsoleEx.DebugLog("newone = " + JsonFx.Json.JsonWriter.Serialize(newone));
	}

}


public class Tdata {
	public string m = null;
	public Dictionary<string, List<float>> dic = null;
}

