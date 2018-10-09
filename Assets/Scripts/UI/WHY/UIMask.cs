using UnityEngine;
using System.Collections;

public class UIMask : MonoBehaviour
{
	private static UIMask instance = null;

	static public UIMask Instance {
		get {
			if (instance == null) {
				instance = UnityEngine.Object.FindObjectOfType (typeof(UIMask)) as UIMask;

				if (instance == null) {
					GameObject prb = PrefabLoader.loadFromPack ("WHY/pbUIMask") as GameObject;
					GameObject prbObj = GameObject.Instantiate (prb) as GameObject;


					instance = prbObj.GetComponent<UIMask> ();
				}
			}
			return instance;
		}
	}

	public float maskW = 0;
	public float maskH = 0;



	public UISprite up;
	public UISprite down;
	public UISprite left;
	public UISprite right;



	void setSprite(UISprite sprite, string spriteName, Quaternion quaternion)
	{
		sprite.spriteName = spriteName;
		sprite.gameObject.transform.localRotation = quaternion;
	}


	void Start()
	{
//		float orthographicSize = Camera.main.orthographicSize;
//		float x = (float)Screen.width / (float)Screen.height * (float)orthographicSize;
//		float y = orthographicSize;
	}

	public void setRoot (GameObject root)
	{
		gameObject.transform.parent = root.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
	}

	public void show(Vector3 pos, int w, int h)
	{
		maskW = Screen.width;
		maskH = Screen.height;

		Vector3 screenPoint = Camera.main.WorldToScreenPoint(pos);

		this.up.height = (int)(maskH - screenPoint.y - h / 2.0f);

		this.down.height = (int)(maskH - this.up.height - h);

		this.right.width = (int)(maskW - screenPoint.x - w / 2.0f);

		this.left.width = (int)(maskW - this.right.width - w);

		this.down.width = (int)(maskW - this.left.width - this.right.width);
		this.up.width = this.down.width;

		this.up.transform.localPosition = new Vector3(this.left.transform.localPosition.x + this.left.width + this.up.width / 2.0f, this.up.transform.localPosition.y, this.up.transform.position.z);

		this.down.transform.localPosition = new Vector3(this.left.transform.localPosition.x + this.left.width + this.up.width / 2.0f, this.down.transform.localPosition.y, this.down.transform.position.z);

	}
}
