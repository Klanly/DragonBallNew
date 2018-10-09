using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GuideMask : MonoBehaviour {

	public Transform pBox;
	public UISprite uiBox;
	public Transform pCylinder;
	public GameObject MultiSelect;
	public BoxCollider boxCollider;
	public TweenScale boxTween;
	
	List<BoxCollider> list_collider = new List<BoxCollider>();
	
	
	
//	void Start()
//	{
//		List<Rect> rects = new List<Rect>(){new Rect(0,0,100,100),new Rect(100,0,100,100),new Rect(200,0,100,100),new Rect(300,0,100,100)};
//		SetScale(1,1,5f,1f,rects.ToArray());
//	}
	
	
	
	//设置缩放比例   type 0:圆形缩放
	public void SetScale(int Type,int RenderMask, float ScaleX, float ScaleY =1f ,List<int[]> rect = null)
	{
		bool isRender = RenderMask == 1 ? true : false;
		
		boxCollider.size = new Vector3(100f*ScaleX,100f*ScaleY,1f);
		
		if(rect != null && rect.Count > 0)
		{
			//如果是多选框
			boxCollider.enabled = false;		
			MultiSelect.gameObject.SetActive(true);
			int cha = rect.Count - list_collider.Count;			
			if(cha > 0)
			{
				//不够补
				for(int i= list_collider.Count;i<rect.Count;i++)
				{
					GameObject tempobj = new GameObject();
					RED.AddChild(tempobj,MultiSelect);
					UIButtonMessage message = tempobj.AddComponent<UIButtonMessage>();		
					if(message != null)
					{
						message.target = transform.parent.parent.gameObject;
						message.functionName = "SpecifiedAreaClick";
					}
					
					list_collider.Add( tempobj.AddComponent<BoxCollider>() );
				}
			}
			//多选框赋值
			for(int i=0;i<list_collider.Count;i++)
			{
				if(i < rect.Count)
				{
					list_collider[i].gameObject.name = i.ToString();
					if(!list_collider[i].gameObject.activeSelf) list_collider[i].gameObject.SetActive(true);
					list_collider[i].transform.localPosition = new Vector3( rect[i][0],rect[i][1],0);
					list_collider[i].size = new Vector3(rect[i][2],rect[i][3],0);
				}
				else
				{
					list_collider[i].gameObject.SetActive(false);
				}
			}		
		}
		else
		{
			//如果是单选框
			boxCollider.enabled = true;		
			MultiSelect.gameObject.SetActive(false);
			boxCollider.size = new Vector3(100f*ScaleX,100f*ScaleY,1f);
		}
		
		if(Type == 0)
		{
			if(!pCylinder.parent.gameObject.activeSelf)pCylinder.parent.gameObject.SetActive(true);
			if(pBox.parent.gameObject.activeSelf)pBox.parent.gameObject.SetActive(false);
			
			pCylinder.localScale = new Vector3(ScaleX,ScaleX,1f);
			pCylinder.GetComponent<MeshRenderer>().enabled = isRender;
		}
		else if(Type==1)
		{
			if(pCylinder.parent.gameObject.activeSelf)pCylinder.parent.gameObject.SetActive(false);
			if(!pBox.parent.gameObject.activeSelf)pBox.parent.gameObject.SetActive(true);

			pBox.localScale = new Vector3(ScaleX,ScaleY,1f);
			uiBox.width = (int) (92f*ScaleX + 25f);
			uiBox.height =(int) (92f*ScaleY +25f );
			boxTween.to = new Vector3(1f+0.05f/ScaleX,1+0.05f/ScaleY,1f);
			
			pBox.GetComponent<MeshRenderer>().enabled = isRender;
		}
	}
	
	//不渲染遮罩层，但是该层还是存在的，阻挡也是存在的
	public void HideMask()
	{
		pCylinder.GetComponent<MeshRenderer>().enabled = false;
		pBox.GetComponent<MeshRenderer>().enabled = false;
	}
	
}
