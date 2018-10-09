using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//战斗Combo清算UI
public class LiquidateBattle : MonoBehaviour {

    public UISprite uiBg;
    public UISprite uiWord;

    //--- 背景动画
    public float PerAmount = 0.05f;
    public float PerTime = 0.01f;

    // --- 文字动画
    public Vector3 BeginScale;
    public float ScaleTime;
    public MiniItween.EasingType ScaleType;

    //--- 数字动画
    public Vector3 BeginNumScale;
    public float ScaleNumTime;
    public MiniItween.EasingType ScaleNumType;

    // --- shake
    public Vector3 ShakeVec3;
    public float ShakeTime = 0.1f;
    public MiniItween.EasingType ShakeType;

    public List<UISprite> UINumber1;
    public List<UISprite> UINumber2;
    public List<UISprite> UINumber3;

    //数字动画
    public GameObject goNum1;
    public GameObject goNum2;
    public GameObject goNum3;

    //播放Combo清算动画
    public IEnumerator showAnim(float delay) {
        init();

        yield return new WaitForSeconds(delay);
        int count = Core.Data.temper.MaxCombo;

        if(count > 0 && Core.Data.temper.SkipBattle == false) {
            List<int> separate = new List<int>();
            //多少位
            int howMany = MathHelper.howMany(count, separate);

            //开始显示背景动画
            yield return StartCoroutine(fillAmount());

            //开始显示文字
            uiWord.gameObject.SetActive(true);
            uiWord.transform.localScale = BeginScale;
            //缩放
            MiniItween.ScaleTo(uiWord.gameObject, Vector3.one, ScaleTime, ScaleType);
            yield return new WaitForSeconds(ScaleTime);

            //开始显示最大攻击数量

            List<UISprite> working = null;
            if(howMany == 1){
                working = UINumber1;

                goNum1.gameObject.SetActive(true);
                goNum2.gameObject.SetActive(false);
                goNum3.gameObject.SetActive(false);
            } else if(howMany == 2) {
                working = UINumber2;

                goNum1.gameObject.SetActive(false);
                goNum2.gameObject.SetActive(true);
                goNum3.gameObject.SetActive(false);
            } else if(howMany == 3) {
                working = UINumber3;

                goNum1.gameObject.SetActive(false);
                goNum2.gameObject.SetActive(false);
                goNum3.gameObject.SetActive(true);
            } else {
                ConsoleEx.DebugLog("Max Combo should not be larger then one thousand");
            }

            if(working != null) {
                int length = working.Count;
                UISprite num = null;
                for(int i = 0; i < length; ++ i) {
                    num = working[i];
                    num.spriteName = "hit_" + separate[i];
                    num.transform.localScale = BeginNumScale;
                    //缩放
                    MiniItween.ScaleTo(num.gameObject, Vector3.one, ScaleNumTime, ScaleNumType);
                }
                yield return new WaitForSeconds(ScaleNumTime);

                GameObject goShake = null;
                if(howMany == 1){
                    goShake = goNum1;
                } else if(howMany == 2) {
                    goShake = goNum2;
                } else if(howMany == 3) {
                    goShake = goNum3;
                }
                if(goShake != null)
                    MiniItween.Shake(goShake, ShakeVec3, ShakeTime, ShakeType, false);
            }
        }
     
    }

    public IEnumerator fillAmount() {
        while( uiBg.fillAmount <= 0.98f ) {
            uiBg.fillAmount += PerAmount;
            yield return new WaitForSeconds(PerTime);
        }
    }

    //初始化设置
    private void init() {
        goNum1.gameObject.SetActive(false);
        goNum2.gameObject.SetActive(false);
        goNum3.gameObject.SetActive(false);

        uiBg.fillAmount = 0;
        uiWord.gameObject.SetActive(false);
    }


//    void OnGUI() {
//        if(GUI.Button(new Rect(0,0,100,100), "show")) {
//            Core.Data.temper.MaxCombo = 179;
//            StartCoroutine(showAnim(0.3f));
//        }
//    }
}
