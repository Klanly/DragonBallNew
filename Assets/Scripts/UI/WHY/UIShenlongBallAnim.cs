using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIShenlongBallAnim : MonoBehaviour {

    public Transform RootPos;
    public List<Transform> StartPosList;
    public Vector3 EndPos;
    public float MoveTime = 0.4f;

    public GameObject Rt;

    private List<UISprite> BallList;

    public GameObject highAni;
	public UILabel lblTimeEffect;
    public UISprite boxSide;
    public UILabel lblCallDragon;
	
    public GameObject balls;
    public GameObject block;
    public GameObject lights;



    private Vector3 timerLabelPos = new Vector3(9.91f,186.81f,0);
    private Vector3 timerStartPos = new Vector3(9.91f,50,0);

	public Vector3 targetPos = new Vector3(0,-270,0);
    public Vector3 startPos = new Vector3 (0,-550,0);

    Vector3 bgStartPos = new Vector3(0,-12000,0);
    Vector3 bgEndPos = new Vector3(0,10000,0);

     Color localBlack = new Color(11f/255f,11f/255f,11f/255f,1f);

	 Color localGreen = new Color (4f/255f,188/25f,43f/255f,1f);
    
    //  public GameObject succeedPanelObj;

    public UILabel[] lblFont;
    [HideInInspector]
    public bool setTimeBoxEffect =false;

    void Awake(){
        BallList = new List<UISprite>();
    }
    void Start() {
    }
    void OnEnable(){
        this.SetShowBoxEffect ();
    }
	
    private Object getBallObj {
        get {
            return PrefabLoader.loadFromPack("WHY/pbDragonBall");
        }
    }
	
    //star >= 1 && star <= 7
    public UISprite getBallGo(int star) {
        Object obj = getBallObj;
        GameObject go = Instantiate(obj, StartPosList[star - 1].position, Quaternion.identity) as GameObject;
        UISprite sprite = go.GetComponent<UISprite>();
        sprite.spriteName = "dragon-" + star.ToString();

        RED.AddChild(go, Rt);

        return sprite;
    }


    public void ShowTimeEffect(){
        lblTimeEffect.gameObject.SetActive (true);
        lblTimeEffect.transform.localPosition = timerStartPos;
        lblTimeEffect.transform.localScale = Vector3.one * 1.5f;
        lblTimeEffect.alpha = 1f;

        lblTimeEffect.text = Core.Data.stringManager.getString(6047)+"   "+"[ffff00]" + UIShenLongManager.Instance.callDragonTimeLabel.text;
        TweenScale.Begin(lblTimeEffect.gameObject,0.3f,Vector3.one * 2).onFinished.Add(new EventDelegate(this,"OnMoveToTarget")) ;

    }
        
    void OnMoveToTarget(){
        lblTimeEffect.GetComponent<TweenScale> ().onFinished.Clear ();

        MiniItween.MoveTo(lblTimeEffect.gameObject,timerLabelPos,0.8f);
        TweenScale.Begin(lblTimeEffect.gameObject,0.8f,Vector3.one);
        setTimeBoxEffect = true;
       
        lblTimeEffect.GetComponent<TweenScale> ().onFinished.Add (new EventDelegate(this,"SetShowBoxEffect"));

    }

    public void  SetShowBoxEffect(){
        StopCoroutine("ShowBoxEffect");
        StartCoroutine ("ShowBoxEffect");
    }

    public IEnumerator ShowBoxEffect(){
        if(lblTimeEffect != null && lblTimeEffect.GetComponent<TweenScale>() != null)
            lblTimeEffect.GetComponent<TweenScale> ().onFinished.Clear ();
        lblTimeEffect.alpha = 0f;
        boxSide.gameObject.SetActive (setTimeBoxEffect);
        lblCallDragon.gameObject.SetActive (setTimeBoxEffect);
        lblCallDragon.text = Core.Data.stringManager.getString (6124) ;

        string tStrPoint = "." ;
        int count = 0;

        while (setTimeBoxEffect) {
            if (count > 6) {
                count = 0;
                tStrPoint = "";
            }else{
                tStrPoint += ".";
            }

            lblCallDragon.text = Core.Data.stringManager.getString (6124)+tStrPoint;
            TweenAlpha.Begin (boxSide.gameObject, 0.3f, 1f);
            yield return new WaitForSeconds (0.3f);
            TweenAlpha.Begin (boxSide.gameObject, 0.3f, 0.3f);
            yield return new WaitForSeconds (0.3f);
            count++;
        }
    }


    private void initBalls() {
        if(BallList.Count != 7) {
            BallList.Clear();

            for(int i = 1; i < 8; ++ i) {
                BallList.Add(getBallGo(i));
            }
        } 
        //归位
        for(int i = 0; i < 7; ++ i) {
            BallList[i].gameObject.SetActive(true);
            BallList[i].transform.localPosition = StartPosList[i].localPosition;
        }
    }


    IEnumerator MoveBalls() {

        for(int i = 0; i < 7; ++ i) {
            BallList[i].gameObject.SetActive(true);
            MiniItween.MoveTo(BallList[i].gameObject, EndPos, MoveTime);
        }

        yield return new WaitForSeconds(MoveTime + 0.1f);

//        Debug.Log("  balls = "+ BallList.Count);
//        for(int i = 0; i < 7; ++i) {
//            BallList[i].gameObject.SetActive(false);
//        }
		
    }

    public void  showUpgradeDragonCompleted() {
    
        initBalls();

         StartCoroutine(MoveBalls());

//        if(dragonType == DragonManager.DragonType.EarthDragon) {
//            StartCoroutine(ShowDragon());
//        } else {
            StartCoroutine(ShowDragon());
		//    }
       
    }




	IEnumerator ShowDragon(){
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_SummonDragon,null);
        yield return new WaitForSeconds( 0.5f);
        for(int i = 0; i < BallList.Count; ++i) {
            BallList[i].gameObject.SetActive(false);
        }
  
        //GameObject balls = highAni.transform.FindChild("BallObj").gameObject;
       
        //GameObject block = highAni.transform.FindChild("Block").gameObject;

        //GameObject lights = highAni.transform.FindChild ("LightObj").gameObject;

        highAni.SetActive(true);
        block.SetActive(true);
        balls.SetActive(true);
        lights.SetActive(true);
		
        lights.GetComponent<UISprite>().color = localBlack;
        lights.GetComponent<UISprite>().alpha = 1f;

        //展示文字
        string tChar = Core.Data.stringManager.getString(7390);
        for(int i=0;i< lblFont.Length;i++){
            if (lblFont.Length - 1 == i)
            {
                if(tChar != null){

                    lblFont[i].text = "";
                    for (int j = 0; j < tChar.ToCharArray().Length; j++)
                    {
                        lblFont[i].gameObject.SetActive(true);
                        lblFont[i].text += tChar.Substring(j,1);
                        MiniItween.Shake(lblFont[i].gameObject, Vector3.one*5, 0.1f, MiniItween.EasingType.EaseInCubic,false);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            else
            {
                lblFont[i].gameObject.SetActive(true);
                lblFont[i].text = Core.Data.stringManager.getString(7389) + "!";
                MiniItween.Shake(lblFont[i].gameObject, Vector3.one *6, 0.1f, MiniItween.EasingType.EaseInCubic,false);
                yield return new WaitForSeconds(0.8f + i * 0.1f);
            }
        }
        balls.transform.localPosition = startPos;
        block.transform.localPosition = bgEndPos;
        yield return new WaitForSeconds (1f);

        lights.SetActive(false);

        for (int i = 0; i < lblFont.Length; i++)
        {
            lblFont[i].gameObject.SetActive(false);
        }

        MiniItween.Shake(balls,Vector3.one,1f,MiniItween.EasingType.EaseInBack,false);
        MiniItween.MoveTo(balls, startPos + Vector3.up * 3, 0.1f);
        MiniItween.MoveTo(block,  bgStartPos,3f,MiniItween.EasingType.Linear);
        for (int i = 0; i < 20; i++)
        {
            yield return MiniItween.MoveTo(balls, balls.transform.localPosition + Vector3.up * 20, 0.1f,MiniItween.EasingType.EaseInBack);
            yield return new WaitForSeconds(0.08f);
            yield return MiniItween.MoveTo(balls, balls.transform.localPosition - Vector3.up * 10f, 0.1f);
        }

        block.SetActive(false);
        balls.SetActive(false);
        lights.SetActive(true);
      
        lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = Color.white;
        yield return new WaitForSeconds (0.1f);

        lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = localBlack;
        yield return new WaitForSeconds (0.1f);
		lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = Color.gray;
        yield return new WaitForSeconds (0.1f);
		lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = Color.white;
        yield return new WaitForSeconds (0.1f);
        lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = localGreen;
        yield return new WaitForSeconds (0.1f);
		lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = Color.white;
        yield return new WaitForSeconds (0.1f);
        lights.GetComponent<UISprite>().alpha = 1f;
        lights.GetComponent<UISprite>().color = localBlack;
		
        TweenAlpha.Begin(lights,1f,0f);
        yield return new WaitForSeconds(1f);

        highAni.SetActive(false);

    }
    public void StopLabelEffect(){
        setTimeBoxEffect = false;
        StopCoroutine ("ShowBoxEffect");
        lblTimeEffect.gameObject.SetActive (false);
        boxSide.gameObject.SetActive (false);
        lblCallDragon.gameObject.SetActive (false);

    }




}
