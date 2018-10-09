using System;
using System.Collections.Generic;

public class MonsterTeam {

	//队伍里面的信息是否更改过
	private short validate = 0;

	private const short GS_POS = 1;
	private const short ATT_POS = 2;
	private const short DEF_POS = 4;
	private const short HP_POS = 8;
	private const short MEMBER_COUNT_POS = 16;
	private const short EQUIP_COUNT_POS = 32;

	public const short MAIN_TEAM_ID = 1;
	public const short SUB_TEAM_ID = 2;

	//当前队伍的战斗力
	private int cachedTeamGs;
	public int teamGs {
		get {
			if( (validate & GS_POS) == GS_POS)
				return cachedTeamGs;
			else {
				int tGs = 0;

				if(teamMember != null) {
					foreach(Monster mon in teamMember) {
						if(mon != null) {
							tGs += mon.BTData.Gs;
						}
					}
				}

				validate |= GS_POS;
				cachedTeamGs = tGs;
				return tGs;
			}
		}

		private set {  
			cachedTeamGs = value;
		}
	}
	//当前队伍的总攻击力
	private int cachedTeamAttack;
	public int teamAttack {
		get {
			if( (validate & ATT_POS) == ATT_POS)
			{
				return cachedTeamAttack;
			}
			else
			{
				int tAtt = 0;

                int count = teamMember.Count;
                for(int i = 0; i < count; ++ i) {
                    tAtt += MemberAttack(i);
                }

				validate |= ATT_POS;
				cachedTeamAttack = tAtt;

				return tAtt;
			}
		}
		private set {
			cachedTeamAttack = value;
		}
	}


	/// <summary>
	/// 获得指定位置武者的总攻击力
	/// </summary>
	/// <param name="listPos"> 武者在队伍中的位置</param>
	public int GetTeamMeberAtk(List<int> listPos)
	{
		if (listPos == null || listPos.Count == 0)
		{
			return 0;
		}

		int atk = 0;
		for (int i = 0; i < listPos.Count; i++)
		{
			atk += MemberAttack (listPos [i]);
		}
		return atk;
	}


	/// <summary>
	/// 获得指定位置武者的总防御力
	/// </summary>
	/// <param name="listPos"> 武者在队伍中的位置</param>
	public int GetTeamMeberDef(List<int> listPos)
	{
		if (listPos == null || listPos.Count == 0)
		{
			return 0;
		}

		int def = 0;
		for (int i = 0; i < listPos.Count; i++)
		{
			def += MemberDefend (listPos [i]);
		}
		return def;
	}

	//当前队伍的总防御力
	private int cachedTeamDefend;
	public int teamDefend {
		get {

			if( (validate & DEF_POS) == DEF_POS)
				return cachedTeamDefend;
			else {
				int tdef = 0;

                int count = teamMember.Count;
                for(int i = 0; i < count; ++ i) {
                    tdef += MemberDefend(i);
                }

				validate |= DEF_POS;
				cachedTeamDefend = tdef;

				return tdef;
			}
		}
		private set {
			cachedTeamDefend = value;
		}
	}

	//当前队伍的容量
	public int capacity {
		get; private set;
	}

	//装备的容量（EquipCapacity = Capacity * 2)
	public int EquipCapacity {
		get; private set;
	}

	public short TeamId {
		get; private set;
	}

	private int cachedValidateMem = 0;
	//当前队伍中队员的个数
	public int validateMember {
		get {
			if( (validate & MEMBER_COUNT_POS) == MEMBER_COUNT_POS) 
				return cachedValidateMem;
			else {
				int mem = 0;
				if(teamMember != null) {
					foreach(Monster mon in teamMember) {
						if(mon != null) 
							mem ++;
					}
				}

				validate |= MEMBER_COUNT_POS;
				cachedValidateMem = mem;
				return mem;
			}
		}
	}


	private int cachedvalidateEquip = 0;
	//当前队伍里面装备的数量
	public int validateEquipCount {
		get {
			if( (validate & EQUIP_COUNT_POS) == EQUIP_COUNT_POS) 
				return cachedvalidateEquip;
			else {
				int mem = 0;
				if(teamMember != null) {
					foreach(Equipment mon in equipMember) {
						if(mon != null) 
							mem ++;
					}
				}

				validate |= EQUIP_COUNT_POS;
				cachedvalidateEquip = mem;
				return mem;
			}
		}
	}

	public int GetValidEquipCount(int type)
	{
		int mem = 0;
		if(teamMember != null)
		{
			foreach(Equipment mon in equipMember)
			{
				if(mon != null && mon.ConfigEquip.type == type && equipInMyTeam(mon.RtEquip.id)) 
					mem ++;
			}
		}
		return mem;
	}

