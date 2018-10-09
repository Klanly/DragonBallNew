using System;
using System.Collections.Generic;

namespace AW.Battle {

    /// <summary>
    /// 战斗时后的奥义配置对象定义
    /// </summary>
    public class BT_AoYi {

        public int _ID;
        public string _Name;
        // 效果值
        public List<float[]> _effect;
        //效果参数的说明
        public string[] _efinfo;
        // OP的ID
        public int _skillOp;
        // 加成类型
        public int _type;
        // 加成的基础值
        public float _basic;
        public float _growth;

        // OP的配置信息
        public SkillOpData _SkillOpConfig;

        //提高金属性武者技能效果
        public const int ImproveGold = 1;
        //提高木属性武者技能效果
        public const int ImproveWood = 2;
        //提高土属性武者技能效果
        public const int ImproveLand = 3;
        //提高水属性武者技能效果
        public const int ImproveWater = 4;
        //提高火属性武者技能效果
        public const int ImproveFire = 5;

        public const int ReduceEnemySkill = 8;
        public const int ImproveAoYi = 10;
        public const int ImproveSkill = 11;
    

        public BT_AoYi(AoYiData aoyiConfig, int ID, SkillManager skillMgr) {
            if(aoyiConfig == null) throw new DragonException("AoYi configure is empty.");
            if(skillMgr == null) throw new DragonException("Skill Manager is empty.");

            this._ID      = ID;
            this._Name    = aoyiConfig.name;
            this._effect  = aoyiConfig.effect;

            this._efinfo  = aoyiConfig.efinfo;
            this._skillOp = aoyiConfig.skillID;
            this._type    = aoyiConfig.type;
            this._basic   = aoyiConfig.basic;
            this._growth  = aoyiConfig.growth;

            this._SkillOpConfig = skillMgr.getSkillOpDataConfig(this._skillOp);

        }

    }

    /*
    * 战斗时候奥义技能的定义
    */
    public class CAoYi {
        // 奥义释放过嘛？0 = 没有使用过， 1 = 使用过
        private short _used; 
        // 奥义配置
		private BT_AoYi _AoYiItem = null;
        // 奥义等级
        private int _Lv;

        public CAoYi(AoYiData config, int lv, int Id, SkillManager skillMgr) {
            this._AoYiItem = new BT_AoYi (config, Id, skillMgr);
            this._Lv = lv;
            this._used = 0;
        }

        //返回使用的情况
        public int getUsed {
            get {
                return _used;
            }
        }

        //设定使用过的情况
        public void setUsed() {
              _used = 1;
        }

        //重置使用情况
        public void resetUsed() {
            this._used = 0;
        }

        //返回等级信息
        public int getLv {
            get {
                return this._Lv;
            }
        }

        //返回奥义信息
        public BT_AoYi getAoYiItem {
            get {
                return _AoYiItem;
            }
        }

        // 根据等级获取加成配置信息
        public float[] getEffectByLv(int lv) {
            int count = this._AoYiItem._effect.Count;
            if (lv <= count) {
                return this._AoYiItem._effect [lv - 1];
            } else {
                throw new DragonException ( "Can't get Effect info. lv = " + _Lv.ToString());
            }
        }

        /// <summary>
        /// Set skill's parameter.
        /// </summary>
        /// <param name="skill">Skill.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        private void setParam(BT_Skill skill, string key, int value) {
            if(skill != null && !string.IsNullOrEmpty(key)) {
                switch(key) {
                case "rate":
                    skill.param.rate = value;
                    break;
                case "rate2":
                    skill.param.rate2 = value;
                    break;
                case "gailv":
                    skill.param.gailv = value;
                    //保证概率不超过100
                    if(skill.param.gailv > 100) skill.param.gailv = 100;
                    break;
                case "damage":
                    skill.param.damage = value;
                    break;
                case "nuqi":
                    skill.param.nuqi = value;
                    break;
                case "num":
                    skill.param.num = value;
                    break;
                }
            }
        }

        /// <summary>
        /// Get skill's parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="skill">Skill.</param>
        /// <param name="key">Key.</param>
        private int getParam(BT_Skill skill, string key) {
            int value = -1;
            if(skill != null && !string.IsNullOrEmpty(key)) {
                switch(key){
                case "rate":
                    value = skill.param.rate;
                    break;
                case "rate2":
                    value = skill.param.rate2;
                    break;
                case "gailv":
                    value = skill.param.gailv;
                    break;
                case "damage":
                    value = skill.param.damage;
                    break;
                case "nuqi":
                    value = skill.param.nuqi;
                    break;
                case "num":
                    value = skill.param.num;
                    break;
                }
            }
            return value;
        }

