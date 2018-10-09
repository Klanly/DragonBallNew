/*
 * Created By Allen Wu. All rights reserved.
 */

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

//背景音效的枚举
public enum SceneBGM {
	BGM_Login  = 0x01,
	BGM_GAMEUI = 0x02,
	BGM_BATTLE = 0x03,
	BGM_BOSS   = 0x04,
	BGM_PROLOG = 0x05,
	BGM_14YEAR = 0x06,
	BGM_CG_BiKe= 0x07,
}

//声效的枚举
public enum SoundFx {
    FX_Searching = 0xA0,
    FX_SerachGet = 0xA1,
    FX_OpenChest = 0xA2,
    FX_Warning = 0xA3,
    FX_Win = 0xA4,
    FX_Fail = 0xA5,
    FX_UserLevelUp = 0xA6,
    FX_CoinFromBuilding = 0xA7,
    FX_TiliFromBuilding = 0xA8,
    FX_BuildingLevelUp = 0xA9,
    FX_Diamonds = 0xAA,
    FX_Strengthen_Weapon = 0xAB,
    FX_Strengthen_Card = 0xAC,
    FX_Building_Done = 0xAD,
    FX_YiQuan = 0xAE,
    FX_ZhongQuan = 0xAF,
    FX_ZhuHeQuan = 0xB0,
    FX_QingQuan = 0xB1,
    FX_Ti = 0xB2,
    FX_QingTi = 0xB3,
    FX_BaoZha1 = 0xB4,
    FX_BaoZha2 = 0xB5,
    FX_BaoZha3 = 0xB6,
    FX_BaoZha4 = 0xB7,
    FX_ChuBo1 = 0xB8,
    FX_ChuBo2 = 0xB9,
    FX_ChuBo3 = 0xBA,
    FX_ChuBo4 = 0xBB,
    FX_ZanBo1 = 0xBC,
    FX_ZanBo2 = 0xBD,
    FX_ZanBo3 = 0xBE,
    FX_ZanBo4 = 0xBF,
    FX_Free1 = 0xC0,
    FX_Free2 = 0xC1,
    FX_Free3 = 0xC2,
    FX_Free4 = 0xC3,
    FX_Show1 = 0xC4,
    FX_Show2 = 0xC5,
    FX_Show3 = 0xC6,
    FX_Jump = 0xC7,
    FX_Hurt_Male1 = 0xC8,
    FX_Hurt_Male2 = 0xC9,
    FX_Hurt_Male3 = 0xCA,
    FX_Hurt_Female1 = 0xCB,
    FX_Hurt_Female2 = 0xCC,
    FX_Hurt_Female3 = 0xCD,
    FX_Gun = 0xCE,
    FX_Male_Happy1 = 0xCF,
    FX_Male_Happy2 = 0xD0,
    FX_Male_Happy3 = 0xD1,
    FX_Female_Happy1 = 0xD2,
    FX_Female_Happy2 = 0xD3,
    FX_Female_Happy3 = 0xD4,
    FX_Monster_Happy1 = 0xD5,
    FX_Monster_Happy2 = 0xD6,
    FX_Monster_Happy3 = 0xD7,
    FX_Robot_Free1 = 0xD8,
    FX_Robot_Free2 = 0xD9,
    FX_Robot_Free3 = 0xDA,
    FX_Sword1 = 0xDB,
    FX_Sword2 = 0xDC,
    FX_Sword3 = 0xDD,
    FX_Hummer1 = 0xDE,
    FX_Hummer2 = 0xDF,
    FX_Hummer3 = 0xE0,
    FX_Else1 = 0xE1,
    FX_Else2 = 0xE2,
    FX_Else3 = 0xE3,
    FX_Else4 = 0xE4,
    FX_Else5 = 0xE5,
    FX_C_Attack1 = 0xE6,
    FX_C_DoubleSkill = 0xE7,
    FX_C_Draw = 0xE8,
    FX_C_GroupSkill1 = 0xE9,
    FX_C_GroupSkill2 = 0xEA,
    FX_C_GroupSkill3 = 0xEB,
    FX_C_OverSkill1 = 0xEC,
    FX_C_OverSkill2 = 0xED,
    FX_C_OverSkill3 = 0xEE,
    FX_C_Show = 0xEF,
    FX_C_Skill = 0xF0,
    FX_Stick1 = 0xF1,
    FX_Stick2 = 0xF2,
    FX_Stick3 = 0xF3,
    FX_Alalei = 0xF4,
    FX_Throw = 0xF5,
    FX_Buchong1 = 0xF6,
    FX_Buchong2 = 0xF7,
    FX_BuShout1 = 0xF8,
    FX_BuShout2 = 0xF9,
    FX_BuShout3 = 0xFA,
    FX_SuperDong = 0xFB,
    FX_Buerma = 0xFC,
    FX_QiGongBo = 0xFD,
    FX_QiyuanZhan = 0xFE,
    FX_ShiXianBo = 0xFF,
    FX_dbzgj = 0x100,
    FX_GuiPaiQiGong = 0x101,
    FX_Shout_dbzgj = 0x102,
    FX_XingQiGongPao = 0x103,
    FX_Wear = 0x104,
    FX_Egg = 0x105,
    FX_Strength_fail = 0x106,
    FX_Dragon = 0x107,
    FX_C_hurt1 = 0x108,
    FX_C_hurt2 = 0x109,
    FX_C_hurt3 = 0x10A,
    FX_C_shout1 = 0x10B,
    FX_C_shout2 = 0x10C,
    FX_F_hurt1 = 0x10D,
    FX_F_hurt2 = 0x10E,
    FX_F_hurt3 = 0x10F,
    FX_F_shout1 = 0x110,
    FX_F_shout2 = 0x111,
    FX_G_hurt1 = 0x112,
    FX_G_hurt2 = 0x113,
    FX_G_hurt3 = 0x114,
    FX_G_shout1 = 0x115,
    FX_G_shout2 = 0x116,
    FX_M_hurt1 = 0x117,
    FX_M_hurt2 = 0x118,
    FX_M_hurt3 = 0x119,
    FX_M_shout1 = 0x11A,
    FX_M_shout2 = 0x11B,
    FX_M_shout3 = 0x11C,
    FX_M_shout4 = 0x11D,
    FX_M_shout5 = 0x11E,
    FX_Explosion = 0x11F,

