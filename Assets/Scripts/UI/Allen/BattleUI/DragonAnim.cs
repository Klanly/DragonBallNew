using UnityEngine;
using System.Collections;

public class DragonAnim : MonoBehaviour {

    public GameObject go_Left;
    public GameObject go_Right;

    // -------------  black panel ---------------

    //for split
    public string m = "------- black panel -------- ";
    public UISprite blackPanelTop;
    public UISprite blackPanelBottom;

    public Vector3 FromToptoBottom;
    public Vector3 FromBottomToTop;

    public Vector3 SrcTop;
    public Vector3 SrcBottom;

    public float blackTime = 0.1f;

    public string mm = "------- Light panel -------- ";

    public UISprite LightLeft;
    public UISprite LightRight;

    public float LightTime = 0.13f;

    private const int Light_Width = 100;
    private const int Light_Height = 70;

    private const int Target_Width = 700;
    private const int Target_Height = 250;

    public string mmm = "-------------  Dragon Appear Panel -----------------";

    public GameObject Go_DragonLeft;
    public GameObject Go_DragonRight;

    public UISprite DragonLeft;
    public UISprite DragonLeftShadow;
    public UISprite Dragon_EyeLeft;
    public UISprite[] DianLeft;


    public UISprite DragonRight;
    public UISprite DragonRightShadow;
    public UISprite Dragon_EyeRight;
    public GameObject DianRight;


    private const int Dragon_Left_Eye_Src = 138;
    private const int Dragon_Left_Eye_Tar = 225;

    private const int Dragon_Eye_Max_Height = 68;
    private const int Dragon_Eye_Min_Height = 18;

    private Vector3 Pos_DragonL;
    private Vector3[] Pos_DianL;

    private Vector3 Pos_DragonR;
    private Vector3 Pos_DianR;

    public float DragonEyeTime = 0.08f;
    public float DragonEyeTwink = 0.2f;

    public float DragonTime = 2f;
    public Vector3 DragonToLeft;

    public Vector3 DragonToRight;

    public EffectManager skillNameEffectLeft;
    public EffectManager skillNameEffectRight;


    // --------------------------- Dragon AoYi Skill Icon --------------------
    public DragonSkillManager dragonMgrLeft;
    public DragonSkillManager dragonMgrRight;


    //左边还是右边
    private bool mLeftOrRigth;
    private int dragonSkillPos;
    //神龙技能
    public string DragonName;

    //声效
    public SoundFx Sound;

    //礼花
	private bool defaultFirework = true;   // 默认的礼花
	private GameObject Go_DefaultFirework; 
    private GameObject Go_Firework1;
    private GameObject Go_Firework2;

    public void ShowFirework(){
		//默认的礼花
		if(defaultFirework) {

			Object obj = null;
			if(Go_Firework1 == null || Go_Firework2 == null) {
				obj = PrefabLoader.loadFromUnPack("Allen/RibbonColor", false, false);
				Go_DefaultFirework = Instantiate(obj, new Vector3(0f, 4.15f, 0f), Quaternion.identity) as GameObject;
			}

			obj = null;
		} else {
			//光影的礼花
			Object obj = null;
			if(Go_Firework1 == null || Go_Firework2 == null) {
				obj = PrefabLoader.loadFromUnPack("Allen/FXFireworkShowLooper", false, false);
			}

			if(Go_Firework1 == null) {
				Go_Firework1 = Instantiate(obj) as GameObject;
				RED.AddChild(Go_Firework1, gameObject);
				Go_Firework1.transform.localPosition += new Vector3(-447f, 44f, 0f);
			} 

			if(Go_Firework2 == null) {
				Go_Firework2 = Instantiate(obj) as GameObject;
				RED.AddChild(Go_Firework2, gameObject);
				Go_Firework2.transform.localPosition += new Vector3(407f, 44f, 0f);
			}

			obj = null;
		}
    }

    public void DestoryFireWork() {
		//默认的礼花
		if(defaultFirework) {
			if(Go_DefaultFirework != null) {
				Destroy(Go_DefaultFirework);
				Go_DefaultFirework = null;
			}
		} else {
			if(Go_Firework1 != null) {
				Destroy(Go_Firework1);
				Go_Firework1 = null;
			}

			if(Go_Firework2 != null) {
				Destroy(Go_Firework2);
				Go_Firework2 = null;
			}
		}
    }



    public bool isPlaying = false;

    public void PlayDragon(bool LeftOrRight, int pos) {
        isPlaying = true;

        BackUpDragon();

        if(Core.Data != null)
            Core.Data.soundManager.SoundFxPlay(Sound);

        mLeftOrRigth = LeftOrRight;
        dragonSkillPos = pos;

        blackPanelTop.gameObject.SetActive(true);
        blackPanelBottom.gameObject.SetActive(true);
        blackPanelTop.transform.localPosition = SrcTop;
        blackPanelBottom.transform.localPosition = SrcBottom;

        if(mLeftOrRigth) {
            go_Left.SetActive(true);
            go_Right.SetActive(false);
        } else {
            go_Right.SetActive(true);
            go_Left.SetActive(false);
        }

        StartCoroutine(playBlackAnim());
    }


    IEnumerator playBlackAnim() {
        MiniItween.MoveTo(blackPanelTop.gameObject, FromToptoBottom, blackTime);
        MiniItween.MoveTo(blackPanelBottom.gameObject, FromBottomToTop, blackTime);

        yield return new WaitForSeconds(blackTime);
        StartCoroutine(playLight());

        if(mLeftOrRigth)
            dragonMgrLeft.showSkill(dragonSkillPos);
        else 
            dragonMgrRight.showSkill(dragonSkillPos);
    }