    //展示当前队员的攻击力（武者基础值+（武者等级-1）×武者等级成长 + 潜力培养值）×（缘1+缘2+……缘n）+装备基础值+（装备等级-1）×装备等级成长+宝石数值+装备宝石加成}×（神龙加减成+建筑加减成）
	public int MemberAttack (int pos) {
        float att = 0;

		Monster member = getMember(pos);
		if(member != null) {
            //（武者基础值+（武者等级-1）× 武者等级成长 + 潜力培养值
			att += member.getAttack;

			//计算缘分的加成
            float enFated = 1.0f;
			List<FateData> mFate = member.getMyFate(Core.Data.fateManager);
			if(mFate != null) {
				foreach(FateData fd in mFate) {
                    if(fd != null && fd.ImproveAttack && member.checkMyFate(fd, this, Core.Data.dragonManager.usedAoYiToList())) {
                        enFated += (fd.effect[FateData.EFFECT_SELF_ATTACK] * MathHelper.ONE_HUNDRED);
					}
				}
			}

            att = att * enFated;

			Equipment eqiup = getEquip( pos, EquipData.TYPE_ATTACK );

			if(eqiup != null) {
				att += eqiup.getAttack;
			}

			eqiup = getEquip(pos, EquipData.TYPE_DEFEND);
			if(eqiup != null) {
				att += eqiup.getAttack;
			}

		}

        float aoyiEnhanced = Core.Data.dragonManager.getAttAoYi();
        float buildingEnhanced = Core.Data.BuildingManager.GetBuildAtk();

        att = att * ( (aoyiEnhanced + buildingEnhanced) * MathHelper.ONE_HUNDRED + 1.0f);

        return MathHelper.MidpointRounding(att);
	}

	//展示当前队员的防御力（基础防御力 (宠物基础防御力 + 查克拉丸的防御力） + 装备提供的防御力（装备的基础防御力+宝石的防御力+宝石额外的颜色加成) ）
	public int MemberDefend (int pos) {
        float def = 0;

		Monster member = getMember(pos);
		if(member != null) {
			def += member.getDefend;

			//计算缘分的加成
            float enFated = 1.0f;
			List<FateData> mFate = member.getMyFate(Core.Data.fateManager);
			if(mFate != null) {
				foreach(FateData fd in mFate) {
                    if(fd != null && fd.ImproveDefend && member.checkMyFate(fd, this, Core.Data.dragonManager.usedAoYiToList())) {
                        enFated += (fd.effect[FateData.EFFECT_SELF_DEFEND] * MathHelper.ONE_HUNDRED);
					}
				}
			}

            def = def * enFated;

			Equipment eqiup = getEquip(pos, EquipData.TYPE_ATTACK);

			if(eqiup != null) {
				def += eqiup.getDefend;
			}

			eqiup = getEquip(pos, EquipData.TYPE_DEFEND);
			if(eqiup != null) {
				def += eqiup.getDefend;
			}
		}

        float aoyiEnhanced = Core.Data.dragonManager.getDefAoYi();
        float buildingEnhanced = Core.Data.BuildingManager.GetBuildDef();

        def = def * ((aoyiEnhanced + buildingEnhanced) * MathHelper.ONE_HUNDRED  + 1.0f);

        return MathHelper.MidpointRounding(def);
	}


	//队伍列表
	private List<Monster> teamMember = null;
	//装备列表
	private List<Equipment> equipMember = null;

    public List<Monster> TeamMember {
        get { return teamMember; }
    }

	public MonsterTeam() { }
	/// <summary>
	/// Initializes a new instance of the <see cref="MonsterTeam"/> class.
	/// 传入一个队伍容量的参数
	/// </summary>
	/// <param name="memberCapacity">Member capacity.</param>
	public MonsterTeam (short teamId, int memberCapacity) {
		teamGs = 0;
		validate = 0x0;

		capacity = memberCapacity;
		teamMember = new List<Monster>();
		for(int i = 0; i < capacity; ++ i) {
			teamMember.Add(null);
		}

		EquipCapacity = memberCapacity * 2;
		equipMember = new List<Equipment>();
		for(int i = 0; i < EquipCapacity; ++ i) {
			equipMember.Add(null);
		}

		TeamId = teamId;
	}

	//扩展队伍的容量上限
	//capacityMember参数为扩展到多少个
	public void extendTeamCapacity(int capacityMember) {
		int extend = capacityMember - capacity;
		if(extend > 0) {
			capacity = capacityMember;
			EquipCapacity = capacityMember * 2;

			for(int i = 0; i < extend; ++ i) 
				teamMember.Add(null);
			
			for(int i = 0; i < extend * 2; ++ i)
				equipMember.Add(null);

		} else if(extend == 0) {
			//Useless case
			//没什么要做的.
		} else {
			//让队伍的容量减少？不合理
			throw new DragonException("Reduce down team capacity is illegal.");
		}
	}