    FX_10102_1  = 0x120,
    FX_10102_2  = 0x121,
    FX_10103_1  = 0x122,
    FX_10103_2  = 0x123,
    FX_10105_1  = 0x124,
    FX_10105_2  = 0x125,
    FX_10108_1  = 0x126,
    FX_10108_2  = 0x127,
    FX_10109_1  = 0x128,
    FX_10109_2  = 0x129,
    FX_10117_1  = 0x12A,
    FX_10118_1  = 0x12B,
    FX_10118_2  = 0x12C,
    FX_10122_1  = 0x12D,
    FX_10122_2  = 0x12E,
    FX_10123_1  = 0x12F,
    FX_10123_2  = 0x130,
    FX_10133_1  = 0x131,
    FX_10133_2  = 0x132,
    FX_10134_1  = 0x133,
    FX_10134_2  = 0x134,
    FX_10137_1  = 0x135,
    FX_10137_2  = 0x136,
    FX_10138_1  = 0x137,
    FX_10138_2  = 0x138,
    FX_10140_1  = 0x139,
    FX_10140_2  = 0x13A,
    FX_10142_1  = 0x13B,
    FX_10143_1  = 0x13C,
    FX_10145_1  = 0x13D,
    FX_10146_1  = 0x13E,
    FX_10149_1  = 0x13F,
    FX_10151_1  = 0x140,
    FX_10151_2  = 0x141,
    FX_10161_1  = 0x142,
    FX_10166_1  = 0x143,
    FX_10166_2  = 0x144,
    FX_10172_1  = 0x145,
    FX_10178_1  = 0x146,
    FX_10178_2  = 0x147,
    FX_10186_1  = 0x148,
    FX_10191_1  = 0x149,
    FX_10193_1  = 0x14A,
    FX_10193_2  = 0x14B,
    FX_10198_1  = 0x14C,
    FX_10198_2  = 0x14D,
    FX_10201_1  = 0x14E,
    FX_10206_1  = 0x14F,
    FX_10206_2  = 0x150,
    FX_10207_1  = 0x151,
    FX_10217_1  = 0x152,
    FX_10217_2  = 0x153,
    FX_NormalFemale1    = 0x154,
    FX_NormalFemale2    = 0x155,
    FX_NormalGear   = 0x156,
    FX_NormalMale1  = 0x157,
    FX_NormalMale2  = 0x158,
    FX_NormalMale3  = 0x159,
    FX_NormalMale4  = 0x15A,
    FX_NormalMonster    = 0x15B,
    FX_tuto1    = 0x15C,
    FX_tuto2    = 0x15D,
    FX_tuto3    = 0x15E,
    FX_tuto4    = 0x15F,
    FX_tuto5    = 0x160,
    FX_tuto6    = 0x161,
    FX_tuto7    = 0x162,
    FX_tuto8    = 0x163,
    FX_tuto9    = 0x164,
    FX_tuto10   = 0x165,
    FX_tuto11   = 0x166,
    FX_tuto12   = 0x167,
    FX_tuto13   = 0x168,
    FX_tuto14   = 0x169,
    FX_tuto15   = 0x16A,
    FX_tuto16   = 0x16B,
    FX_tuto17   = 0x16C,
    FX_tuto18   = 0x16D,
    FX_tuto19   = 0x16E,
    FX_tuto20   = 0x16F,
    FX_tuto21   = 0x170,
    FX_tuto22   = 0x171,
    FX_tuto23   = 0x172,
    FX_tuto24   = 0x173,
    FX_tuto25   = 0x174,
    FX_10215_1  = 0x175,
    FX_10215_2  = 0x176,
    FX_10181_1  = 0x177,
    FX_10181_2  = 0x178,
    FX_10113_1  = 0x179,
    FX_10113_2  = 0x17A,
    FX_NormalMale5 = 0x17B,
    FX_NormalMale6 = 0x17C,
    FX_10191_2 = 0x17D,
    FX_10117_2 = 0x17E,
    FX_OldMale = 0x17F,
    FX_10100_1 = 0x180,
    FX_10100_2 = 0x181,
    FX_10137_3 = 0x182,
    FX_10163_1 = 0x183,
    FX_tuto26 = 0x184,
    FX_tuto27 = 0x185,
    FX_prolog0 = 0x186,
    FX_prolog1 = 0x187,
    FX_prolog2 = 0x188,
    FX_prolog3 = 0x189,
    FX_prolog4 = 0x18A,
    FX_prolog5 = 0x18B,
    FX_prolog6 = 0x18C,
    FX_prolog7 = 0x18D,
    FX_10150_2 = 0x18E,
    FX_tutoNew_1 = 0x18F,
    FX_tutoNew_2 = 0x190,
    FX_tutoNew_3 = 0x191,
    FX_tutoNew_4 = 0x192,
    FX_tutoNew_0 = 0x193,
	FX_prologNew1 = 0x194,
	Fx_prologNew2 = 0x195,
	Fx_PickCard   = 0x196,
	Fx_float      = 0x197,
	Fx_walk       = 0x198,
    Fx_XXX1 = 0x199,
    Fx_XXX2 = 0x19A,
    Fx_openTRBox  = 0x19B,
	Fx_tutoNew_5  = 0x19C,
	Fx_tutoNew_6  = 0x19D,
	Fx_tutoNew_7  = 0x19E,
	Fx_tutoNew_8  = 0x19F,
	Fx_tutoNew_9  = 0x1A0,
	Fx_tutoNew_10  = 0x1A1,
	Btn1  = 0x1A2,
	Btn2  = 0x1A3,
	Btn3  = 0x1A4,
	FX_JumpWar = 0x1A5,
	FX_Vs = 0x1A6,
	FX_KO = 0x1A7,
	FX_CG_BiKe = 0x1A8,
	FX_PVEBOSS = 0x1A9,
	FX_BrokeGold = 0x1AA,
	FX_DragonBeadDrop = 0x1AB,
	FX_SummonDragon   = 0x1AC,
}

