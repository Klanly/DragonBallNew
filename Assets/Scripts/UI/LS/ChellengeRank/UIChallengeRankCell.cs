using UnityEngine;
using System.Collections;

public class UIChallengeRankCell : RUIMonoBehaviour {

	public UILabel _Name;
	public UILabel _Level;
	public UILabel _Coin;
	public UILabel _Rank;

	public void OnShow(GetChallengeRank data, int _rank)
	{
		_Name.text = data.name;
		_Level.text = data.level.ToString();
		_Coin.text = data.robcoins.ToString();
		_Rank.text = _rank.ToString() + ".";
	}

	void OnDestroy()
	{
		_Name = null;
		_Level = null;
		_Coin = null;
		_Rank = null;
	}

}
