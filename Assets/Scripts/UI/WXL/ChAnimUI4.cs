using UnityEngine;
using System.Collections;
using System;

public class ChAnimUI4 : RUIMonoBehaviour {

    private static ChAnimUI4 instance;
    public static ChAnimUI4 Instance
    {
        get
        {
            return instance;
        }
    }
    public Action OnFinished;
    public UISprite[] spAni;



	public static ChAnimUI4 OpenUI(Transform tfParent){
        if (instance == null)
        {
            UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIAnimationFour);
            if (obj != null)
            {
                GameObject go = Instantiate(obj) as GameObject;
                instance = go.GetComponent<ChAnimUI4>();
                go.transform.parent = tfParent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }
        }
        else
        {
            instance.transform.parent = tfParent;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;
        }
		return instance;
	}

    void Awake(){
        instance = this;
    }

    void Start(){
        if (spAni.Length != 0 && spAni != null)
        {
            this.InitPic();
            this.StepOne();
            Invoke("StepTwo",0.3f);
            Invoke("StepThree",1.0f);
            Invoke("StepFour",1.15f);
            Invoke("StepFive",1.9f);
            Invoke("StepSix",3.1f);
            Invoke("StepSeven",3.7f);
            Invoke("StepEight",5.4f);
            Invoke("StepNine",7.5f);
            Invoke("AtLastCancel",8.2f);
        }
    }

    void InitPic(){
        for (int i = 0; i < 4; i++)
        {
            spAni[i].color = Color.white;
        }
        spAni[1].transform.localPosition = new Vector3(-720,159,0);
        spAni[0].transform.localPosition = new Vector3(-720,159,0);

        spAni[2].transform.localScale = Vector3.zero;
        spAni[3].transform.localScale = Vector3.zero;

        spAni[4].color = new Color(1,1,1,0);
        spAni[4].transform.localPosition = new Vector3(700,159,0);

       
        spAni[5].color = Color.white;
        spAni[6].color = Color.white;
        spAni[5].transform.localScale = Vector3.one * 3;
        spAni[5].gameObject.SetActive(false);
        spAni[6].gameObject.SetActive(false);
        spAni[7].color = new Color(1,1,1,0);
    }


    void StepOne(){
        GameObject Obj_1 = spAni[1].gameObject;
        Vector3 targetPos = new Vector3(-147,159,0);
        MiniItween.MoveTo(Obj_1,targetPos,0.3f,MiniItween.EasingType.Bounce);
    }

    void StepTwo(){
        GameObject obj_0 = spAni[0].gameObject;
        Vector3 targetPos = new Vector3(-421,159,0);
        MiniItween.MoveTo(obj_0,targetPos,0.3f,MiniItween.EasingType.Bounce);
    }
    void StepThree(){
        GameObject obj_2 = spAni[2].gameObject;
        Vector3 targetScale = Vector3.one * 1.3f;
        MiniItween.ScaleTo(obj_2,targetScale,0.15f).myDelegateFunc += () => {
            MiniItween.ScaleTo(obj_2,Vector3.one,0.1f);
        };
        //  MiniItween.Shake(obj_2,new Vector3(6,6,0),0.2f,MiniItween.EasingType.EaseInCubic,false);
    }
    void StepFour(){
        GameObject obj_3 = spAni[3].gameObject;
        GameObject obj_2 = spAni[2].gameObject;
        Vector3 targetScale = Vector3.one * 1.3f;
        MiniItween.ScaleTo(obj_3,targetScale,0.15f).myDelegateFunc += () => {
            MiniItween.ScaleTo(obj_3,Vector3.one,0.1f);
            MiniItween.Shake(obj_2,new Vector3(6,6,0),0.2f,MiniItween.EasingType.EaseInCubic,false);
        };

    }


    void StepFive(){
        GameObject obj_4 = spAni[4].gameObject;
        Vector3 targetPos = new Vector3(436,159,0);
        MiniItween.MoveTo(obj_4,targetPos,0.8f);
        MiniItween.ColorTo(obj_4,new V4(Color.white),0.7f,MiniItween.Type.ColorWidget);
    }


    void StepSix(){
        GameObject obj_5 = spAni[5].gameObject;
        GameObject obj_0 = spAni[0].gameObject;
        obj_5.SetActive(true);
        MiniItween.ScaleTo(obj_5, Vector3.one, 0.2f).myDelegateFunc += () =>
        {
            MiniItween.Shake(obj_0,new Vector3(10,10,0),0.2f,MiniItween.EasingType.EaseInCubic,false);
        };
       
    }

    void StepSeven(){
        GameObject obj_6 = spAni[6].gameObject;
        Invoke("ShowObj",0);
        Invoke("HideObj",0.1f);
        Invoke("ShowObj",0.2f);
        Invoke("HideObj",0.3f);
        Invoke("ShowObj",0.4f);
    }
    void HideObj(){
        GameObject obj_6 = spAni[6].gameObject;
        obj_6.SetActive(false);
    }

    void ShowObj(){
        GameObject obj_6 = spAni[6].gameObject;
        obj_6.SetActive(true);
    }


    void StepEight(){
        GameObject obj_7 = spAni[7].gameObject;
        MiniItween.ColorTo(obj_7,new V4(Color.white),1f,MiniItween.Type.ColorWidget);
    }


    void StepNine(){
        for (int i = 0; i < spAni.Length; i++)
        {
            MiniItween.ColorTo(spAni[i].gameObject, new V4(1, 1, 1, 0), 0.6f, MiniItween.Type.ColorWidget);
        }
       
    }
  


    void AtLastCancel()
    {
		if(OnFinished != null)
        {
			OnFinished();
			OnFinished = null;
        }
        this.dealloc();
    }

    void OnDestroy()
    {
        instance = null;
    }



}
