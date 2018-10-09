using UnityEngine;
using System.Collections;

/// <summary>
/// 主要用于控制怒气的改变动画
/// </summary>
public class AngryChgAnim : MonoBehaviour {

    public UILabel lblChgNum;
    public float StayTime = 1f;
    public float MoveTime = 0.2f;
    public float UpDis    = 1f;


    public static AngryChgAnim createOne(int changed, GameObject GoParent) {
        Object obj = null;
        obj = PrefabLoader.loadFromUnPack("Ban/AngryChg", false);
        GameObject go = Instantiate(obj) as GameObject;
        RED.AddChild(go, GoParent);

        AngryChgAnim aca = go.GetComponent<AngryChgAnim>();
        aca.showAnim(changed);
        return aca;
    }

    /// <summary>
    /// 开始展示动画
    /// </summary>
    public void showAnim(int changed) {
        string showText = changed >= 0 ? "+" + changed.ToString() : changed.ToString();
        lblChgNum.text = showText;

        StartCoroutine(UpAnim());
    }

    IEnumerator UpAnim() {
        yield return new WaitForSeconds(StayTime);
        UpAnimImmediate();
        yield return new WaitForSeconds(MoveTime);
        Destroy(gameObject);
    }

    public void UpAnimImmediate() {
        Vector3 target = transform.localPosition + new Vector3(0f, UpDis, 0f);
        MiniItween.MoveTo(gameObject, target, MoveTime, MiniItween.EasingType.EaseInCubic);
        MiniItween.ColorTo(lblChgNum.gameObject, new V4(new Color(1.0f, 1.0f, 1.0f, 0.0f)), MoveTime, MiniItween.Type.ColorWidget);
    }
}