public class SoundManager : Manager {
	private const int AUDIO_EFFECT = 0xFE;
	private const int AUDIO_BMG = 0x01;
	//声效的路径
	private const string AUDIO_ROOT_PATH = "Audio/";

	private UserConfigManager uMgr;
	private bool bMute;
	private bool cached = true;
	//key is ID
	public Dictionary<int, SoundData> soundConfig = null;

    private AudioLoader mAudioLoader = null;

    public SoundManager(UserConfigManager u, AudioLoader al) {
		cached = true;
        mAudioLoader = al;
		soundConfig = new Dictionary<int, SoundData>();
		uMgr = u;
		bMute = uMgr.UserConfig.mute == 0 ? false : true;
		base.readFromLocalConfigFile<SoundData>(ConfigType.SoundConfig, soundConfig);

	}

	public override bool loadFromConfig () {
		return true;
	}


	/// <summary>
	/// 在第一次运行Android平台上，因为执行顺序的问题，Audio Data会没加载上
	/// </summary>
	public void reloadConfig() {
		soundConfig.Clear();
		base.readFromLocalConfigFile<SoundData>(ConfigType.SoundConfig, soundConfig);
	}


	#region 获取声效文件的名字

	public string getBGM (SceneBGM bgm) {
		string fileName = string.Empty;
		SoundData sound = null;
		if(soundConfig.TryGetValue((int)bgm, out sound)) {
			fileName = sound.name;
		}
		return fileName;
	}

	public string getSoundFx(SoundFx fx) {
		string fileName = string.Empty;
		SoundData sound = null;
		if(soundConfig.TryGetValue((int)fx, out sound)) {
			fileName = sound.name;
		}
		return fileName;
	}

