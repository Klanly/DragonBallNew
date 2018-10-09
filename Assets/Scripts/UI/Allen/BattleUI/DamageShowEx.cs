using UnityEngine;
using System.Collections;

/// <summary>
/// 新版本的动画 这是把血条放在两边的动画，而DamageShow则是在上面的动画（这个要考虑居中对齐）
/// </summary>
public class DamageShowEx : MonoBehaviour {

    //停止的时间
    public float StayTime = 1f;
    //对决停止的时间
    public float StayTimeEx = 2f;

    //向上移动的距离
    public float UpDis    = 100f;
    //向上移动的时间
    public float MoveTime = 0.4f;
    //打击的时间
    public float HitTime  = 0.1f;
    //放大的倍数
    public float HitScale = 3f;
    //对决停止的放大的倍数
    public float HitScaleEx = 3f;

    public Vector3 ShakeVec3 = new Vector3(3f, 3f, 3f);
    public float ShakeTime   = 0.1f;
    //对决震动的时间
    public float ShakeTimeEx = 0.1f;

    public float VS_InitScale = 1.2f;

    public UILabel dmgWord;
    public UILabel dmgNum;
	public UISprite dmgIcon;

    /// <summary>
    /// Sets the damage.
    /// </summary>
    /// <param name="leftAttOrDef">左边是攻击还是防守</param>
    /// <param name="IsLeft">If set to <c>true</c> 是左边吗？</param>
    /// <param name="change">HP变化量.</param>
    /// <param name="fparent">父节点.</param>
    /// <param name="VsFight">是否是对决.</param>
    public void setDamage(int leftAttOrDef, bool IsLeft, int change, Transform fparent, bool VsFight) {

        /// ---- 设定父子关系 ------
        /// ---- 设定位置关系 ------
        RED.AddChild(gameObject, fparent.gameObject);

        string prefix = string.Empty;
        if(change > 0) {
            prefix = "+";
        } else {
            prefix = "-";
        }

        Color txtColor = Color.green;
        if(change > 0) {
            txtColor = Color.green;
        } else {
            txtColor = Color.red;
        }

		if(leftAttOrDef == 1) {//攻击
			dmgIcon.spriteName = "BattleUI_Attack";
		} else {//防御
			dmgIcon.spriteName = "BattleUI_Defense";
		}

        dmgWord.color = txtColor;
        dmgNum.color  = txtColor;
        dmgWord.text  = prefix;
        dmgNum.text   = Mathf.Abs(change).ToString();

        StartCoroutine(HitAnim(VsFight));
        StartCoroutine(UpAnim(VsFight));
    }


    /// <summary>
    /// 撞击动画
    /// </summary>
    /// <returns>The animation.</returns>
    IEnumerator HitAnim(bool vs) {
        float scale = vs ? HitScaleEx : HitScale;
        gameObject.transform.localScale = Vector3.one * scale;

        Vector3 normal = vs ? VS_InitScale * Vector3.one : Vector3.one;
        MiniItween.ScaleTo(gameObject, normal, HitTime, MiniItween.EasingType.EaseInExpo);
        yield return new WaitForSeconds(HitTime);

        MiniItween.Shake(gameObject, ShakeVec3, ShakeTime, MiniItween.EasingType.EaseOutCubic);
    }

    /// <summary>
    /// 向上浮动的动画
    /// </summary>
    /// <returns>The animation.</returns>
    IEnumerator UpAnim(bool Vs) {
        float stay = Vs ? StayTimeEx : StayTime;
        yield return new WaitForSeconds(stay);
        UpImmediateAnim();
        yield return new WaitForSeconds(MoveTime);

        Destroy(gameObject);
    }

    public void UpImmediateAnim() {
        Vector3 target = transform.localPosition + new Vector3(0f, UpDis, 0f);
        MiniItween.MoveTo(gameObject, target, MoveTime, MiniItween.EasingType.EaseInCubic);

        MiniItween.ColorTo(dmgWord.gameObject, new V4(new Color(1.0f, 1.0f, 1.0f, 0.0f)), MoveTime, MiniItween.Type.ColorWidget);
        MiniItween.ColorTo( dmgNum.gameObject, new V4(new Color(1.0f, 1.0f, 1.0f, 0.0f)), MoveTime, MiniItween.Type.ColorWidget);
		MiniItween.ColorTo(dmgIcon.gameObject, new V4(new Color(1.0f, 1.0f, 1.0f, 0.0f)), MoveTime, MiniItween.Type.ColorWidget);
    }

}
