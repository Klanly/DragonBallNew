using UnityEngine;
using System.Collections;

/// <summary>
/// 新版本的动画
/// </summary>
public class DamageShow : MonoBehaviour {

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

    //UI一个数字占据多少个像素
    private const int MAX_LEN = 28;
    //起始位置
    private const int START_POS = 82;

	private Vector3[] RigthWordPos = new Vector3[] {
		new Vector3(99F, 0F, 0F), new Vector3(99F, 0F, 0F), new Vector3(99F, 0F, 0F), new Vector3(111F, 0F, 0F), new Vector3(131F, 0F, 0F), new Vector3(155F, 0F, 0F), new Vector3(176F, 0F, 0F)
	};

	private Vector3[] LeftWordPos = new Vector3[] {
		new Vector3(-14F, 0F, 0F), new Vector3(-14F, 0F, 0F), new Vector3(-30F, 0F, 0F), new Vector3(-56F, 0F, 0F), new Vector3(-56F, 0F, 0F), new Vector3(-98.5F, 0F, 0F), new Vector3(-98.5F, 0F, 0F),
	};

	private Vector3[] LeftNumPos = new Vector3[] {
		new Vector3(0F, 0F, 0F), new Vector3(0F, 0F, 0F), new Vector3(-30F, 0F, 0F), new Vector3(-56F, 0F, 0F), new Vector3(-56F, 0F, 0F), new Vector3(-98.5F, 0F, 0F), new Vector3(-98.5F, 0F, 0F)
	};


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
        ///位数
        /// 不足3位的补齐空格（目的是居中） 2位的补2个空格，1位的补齐4个空格
        int count = MathHelper.howMany(Mathf.Abs(change), null);
		int index = count <= 7 ? count - 1 : 6;
        //补齐空格
        string makeUp = string.Empty;
        if(IsLeft) { //左边的判定规则
			dmgNum.transform.localPosition = LeftNumPos[index];
			dmgWord.transform.localPosition = LeftWordPos[index];
        }  else { //右边的判定规则
			dmgWord.transform.localPosition = RigthWordPos[index];

			switch(count) {
			case 1: 
				makeUp = "      ";
				break;
			case 2: 
				makeUp = "    ";
				break;
			case 3:
				makeUp = "  ";
				break;
			case 4:
				makeUp = " ";
				break;
			case 5:
			case 6:
			case 7:
				break;
			}
        }

        string prefix = string.Empty;

        if(leftAttOrDef == 1) {
            prefix = Core.Data.stringManager.getString(26);
        } else {
            prefix = Core.Data.stringManager.getString(27);
        }

        if(change > 0) {
            prefix = prefix + "+";
        } else {
            prefix = prefix + "-";
        }

        Color txtColor = Color.green;
        if(change > 0) {
            txtColor = Color.green;
        } else {
            txtColor = Color.red;
        }

        dmgWord.color = txtColor;
        dmgNum.color  = txtColor;
        dmgWord.text  = prefix;
        dmgNum.text   = Mathf.Abs(change).ToString() + makeUp;

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

    }


}
