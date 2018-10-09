using UnityEngine;
using System.Collections;
using System.Text;

public class UIPVPPlotBuilding : RUIMonoBehaviour
{		
	public StarsUI stars;
	public UISprite Spr_Buliding;
	public UISprite[] ItemIcon;
	public UILabel[] ItemValue;
	public GameObject Circle;
	public UISprite mGuang;
	[HideInInspector]
	public NewMapFinalTrial mNewMapFinalTrial;

	private readonly int TotalMapWidth = 2144;


	public void SetData(NewMapFinalTrial m_NewMapFinalTrial)
	{
		if(m_NewMapFinalTrial == null)return;
		mGuang.gameObject.SetActive(false);
		mNewMapFinalTrial = m_NewMapFinalTrial;
		
		Spr_Buliding.pivot = UIWidget.Pivot.Bottom;
		transform.localPosition = m_NewMapFinalTrial.localPosition;
		
		Spr_Buliding.spriteName = m_NewMapFinalTrial.Data.TextrueID;
		Spr_Buliding.MakePixelPerfect();
//		ChangeScale();
		UIButtonMessage message = gameObject.AddComponent<UIButtonMessage>();
		if(message != null)
		{
			message.target = FinalTrialMgr.GetInstance().m_PvpShaluBuouRoot.m_UIMapOfFinalTrial.gameObject;
			message.functionName = "OnClickBuild";
		}
		
		stars.SetStar(0);
		
		SetBuildingState(m_NewMapFinalTrial);
	}

	void ChangeScale()
	{
		Spr_Buliding.width = (int)((float)Spr_Buliding.width*1.2f);
		Spr_Buliding.height = (int)((float)Spr_Buliding.height*1.2f);
		Transform[] tran = stars.GetComponentsInChildren<Transform>();
		for(int i=0; i<tran.Length; i++)
		{
			UISprite _sprite = tran[i].GetComponent<UISprite>();
			if(_sprite == null)continue;
			_sprite.width = (int)((float)_sprite.width*1.2f);
			_sprite.height = (int)((float)_sprite.height*1.2f);
		}
	}

	void SetCirclePos()
	{
		Vector3 _pos = Vector3.zero;
		if(TotalMapWidth - ((int)transform.localPosition.x + Spr_Buliding.width) < Circle.GetComponent<UISprite>().width)
		{
			_pos = new Vector3(Circle.transform.localPosition.x - (float)Circle.GetComponent<UISprite>().width, Circle.transform.localPosition.y + (float)Spr_Buliding.height/2, Circle.transform.localPosition.z);
		}
		else
		{
			_pos = new Vector3(Circle.transform.localPosition.x + (float)Circle.GetComponent<UISprite>().width, Circle.transform.localPosition.y + (float)Spr_Buliding.height/2, Circle.transform.localPosition.z);
		}
		Circle.transform.localPosition = _pos;

		SetMoneyAndKeys();
	}

	void SetMoneyAndKeys()
	{
		ItemIcon[0].gameObject.SetActive(false);
		ItemIcon[1].gameObject.SetActive(false);
		ItemValue[0].SafeText("");
		ItemValue[1].SafeText("");
		if(mNewMapFinalTrial.Data.Reward.Count > 2)return;
		for(int i=0; i<mNewMapFinalTrial.Data.Reward.Count; i++)
		{
			ItemIcon[i].gameObject.SetActive(true);
			ConfigDataType datatype = DataCore.getDataType(mNewMapFinalTrial.Data.Reward[i][0]);
			if (datatype == ConfigDataType.Monster)
			{
				AtlasMgr.mInstance.SetHeadSprite (ItemIcon[i], mNewMapFinalTrial.Data.Reward[i][0].ToString ());
				continue;
			}
			else if (datatype == ConfigDataType.Equip)
			{
				ItemIcon[i].atlas = AtlasMgr.mInstance.equipAtlas;
			}
			else if (datatype == ConfigDataType.Gems)
			{
				ItemIcon[i].atlas = AtlasMgr.mInstance.commonAtlas;
			}
			else if (datatype == ConfigDataType.Item)
			{
				ItemIcon[i].atlas = AtlasMgr.mInstance.itemAtlas;
			}
			else if(datatype == ConfigDataType.Frag)
			{
				int id = (mNewMapFinalTrial.Data.Reward[i][0]%150000)+10000;
				if (DataCore.getDataType(id) == ConfigDataType.Monster)
				{
					AtlasMgr.mInstance.SetHeadSprite (ItemIcon[i], id.ToString ());
					continue;
				}
				else if (DataCore.getDataType(id) == ConfigDataType.Item)
				{
					ItemIcon[i].atlas = AtlasMgr.mInstance.itemAtlas;
				}
				
			}
			
			ItemIcon[i].spriteName = mNewMapFinalTrial.Data.Reward[i][0].ToString();
			StringBuilder builder = new StringBuilder("x");
			builder.Append(mNewMapFinalTrial.Data.Reward[i][1]);
			ItemValue[i].SafeText(builder.ToString());
		}
	}
	
	void SetBuildingState(NewMapFinalTrial m_NewMapFinalTrial)
	{
		Spr_Buliding.spriteName = m_NewMapFinalTrial.Data.TextrueID;

		switch(m_NewMapFinalTrial.State)
		{
			case NewFloorState.Current:
				mGuang.gameObject.SetActive(true);
				Spr_Buliding.color = new Color(1f,1f,1f,1f);
				Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
				Circle.gameObject.SetActive(true);
				SetCirclePos();
				break;
			case NewFloorState.Pass:
				mGuang.gameObject.SetActive(true);
				Spr_Buliding.color = new Color(1f,1f,1f,1f);
				Spr_Buliding.GetComponent<BoxCollider>().enabled = true;
				Circle.gameObject.SetActive(false);
				break;
			case NewFloorState.Unlocked:
                Spr_Buliding.color = new Color(0,0,0,1f);
				Spr_Buliding.GetComponent<BoxCollider>().enabled = false;
				Circle.gameObject.SetActive(false);
				break;
        }
        
//        Spr_Buliding.MakePixelPerfect();
    }
}
