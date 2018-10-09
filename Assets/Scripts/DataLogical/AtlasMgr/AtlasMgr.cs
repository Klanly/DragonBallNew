using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AtlasMgr : MonoBehaviour 
{
	private static AtlasMgr _mInstance;

	public static AtlasMgr mInstance
	{
		get
		{
			return _mInstance;
		}
	}

	public const string HEAD_LOCK = "head0";
	public const string HEAD_ADD = "headAdd";

	//num 10100~10217 headAtlas【0】，
	//10217以后在headAtlas【1】
	public UIAtlas[] headAtlas;

	//技能atlas
	public UIAtlas skillAtlas;

	//装备atlas
	public UIAtlas equipAtlas;

	//道具atlas
	public UIAtlas itemAtlas;
    //通用中的 主要是钻石
    public UIAtlas commonAtlas;
	
//	public UIAtlas mallAtlas;

	//简体字体
	public Font normalFont;

	//卡牌贴图
	private Texture[] m_cardFrame;
	private Texture[] m_cardBg;
	private Texture[] m_cardBtm;

	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		m_cardBg = new Texture[6];
		m_cardBtm = new Texture[6];
		m_cardFrame = new Texture[6];
	}


	/// <summary>
	/// 设置头像ID
	/// </summary>
	/// <param name="spHead">头像sprite.</param>
	/// <param name="headNum">头像num号.</param>
	public void SetHeadSprite(UISprite spHead, string strHeadID)
	{
		if (spHead == null || string.IsNullOrEmpty(strHeadID))
		{
			RED.LogWarning ("SetHeadSprite:: param is error!");
			return;
		}

		int num = 0;
		if(strHeadID != HEAD_LOCK && strHeadID != HEAD_ADD)
		{
			int headNum = int.Parse(strHeadID);
			num = (headNum < 10217 ? 0 : 1);
		}

		spHead.atlas = headAtlas [num];
		spHead.spriteName = strHeadID;
	}
		
	
	Dictionary<string,UIAtlas> FB2Atlas = new Dictionary<string, UIAtlas>();
	//List<string> FB2AtlasNames = new List<string>();
	List<string> Loadinglist = new List<string>();

	public UnityEngine.AssetBundle  FB2_assertBundle{get;set;}
	
	public void SetFB2CityImage(UISprite sprite,int cityID)
	{
		int atlasID = (cityID - 50101)/1;
		string atlasName = "FB2_"+atlasID.ToString();	
		
		
		if(FB2Atlas.ContainsKey(atlasName))
		{
			sprite.atlas = FB2Atlas[atlasName];
			sprite.spriteName = cityID.ToString();
			sprite.MakePixelPerfect();
		}
		else
		{
			 //如果没有任务就添加一个加载任务
		    if(!Loadinglist.Contains(atlasName))
			{
				Loadinglist.Add(atlasName);
				StartCoroutine( loadAtlas(atlasName,sprite,cityID) );
			}
			else
			   StartCoroutine(SetFB2CityImageThread(atlasName, sprite, cityID));
		}
		
	}
	
	public IEnumerator SetFB2CityImageThread(string AtlasName,UISprite sprite,int cityID)
	{
	    //如果加载列表中有任务
		while(Loadinglist.Contains(AtlasName))
		{
			yield return new WaitForEndOfFrame();
		}			
	    if(FB2Atlas.ContainsKey(AtlasName))
		{
			sprite.atlas = FB2Atlas[AtlasName];
			sprite.spriteName = cityID.ToString();
			sprite.MakePixelPerfect();
		}
		else
		{
			StartCoroutine( loadAtlas(AtlasName,sprite,cityID) );		   
		}
	    
	}
	
	
	IEnumerator loadAtlas (string AtlasName,UISprite sprite,int cityID)
	{
		string url = @"http://114.215.183.29/down/gameconf/1/";
		string filename = AtlasName+".assetbundle";
		url=url+filename;
		
		WWW www = WWW.LoadFromCacheOrDownload(url,Core.Data.sourceManager.getLocalNum(filename));
		yield return www;

		GameObject atlasObj= www.assetBundle.Load(AtlasName,typeof(GameObject)) as GameObject;
		
		www.assetBundle.Unload(false);
		//Debug.Log("filename="+filename);
		
		//添加到资源表中
		FB2Atlas.Add(AtlasName,atlasObj.GetComponent<UIAtlas>());
		//删除加载任务
		if(Loadinglist.Contains(AtlasName))Loadinglist.Remove(AtlasName);

		sprite.atlas = FB2Atlas[AtlasName];
		sprite.spriteName = cityID.ToString();
		sprite.MakePixelPerfect();
		www.Dispose();
	}

	//得到卡牌边框贴图
	public Texture Get3DCardFrameTexture(int Star)
	{
		if (Star > 0 && Star <= 6)
		{
			Texture pic = m_cardFrame [Star - 1];
			if (pic == null)
			{
				string strPath = "Card/frame" + Star.ToString ();
				pic = PrefabLoader.loadFromUnPack (strPath,false) as Texture;
			}

			return pic;
		}
		RED.LogWarning ("star is wrong   " + Star);
		return null;
	}

	//得到卡牌背景贴图
	public Texture Get3DCardBgTexture(int Star)
	{
		if (Star > 0 && Star <= 6)
		{
			Texture pic = m_cardBg [Star - 1];
			if (pic == null)
			{
				string strPath = "Card/bg" + Star.ToString ();
				pic = PrefabLoader.loadFromUnPack (strPath,false) as Texture;
			}

			return pic;
		}
		RED.LogWarning ("star is wrong   " + Star);
		return null;
	}
		
	//得到卡牌底边贴图
	public Texture Get3DCardBtmTexture(int Star)
	{
		if (Star > 0 && Star <= 6)
		{
			Texture pic = m_cardBtm [Star - 1];
			if (pic == null)
			{
				string strPath = "Card/btm" + Star.ToString ();
				pic = PrefabLoader.loadFromUnPack (strPath,false) as Texture;
			}

			return pic;
		}
		RED.LogWarning ("star is wrong   " + Star);
		return null;
	}
}
