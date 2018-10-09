using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// yangchenguang 点击空白处关闭不是全屏的UI
public class CloseUILogic : MonoBehaviour
{
    public GameObject _game;
//直接DES掉对象
    public GameObject _gameVisible;
    // 不关闭物体直接隐藏
    // Use this for initialization
    void Start()
    {
	
    }

    void OnClick()
    {
        if (_game == null)
        {
            if (_gameVisible != null)
                {
                UIInformation.GetInstance().OnCancel();
                _gameVisible.SetActive(false);
               
            }
            return;
        }


		if (_game.GetComponent<MonsterInfoUI> () != null) {
			MonsterInfoUI MonsterInfoUI = _game.GetComponent<MonsterInfoUI> ();
			MonsterInfoUI.OnClickClose ();
		} else if (_game.GetComponent<GetRewardSucUI> () != null) {
			//
			GetRewardSucUI RewardSucUI = _game.GetComponent<GetRewardSucUI> ();
			RewardSucUI.OnBtnOK ();
		} else if (_game.GetComponent<SelUserHeadUI> () != null) {
			SelUserHeadUI SelUserHead = _game.GetComponent<SelUserHeadUI> ();
			SelUserHead.OnClickExit ();
		} else if (_game.GetComponent<UIMessageMail> () != null) {
			UIMessageMail MessageMail = _game.GetComponent<UIMessageMail> ();
			MessageMail.OnBtnClick ("Close");

		} else if (_game.GetComponent<JCEquipmentDesInfoUI> () != null) { // 装备查看
			JCEquipmentDesInfoUI JCEquipmentDesInfo = _game.GetComponent<JCEquipmentDesInfoUI> ();
			JCEquipmentDesInfo.OnXBtnClick ();
		} else if (_game.GetComponent<WXLActivityFestivalController> () != null) { // 武者节日说明界面
			WXLActivityFestivalController WXLActivityFestival = _game.GetComponent<WXLActivityFestivalController> ();
			WXLActivityFestival.On_BtnClose ();
			//WXLActivityFestivalController
		} else if (_game.GetComponent<TrucePanelScript> () != null) {// 神龙购买免战牌

			TrucePanelScript TrucePanel = _game.GetComponent<TrucePanelScript> ();
			TrucePanel.OnXBtnClick ();      
			//TrucePanelScript
		} else if (_game.GetComponent<UIAnnounce> () != null) { // 公告
			UIAnnounce uia = _game.GetComponent<UIAnnounce> ();
			uia.Back_OnClick ();   
		} else if (_game.GetComponent<UIDateSignController> () != null) { // 签到
			UIDateSignController uds = _game.GetComponent<UIDateSignController> ();
			uds.OnBackBtn ();   
		} else if (_game.GetComponent<GemSyntheticSystemUI_View> () != null) { // 宝石合成功能说明
			GemSyntheticSystemUI_View gss = _game.GetComponent<GemSyntheticSystemUI_View> ();
			gss.CloseDesp ();  
		} else if (_game.GetComponent<LevelUpUI> () != null) {
			LevelUpUI lp = _game.GetComponent<LevelUpUI> ();
			lp.OnBtnOK ();
		} else if (_game.GetComponent<UIGambleController> () != null) {
			UIGambleController uc = _game.GetComponent<UIGambleController> ();
			uc.OnClose ();
		} else if (_game.GetComponent<UITask> () != null) {
			UITask uiT = _game.GetComponent<UITask> ();
			uiT.UIDestory ();
		}
        else if (_game != null) //直接干掉界面不需要调什么东西
        {
            ComLoading.Close(); // 关闭界面关闭Comloading.Close();
            UIInformation.GetInstance().OnCancel();
            Destroy(_game);

        }
        else if (_gameVisible != null) //直接隐藏界面
        {
            UIInformation.GetInstance().OnCancel();
            _gameVisible.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
	
    }
}
