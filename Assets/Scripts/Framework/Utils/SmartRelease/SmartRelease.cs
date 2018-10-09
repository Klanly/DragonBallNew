/*
 * Created By Allen Wu. All rights reserved.
 */

using UnityEngine;
using System.Collections;

/*
 * 以下都是Allen的个人猜测
 * NGUI，没有任何引用关系的NGUI物体，即使在场景跳转的时候，也不会删除资源
 * 所以目前能做的事情，只有强制删除特定的资源
 * 
 */
public class SmartRelease {
	//从Login跳转到GameUI的时候
	#if UNITY_4_3
	private static string[] RemoveAsset_1 =	{"login-0005", "login-0004", "caodicj01"};
	#else
	private static string[] RemoveAsset_1 = {};
	#endif


	//从GameBattle跳转到GameUI的时候
	private static string[] RemoveAsset_3 = {"BattleUI", "Fight", "BattleEff", "numberRed", "dragon", "XXX", "fengshachangjing03", "fengshachangjing02", "fengshachangjing"
											, "chuwudaohui01", "caodicj01", "chuwudaohui02", "shendian04", "stone1", "Wind", "Wind2", "Wind 1", "FX_Star_2"};
	private static string[] RemoveAudio = {"tutoNew_1", "tutoNew_2", "bg_14year", "tuto1", "tuto2", "tuto3", "tuto4", "tuto5", "tuto6",
		"tuto7", "tuto8", "tuto9", "tuto10", "tuto11", "tuto12"};

	#if ReleaseAction
	private static string[] RemoveAnimClip = {"Free1", "Free2", "Attack", "OverSkill_0", "GroupSkill", "OverSkill_2", "Skill", "PowerSkill", "Injure_G0", "Injure_G1", "Injure_G2", "Injure_Fly_Go", "GoRush", "BackRush", "StardUp"
		, "Injure_1","Injure_2", "Injure_0", "Injure_Fly_Up", "Injure_Fly_Down"};
	#endif
	/// <summary>
	/// 从Login跳转到GameUI的时候, 也就是在跳到GameUI场景的时候一定要去掉这些Asset
	/// </summary>
	public static void SmartRemoveAsset_1() {
		releaseAsset<UIAtlas>(RemoveAsset_1);
		releaseAsset<Material>(RemoveAsset_1);
		releaseAsset<Texture>(RemoveAsset_1);
	}


	public static void RemoveAsset_WhenGameUI () {
		//releaseAsset<UIAtlas>(RemoveAsset_3);
		releaseAsset<Material>(RemoveAsset_3);
		releaseAsset<Texture>(RemoveAsset_3);
		releaseAsset<AudioClip>(RemoveAudio);

		#if ReleaseAction
		releaseAsset<AnimationClip>(RemoveAnimClip);
		#endif
	}


	static void releaseAsset<T>(string[] assets) {
		Object[] atlas = Resources.FindObjectsOfTypeAll(typeof(T));
		ConsoleEx.DebugLog("total " + typeof(T).ToString() + "  " + atlas.Length);

		if(atlas != null) {
			foreach(var at in atlas)
				if(Utils.inArray<string>(at.name, assets)) {
					Resources.UnloadAsset(at);
				}
		}
	}


}
