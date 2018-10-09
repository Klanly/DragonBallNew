using UnityEngine;
using System.Collections;
using System;
using UObject = UnityEngine.Object;

public class SQYCharatorView : RUIMonoBehaviour {
	
	public Action<GameObject,Vector2> OnDrag;
	public Action<GameObject,bool> OnPress;
	public Action<GameObject> OnClick;
	
    public UISprite sp_head;
	
	public UILabel lab_Index;
	
	public UISprite sp_Attribut;
	
	public UISprite sp_Star;
	
	
	
	
	[HideInInspector]
	public int mIndex;
	[HideInInspector]
	public bool _needUnLock = false;
	
	Monster _monster = null;
	public Monster curMonster{
		set{
			_monster = value;
			freshCharatorView(_monster);
		}
		get{
			return _monster;
		}
	}
        

	void Start () {

		UIEventListener.Get(gameObject).onDrag +=this.On_Drag;
		UIEventListener.Get(gameObject).onPress +=this.On_Press;
		UIEventListener.Get(gameObject).onClick +=this.On_Click;
	}
 
	void freshCharatorView(Monster mt)
	{
		if(mt == null)
		{
			lab_Index.text ="";
			sp_Attribut.enabled = false;
			sp_Star.enabled = false;
		}
		else{
			sp_Star.enabled = true;
			sp_Attribut.enabled = true;
			sp_head.spriteName = mt.num.ToString();
			sp_head.MakePixelPerfect();
			lab_Index.text = "Lv" + mt.RTData.curLevel.ToString();
			sp_Attribut.spriteName = "Attribute_"+((int)mt.RTData.Attribute).ToString();

			sp_Star.spriteName = "star"+mt.Star.ToString();
			
		}
	}
	public void charatorNeedUnLock(bool needul)
	{
		_needUnLock = needul;
		sp_head.enabled = true;
		if(_monster == null)
		{
		
			lab_Index.text ="";
			sp_Attribut.enabled = false;
			sp_Star.enabled = false;
			if(needul){
				sp_head.spriteName = "head0";
			}else{
				sp_head.spriteName = "headAdd";
			}
			sp_head.MakePixelPerfect();
		}
		
	}
	
	void On_Drag(GameObject go,Vector2 vt)
	{
		if(OnDrag !=null){
			OnDrag(go,vt);
		}	
	}
	void On_Press(GameObject go,bool press)
	{
		if(OnPress != null)
		{
			OnPress(go,press);
		}
	}
	void On_Click(GameObject go)
	{
		if(OnClick != null)
		{
			OnClick(go);
		}
	}
	
	
	
	public static SQYCharatorView CreateCharatorView()
	{
		UObject obj = PrefabLoader.loadFromPack("SQY/pbSQYCharatorView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYCharatorView cv = go.GetComponent<SQYCharatorView>();
			obj = null;
			return cv;
		}
		return null;
	}
	
	
	
	
	
}
