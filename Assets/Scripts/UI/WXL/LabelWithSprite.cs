using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class LabelWithSprite : MonoBehaviour {
    public UISprite leftSpite;
    public UISprite rightSpite;
    public int length;


    void Start(){
        int width = gameObject.GetComponent<UILabel>().width;
        float localX = transform.localPosition.x;
//        if (localX > 0) {
//
//        } else {
//
//        }
        leftSpite.transform.localPosition =new Vector3(localX - width / 2 - length,leftSpite.transform.localPosition.y,0);
        rightSpite.transform.localPosition = new Vector3( (localX + width / 2) + length,leftSpite.transform.localPosition.y,0);
    }
}
