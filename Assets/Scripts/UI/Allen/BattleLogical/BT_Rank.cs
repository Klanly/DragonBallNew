using System;
using System.Collections.Generic;

namespace AW.Battle {

	/// <summary>
	/// 战斗评价，负责收集战斗信息的模块
	/// </summary>
	public class BT_Statictis {
		//一场战斗普通技能的己方释放次数
		public int _totalNoramSkill;
		//一场战斗普通技能的己方释放的技能ID array
		public List<int> _NorSkillArr;

		//记录下，敌方死亡的索引值
		public int enemyIndex = -1;

		//记录下战斗刚刚开始时候血量
		public int AllHpWhenWarStart = -1;
		//记录下战斗结束时候的血量
		public int AllHpWhenWarEnd   = -1;


		/// 
		///  ---常量---
		///  51% - 100% 3星   21%-50% 2星  0%-20% 1星
		/// 
		float Star3 = 0.35f;
		float Star2 = 0.2f;


		public BT_Statictis() {
			_NorSkillArr = new List<int>();
			//初始化战斗统计
			_totalNoramSkill  = 0;
			enemyIndex        = -1;
			AllHpWhenWarStart = -1;
			AllHpWhenWarEnd   = -1;
		}

		/// <summary>
		/// 分析当前队伍的血量总和
		/// </summary>
		public void anaylizeHp(BT_MonsterTeam MonTeam) {
			int starttotalHp = 0;
			int endTotalHp   = 0;
			if(MonTeam != null) {
				foreach(BT_Monster mon in MonTeam._team) {
					if(mon != null) {
						if(mon.alive)
							endTotalHp += mon.curAtt;

						starttotalHp += mon.initAtt;
					}
				}
			}

			AllHpWhenWarEnd   = endTotalHp;
			AllHpWhenWarStart = starttotalHp;
		}

		/// <summary>
		/// 战斗结算,评星级
		/// </summary>
		/// WarRes 0 输， 1赢
		public int getRank(int WarRes) {
			float ratio = AllHpWhenWarEnd * 1.0f / AllHpWhenWarStart;
			int rank = 0;

			if( WarRes == 1) {
				if(ratio > Star3) {
					rank = 3;
				} else if(ratio > Star2) {
					rank = 2;
				} else if(ratio > 0) {
					rank = 1;
				} 
			}

			return rank;
		}

	}
}

