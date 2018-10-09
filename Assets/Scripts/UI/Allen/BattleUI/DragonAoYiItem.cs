using UnityEngine;
using System.Collections;

public class DragonAoYiItem : MonoBehaviour {
    public UISprite Icon;
    public UISprite Boarder;
    public MiniItween.EasingType OutType = MiniItween.EasingType.Linear;
    public MiniItween.EasingType InType = MiniItween.EasingType.Linear;
    public float Time = 0.05f;

    public void CastDragonAoYi () {
        StartCoroutine(CastDragonAoYiAnim());
    }

    IEnumerator CastDragonAoYiAnim() {
        Icon.color = new Color (0.0f, 0.0f, 0.0f, 1.0f);

        MiniItween.ScaleTo(gameObject, new Vector3(1.2f, 1.2f, 1.2f), Time, OutType);
        yield return new WaitForSeconds(Time);
        MiniItween.ScaleTo(gameObject, Vector3.one, Time, InType).myDelegateFunc += () =>{  
            Icon.depth = 20;
            Boarder.depth = 21;
        };
    }


    public void ResetDragonAoyi () {
        Icon.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

        Icon.depth = 0;
        Boarder.depth = 1;

		gameObject.transform.localScale = Vector3.one * 0.9f;
    }

}
