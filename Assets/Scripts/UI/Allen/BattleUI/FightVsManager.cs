using UnityEngine;
using System;
using System.Collections;


public class FightVsManager : MonoBehaviour {

    public UISpriteAnimation bgAnim;

    public UISprite LeftRole;
    public UISprite RightRole;

    public UISprite LeftWord;
    public UISprite RightWord;

    //快速登场的时间
    public float QuickAttendTime = 0.2f;
    //慢速登场的时间
    public float AttendTime = 0.8f;
    //中间的等待时间
    public float WaitTime = 0.03f;
    //冲刺时间
    public float RushTime = 0.1f;

    //撞击的偏移量
    public float RockOffset = 50f;

    //快进的偏移量
    public float QuickOffset = 75f;

    //人物离开的时间
    public float QuitTimeRole = 0.15f;
    //文字离开的时间
    public float QuitTimeWord = 0.1f;
    //维持时间
    public float HodingTime = 0.5f;

    public float DisappearTime = 0.4f;

    //登场的动画曲线
    public MiniItween.EasingType attendType;

    //淡入的颜色
    public Color FadeOut;
    public Color full;

    //震动幅度
    public Vector3 shakeVec;
    public float shakeTime;

    //左右文字在的位置
    public Vector3 LeftWStartPos;
    public Vector3 RightwStartPos;

    public Vector3 LeftWEndPos;
    public Vector3 RightWEndPos;

    // -- 闪光 --
    public ParticleSystem pslight;

    //---------  const ------------
    private float hideDis = 180f;

    private const string left_attend = "battle-1048";
    private const string left_fight = "battle-1046";

    private const string right_attend = "battle-1049";
    private const string right_fight = "battle-1047";

    //-------- 记录初始的值 ----------
    private float LeftX;
    private float RightX;

    // ------ 打斗两个人物的长高值 -----
    private float LeftRoleWidth_1 = 328f;
    private float RightRoleWidth_1 = 256f;

    private float LeftRoleWidth_2 = 416f;
    private float RightRoleWidth_2 = 392f;


    void init() {
        LeftX = LeftRole.transform.parent.localPosition.x;
        RightX = RightRole.transform.parent.localPosition.x;

        LeftRole.transform.localPosition = new Vector3(-hideDis, 0f, 0f);
        RightRole.transform.localPosition = new Vector3(hideDis, 0f, 0f);
        LeftWord.gameObject.SetActive(false);
        RightWord.gameObject.SetActive(false);
    }

    public void Forward() {
        gameObject.SetActive(true);
        init();

        LeftRole.transform.localPosition = new Vector3(-hideDis, 0f, 0f);
        RightRole.transform.localPosition = new Vector3(hideDis, 0f, 0f);

        LeftWord.gameObject.SetActive(false);
        RightWord.gameObject.SetActive(false);

        bgAnim.AnimationEndDelegate = (t) => { StartCoroutine(RoleAttend(t)); };
        bgAnim.Forward();
    }

    //人物登场
    IEnumerator RoleAttend(UISpriteAnimation an) {
        LeftRole.spriteName = left_attend;
        RightRole.spriteName = right_attend;

        LeftRole.width = (int)LeftRoleWidth_1;
        RightRole.width = (int)RightRoleWidth_1;

        MiniItween.MoveTo(LeftRole.gameObject, new Vector3(hideDis - QuickOffset, 0f, 0f), QuickAttendTime);
        MiniItween.MoveTo(RightRole.gameObject, new Vector3(-hideDis + QuickOffset, 0f, 0f), QuickAttendTime);

        yield return new WaitForSeconds(QuickAttendTime);

        MiniItween.MoveTo(LeftRole.gameObject, new Vector3(hideDis, 0f, 0f), AttendTime, attendType);
        MiniItween.MoveTo(RightRole.gameObject, new Vector3(-hideDis, 0f, 0f), AttendTime, attendType).myDelegateFunc += delegate() {
            StartCoroutine(WaitToRush());
        };
    }

    //冲刺
    IEnumerator WaitToRush() {
        yield return new WaitForSeconds(WaitTime);

        //播放声效
        Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Else3);

        LeftRole.spriteName = left_fight;
        RightRole.spriteName = right_fight;
        LeftRole.width = (int)LeftRoleWidth_2;
        RightRole.width = (int)RightRoleWidth_2;

        float LefttargetX = -LeftX - LeftRoleWidth_2 * 0.5f + RockOffset;
        float RighttargetX = -RightX + RightRoleWidth_2 * 0.5f - RockOffset;
        MiniItween.MoveTo(LeftRole.gameObject, new Vector3(LefttargetX, 0f, 0f), RushTime);
        MiniItween.MoveTo(RightRole.gameObject, new Vector3(RighttargetX, 0f, 0f), RushTime).myDelegateFunc += delegate() {
            ShakeAndLight();
        };

        //出现文字
        AppearWord();
    }

    //出现文字
    void AppearWord() {
        LeftWord.gameObject.SetActive(true);
        RightWord.gameObject.SetActive(true);

        LeftWord.transform.localPosition = LeftWStartPos;
        RightWord.transform.localPosition = RightwStartPos;

        LeftWord.color = FadeOut;
        RightWord.color = FadeOut;

        //淡入
        MiniItween.ColorTo(LeftWord.gameObject, new V4(full), RushTime, MiniItween.Type.ColorWidget);
        MiniItween.ColorTo(RightWord.gameObject, new V4(full), RushTime, MiniItween.Type.ColorWidget);
        //移动
        MiniItween.MoveTo(LeftWord.gameObject, LeftWEndPos, RushTime);
        MiniItween.MoveTo(RightWord.gameObject, RightWEndPos, RushTime);
    }

    void ShakeAndLight() {
        pslight.Play();
        MiniItween.Shake(gameObject, shakeVec, shakeTime, MiniItween.EasingType.Bounce).myDelegateFunc += delegate() {
            StartCoroutine(Quit());
        };
    }

    //文字消失
    IEnumerator DisappearWord() {
        //淡出
        MiniItween.ColorTo(LeftWord.gameObject, new V4(FadeOut), QuitTimeWord, MiniItween.Type.ColorWidget);
        MiniItween.ColorTo(RightWord.gameObject, new V4(FadeOut), QuitTimeWord, MiniItween.Type.ColorWidget);
        //移动
        MiniItween.MoveTo(LeftWord.gameObject, LeftWStartPos, QuitTimeWord);
        MiniItween.MoveTo(RightWord.gameObject, RightwStartPos, QuitTimeWord);

        yield return new WaitForSeconds(QuitTimeWord);

        LeftWord.gameObject.SetActive(false);
        RightWord.gameObject.SetActive(false);
    }

    //人物消失
    IEnumerator DisappearRole() {
        MiniItween.MoveTo(LeftRole.gameObject, new Vector3(-hideDis, 0f, 0f), QuitTimeRole);
        MiniItween.MoveTo(RightRole.gameObject, new Vector3(hideDis, 0f, 0f), QuitTimeRole);

        yield return new WaitForSeconds(QuitTimeRole);
        bgAnim.AnimationEndDelegate = null;
        bgAnim.Reverse();
    }

    IEnumerator Quit() {
        yield return new WaitForSeconds(HodingTime);

        StartCoroutine(DisappearWord());
        StartCoroutine(DisappearRole());

        yield return new WaitForSeconds(DisappearTime);
        gameObject.SetActive(false);
    }

//    void OnGUI() {
//        if(GUI.Button(new Rect(0,0,100,100), "Forward")) {
//            Forward();
//        }
//    }
}
