using UnityEngine;
using System.Collections;

public class JCPVEDifficultyElement : MonoBehaviour {

	// Use this for initialization
	
	private string[] luoma = new string[]{"I","II","III","IV","V","VI"};
	private string[] daxie = new string[]{"一","二","三","四","五","六","七","八","九","十"};
	//private int[] jiesuo = new int[]{10,20,30,40,50,60};
	public UILabel Lab_name;
	public UISprite Spr_Suo;
	public UISprite Spr_head;
	private int UnLockLevel = 0;
	private int TeamSize = 0;
	void Start () 
	{
		int index = int.Parse(name);
		Spr_head.spriteName = "nandu 0"+(index+1).ToString();
		Spr_head.MakePixelPerfect();
		
	}
	
	//是否解锁{"ID":7320,"txt":"{0}级解锁"}{"ID":9082,"txt":"难度"}{"ID":9083,"txt":"人战"}
	public bool isLock
	{
		set
		{
			StringManager sm = Core.Data.stringManager;
			int index = int.Parse(name);
			if(!value)
			{
				Lab_name.text = "[ffff00]"+sm.getString(9082)+luoma[index]+"[-]  "+daxie[this.TeamSize-1]+sm.getString(9083);
				Spr_head.color = new Color(1f,1f,1f,1f);
			}
			else
			{
				Lab_name.text = sm.getString(7320).Replace("{0}",UnLockLevel.ToString());
				Spr_head.color = new Color(0,0,0,1f);
			}
			Spr_Suo.gameObject.SetActive(value);
			GetComponent<BoxCollider>().enabled = !value;
		}
	}
	
	//设置关卡解锁等级
	public void SetUnlockLevel(int level)
	{
		UnLockLevel = level;
		isLock = Core.Data.playerManager.Lv < UnLockLevel ;	
	}
	
	//设置队伍人数
	public void SetTeamSize(int teamSize)
	{
		this.TeamSize = teamSize;
	}
}