	public string getBtnFX(ButtonType btn) {
		string fileName = string.Empty;
		SoundData sound = null;
		if(soundConfig.TryGetValue((int)btn, out sound)) {
			fileName = sound.name;
		}
		return fileName;
	}

	#endregion


	#region 各种方便的操作


    /// <summary>
    /// 新手引导过程播放的声音,使用完就释放
    /// </summary>
    /// <returns>The fx play.</returns>
    /// <param name="fx">Fx.</param>
    /// <param name="SoundFinished">Sound finished.</param>
    /// <param name="DefaultLayer">Default layer.</param>
    public int GuideFxPlay(SoundFx fx, System.Action SoundFinished = null, int DefaultLayer = 0x02) {
        int layer = -1;
        if(!bMute) {
            string fileName = getSoundFx(fx);
            if(string.IsNullOrEmpty(fileName)) {
                ConsoleEx.DebugLog("Can't find Button Sound Effect. Button Type = " + fx.ToString());
            } else {
                AudioClip clip = null;
                clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + fileName, false, cached) as AudioClip;
                layer = Core.SoundEng.PlayClipForce(clip, DefaultLayer, false, 1.0f);

                if(SoundFinished != null) {
                    AsyncTask.QueueOnMainThread(SoundFinished, clip.length);
                }

                clip = null;
            }
        }
        return layer;
    }

    /// <summary>
    /// 特效声音的播放, 需要管理内存, 第三个参数支持循环播放声效
    /// </summary>
    /// <param name="fx">Fx.</param>
	public int SoundFxPlay(SoundFx fx, System.Action SoundFinished = null, bool loop = false) {
        int layer = -1;
		if(!bMute) {
			string fileName = getSoundFx(fx);
			if(string.IsNullOrEmpty(fileName)) {
				ConsoleEx.DebugLog("Can't find Sound Effect. Sound Type = " + fx.ToString());
			} else {
				AudioClip clip = null;
				clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + fileName, false, cached) as AudioClip;
				layer = Core.SoundEng.PlayClip(clip, AUDIO_EFFECT, loop);

                if(SoundFinished != null) {
                    AsyncTask.QueueOnMainThread(SoundFinished, clip.length);
                }

                mAudioLoader.RefAsset(AUDIO_ROOT_PATH + fileName);
			}
		}
        return layer;
	}

	/// <summary>
	/// 播放背景音乐
	/// </summary>
    public void BGMPlay(SceneBGM BGM) {
		if(!bMute) {
			string fileName = getBGM(BGM);
			if(string.IsNullOrEmpty(fileName)) {
				ConsoleEx.DebugLog("Can't find Button Sound Effect. Button Type = " + BGM.ToString());
			} else {
				AudioClip clip = null;
				clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + fileName, false, cached) as AudioClip;
                Core.SoundEng.PlayClipForce(clip, AUDIO_BMG, true, 0.8f);
			}
		}
	}

    /// <summary>
    /// 关闭背景音乐
    /// </summary>
    public void BGMStop() {
        if(!bMute) {
            Core.SoundEng.StopChannel(0);
        }
    }  

	/// <summary>
	/// 按钮的音效
	/// </summary>
	/// <param name="type">Type.</param>
	public void BtnPlay(ButtonType type) {
		if(!bMute) {
			AudioClip clip = null;
			switch(type) {
			case ButtonType.Confirm:
				clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + "sfx_button", false, cached) as AudioClip;
				break;
			case ButtonType.Cancel:
				clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + "sfx_button1", false, cached) as AudioClip;
				break;
			}
			if(clip != null) Core.SoundEng.PlayClip(clip, AUDIO_EFFECT, false);
		}
	}

	public bool SoundMute {
		get {
			return bMute;
		}
	}

	/// <summary>
	/// 这个方法只能提供给GameUI或GameBattle场景使用，
	/// </summary>
	public void SwitchSound(bool usedInBattle = false) {
		bMute = !bMute;

		uMgr.UserConfig.mute = bMute ? (short)1 : (short)0;
		uMgr.save();

		//Debug.LogError(bMute.ToString());

        if(bMute) {
            Core.SoundEng.StopChannel(0);
			NGUITools.soundVolume = 0;
        }
		else 
		{
			NGUITools.soundVolume = 1f;
			string fileName = getBGM( usedInBattle ? SceneBGM.BGM_BATTLE : SceneBGM.BGM_GAMEUI);
            AudioClip clip = null;
            clip = PrefabLoader.loadFromUnPack(AUDIO_ROOT_PATH + fileName, false, cached) as AudioClip;
            Core.SoundEng.PlayClipForce(clip, AUDIO_BMG, true, 0.8f);
        }
            
	}

	#endregion
}


