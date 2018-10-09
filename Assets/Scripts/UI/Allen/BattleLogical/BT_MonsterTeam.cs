using System;
using System.Collections.Generic;

/// <summary>
/// 战斗过程中的宠物队伍
/// </summary>
namespace AW.Battle {
    public class BT_MonsterTeam {
        private int _usrId = -1;
        private string _tName;
        //战斗的队员列表
        public  List<BT_Monster> _team;
        private int _angryCnt = 0;
        private bool _teamOver = false;

        private int _totalPets;
        private int _curPet;

        //----------- get 属性 -------------
        public int curAngry {
            get {
                return _angryCnt;
            }
        }

        //返回队伍的名字 'Att' or 'Def'
        public string getTeamName {
            get {
                return _tName;
            }
        }


        //返回当前队伍是否都死完了
        public bool over {
            get {
                return _teamOver;
            }
        }

        public int curPetTeamIndex {
            get {
                return _curPet;
            }
        }

        public BT_Monster curPet {
            get {
                if(_team != null) {
                    if(_curPet >= 0 && _curPet < _team.Count)
                        return _team[_curPet];
                    else
                        return null;
                }
                else 
                    return null;
            }
        }
        //----------- End Of Get 属性 -------------

        public void costAngry(int angry){
            this._angryCnt -= angry;
            if(_team != null) {
                _team[_curPet].useFateAngry();
            }
        }

        //保证怒气值不超过上限
        public void maxAngryCount() {
            if(this._angryCnt > BT_WarUtils.Max_Angry)
                this._angryCnt = BT_WarUtils.Max_Angry;
        }

        //加气
        public void addAngry(int angry) {
            this._angryCnt += angry;
            this.maxAngryCount();
        }

        public void teamLenChanged(){
            if(this._team != null)
                this._totalPets = this._team.Count;
        }

        //如果是满缘的情况下，不增加新的怒气点（一增一减不变）
		public void curPetDie(bool fullFated, bool usedFateAngry, int Ap){

			///
			/// --------- 目前的情况是：如果死亡，则不扣除缘提供的怒气 ------
			///

			/*
            //如果是满缘的清况下，则要考虑有没有使用缘的怒气
            //如果使用了缘的怒气，则不减少怒气
            if(fullFated) {
                if(!usedFateAngry) {
                    _angryCnt -= BT_WarUtils.Unit_Angry;
                }
            }*/

			_angryCnt += Ap;
            maxAngryCount();

            _curPet++;
            if(_curPet >= _totalPets){
                _teamOver = true;
                ConsoleEx.DebugLog("FUN curPetDie: 队伍:全军覆没啦!" + getTeamName);
            }
        }

        public Monsterteam toMonsterTeam() {
            Monsterteam teamInfo = new Monsterteam() {
                angryCnt = (short)_angryCnt,
                roleId   = _usrId,
                type     = _tName == "Att" ? (short)0 : (short)1,
                team     = new Monster[_team.Count],
            };

            int length = _team.Count;
            for(int i = 0; i < length; ++ i) {
                teamInfo.team[i] = _team[i].toMonster();
            }

            return teamInfo;
        }


        public BT_MonsterTeam(Battle_Data data, BT_Logical war, int pveIndexStart, string tName, BattleAoYi attAoyi, BattleAoYi defAoyi) {
            _tName = tName;
			if(!string.IsNullOrEmpty(data.roleId)) _usrId = Convert.ToInt32(data.roleId);
			_team  = new List<BT_Monster>();

            foreach (Battle_Monster mon in data.team) {
                if(mon != null) {
                    BT_Monster pet = new BT_Monster(mon, pveIndexStart + _team.Count, war, this, null);
                    //计算神龙奥义的加成
                    attAoyi.improveAttribute(pet);
                    attAoyi.enHanceSkillPossibility(pet);
                    attAoyi.improveAoYiPossibility();
                    defAoyi.reduceSkillPossibility(pet);

                    _team.Add(pet);
                }
            }
            //加入奥义的技能
            attAoyi.trickAoYiToSkill(_team);
            this.teamLenChanged();
            this._curPet = 0;
        }

        //设定敌人的关系
        public void setupRelation(BT_MonsterTeam enemyTeam){
            foreach (BT_Monster pet in _team) {
                pet.setEnmeyTeam(enemyTeam);
            }
        }

    }
}