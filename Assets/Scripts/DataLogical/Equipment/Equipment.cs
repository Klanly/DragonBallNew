using System;

public enum SlotOrGemsColor {
	Default_No = 0x0,
	Red = 0x1,
	Blue = 0x2,
	Yellow = 0x3,
}


/// <summary>
/// Equipment.装备
/// </summary>
public class Equipment {

	public EquipInfo RtEquip;
	public EquipData ConfigEquip;
	public bool equipped;
	public bool isNew;

	public Equipment () { }

	public Equipment (EquipInfo rt, EquipData config, GemsManager gemsMana) { 
		Utils.Assert(rt == null, "Invalidate Equip Data from Server.");
		Utils.Assert(config == null, "Invalidate Equip Data from config file.");

		RtEquip = rt;
		ConfigEquip = config;
		equipped = false;
		isNew = true;

		if(RtEquip.slot != null) {
			foreach(EquipSlot es in RtEquip.slot) {
				if(es != null) {
					es.mGem = gemsMana.equipGems(es.id);
				}
			}
		}

	}

	#region operation
	/// <summary>
	/// Gets the type of the equipment. Attack or Defend
	/// </summary>
	/// <value>The type of the equipment.</value>
	public short EquipmentType {
		get {
			if(ConfigEquip != null ) 
				return ConfigEquip.type;
			else 
				return -1;
		}
	}

	//Maybe no attack value
	public int getAttack {
		get {
			float attack = 0;
			if(RtEquip != null && ConfigEquip != null) 
			{
				attack = ConfigEquip.atk + (RtEquip.lv - 1) * ConfigEquip.atkGrowth;
			
				//if Gems Color equal with Slot Color, add more attack
				if(RtEquip.slot != null && RtEquip.slot.Length > 0)
				{
					bool same = true;
					foreach(EquipSlot es in RtEquip.slot) 
					{
						if (es != null && es.mGem != null)
						{
							GemData config = es.mGem.configData;
							if (config != null)
							{
								SlotOrGemsColor gemsColor = config.getColor;
								attack += config.atk;

								SlotOrGemsColor slotColor = es.eqColor;
								if (slotColor != gemsColor)
								{
									same = false;
								}
							}
						}
						else
						{
							same = false;
						}
					}

					if(!ConfigEquip.effect.IsNullOrEmpty() && same)
						attack += ConfigEquip.effect[EquipData.Effect_Attack_Pos];
				}

			} 
			return MathHelper.MidpointRounding(attack);
		}
	}

	//Maybe no defend value
	public int getDefend {
		get {
			float defend = 0;
			if(RtEquip != null && ConfigEquip != null) {
				defend = ConfigEquip.def + (RtEquip.lv -1) * ConfigEquip.defGrowth;
			}

//			RED.LogWarning("equip base def ####################   :: " + defend);

			bool bActive = true;
			//if Gems Color equal with Slot Color, add more defend
			if(RtEquip.slot != null)
			{
				foreach(EquipSlot es in RtEquip.slot)
				{
					if(es != null && es.mGem != null) 
					{
						GemData config = es.mGem.configData;
						if(config != null)
						{
							SlotOrGemsColor gemsColor = config.getColor;
							defend += config.def;

//							RED.LogWarning("gem def:: $$$$$$$$$$$ :: " + defend  +  "   :   " + config.def);

							SlotOrGemsColor slotColor = es.eqColor;
							if(slotColor != gemsColor)
							{
								bActive = false;
							}
						}
						else
							bActive = false;
					}
					else
						bActive = false;
				}

			}

			if(bActive && !ConfigEquip.effect.IsNullOrEmpty())
			{
				defend += ConfigEquip.effect[EquipData.Effect_Defend_Pos];
//				RED.LogWarning("active def ::   " + ConfigEquip.effect[EquipData.Effect_Defend_Pos] + " : " + defend);
			}

			return MathHelper.MidpointRounding(defend);
		}
	}

	public int feedExp {
		get {
			if(ConfigEquip != null)
				return ConfigEquip.exp;
			else 
				return 0;
		}
	}

	public string name {
		get {
			if(ConfigEquip != null)
				return ConfigEquip.name;
			else
				return string.Empty;
		}
	}

	public int Num {
		get {
			if(RtEquip != null)
				return RtEquip.num;
			else
				return 0;
		}
	}

	public int ID {
		get {
			if(RtEquip != null)
				return RtEquip.id;
			else
				return 0;
		}
	}

	#endregion
}

