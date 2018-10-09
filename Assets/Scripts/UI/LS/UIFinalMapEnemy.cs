using UnityEngine;
using System.Collections;

public class UIFinalMapEnemy : MonoBehaviour {

	public UISprite _Head;
	public UILabel _Easy;
	public UILabel _SelfNum;
	public UILabel _EnemyNum;
	public UILabel _StarTitle;
	public UILabel _Satr;

	public void OnShow(int index)
	{
		switch(index)
		{
			case 1:
				_Easy.text = Core.Data.stringManager.getString(20082);
				_Satr.text = "3";
				break;
			case 2:
				_Easy.text = Core.Data.stringManager.getString(20083);
				_Satr.text = "2";
				break;
			case 3:
				_Easy.text = Core.Data.stringManager.getString(20084);
				_Satr.text = "1";
				break;
		}
		_StarTitle.text = Core.Data.stringManager.getString(25022);

	}
}
