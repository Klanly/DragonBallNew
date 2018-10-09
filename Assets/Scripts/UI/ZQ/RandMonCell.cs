using UnityEngine;
using System.Collections;

public class RandMonCell : MonoBehaviour 
{
	public UISprite m_spHead;
	public UISprite m_spAttr;
	public UISprite m_spBg;

	public void InitMonster(Monster mon)
	{
		AtlasMgr.mInstance.SetHeadSprite (m_spHead, mon.config.ID.ToString ());

		if (mon.RTData != null)
		{
			if (mon.num != 19999 && mon.num != 19998)
			{
				m_spAttr.spriteName = "Attribute_" + ((int)(mon.RTData.Attribute)).ToString ();
			}
			else
			{
				m_spAttr.spriteName = "common-1055";
			}
			m_spAttr.MakePixelPerfect ();

			m_spBg.spriteName = "star" + mon.RTData.m_nAttr.ToString ();
		}
	}
}