        // 是否增加某类属性的技能,和概率没关系
        // 这个函数在实例化CPet的时候调用，可以在初始化的时候就决定好奥义的功用
        public void improveAttribute(BT_Monster Pet) {
            int PetAttri = (int)Pet._property;

            if (PetAttri == _AoYiItem._type || PetAttri == (_AoYiItem._type + 10)) {

                foreach (BT_Skill sk in Pet.get_NormalSkillArr) {
                    SkillOpData op = sk.opObj;
                    string[] enhanced = op.enhanced;

                    if(enhanced != null) {
                        foreach (string oneEnHanced in enhanced) {
                            int param = getParam(sk, oneEnHanced);
                            if(param != -1) {
                                param = Formular(param);
                                setParam(sk, oneEnHanced, param);
                            }
                        }
                    }
                }

            }
        }

        #region 增加或降低所有技能的触发概率
        // 核心的方法
        private void changeSkillPossibility(BT_Monster Pet, bool enHanceOrReduce) {
            foreach (BT_Skill sk in Pet.get_NormalSkillArr) {
                int param = getParam(sk, "gailv");

                if(param != -1) {
                    param = Formular(param, enHanceOrReduce);
                    setParam(sk, "gailv", param);
                }
            }

            foreach (BT_Skill sk in Pet.get_AngrySkillArr) {

                int param = getParam(sk, "gailv");

                if(param != -1) {
                    param = Formular(param, enHanceOrReduce);
                    setParam(sk, "gailv", param);
                }
            }
        }

        // --- 提供给外部调用的方法 ---
        public void enHanceSkillPossibility(BT_Monster Pet) {
            if(Pet != null && _AoYiItem._type == BT_AoYi.ImproveSkill) {
                changeSkillPossibility(Pet, true);
            }
        }
        // --- 提供给外部调用的方法 ---
        public void reduceSkillPossibility(BT_Monster Pet) {
            if (Pet != null && _AoYiItem._type == BT_AoYi.ReduceEnemySkill) {
                changeSkillPossibility (Pet, false);
            }
        }

        // 提升己方奥义触发几率
        public void improveAoYiPossibility() {

            if(_AoYiItem._type == BT_AoYi.ImproveAoYi) {
				AoYiData tD = Core.Data.dragonManager.getAoYiData (_AoYiItem._ID);


				List<float[]> tLi = _AoYiItem._effect;
				_AoYiItem._effect = new List<float[]> ();


				foreach (float[] arrItem in tLi) {
					float[] tF = new float[2]{Formularf(arrItem [0]),arrItem[1]};
					tF[0]=Formularf(arrItem [0]);
					tF[1] = arrItem[1];
					_AoYiItem._effect.Add(tF);

				}

				//不知道为什么 还会引用到 config data 
//                foreach(float[] arrItem in _AoYiItem._effect) {
//					ConsoleEx.Write (" arrItem  === " + arrItem[0] + " , " + arrItem[1] + "  iddd   == " + _AoYiItem._ID,"yellow"  );
//                    arrItem[0] = Formularf(arrItem[0]);
//                    arrItem[0] = arrItem[0] > 100 ? 100 : arrItem[0];//保证概率不超过100
//
//                }


            }
        }

        #endregion
      

        private int Formular(int value, bool added = true) {
            int addVolumn = _Lv - 1 < 0 ? 0 : _Lv - 1;
            if (added)
                value = MathHelper.MidpointRounding(value * (1 + _AoYiItem._basic + addVolumn * _AoYiItem._growth));
            else
                value = MathHelper.MidpointRounding(value * (1 - _AoYiItem._basic - addVolumn * _AoYiItem._growth));
            return value;
        }

