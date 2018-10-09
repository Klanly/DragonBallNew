using UnityEngine;
using System.Collections;

public class UITianXiaRankCell : RUIMonoBehaviour
{

	public UILabel _Name;
	public UILabel _Level;
	public UILabel _VipLevel;
	public UILabel _Rank;
	public UISprite _Vip;

	private int _gid;

	public void OnShow(GetChallengeRank data, int _rank, int gid)
	{
		if(data == null)return;
		_gid = gid;

		if(data.name == Core.Data.playerManager.NickName)_Name.text = "[87cefa]"+data.name;
		else _Name.text = data.name;
		_Level.text = data.level.ToString();
		_VipLevel.text = data.vip.ToString();
		if(_rank == 1)_Rank.text = string.Format("[ff0000]" + Core.Data.stringManager.getString(20090), _rank.ToString());
		else if(_rank == 2)_Rank.text = string.Format("[ff6600]" + Core.Data.stringManager.getString(20090), _rank.ToString());
		else if(_rank == 3)_Rank.text = string.Format("[ffd200]" + Core.Data.stringManager.getString(20090), _rank.ToString());
		else _Rank.text = string.Format(Core.Data.stringManager.getString(20090), _rank.ToString());
		
		SetVip(data.vip);
	}

	void SetVip(int viplv)
	{
		if (viplv < 4) {
			_Vip.spriteName = "common-2008";
		} else if (viplv > 3 && viplv < 8) {
			_Vip.spriteName = "common-2009";
		} else if (viplv > 7 && viplv < 12) {
			_Vip.spriteName = "common-2007";
		} else if (viplv > 11 && viplv < 16) {
			_Vip.spriteName = "common-2109";
		}
	}

	
	void OnClick_ZhenXiing()
	{
		DragonBallRankMgr.GetInstance().FinalTrialRankCheckInfoRequest(_gid);
    }
	
	void OnDestroy()
	{
		_Name = null;
		_Level = null;
		_VipLevel = null;
		_Rank = null;
		_Vip = null;
	}

}
