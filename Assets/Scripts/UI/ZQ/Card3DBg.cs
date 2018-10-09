using UnityEngine;
using System.Collections;

public class Card3DBg : MonoBehaviour 
{
	public GameObject m_Frame;
	public GameObject m_bg;
	public GameObject m_btm;

	public void SetMonStar(int star)
	{
		m_Frame.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardFrameTexture (star);
		m_bg.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBgTexture (star);
		m_btm.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBtmTexture (star);
	}
}
