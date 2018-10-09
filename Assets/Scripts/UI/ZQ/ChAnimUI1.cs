using UnityEngine;
using System.Collections;

public class ChAnimUI1 : MonoBehaviour 
{	
	public GameObject[] m_Anims;					
	private readonly float[] INVOKE_TIME = { 0.0f, 0.8f, 1.4f, 2.0f, 2.7f, 3.3f, 3.7f, 5.0f};		 //下次动画播放时间
	private readonly float[] ANIM_TIME = { 0.6f, 0.4f, 0.4f,  0.5f, 0.2f, 0.2f, 0.6f, 0.2f };		//动画播放时间
	private Vector3[] m_animTo;
	private int m_nStep;
	public System.Action OnFinished;

	public static ChAnimUI1 OpenUI(GameObject objParent)
	{
		Object prefab = PrefabLoader.loadFromPack ("ZQ/ChAnimUI1");
		ChAnimUI1 anim = null;
		if (prefab != null) 
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			if (obj != null) 
			{
				RED.AddChild (obj, objParent);
			}
			anim = obj.GetComponent<ChAnimUI1>();
//			if (anim != null)
//				anim.m_callBack = callback;

		}
		return anim;
	}

	void Start()
	{
		m_animTo = new Vector3[7];
		m_animTo [0] = new Vector3 (-560, 0, 0);
		m_animTo [1] = new Vector3 (-110, 178, 0);
		m_animTo [2] = new Vector3 (-110, -30 ,0);
		m_animTo [3] = new Vector3 (-136, -210, 0); 
		m_animTo [4] = new Vector3 (334, 90, 0);
		m_animTo [5] = new Vector3 (1, 1, 1);
		m_animTo [6] = new Vector3 (1, 1, 1);

		for (int i = 0; i < m_Anims.Length; i++)
		{
			string strName = "PlayPosAnim";
			if (i >= 5 && i <= 6) 
			{
				strName = "PlayScaleAnim";
			}

			StartCoroutine (strName, m_nStep);
			m_nStep++;
		}

		Invoke ("HideAllUI", INVOKE_TIME [m_nStep]);
		Invoke("DestyUI", 5.8f);
	}

	IEnumerator PlayPosAnim(int step)
	{
		yield return new WaitForSeconds (INVOKE_TIME [step]);
		TweenPos (step);
	}

	IEnumerator PlayScaleAnim(int step)
	{
		yield return new WaitForSeconds (INVOKE_TIME [step]);
		TweenScale (step);
	}

	void TweenPos(int step)
	{
		RED.LogWarning (step.ToString ());
		MiniItween.MoveTo (m_Anims [step], m_animTo [step], ANIM_TIME [step]);
		TweenColor.Begin (m_Anims [step], ANIM_TIME [step], Color.white);
	}

	void TweenScale(int step)
	{
		RED.SetActive (true, m_Anims [step]);
		RED.LogWarning (step.ToString ());
		MiniItween.ScaleTo (m_Anims [step], m_animTo [step], ANIM_TIME [step]);
		TweenColor.Begin (m_Anims [step], ANIM_TIME [step], Color.white);
	}
		
	void HideAllUI()
	{
		for (int i = 0; i < m_Anims.Length; i++) 
		{
			TweenColor.Begin (m_Anims [i], ANIM_TIME [m_nStep], new Color(1.0f, 1.0f, 1.0f, 0.0f));
		}
	}

	void DestyUI()
	{
		if (OnFinished != null)
			OnFinished ();

		Destroy (this.gameObject);
	}
}
