using UnityEngine;
using System.Collections;

public class UIAwardItem : RUIMonoBehaviour {

    public UISprite mItemSprite;
    public UILabel mLabel;
    public UISprite[] mStar;


    public void OnShow(int icoid, string name)
    {
        mItemSprite.spriteName = icoid.ToString();
		mLabel.text = name;
	}
    
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
