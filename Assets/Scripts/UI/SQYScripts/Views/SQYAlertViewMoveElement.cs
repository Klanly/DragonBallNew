using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SQYAlertViewMoveElement : MonoBehaviour {

	public UILabel lab_Content;
    public UISprite lab_Sp;
    public void alertViewFreshUI(string msg, GameObject root = null)
	{
		if(root == null) {
			if(DBUIController.mDBUIInstance != null) {
				RED.AddChild(gameObject, DBUIController.mDBUIInstance._TopRoot);
			}
		}
            
        else
            RED.AddChild(gameObject, root);

        if(lab_Content != null) {
			lab_Content.text = msg;
		}
      
	}
   
	public void close()
	{
		Destroy(this.gameObject);
	}
    public void closeDes()
    {
        TweenAlpha taaSp =  lab_Sp.gameObject.GetComponent<TweenAlpha>();
        taaSp.onFinished.Clear();

        TweenAlpha taaSpCon =  lab_Content.gameObject.GetComponent<TweenAlpha>();
        taaSpCon.onFinished.Clear();

    }

}
