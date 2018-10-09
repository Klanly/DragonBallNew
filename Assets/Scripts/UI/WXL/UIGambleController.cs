using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIGambleController : RUIMonoBehaviour {

    private static UIGambleController instance;
    public static UIGambleController Instance
    {
        get
        {
            return instance;
        }
    }


    public List<UIGambleItem> gambleItemObList; 
    public System.Action closeMethod = null;
    public UIButton closeBtn;
    public UILabel leftNum;
    GetGambleStateResponse myResponse;

    public static UIGambleController CreateGamblePanel (System.Action OnClose = null,GetGambleStateResponse resp = null)
    {
        if(resp.data.list != null || resp.data.status.total >resp.data.status.yet){
            if (instance == null)
            {

                UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIGamblePanel);
                if (obj != null)
                {
                    GameObject go = Instantiate(obj) as GameObject;
                    UIGambleController fc = go.GetComponent<UIGambleController>();

                    Transform goTrans = go.transform;
                    go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
                    go.transform.localPosition = Vector3.zero;
                    goTrans.localScale = Vector3.one;
                    fc.myResponse = resp;
                    RED.TweenShowDialog(go);

                    fc.closeMethod = OnClose;
                }
            }
            else {
                instance.gameObject.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
                instance.gameObject.transform.localPosition = Vector3.zero;
                instance.gameObject.gameObject.transform.localScale = Vector3.one;
                instance.myResponse = resp;
//                RED.TweenShowDialog(instance.gameObject);
                instance.closeMethod = OnClose;
                return instance;
            }
            return null;        
        }else{
            if (OnClose != null) {
                OnClose ();
            }
            return null;
        }
    }
        
    void Awake(){
        instance = this;
    }

    void Start(){
        // ActivityNetController.GetInstance ().GetGambleStateList ();
        //   ComLoading.Open ();
        this.InitData (myResponse);
    }

    public void InitData(GetGambleStateResponse response){
        if (response != null) {
            for (int i = 0; i < response.data.list.Length; i++) {
                if (i < gambleItemObList.Count) {
                    gambleItemObList [i].SetItemValue ((object)response.data.list[i]);
                }
            }
        }
        leftNum.text = (response.data.status.total - response.data.status.yet).ToString ();
        ComLoading.Close ();
    }

    void Refresh(){
        foreach(UIGambleItem gbItem in gambleItemObList){
            gbItem.Refresh ();
        }
    }

    public void OnClose(){
        Core.Data.temper.gambleTypeId = -1;
        OnBack ();
    }

    public void OnBack(){
        if (closeMethod != null) {
            closeMethod ();
        }

        closeBtn.isEnabled = false;
        Destroy (gameObject);
        ComLoading.Open ();
    }
}
