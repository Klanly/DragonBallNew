using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GetRewardSucUI : MonoBehaviour
{
    public UILabel m_txtTitle;
    public UIButton m_btnOK;
    public TweenScale m_scale;
    
	public RewardCell m_rewardCell;

	private ItemOfReward[] m_data;
	private ItemOfReward[] m_tempData;
	private int m_nIndex = 0;
    private string m_strTitle;
    private static GetRewardSucUI _mInstance;

    private bool isAnalysis = true;


	public delegate void OKCallback();
	private OKCallback m_callBack;

	public static GetRewardSucUI mInstace
	{
		get
		{
			return _mInstance;
		}
	}

	public static void OpenUI(ItemOfReward[] data, string strTitle,bool isAnaly= true, OKCallback callback = null)
    {
        if (_mInstance == null)
        {
            Object prefab = PrefabLoader.loadFromPack("ZQ/GetRewardSucUI");
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab) as GameObject;

                _mInstance = obj.GetComponent<GetRewardSucUI>();
				if(DBUIController.mDBUIInstance._TopRoot != null)
				{
                    RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				}
				else
				{
					_mInstance.m_btnOK.gameObject.layer = 8;  //UI Layer
					RED.AddChild(obj, BanBattleManager.Instance.go_uiPanel);
				}

                RED.TweenShowDialog(obj);
				_mInstance.m_tempData = data;
                _mInstance.m_strTitle = strTitle;
                _mInstance.isAnalysis = isAnaly;
            }
        }
        else
        {
            _mInstance.SetShow(true);
			_mInstance.m_tempData = data;
            _mInstance.m_strTitle = strTitle;
            _mInstance.InitUI();
            _mInstance.isAnalysis = isAnaly;
        }
		_mInstance.m_callBack = callback;
    }

    void Awake()
    {
        _mInstance = this;
    }

    void Start()
    {
        AnalysisReward ();
       
        InitUI();
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Dragon, null);
    }

    public void InitUI()
    {
		if (m_data == null || m_data.Length == 0)
        {
            return;
        }

		m_txtTitle.text = m_strTitle;
		ShowNext ();
    }

	void AnalysisReward()
    {
        if (isAnalysis == true) {

            Dictionary<int, ItemOfReward> dicTemp = new Dictionary<int, ItemOfReward> ();
            for (int i = 0; i < m_tempData.Length; i++) {
                if (dicTemp.ContainsKey (m_tempData [i].pid)) {
                    dicTemp [m_tempData [i].pid].num++;
                } else {
                    dicTemp.Add (m_tempData [i].pid, m_tempData [i]);
                }
            }

            if (m_data == null) {
                m_data = new ItemOfReward[dicTemp.Count];
            }
            m_data.safeFree ();
            int count = 0;
            foreach (KeyValuePair<int, ItemOfReward> itor in dicTemp) {
                m_data [count] = itor.Value;
                count++;
            }
        } else {
            if (m_data == null) {
                m_data = m_tempData;
            }

           
        }
	}

    public void SetShow(bool bShow)
    {
        if (bShow)
        {
            RED.SetActive(true, gameObject);
            m_scale.from = Vector3.one * 0.1f;
            m_scale.to = Vector3.one;
            m_scale.duration = 0.4f;
            m_scale.onFinished.Clear();
            m_scale.PlayForward();
        }
        else
        {
            m_scale.from = Vector3.one;
            m_scale.to = Vector3.one * 0.1f;
            m_scale.duration = 0.4f;
            m_scale.onFinished.Clear();
            m_scale.onFinished.Add(new EventDelegate(this, "HideUI"));
            m_scale.PlayForward();
        }
    }


	public void OnBtnOK()
	{
		if (m_nIndex < m_data.Length)
		{
			ShowNext ();
		}
		else
		{
			HideUI ();
		}
	}


	public void HideUI()
    {
        if(Core.Data.guideManger.isGuiding)
        	Core.Data.guideManger.AutoRUN ();
		if(m_callBack != null)
		{
			m_callBack();
		}
        Destroy(this.gameObject);
        _mInstance = null;
    }

	void ShowNext()
	{
		if (m_nIndex >= m_data.Length)
		{
			return;
		}

		ItemOfReward reward = m_data [m_nIndex];

		m_rewardCell.InitUI (reward);
		m_nIndex++;
		m_rewardCell.transform.localScale = Vector3.one * 0.1f;
		TweenScale.Begin (m_rewardCell.gameObject, 0.2f, Vector3.one);
	}

}
