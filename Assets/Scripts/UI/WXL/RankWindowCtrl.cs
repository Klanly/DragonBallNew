using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankWindowCtrl :RUIMonoBehaviour {
//	private static RankWindowCtrl instance;
//	public static RankWindowCtrl Instance
//	{
//		get
//		{
//			return instance;
//		}
//	}

	public List<GameObject> rankItemObj;
	public bool isWin = true;
	public GameObject WinTitle;
	public GameObject LoseTitle;
	public UILabel lbl_Title;
	public List<UserAtkBossInfo> rankHurtList ;
    public UILabel secondTitle;
    SockBossAtkListData localData;
    void Start(){
        localData = ActivityNetController.GetInstance().TempAtkData;
        if (localData != null) {
            if (localData.isKill == 1)
                isWin = true;
            else
                isWin = false;

            rankHurtList = new List<UserAtkBossInfo> ();// = UIActMonsterComeController.Instance.hurtDataList;

            for (int i = 0; i < rankItemObj.Count; i++) {
                if (i < localData.attStrList.Count) {
                    rankHurtList.Add (localData.attStrList [i]);
                   
                }
            }
        }

		if (isWin == true) {
			lbl_Title.text = Core.Data.stringManager.getString (7128);
			WinTitle.SetActive (true);
			LoseTitle.SetActive (false);

            if (localData.killName != null) {
                secondTitle.text = string.Format (Core.Data.stringManager.getString (7306), localData.killName);
            }else
                secondTitle.gameObject.SetActive(false);

		} else {
			lbl_Title.text = Core.Data.stringManager.getString (7129);
			WinTitle.SetActive (false);
			LoseTitle.SetActive (true);
            secondTitle.text = string.Format (Core.Data.stringManager.getString(7334),UIActMonsterComeController.Instance.lbl_LeftMonsterName.text);
		}

		for (int i = 0; i < rankItemObj.Count; i++) {
            if (i < rankHurtList.Count) {
                rankItemObj [i].GetComponent<ActHurtRankItem> ().SetItemValue (rankHurtList [i]);
            } else {
                rankItemObj [i].GetComponent<ActHurtRankItem> ().SetItemValue (null);
            }
		}

	}
        
    void OnRefresh(){
        for (int i = 0; i < rankItemObj.Count; i++) {
            rankItemObj [i].GetComponent<ActHurtRankItem> ().Refresh ();
        }
    }
    void OnClose(){
        Destroy (gameObject);
    }

    public static RankWindowCtrl CreatRankWindowCtrl(){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIMonsterComerRank);
        if (obj != null) {
            GameObject go = Instantiate (obj) as GameObject;
            RankWindowCtrl fc = go.GetComponent<RankWindowCtrl> ();
            Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.zero;
            RED.TweenShowDialog (go);
            return fc;
        }
        return null;    
    }

}
