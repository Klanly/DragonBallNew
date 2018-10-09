using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Festival rank item.
/// </summary>
public class FestivalRankItem : RUIMonoBehaviour,IItem {
	
	public UILabel lblRank;
	public UILabel lblName;
	public UILabel lblScore;
	public int iRank;
	int iScore;
	string SName;
    //int userId;

	SockFRankItem playerItem;
	/// <summary>
	/// Sets the value.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	public void SetItemValue(object obj){
		playerItem = obj as SockFRankItem;
		iScore = playerItem.point;
		SName = playerItem.userName;
		this.Refresh();
	}
	/// <summary>
	/// Returns the value.
	/// </summary>
	public object ReturnValue(){
		return iRank;
	}
	
	public void Refresh(){
		lblRank.text = iRank.ToString()+".";
		lblName.text = SName;
		lblScore.text = iScore.ToString();
	}
		

	
}
