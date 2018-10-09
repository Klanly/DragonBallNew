using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentTableManager : MonoBehaviour 
{
	public EquipmentShowCellScript m_atkEquip;
	public EquipmentShowCellScript m_defEquip;
	
	public static EquipmentTableManager Instance;


	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		m_atkEquip.Index = 0;
		m_defEquip.Index = 1;
	}
	
	/// 刷新当前位置的装备
	/// </summary>
	/// <param name="index">Index.</param>
	public void RefreshEquipment(int index)
	{
		MonsterTeam _MyCurrentTeam = Core.Data.playerManager.RTData.curTeam;
		
        Equipment equip;
      
        equip = _MyCurrentTeam.getEquip(index, (short)0);
        m_atkEquip.Show(equip, 0);

        equip = _MyCurrentTeam.getEquip(index, (short)1);
        m_defEquip.Show(equip, 1);
    }
}
