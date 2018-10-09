using System;

/// <summary>
/// 检查小悟空第一次触发缘
/// </summary>
public sealed class WukongFate {
    //小悟空的Number号
    private const int Wukong = 10142;

    //userManager 
    private UserConfigManager userMgr;

    private WukongFate() { 
        userMgr = Core.Data.usrManager;
    }

    #region Properties

    public static WukongFate Instance {
        get { return GenericSingleton<WukongFate>.Instance; }
    }

    #endregion

    //小悟空的缘分好了
    public bool WukongJump(MonsterTeam Team) {
        bool jump = false;

        if(userMgr.UserConfig.wukongfate == 0) {
            Monster monWukong = null;
            foreach(Monster mon in Team.TeamMember) {
                if(mon != null && mon.num == Wukong) {
                    monWukong = mon;
                    break;
                }
            }

            if(monWukong != null) {
                bool allFated = Team.IsAllFataActive(monWukong);

                jump = allFated;
                if(jump) {
                    userMgr.UserConfig.wukongfate = 1;
                    userMgr.save();
                }

            }
        }

        return jump;
    }
}
