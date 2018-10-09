using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Dragon skill manager. 神龙的控制
/// </summary>
public class DragonSkillManager : MonoBehaviour {

	//奥义的单个元素
    public DragonAoYiItem[] items;
	//奥义集合
	public GameObject AoYiList;

	//每次移动的间隔
	public Vector3 MoveDis;
	public float MoveTime;

	// Use this for initialization
	void Start () {
        foreach(DragonAoYiItem item in items) {
            item.ResetDragonAoyi();
        }
	}
	
    //第一次展示奥义技能的图标
    public void ReadData(List<int> AoYi) {
        int count = AoYi.Count;
        int count2 = items.Length;
        int min = count > count2 ? count2 : count;
        int max = count > count2 ? count : count2;

        for(int i = 0; i < min; ++i) {
            items[i].Icon.spriteName = AoYi[i].ToString();
            items[i].gameObject.SetActive(true);
        }

        for(int i = min; i < max; ++ i) {
            items[i].gameObject.SetActive(false);
        }
    }

	//隐藏图标 -- 给新的动画添加    wxl 
	public IEnumerator HideIcon(int pos  , float delay ,string name) {
		Debug.Log (" hide icon  = " + pos  + " name = " + name);
		foreach(DragonAoYiItem tI in items){
			if (tI.Icon != null && tI.Icon.spriteName != null) {
				{
					int tAY = int.Parse (tI.Icon.spriteName);
					AoYiData tAYData = Core.Data.dragonManager.getAoYiData (tAY);	

					if (tAYData.name == name) {
						tI.gameObject.SetActive (false);
						yield return new WaitForSeconds (delay);
						AoYiList.GetComponent<UIGrid> ().Reposition ();
					}
				}
			}
		}

//		if(pos >= 0 && pos < items.Length) {
//			items[pos].gameObject.SetActive(false);
//			yield return new WaitForSeconds(delay);
//			//wxl
//			SlideUsedOne(pos);
//		}
	}

	//移动，隐藏掉正在使用的
	void SlideUsedOne(int pos) {
//		MiniItween.MoveTo(AoYiList.gameObject, -MoveDis * (pos + 1), MoveTime, MiniItween.EasingType.Linear);
		AoYiList.GetComponent<UIGrid> ().Reposition ();
	}

	//------------------- 新版本的奥义动画， 下面方法都不会被使用 ----------------------

    //释放技能
    public void showSkill(int pos) {
        if(pos >= 0 && pos < items.Length) {
            items[pos].CastDragonAoYi();
        }
    }

    //重置技能
    public void resetSkill(int pos) {
        if(pos >= 0 && pos < items.Length) {
            items[pos].ResetDragonAoyi();
        }
    }
}