	#region Member Operation

    /// <summary>
    /// 设置升级标志位
    /// </summary>
    /// <returns><c>true</c>, if member was upgraded, <c>false</c> otherwise.</returns>
    public void upgradeMember () {
        validate = 0x0;
    }

	//潜力训练
    public void QianliXunlianMember() {
		validate = 0x0;
	}

	/// <summary>
	/// Sets the member.
	/// </summary>
	/// <returns><c>true</c>, if member was set, <c>false</c> otherwise.</returns>
	/// <param name="member">Member. Can be null.</param>
	/// <param name="pos">where to set new monster, zero-based</param>
	public bool setMember (Monster member, int pos) {
		validate = 0x0;
		bool success = pos < capacity && pos >= 0; //over capacity? YES = error occur

		Monster oldMon = null;
		if(success) 
		{
			if (teamMember [pos] != null)
			{
				oldMon = teamMember [pos];
				teamMember [pos].inTeam = false;
			}
			teamMember[pos] = member;
			if(member != null)
				member.inTeam = true;
		}

		if(oldMon != null)
		{
			foreach (MonsterTeam team in Core.Data.playerManager.RTData.myTeams)
			{
				if (team.TeamId != TeamId)
				{
					foreach (Monster mon in team.teamMember)
					{
						if (mon != null && mon.pid == oldMon.pid)
							oldMon.inTeam = true;
					}
				}
			}
		}
		return success;
	}



	/// <summary>
	/// Removes the member.
	/// </summary>
	/// <param name="pos">Position. Zero-Based</param>
	public bool removeMember(int pos) {
		validate = 0x0;
		//over capacity? YES = error occur
		bool success = pos < capacity && pos >= 0;
		if(success && teamMember[pos] != null) {
			teamMember[pos].inTeam = false;
			teamMember[pos] = null;
		}

		return success;
	}

	/// <summary>
	/// Gets the member. Can be Null.
	/// </summary>
	/// <returns>The member.</returns>
	/// <param name="pos">Position. Zero-Based</param>
	public Monster getMember (int pos) {
		if(pos < capacity && pos >= 0)
			return teamMember[pos];
		else
			return null;
	}

	/// <summary>
	/// Gets the monster position.
	/// </summary>
	/// <returns>The monster position.</returns>
	/// <param name="monsterId">Monster identifier.</param>
	/// added by zhangqiang at 2014-3-13
	public int GetMonsterPos(int monsterId)
	{
		for(int i =0; i < teamMember.Count; i++)
		{
			if(teamMember[i] != null)
			{
				if(teamMember[i].pid == monsterId)
				{
					return i;
				}
			}
		}
		RED.LogWarning("GetMonsterPos :: monster not find " + monsterId);
		return -1;
	}

	public List<Monster> GetMonByNum(int monNum)
	{
		List<Monster> list = new List<Monster> ();
		for(int i =0; i < teamMember.Count; i++)
		{
			if(teamMember[i] != null)
			{
				if(teamMember[i].num == monNum)
				{
					list.Add (teamMember [i]);
				}
			}
		}
		return list;
	}

	//判断monster所有缘是否都开启了
	public bool IsAllFataActive(Monster mt)
	{
		List<FateData> fateList = mt.getMyFate(Core.Data.fateManager);

		int count = fateList.Count;
		int value = 0;
		for (int i = 0; i < count; i++) 
		{
			if(mt.checkMyFate(fateList[i], this, Core.Data.dragonManager.usedToList()))
			{
				value ++;
			}

		}
		return(value == count);
	}

	//关于某个monster是否在该队伍里
	public bool inMyTeam(int MonId) {
		bool inTeam = false;
		foreach(Monster mon in teamMember) {
			if(mon != null && mon.pid == MonId){
				inTeam = true;
				break;
			}
		}
		return inTeam;
	}

	/// <summary>
	/// 根据星级找出个数
	/// </summary>
	/// <returns>The many in team.</returns>
	/// <param name="star">Star.</param>
	public int SplitByStar(int star) {
		int count = 0;
		foreach(Monster mon in teamMember) {
			if(mon != null){
				if(mon.Star == star) 
					count ++;
			}
		}
		return count;
	}

	#endregion

	#region Equipment Opertation

