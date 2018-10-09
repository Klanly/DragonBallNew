using UnityEngine;
using System.Collections;

public class PveRewardTipUI : MonoBehaviour 
{
	public UILabel m_txtTitle;
	public UIGrid m_grid;
	public UISprite m_spBg;
	public TweenScale m_root;

	private NewFloorData m_flrData;

	private Object m_prefab;
	private ItemOfReward[] m_arryReward;
	public void SetReward(ItemOfReward[] reward, int floorID, Transform obj)
	{
		ShowUI ();
		while (m_grid.transform.childCount > 0)
		{
			Transform child = m_grid.transform.GetChild(0);
			child.parent = null;
			Destroy(child.gameObject);
			child = null;
		}

		m_arryReward = reward;
		this.transform.parent = obj;
		this.transform.localScale = Vector3.one;
		this.transform.localPosition = Vector3.zero;	
		if (m_prefab == null)
		{
			m_prefab = PrefabLoader.loadFromPack ("ZQ/PveRewardCell");
		}
		m_flrData = Core.Data.newDungeonsManager.GetFloorConfigData (floorID);

		ShowNormalUI ();
	}
		
	void ShowNormalUI()
	{
		int textID = m_flrData.isBoss > 0 ? 5193 : 9104;
		m_txtTitle.text = Core.Data.stringManager.getString (textID);
		for (int i = 0; i < m_arryReward.Length; i++)
		{
			GameObject obj = Instantiate (m_prefab) as GameObject;
			RED.AddChild (obj, m_grid.gameObject);
			obj.name = i.ToString ();
			obj.transform.localScale = Vector3.one * 0.6f;
			JCPVEPlotDesMonsterHead mon = obj.GetComponent<JCPVEPlotDesMonsterHead>();
			BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
			if(boxCollider != null)
				Destroy( boxCollider );
			mon.SetData (m_arryReward [i]);
		}
		m_grid.Reposition ();

		m_spBg.width = 10 + 80 * m_arryReward.Length;
	}
		
	public void HideUI()
	{
		m_root.delay = 0;
		m_root.duration = 0.25f;
		m_root.from =  Vector3.one;
		m_root.to = new Vector3(0.01f,0.01f,0.01f);
		m_root.onFinished.Clear();
		m_root.onFinished.Add(new EventDelegate(this,"HidePanel"));
		m_root.ResetToBeginning();
		m_root.PlayForward();
	}

	void HidePanel()
	{
		this.transform.parent = JCPVEPlotController.Instance.transform;
		RED.SetActive (false, this.gameObject);
	}

	public void ShowUI()
	{
		RED.SetActive (true, this.gameObject);
		m_root.delay = 0;
		m_root.duration = 0.25f;
		m_root.from =  new Vector3(0.01f,0.01f,0.01f);
		m_root.to = Vector3.one;
		m_root.onFinished.Clear();
		m_root.ResetToBeginning();
		m_root.PlayForward();
	}
}
