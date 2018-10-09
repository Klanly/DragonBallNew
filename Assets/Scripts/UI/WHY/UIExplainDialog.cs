using UnityEngine;
using System.Collections;

public class UIExplainDialog : MonoBehaviour
{

	public UILabel titleLabel;

	public UILabel contentLabel;

	public static UIExplainDialog getExplainDialog(Transform parent)
	{
		GameObject prb;

		prb = PrefabLoader.loadFromPack("WHY/pbUIExplainDialog") as GameObject;


		GameObject obj = Instantiate(prb) as GameObject;

		obj.transform.parent = parent;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;

		return obj.GetComponent<UIExplainDialog>();
	}

	void Start()
	{
		contentLabel.text = Core.Data.stringManager.getString (6121);
	}

	void OnColse()
	{
		Destroy(gameObject);
	}
}
