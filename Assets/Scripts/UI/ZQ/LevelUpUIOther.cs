using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 等级提升
public class LevelUpUIOther : MonoBehaviour 
{
	
    public UILabel leveltxt;
    public UILabel contentTxt;
    private static LevelUpUIOther _mInstance;
    void Awake()
    {
        _mInstance = this;      
        // leveltxt.text = Core.Data.playerManager.curVipLv.ToString();
        
    }
    public static LevelUpUIOther mInstance
	{
        get {
			return _mInstance;
		}
	}
	
    public static void OpenUI()
	{

		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("ZQ/LevelUpUIOther");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localEulerAngles = Vector3.zero;
                RED.TweenShowDialog(obj);
			}
		}
		else
		{

            mInstance.SetShow(true);
		}

	}
    void SetShow(bool bShow)
    {
        RED.SetActive(bShow, this.gameObject);
    }

	public static void DestroyUI()
	{
		if(_mInstance != null)
		{
			Destroy(_mInstance.gameObject);
			_mInstance = null;
		}
	}
    public void showVipUpdate(int viplevelValue){
        leveltxt.text ="VIP"+ viplevelValue.ToString();
        List<VipInfoData> _templist = Core.Data.vipManager.GetVipInfoDataListConfig();
        contentTxt.text=DealWithStr(_templist[viplevelValue].tips);
    }


   private string DealWithStr(string _str)
    {
		_str = _str.Replace("{","");
		_str = _str.Replace("}","");
        string[] _temp = _str.Split(',');
        string newstr = "";
        for(int i=0; i<_temp.Length; i++)
        {
            newstr = newstr + _temp[i] + "\r\n";
        }
        return newstr;
    }
	
    void clostThis(){
        DestroyUI();
    }
    //打开特权界面
    void gotoTequan(){
        UIDragonMallMgr.GetInstance().SetVipLibao();
        DestroyUI();
    }


}
