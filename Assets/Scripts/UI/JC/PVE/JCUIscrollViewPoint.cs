using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCUIscrollViewPoint : MonoBehaviour {

	public UISprite Spr_Point;
	public float CellWidth = 20f;
	
    private List<UISprite> list_points = new List<UISprite>();
	private int PointNum;
	
	void Start () 
	{

	}
	
	//至少一个点
	public void Refresh(int PointNum)
	{
		this.PointNum = PointNum;
		if(list_points.Count == 0) list_points.Add(Spr_Point);
		int cha = list_points.Count - PointNum;
		if(cha > 0)
		{
			for(int i= 0; i<cha; i++)
			{
				list_points[list_points.Count - 1- i].gameObject.SetActive(false);
			}
		}
		else
		{
			int count = Mathf.Abs(cha);
			for(int i=0 ; i<count ;i++)
			{
				 GameObject g = Instantiate( Spr_Point.gameObject) as GameObject;
				 g.transform.parent = transform;
				 g.transform.localScale = Vector3.one;
				 g.name = (i+1).ToString();
				 list_points.Add(g.GetComponent<UISprite>());
			}
		}

		float starx = -((PointNum-1)*CellWidth)/2f;
		for(int i=0;i<PointNum;i++)
		{
			list_points[i].name = i.ToString();
			list_points[i].transform.localPosition = new Vector3(starx+i*CellWidth,0,0);
		}
	}
	
	public void SetBright(int index)
	{
		if(index >=0 && index< PointNum)
		{
			for(int i=0;i<PointNum;i++)
			{
				if(index == i)
				list_points[i].alpha = 1f;
					else
				list_points[i].alpha = 0.5f;
			}
		}
	}
}
