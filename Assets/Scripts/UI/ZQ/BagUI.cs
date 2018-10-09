using UnityEngine;
using System.Collections;

//
public enum BAG_TYPE
{
	QianLiXunLian,  
}

public class BagUI : MonoBehaviour 
{
	private static BagUI m_Instace;
	
	public UIGrid m_grid;
	
	public static BagUI OpenUI(BAG_TYPE bagType)
	{
		if(m_Instace == null)
		{
			Object prefab = PrefabLoader.loadFromPack("ZQ/BagUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				m_Instace = obj.AddComponent<BagUI>();
			}
		}
		else
		{

		}
		return m_Instace;
	}


	public void InitData()
	{
		for(int i = 5; i > 0; i++)
		{

		}
	}

}
