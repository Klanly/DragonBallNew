using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDragonRechargeMain : RUIMonoBehaviour {

	public UITable _Table;

	public UIButton mVipBtn;

	private int _Maxcount;

	List<UIDragonRechargeCell> _RechargeCellList = new List<UIDragonRechargeCell>();

	//当前正在支付的cell
	private UIDragonRechargeCell m_curPayCell;

    public PayCountData payData =null;

	private RechargeDataMgr mDataMgr
	{
		get{
			return Core.Data.rechargeDataMgr;
		}
	}

	public void CreateCell(PayCountResponse res) 
	{
		List<RechargeData> Datalist = mDataMgr.GetShowDataList();

		Object prefab = PrefabLoader.loadFromPack ("LS/pbLSFanbeiCell");
		if (prefab != null)
		{
			for(int i=0; i<Datalist.Count; i++)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RechargeData m_data = Datalist[i];
				if(m_data != null)
				{
					obj.name = (i+1).ToString();
					RED.AddChild (obj, _Table.gameObject);
					UIDragonRechargeCell mm = obj.GetComponent<UIDragonRechargeCell> ();
					_RechargeCellList.Add(mm);
					if(res != null && res.data != null && res.data.buyCounts != null)
					{
						if(res.data.buyCounts.Count == 0)mm.OnShow(m_data);
						else
						{
							for(int j=0; j<res.data.buyCounts.Count; j++)
							{
								if(res.data.buyCounts[j] != null && res.data.buyCounts[j].Length == 2 && res.data.buyCounts[j][0] == Datalist[i].ID && res.data.buyCounts[j][1] != 0)
								{
									mm.OnShow(m_data, res.data.buyCounts[j][1]);
									break;
                                }  
                                if(j == res.data.buyCounts.Count-1)
                                {
                                    mm.OnShow(m_data);
                                }
                            }
						}

					}
					else
					{
						mm.OnShow(m_data);

					}

				}
			}
			
		}
		_Table.Reposition();
	}
	
	void Start()
	{
		CreateCell(Core.Data.rechargeDataMgr.res);
//		if(Core.Data.rechargeDataMgr._RechageStatus == 1)
//		{
//			mVipBtn.isEnabled = false;
//		}
//		else
//		{
//			mVipBtn.isEnabled = true;
//		}
	}


	public void Refresh()
	{
		for(int i=0; i<_RechargeCellList.Count; i++)
		{
			_RechargeCellList[i].dealloc();
        }
		_RechargeCellList.Clear();

		Core.Data.rechargeDataMgr.SendHttpRequest();
    }

    void Back_OnClick()
    {
		DeleteAll();
    }

	void DeleteAll()
	{
		for(int i=0; i<_RechargeCellList.Count; i++)
		{
			_RechargeCellList[i].dealloc();
		}
		Destroy(gameObject);
		UIDragonMallMgr.GetInstance()._UIDragonRechargeMain = null;
	}

    void OpenRecharge_OnClick()
    {
		DeleteAll();
        UIDragonMallMgr.GetInstance().SetRechargePanelActive();
       
    }

	void OpenVip_OnClick()
	{
        Destroy(gameObject);
		UIDragonMallMgr.GetInstance().SetVipLibao();
      
	}

	public void SetCurPayCell(UIDragonRechargeCell cell)
	{
		m_curPayCell = cell;
	}

	public void LoopQueryPayStatus()
	{
		m_curPayCell.LoopQueryPayStatus ();
	}



	public const string Stone_Purchase = "android.test.purchased";
	public const string Stone_Unavailable = "android.test.item_unavailable";
	public const string Stone_Refunded = "android.test.refunded";
	public const string Purchse_Cancel = "android.test.canceled";
	public const string Purchse_Purchased_100 = "android.test.purchased_100";
	public const string Purchse_Purchased_200 = "android.test.purchased_200";

	public void init() {
		GPaymnetManagerExample.init ();
	}
	void OnGUI(){
		if (GUI.Button (new Rect (350, 200, 100, 50), "INIT store ")) {
			init ();
		}
		if (GPaymnetManagerExample.isInited) {
			if (GUI.Button (new Rect (200, 200, 100, 50), "purchase  !!!! ")) {
				AndroidInAppPurchaseManager.instance.purchase (Stone_Purchase);
			}
			if (GUI.Button (new Rect (200, 260, 100, 50), " unavailable !!!")) {
				AndroidInAppPurchaseManager.instance.purchase (Stone_Unavailable);
			}
			if (GUI.Button (new Rect (200, 320, 100, 50), "refunded  !!!")) {
				AndroidInAppPurchaseManager.instance.purchase (Stone_Refunded);
			}
			if (GUI.Button (new Rect (200, 380, 100, 50), " cancel !!!!")) {
				AndroidInAppPurchaseManager.instance.purchase (Purchse_Cancel);
			}
			if (GUI.Button (new Rect (300, 320, 100, 50), "100  !!!")) {
				AndroidInAppPurchaseManager.instance.purchase (Purchse_Purchased_100);
			}
			if (GUI.Button (new Rect (300, 380, 100, 50), " 200 !!!!")) {
				AndroidInAppPurchaseManager.instance.purchase (Purchse_Purchased_200);
			}

		}
	}



}
