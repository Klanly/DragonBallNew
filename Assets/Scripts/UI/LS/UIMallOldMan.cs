using UnityEngine;
using System.Collections;

public class UIMallOldMan : RUIMonoBehaviour
{
    public UILabel mText;
    public UILabel lbl_BtnYes;
    public UISprite[] spt_Role;


   public  void OnClickOK()
    {
        UIInformation.GetInstance().OnCallback();
        gameObject.SetActive(false);
    }

	public void OnClickExit()
    {
       UIInformation.GetInstance().OnCancel();
        RED.SetActive(false, this.gameObject);
        Destroy(gameObject);
    }

	void SetText(string m_text, string m_BtnText1)
    {
        if (mText != null)
            mText.text = m_text;
		lbl_BtnYes.text = m_BtnText1;
    }

    /// <summary>
    /// 1：布玛  2：小林 3：饺子   其他的  btn_test 没用了 
    /// </summary>
    /// <param name="mtype">Mtype.</param>
    /// <param name="m_Text">M_ text.</param>
    public void SetAllData(InformationType mtype, string m_Text,string m_BtnText1,string m_BtnText2,string m_BtnText3)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((int)mtype == i+1)
            {
                spt_Role[i].gameObject.SetActive(true);
            }
            else
            {   
                spt_Role[i].gameObject.SetActive(false);

            }
        }
		SetText(m_Text, m_BtnText1);
    }




   
}