        private float Formularf(float value) {
            int addVolumn = _Lv - 1 < 0 ? 0 : _Lv - 1;
            value = value * (1 + _AoYiItem._basic + addVolumn * _AoYiItem._growth);
            return value;
        }

    }

 /*
 * 奥义队列
 */
   public class BattleAoYi {
        //Key is num
        private Dictionary<int, CAoYi> _AoYi = null;
        //按照顺序存储奥义的Num
        private List<int> AoYiNumList = null;

        public BattleAoYi(Battle_AoYi[] data, DragonManager dragonMgr, SkillManager skillMgr) {
			//初始化
			_AoYi = new Dictionary<int, CAoYi>();
			AoYiNumList = new List<int>();

            if (data != null && dragonMgr != null && skillMgr != null) {
                foreach (Battle_AoYi item in data) {
                    if(item != null) {
                        AoYiNumList.Add(item.num);

//						AoYiData config = dragonMgr.getAoYiData(item.num);

						AoYiData config = Core.Data.dragonManager.getAoYiData (item.num);

						AoYiData tConfig = new AoYiData ();

						tConfig.basic = config.basic;
						tConfig.description = config.description;
						tConfig.dragonType = config.dragonType;
						tConfig.effect = new List<float[]> ();
						for (int i = 0; i < config.effect.Count; i++) {
							float[] tI= new float[2];
							tI = config.effect [i];
							tConfig.effect.Add(tI);
//							tConfig.effect.Add (config.effect[i]);
						}
						tConfig.efinfo = config.efinfo;
						tConfig.ef_first = config.ef_first;
						tConfig.enhanced = config.enhanced;
						tConfig.exp = config.exp;
						tConfig.growth = config.growth;
						tConfig.ID = config.ID;
						tConfig.info = config.info;
						tConfig.maxLevel = config.maxLevel;
						tConfig.name = config.name;
						tConfig.skillID = config.skillID;
						tConfig.type = config.type;
						tConfig.unlockLevel = config.unlockLevel;

						_AoYi[item.num] = new CAoYi(tConfig, item.lv, item.num, skillMgr);
                    }
                }
            }
        }

        // 获取攻击方or防守方的奥义Num列表
        public List<int> getAoYi {
            get {
                List<int> AoYiNum = new List<int>();
                foreach(int num in _AoYi.Keys) {
                    AoYiNum.Add(num);
                }

                return AoYiNum;
            }
        }


        public void improveAttribute(BT_Monster Pet) {
			if(_AoYi.Count > 0) {
				foreach (CAoYi item in _AoYi.Values) {
					if(item != null) item.improveAttribute (Pet);
				}
			}
        }

        //判定是否为第一个奥义
        public bool isFirstAoYi(int AoYiID) {
            if(AoYiNumList.Count > 0) {
                int firstAoYiNum = AoYiNumList[0];
                return AoYiID == firstAoYiNum;
            } else {
                return false;
            }
        }


        //之前的奥义释放过嘛？
        public bool prevAoYiCast(int AoYiSkillID) {
            if(AoYiNumList.Count == 0) return false;
            int firstAoYiNum = AoYiNumList[0];

            bool preUsed = false;
            //找到第一个
            CAoYi firstAoYi = _AoYi[firstAoYiNum];

            bool found = false;
            int index = 0;
            foreach(int key in AoYiNumList) {
                if(key == AoYiSkillID) {
                    found = true;
                    break;
                }
                index ++;
            }

            //如果找到了当前的
            if(found) {
                index -= 1;

                if(index >= 0) {
                    //如果不是第一个，则考虑前一个是否释放过。如果是，则表明可能可以释放当前的。
                    CAoYi prevAoYi = _AoYi[AoYiNumList[index]];
                    preUsed = prevAoYi.getUsed == 1;
                } else {
                    //如果是第一个，则考虑当前是否释放过。如果是，则表明不可以释放当前的。
                    preUsed = firstAoYi.getUsed == 0;
                }

            }

            return preUsed;
        }

        //当前的奥义释放过嘛？
        public bool curAoYiCast(int AoYiSkillId) {
            bool curUsed = false;

            CAoYi temp = null;
            foreach (CAoYi ao in _AoYi.Values) {
                if(ao.getAoYiItem._ID == AoYiSkillId) {
                    temp = ao;
                    break;
                }
            }
            if(temp != null) {
                if(temp.getUsed == 0) {
                    curUsed = false;
                } else {
                    curUsed = true;
                }
            }

            return curUsed;
        }

        //这只该奥义已经处于已释放的状态
        public void setCurAoYiCast(int AoYiSkillId) {
            CAoYi temp = null;
            foreach (CAoYi ao in _AoYi.Values) {
                if(ao.getAoYiItem._ID == AoYiSkillId) {
                    temp = ao;
                    break;
                }
            }

            if(temp != null) temp.setUsed();
        }

        //充值该奥义为 可释放状态
        public void resetCurAoYi(int AoYiSkillId){
            CAoYi temp = null;
            foreach (CAoYi ao in _AoYi.Values) {
                if(ao.getAoYiItem._ID == AoYiSkillId) {
                    temp = ao;
                    break;
                }
            }

            if(temp != null) temp.resetUsed();
        }

        //增加己方的技能概率
        public void enHanceSkillPossibility(BT_Monster Pet) {
            foreach (CAoYi item in _AoYi.Values) {
                item.enHanceSkillPossibility (Pet);
            }
        }

        //减少敌人的技能概率
        public void reduceSkillPossibility(BT_Monster Pet) {
            foreach (CAoYi item in _AoYi.Values) {
                item.reduceSkillPossibility(Pet);
            }
        }

        // 提升己方奥义触发几率
        public void improveAoYiPossibility() {
            foreach (CAoYi item in _AoYi.Values) {
                item.improveAoYiPossibility();
            }
        }

        /// <summary>
        /// Tricks the ao yi to skill. 伪装奥义为技能
        /// </summary>
        /// <returns>The ao yi to skill.</returns>
        /// <param name="">.</param>
        public void trickAoYiToSkill(List<BT_Monster> team) {
            //加入奥义的主动技能
            if(team != null) {
                foreach (BT_Monster pet in team) {
                    foreach (CAoYi ao in _AoYi.Values) {
                        BT_Skill VirtualSk = new BT_Skill(ao, pet);

                        //奥义伪造为技能
                        VirtualSk.real = 1;

                        short period = ao.getAoYiItem._SkillOpConfig.type;
                        if(period == SkillOpData.Anger_Skill) {
                            //添加怒气技能
                            pet.set_AngrySkillArr(VirtualSk);
                        } else {
                            //添加Normal技能
                            pet.set_NormalSkillArr(VirtualSk);
                        }
                    }
                }
            }
        }
    }
}
