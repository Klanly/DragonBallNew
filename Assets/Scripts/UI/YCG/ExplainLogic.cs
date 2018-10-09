using UnityEngine;
using System.Collections;
//yangchenguang 轮盘功能图标说明
public class ExplainLogic : MonoBehaviour
{
    private UISprite TitleSp ; // 标题图标
    public UILabel TitleName; //标题名字
    public UILabel ShuoMing;  //功能说明
	// Use this for initialization
	void Start () 
    {
	
	}
    public void Init(string titleSpName ,string titleName ,string shouming)
    {
        TitleSp = gameObject.GetComponent<UISprite>();
        TitleSp.spriteName = titleSpName;
        TitleName.text = Core.Data.stringManager.getString(int.Parse(titleName));
        ShuoMing.text = Core.Data.stringManager.getString(int.Parse(shouming));
    }
    void OnDestroy()
    {
        TitleSp = null ; 
        TitleName.text =""; 
        ShuoMing.text ="" ;
    }

	
}
