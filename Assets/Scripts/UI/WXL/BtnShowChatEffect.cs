using UnityEngine;
using System.Collections;

public class BtnShowChatEffect : MonoBehaviour {

    public UISprite[] spPoint;

    public float interval = 0.5f;
    int count =1;

    void OnEnable(){
        InvokeRepeating ("ShowPointEffect",0,interval);
    }

    void ShowPointEffect(){
        if (count > 3) {
            count = 1;
        }
        for (int i = 1; i < spPoint.Length; i++) {
            if (i < count)
                spPoint [i].gameObject.SetActive (true);
            else
                spPoint [i].gameObject.SetActive (false);
        }
        count++;
    }

    void OnDisable(){
        CancelInvoke ("ShowPointEffect");
        foreach (UISprite usp in spPoint) {
            usp.gameObject.SetActive (true);
        }
    }
}
