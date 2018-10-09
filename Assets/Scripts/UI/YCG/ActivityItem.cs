using UnityEngine;
using System.Collections;
//yangcg
public class ActivityItem : MonoBehaviour 
{
    public UISprite activityIcon ;
    public UILabel activityName ; 
	public int activityIndex;
	// Use this for initialization
	void Start () 
    {
	
	}
    //活动Icon
    public string  setSpriteIcon
    {
        set {
            activityIcon.spriteName = value  ;
             activityIcon.MakePixelPerfect();
        }
    }
    // 活动名字 
    public string  setName
    {
        set {
            activityName.text = value  ;

        }
    }
    //是否开启这个活动
    public bool IsActivation
    {
        set 
        {
            activityIcon.enabled = false ; 
            activityIcon.color = Color.black ; 
        }
    }
   

	
}
