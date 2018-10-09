using UnityEngine;
using System.Collections;

public class BanAttributeConflict : MonoBehaviour {

	public UISprite sprite_Attack;

	public UISprite sprite_Defense;

    public UISprite sprite_KeZhi;

    public Vector3 shakeV3;

    public float shakeTime;

    public MiniItween.EasingType ShakeType;

    public string Split = "-----------------------------";

    public float EnlargeFactor = 1.5f;
    public float ScaleTime = 0.2f;
    public MiniItween.EasingType ScaleType;

    public float MoveDistance = 150f;
    public Vector3 OverDistance = new Vector3(30f, 0f, 0f);
    public GameObject Go_Thunder;

    //当前的状态
    private int state = 1;

	/// <summary>
	/// 返回值表示是否有属性克制, True代表确定有属性克制
	/// </summary>

    public bool Set(BanBattleRole.Attribute attackAttribute, BanBattleRole.Attribute defenseAttribute){
        Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Explosion);

		sprite_Attack.spriteName = BanTools.GetAttributeName(attackAttribute);
		sprite_Defense.spriteName = BanTools.GetAttributeName(defenseAttribute);

        sprite_Attack.MakePixelPerfect();
        sprite_Defense.MakePixelPerfect();

        transform.localScale = new Vector3(15f, 15f, 15f);

        state = BanTools.GetAttributeState(attackAttribute, defenseAttribute);
		if(state == 1){
            sprite_Attack.transform.localPosition = new Vector3(sprite_Attack.transform.localPosition.x - 10f, sprite_Attack.transform.localPosition.y, sprite_Attack.transform.localPosition.z);
            sprite_Attack.transform.localScale = new Vector3(EnlargeFactor, EnlargeFactor, EnlargeFactor);
            sprite_Defense.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        } else if(state == -1){
            sprite_Defense.transform.localPosition = new Vector3(sprite_Defense.transform.localPosition.x + 10f, sprite_Defense.transform.localPosition.y, sprite_Defense.transform.localPosition.z);
            sprite_Attack.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            sprite_Defense.transform.localScale = new Vector3(EnlargeFactor, EnlargeFactor, EnlargeFactor);
        } 

        MiniItween itween = MiniItween.ScaleTo(gameObject, new Vector3(1.0f, 1.0f, 1.0f), ScaleTime, ScaleType);
        itween.myDelegateFunc += shake;

		return state != 0;
	}

	void shake() {
		StartCoroutine(shakeEx());
	}

	IEnumerator shakeEx() {
        MiniItween.Shake(Camera.main.gameObject, shakeV3, shakeTime, ShakeType, false);
		yield return new WaitForSeconds(shakeTime);
		Back();
    }

    void Back() {

        if(state == 1){
            Vector3 localPos = sprite_Attack.transform.localPosition;
            MiniItween.MoveTo(sprite_Attack.gameObject, new Vector3(localPos.x - 10f , localPos.y, localPos.z), 0.08f).myDelegateFunc += Replace;
        } else if(state == -1) {
            Vector3 localPos = sprite_Defense.transform.localPosition;
            MiniItween.MoveTo(sprite_Defense.gameObject, new Vector3(localPos.x + 10f , localPos.y, localPos.z), 0.08f).myDelegateFunc += Replace;
        }
    }

    void Replace() {
        NGUITools.SetActive(sprite_KeZhi.gameObject, false);
        if(state == 1) {
            MiniItween.MoveTo(sprite_Attack.gameObject, Vector3.zero + OverDistance, 0.1f).myDelegateFunc += PlayThunder;
        } else if(state == -1) {
            MiniItween.MoveTo(sprite_Defense.gameObject, Vector3.zero - OverDistance, 0.1f).myDelegateFunc += PlayThunder;
        }
    }

    void PlayThunder() {

        if(state == 1) {
            MiniItween.MoveTo(sprite_Attack.gameObject, Vector3.zero, 0.1f);

            Vector3 localPos = sprite_Defense.transform.localPosition;
            Vector3 toPos = new Vector3(localPos.x + MoveDistance, localPos.y, localPos.z);
            MiniItween.MoveTo(sprite_Defense.gameObject, toPos, 0.1f, MiniItween.EasingType.EaseInQuad);
            MiniItween.ColorTo(sprite_Defense.gameObject, new V4( new Color(1.0f,1.0f,1.0f,0.0f)), 0.1f, MiniItween.Type.ColorWidget);
        } else if(state == -1) {
            MiniItween.MoveTo(sprite_Defense.gameObject, Vector3.zero, 0.1f);

            Vector3 localPos = sprite_Attack.transform.localPosition;
            Vector3 toPos = new Vector3(localPos.x - MoveDistance, localPos.y, localPos.z);
            MiniItween.MoveTo(sprite_Attack.gameObject, toPos, 0.1f, MiniItween.EasingType.EaseInQuad);
            MiniItween.ColorTo(sprite_Attack.gameObject, new V4( new Color(1.0f,1.0f,1.0f,0.0f)), 0.1f, MiniItween.Type.ColorWidget);
        }

        GameObject thunder = GameObject.Instantiate(Go_Thunder) as GameObject;
        thunder.transform.parent = gameObject.transform;
        thunder.transform.localPosition = Vector3.zero;

        Invoke("ZoomOut", 1.5f);
    }

    void ZoomOut() {
        if(state == 1) {
            MiniItween.ScaleTo(sprite_Attack.gameObject, new Vector3(3.0f, 3.0f, 3.0f), 0.2f );
            MiniItween.ColorTo(sprite_Attack.gameObject, new V4( new Color(1.0f,1.0f,1.0f,0.0f)), 0.2f, MiniItween.Type.ColorWidget);
        } else if(state == -1) {
            MiniItween.ScaleTo(sprite_Defense.gameObject, new Vector3(3.0f, 3.0f, 3.0f), 0.2f );
            MiniItween.ColorTo(sprite_Defense.gameObject, new V4( new Color(1.0f,1.0f,1.0f,0.0f)), 0.2f, MiniItween.Type.ColorWidget);
        }

    }



}
