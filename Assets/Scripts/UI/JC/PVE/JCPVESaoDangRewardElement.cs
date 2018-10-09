using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVESaoDangRewardElement : MonoBehaviour {

	
	public List<TweenScale> List_Scales = new List<TweenScale>();
	
	public float AnimationDuration = 0.3f;
	
	private int RewardCount = 0;
	
	public System.Action onFinished;
	
	public UILabel Lab_Title;
	
	public UILabel Lab_AddExp;
	
	public UILabel Lab_AddCoin;
	
	public List<RewardCell> list_cells = new List<RewardCell>();
	
	//private BattleSequence data = null;
	
	void Start () 
	{
	    for(int i=0; i< List_Scales.Count; i++)
		{
			List_Scales[i].duration = AnimationDuration;
			//List_Scales[i].onFinished.Add(new EventDelegate(OnFinished)   );
		}
	}
	
	
	int AnimationIndex = 0;
	void OnFinished()
	{
		AnimationIndex++;
		if(AnimationIndex < RewardCount+1)
		{
		     List_Scales[AnimationIndex].PlayForward();
			 Invoke("OnFinished",AnimationDuration);
		}
		else
		{
			if(onFinished != null)
			{
				onFinished();
			}
		}
	}
	
	
	public JCPVESaoDangRewardElement Play()
	{
		if( !gameObject.activeSelf ) gameObject.SetActive(true);
		List_Scales[0].PlayForward();
		Invoke("OnFinished",AnimationDuration);
		return this;
	}

	public void SetData(BattleSequence data,int index)
	{
		ItemOfReward[] rewards = null;
		if(data.reward != null)
		{
		    rewards = data.reward.p;
		}
		
		
		if(rewards != null )
		{
			RewardCount = rewards.Length;
			for(int i = 0;i<rewards.Length;i++)
			{
				Core.Data.itemManager.AddRewardToBag(rewards[i]);
				list_cells[i].InitUI(rewards[i]);
			}
		}
		
		
		Lab_Title.text = Core.Data.stringManager.getString(9080).Replace("{}",(index+1).ToString());
		Lab_AddExp.text = "+"+data.reward.bep.ToString();
		Lab_AddCoin.text = "+"+data.reward.bco.ToString();
	}
	
	public void SetData(ItemOfReward[] rewards,string title)
	{
		if(rewards != null )
		{
			RewardCount = rewards.Length;
			for(int i = 0;i<rewards.Length;i++)
			{
				Core.Data.itemManager.AddRewardToBag(rewards[i]);
				list_cells[i].InitUI(rewards[i]);
			}
		}
		
	}
}
