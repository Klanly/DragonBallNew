using UnityEngine;
using System.Collections;

public class DescribeMessageBox : MonoBehaviour {

	public UILabel Lab_Title;
	public UILabel Lab_Des;
	public static void  Open(string title,string des)
	{
			Object  prefab = PrefabLoader.loadFromPack("JC/DescribeMessageBox");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
						RED.TweenShowDialog (obj);

				DescribeMessageBox _mInstance = obj.GetComponent<DescribeMessageBox>();
				_mInstance.Lab_Title.text = title;
			    BoxCollider collider = _mInstance.Lab_Title.gameObject.GetComponent<BoxCollider>();
			    if(collider != null)
				{
					collider.size = new Vector3( _mInstance.Lab_Des.width ,_mInstance.Lab_Des.height ,0);
				    collider.center = new Vector3(collider.size.x/2,-collider.size.y/2,1f);
				}
				_mInstance.Lab_Des.text = des;
			}
	}
	
	void Close()
	{
		Destroy(this.gameObject);
	}
}