	/// <summary>
	/// Sets the equip.Pos只需要填写第几个宠物位置就可以。
	/// </summary>
	/// <returns><c>true</c>, if equip was set, <c>false</c> otherwise.</returns>
	/// <param name="eq">Eq.</param>
	/// <param name="pos">Position.</param>
	public bool setEquip(Equipment eq, int pos) {
		bool success = pos < capacity && pos >= 0 && eq != null;
		if(success) {
			if(eq.EquipmentType == EquipData.TYPE_ATTACK) {
				setEquipInner(eq, 2 * pos);
			} else if(eq.EquipmentType == EquipData.TYPE_DEFEND) {
				setEquipInner(eq, 2 * pos + 1);
			} else {
				Utils.Assert(true, "Equipment Type is Wrong.");
			}
			eq.equipped = true;
		}

		return success;
	}

	/// <summary>
	/// Removes the equip.Pos只需要填写第几个宠物位置就可以
	/// </summary>
	/// <returns><c>true</c>, if equip was removed, <c>false</c> otherwise.</returns>
	/// <param name="eq">Eq.</param>
	/// <param name="pos">Position.</param>
	public bool removeEquip(Equipment eq, int pos) {
		bool success = pos < capacity && pos >= 0 && eq != null;

		if(success) 
		{
			if(eq.EquipmentType == EquipData.TYPE_ATTACK)
			{
				removeEquipInner(2 * pos);
			} 
			else if(eq.EquipmentType == EquipData.TYPE_DEFEND) 
			{
				removeEquipInner(2 * pos + 1);
			} 
			else 
			{
				Utils.Assert(true, "Equipment Type is Wrong.");
			}
			eq.equipped = false;

			//检查别的队伍有没有
			foreach (MonsterTeam mt in Core.Data.playerManager.RTData.myTeams)
			{
				if (mt.TeamId != Core.Data.playerManager.RTData.curTeam.TeamId)
				{
					foreach (Equipment subEquip in mt.equipMember)
					{
						if (subEquip != null && subEquip.RtEquip != null)
						{
							if (subEquip.RtEquip.id == eq.RtEquip.id)
							{
								eq.equipped = true;
								break;
							}
						}
					}
				}
			}
		}

		return success;
	}

	/// <summary>
	/// Set equipment.
	/// </summary>
	/// <returns><c>true</c>, if equip was set, <c>false</c> otherwise.</returns>
	/// <param name="eq">Eq.</param>
	/// <param name="pos">Position. (队员位置 -1)*2 + 装备位置（0 或 1）</param>
	private bool setEquipInner(Equipment eq, int pos) {
		validate = 0x0;
		bool success = pos < EquipCapacity && pos >= 0;

		if(success)
			equipMember[pos] = eq;

		return success;
	}

	/// <summary>
	/// Removes the equip.
	/// </summary>
	/// <returns><c>true</c>, if equip was removed, <c>false</c> otherwise.</returns>
	/// <param name="pos">Position.(队员位置 -1)*2 + 装备位置（0 或 1）</param>
	private bool removeEquipInner(int pos) {
		validate = 0x0;
		bool success = pos < EquipCapacity && pos >= 0;

		if(success)
			equipMember[pos] = null;

		return success;
	}

	/// <summary>
	/// Gets the equip. 具体的位置队员位置 + 装备位置（0 或 1）
	/// </summary>
	/// <returns>The equip.</returns>
	/// <param name="pos">Position.</param>
	public Equipment getEquip(int pos, short EquipType) {
		if( (2 * pos + EquipType) < EquipCapacity && pos >= 0) 
			return equipMember[2 * pos + EquipType];
		return null;
	}

	/// <summary>
	/// GetEquipPosByEquipID
	/// </summary>
	/// <returns> equip pos in cur team
	/// added by zhangqiang at 2014-3-13
	public int GetEquipPosByEquipID(int equipId)
	{
		for(int i = 0; i < equipMember.Count; i++)
		{
			if(equipMember[i] != null)
			{
				if(equipMember[i].RtEquip.id == equipId)
				{
					int index = i / 2;
					return index;
				}
			}
		}

		RED.LogWarning("GetEquipPosByEquipID :: not find equip " + equipId);
		return -1;
	}

	//关于某个Equipment是否在该队伍里
	public bool equipInMyTeam(int EquipId) {
		bool inTeam = false;
		foreach(Equipment equ in equipMember) {
			if(equ != null && equ.ID == EquipId){
				inTeam = true;
				break;
			}
		}
		return inTeam;
	}

	/// <summary>
	/// 根据星级找出个数
	/// </summary>
	/// <returns>The many in team.</returns>
	/// <param name="star">Star.</param>
	public int equipSplitByStar(int star) {
		int count = 0;
		foreach(Equipment equ in equipMember) {
			if(equ != null){
				if(equ.ConfigEquip.star == star) 
					count ++;
			}
		}
		return count;
	}

	#endregion

	//clean all data
	public void Dispose () {
		teamMember.safeFree();
	}

	//Clear int Vars. to Zero
	//Clear List
	public void Reset () {
		teamMember.safeClear();
	}

}