    IEnumerator playLight() {
      
        UISprite sprite = mLeftOrRigth ? LightLeft : LightRight;

        sprite.width = Light_Width;
        sprite.height = Light_Height;

        sprite.gameObject.SetActive(true);

        TweenWidth.Begin(sprite, LightTime, Target_Width);
        yield return new WaitForSeconds(LightTime);
        TweenHeight.Begin(sprite, LightTime, Target_Height);
        yield return new WaitForSeconds(LightTime);

        sprite.gameObject.SetActive(false);

        StartCoroutine(DragonAppear());
    }


    //记录神龙动画的各种初始状态
    void BackUpDragon() {
        if(Pos_DianL == null || Pos_DianL.Length == 0) {
            Pos_DragonL = DragonLeft.transform.localPosition;
            int length = DianLeft.Length;
            Pos_DianL = new Vector3[length];
            for(int i = 0; i < length; ++ i) {
                Pos_DianL [i] = DianLeft[i].transform.localPosition;
            }
        }

        if(Pos_DianR == Vector3.zero) {
            Pos_DragonR = DragonRight.transform.localPosition;
            Pos_DianR = DianRight.transform.localPosition;
        }

    }

    //回复初始状态
    void DragonReset() {

        if(mLeftOrRigth) {
            DragonLeft.transform.localPosition = Pos_DragonL;
            int length = DianLeft.Length;
            for(int i = 0; i < length; ++ i) {
                DianLeft[i].transform.localPosition = Pos_DianL [i];
            }
        } else {
            DragonRight.transform.localPosition = Pos_DragonR;
            DianRight.transform.localPosition = Pos_DianR;
        }

    }

    IEnumerator DragonAppear() {

        if(mLeftOrRigth) {
            Go_DragonLeft.SetActive(true);
            DragonReset();

            skillNameEffectLeft.Text = DragonName;
            skillNameEffectLeft.PlayAnimation();

            DragonLeftShadow.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            DragonLeftShadow.gameObject.SetActive(false);

            TweenPosition.Begin(DragonLeft.gameObject, DragonTime, DragonToLeft);

            StartCoroutine(DragonEyeTwinke());

            foreach(UISprite sp in DianLeft) {
                TweenPosition.Begin(sp.gameObject, DragonTime , new Vector3(sp.transform.localPosition.x-800f, sp.transform.localPosition.y, sp.transform.localPosition.z));
            }

            yield return new WaitForSeconds(DragonTime);

            Go_DragonLeft.SetActive(false);
        } else {

            Go_DragonRight.SetActive(true);
            DragonReset();

            skillNameEffectRight.Text = DragonName;
            skillNameEffectRight.PlayAnimation();

            DragonRightShadow.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            DragonRightShadow.gameObject.SetActive(false);

            TweenPosition.Begin(DragonRight.gameObject, DragonTime, DragonToRight);

            StartCoroutine(DragonEyeTwinke());

            MiniItween.MoveTo(DianRight.gameObject, new Vector3(DianRight.transform.localPosition.x+800f, DianRight.transform.localPosition.y, DianRight.transform.localPosition.z), DragonTime );

            yield return new WaitForSeconds(DragonTime);

            Go_DragonRight.SetActive(false);
        }

        blackPanelTop.gameObject.SetActive(false);
        blackPanelBottom.gameObject.SetActive(false);

        if(mLeftOrRigth) dragonMgrLeft.resetSkill(dragonSkillPos);
        else dragonMgrRight.resetSkill(dragonSkillPos);

        isPlaying = false;
    }

    IEnumerator DragonEyeTwinke() {
        if(mLeftOrRigth) {
            TweenWidth.Begin(Dragon_EyeLeft, DragonEyeTime, Dragon_Left_Eye_Tar * 3);
            TweenHeight.Begin(Dragon_EyeLeft, DragonEyeTime, Dragon_Eye_Min_Height);
            yield return new WaitForSeconds(DragonEyeTime);
            TweenWidth.Begin(Dragon_EyeLeft, DragonEyeTime, Dragon_Left_Eye_Src);
            TweenHeight.Begin(Dragon_EyeLeft, DragonEyeTime, Dragon_Eye_Max_Height);

            yield return new WaitForSeconds(0.5f);

            for(int i = 0; i < 2; ++ i) {
                TweenWidth.Begin(Dragon_EyeLeft, DragonEyeTwink, (int)(Dragon_Left_Eye_Src * 0.5f));
                yield return new WaitForSeconds(DragonEyeTwink);
                TweenWidth.Begin(Dragon_EyeLeft, DragonEyeTwink, Dragon_Left_Eye_Src);
                yield return new WaitForSeconds(DragonEyeTwink);
            }

        } else {

            TweenWidth.Begin(Dragon_EyeRight, DragonEyeTime, Dragon_Left_Eye_Tar * 3);
            TweenHeight.Begin(Dragon_EyeRight, DragonEyeTime, Dragon_Eye_Min_Height);
            yield return new WaitForSeconds(DragonEyeTime);
            TweenWidth.Begin(Dragon_EyeRight, DragonEyeTime, Dragon_Left_Eye_Src);
            TweenHeight.Begin(Dragon_EyeRight, DragonEyeTime, Dragon_Eye_Max_Height);

            yield return new WaitForSeconds(0.5f);

            for(int i = 0; i < 3; ++ i) {
                TweenWidth.Begin(Dragon_EyeRight, DragonEyeTwink, (int)(Dragon_Left_Eye_Src * 0.5f));
                yield return new WaitForSeconds(DragonEyeTwink);
                TweenWidth.Begin(Dragon_EyeRight, DragonEyeTwink, Dragon_Left_Eye_Src);
                yield return new WaitForSeconds(DragonEyeTwink);
            }

        }
    }

}
