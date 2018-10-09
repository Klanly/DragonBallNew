using UnityEngine;
using System.Collections;

public class BanHpBar : MonoBehaviour {
	/// <summary>
	/// Hp的两个血条
	/// </summary>
	public UISprite SpriteHp;

	private float _value;

	public float value {
		set {
			if(value >= 0 && value <= 1) {
				SpriteHp.fillAmount  = value;
				_value = value;
			}
		} get {
			return _value;
		}
	}



}
