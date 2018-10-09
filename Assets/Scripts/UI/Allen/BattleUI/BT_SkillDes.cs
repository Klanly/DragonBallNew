using UnityEngine;
using System.Collections;

public class BT_SkillDes : MonoBehaviour {

	public UILabel skillTips;
	public UILabel skillName;
	public UISprite spBg;

	public static BT_SkillDes Instance;

	void Awake() {
		Instance = this;
		gameObject.SetActive(false);
	}

	public void showTips(GameObject Anchor, string title, string content, string makeUp = "") {
		if(content == null || title == null) return;

		skillName.text = title;
		gameObject.SetActive(true);
		skillTips.text = content + makeUp;

		RED.AddChild(gameObject, Anchor);
		gameObject.transform.localPosition = new Vector3( 93f, Anchor.transform.localPosition.y + 110f, 0f);

		Vector3 vec3 = skillTips.transform.localPosition;
		Vector3 vec3_t = skillName.transform.localPosition;

		if(Anchor.transform.position.x > 1f) {
			skillTips.transform.localScale = new Vector3(-1f, 1f, 1f);
			skillName.transform.localScale = new Vector3(-1f, 1f, 1f);

			vec3.x = Mathf.Abs(vec3.x);
			vec3_t.x = Mathf.Abs(vec3_t.x);

			skillTips.transform.localPosition = vec3;
			skillName.transform.localPosition = vec3_t;
		} else {
			skillTips.transform.localScale = Vector3.one;
			skillName.transform.localScale = Vector3.one;

			vec3.x = -Mathf.Abs(vec3.x);
			skillTips.transform.localPosition = vec3;

			vec3_t.x = -Mathf.Abs(vec3_t.x);
			skillName.transform.localPosition = vec3_t;
		}
	}

	public void HideTips() {
		gameObject.SetActive(false);
	}

}
