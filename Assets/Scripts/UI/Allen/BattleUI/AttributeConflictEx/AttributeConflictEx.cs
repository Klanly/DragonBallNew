using UnityEngine;
using System.Collections;

public class AttributeConflictEx : MonoBehaviour {

	//左边属性
	public UISprite LeftAttri;
	//右边属性
	public UISprite RightAttri;
	//克制
	public UISprite KeZhi;
	//闪电
	public GameObject Ray;
	//靠近时间
	private float Closed = 0.22f;
	private float Apart = 0.08f;
	private float Thunder = 0.4f;
	private float Disappear = 0.2f;

	private float BigPoint = 72f;
	private float SmallPoint = 50f;
	private float ApartDis = 48f;

	private void init() {
		LeftAttri.transform.localPosition = new Vector3(-700f, 0f, 0f);
		LeftAttri.alpha = 0.0f;

		RightAttri.transform.localPosition = new Vector3(700f, 0f, 0f);
		RightAttri.alpha = 0.0f;

		KeZhi.transform.localPosition = Vector3.zero;
		KeZhi.transform.localScale = Vector3.zero;
		KeZhi.alpha = 0.0f;
	}

	public bool Set(BanBattleRole.Attribute attackAttribute, BanBattleRole.Attribute defenseAttribute) {
		int state = BanTools.GetAttributeState(attackAttribute, defenseAttribute);
		StartCoroutine(ShowKezhi(state, attackAttribute, defenseAttribute));
		return state != 0;
	}

	//Left = true，代表左边克制右边。否则，右边克制左边
	IEnumerator ShowKezhi(int state, BanBattleRole.Attribute attackAttribute, BanBattleRole.Attribute defenseAttribute) {
		LeftAttri.spriteName = BanTools.GetAttributeName(attackAttribute);
		RightAttri.spriteName = BanTools.GetAttributeName(defenseAttribute);

		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Explosion);
		bool Left = state == 1;

		init();

		LeftAttri.transform.localScale = Left ? 1.5f * Vector3.one : Vector3.one;
		RightAttri.transform.localScale = Left ? Vector3.one : 1.5f * Vector3.one;

		Vector3 leftPos = Vector3.zero;
		Vector3 rightPos = Vector3.zero;
		if(Left) {
			leftPos = new Vector3(-BigPoint, 0f, 0f);
			rightPos = new Vector3(SmallPoint, 0f, 0f);
		} else {
			leftPos = new Vector3(-SmallPoint, 0f, 0f);
			rightPos = new Vector3(BigPoint, 0f, 0f);
		}

		//start move
		MiniItween.MoveTo(LeftAttri.gameObject, leftPos, Closed);
		MiniItween.MoveTo(RightAttri.gameObject, rightPos, Closed);

		MiniItween.ColorTo(LeftAttri.gameObject, new V4(Color.white), Closed, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(RightAttri.gameObject, new V4(Color.white), Closed, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Closed);

		//分离
		MiniItween.MoveTo(LeftAttri.gameObject, new Vector3(leftPos.x - ApartDis, 0f, 0f), Apart);
		MiniItween.MoveTo(RightAttri.gameObject, new Vector3(rightPos.x + ApartDis, 0f, 0f), Apart);

		MiniItween.ScaleTo(KeZhi.gameObject, 1.1f * Vector3.one, Apart);
		MiniItween.ColorTo(KeZhi.gameObject, new V4(Color.white), Apart, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Apart);
		yield return new WaitForEndOfFrame();
		KeZhi.transform.localScale = Vector3.one;

		//闪电出现
		GameObject go = Instantiate(Ray) as GameObject;
		if(Left)
			RED.AddChild(go, LeftAttri.gameObject);
		else 
			RED.AddChild(go, RightAttri.gameObject);

		yield return new WaitForSeconds(Thunder);
		Destroy(go);

		//消失
		MiniItween.MoveTo(LeftAttri.gameObject, new Vector3(-700f, 0f, 0f), Disappear);
		MiniItween.MoveTo(RightAttri.gameObject, new Vector3(700f, 0f, 0f), Disappear);
		MiniItween.ScaleTo(KeZhi.gameObject, Vector3.zero, Disappear);

		MiniItween.ColorTo(LeftAttri.gameObject, new V4(new Color(1f, 1f, 1f, 0f)), Disappear, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(RightAttri.gameObject, new V4(new Color(1f, 1f, 1f, 0f)), Disappear, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(KeZhi.gameObject, new V4(new Color(1f, 1f, 1f, 0f)), Disappear, MiniItween.Type.ColorWidget);

		yield return new WaitForSeconds(Disappear);


	}

	/*void OnGUI() {
		if(GUI.Button(new Rect(0f, 0f, 100f, 100f), "TEST")) {
			StartCoroutine(ShowKezhi(BanBattleRole.Attribute.Huo, BanBattleRole.Attribute.Jin));
		}

		if(GUI.Button(new Rect(100f, 0f, 100f, 100f), "TEST")) {
			StartCoroutine(ShowKezhi(BanBattleRole.Attribute.SShui, BanBattleRole.Attribute.STu));
		}
	} */

}
