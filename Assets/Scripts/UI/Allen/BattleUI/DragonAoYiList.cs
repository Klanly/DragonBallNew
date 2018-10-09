using UnityEngine;
using System.Collections;

public class DragonAoYiList : MonoBehaviour {
    // --------------------------- Dragon AoYi Skill Icon --------------------
    public DragonSkillManager dragonMgrLeft;
    public DragonSkillManager dragonMgrRight;

    //礼花
    private bool defaultFirework = true;   // 默认的礼花
    private GameObject Go_DefaultFirework; 
    private GameObject Go_Firework1;
    private GameObject Go_Firework2;

    public void ShowFirework(){
        //默认的礼花
        if(defaultFirework) {

            Object obj = null;
            if(Go_Firework1 == null || Go_Firework2 == null) {
                obj = PrefabLoader.loadFromUnPack("Allen/RibbonColor", false, false);
                Go_DefaultFirework = Instantiate(obj, new Vector3(0f, 4.15f, 0f), Quaternion.identity) as GameObject;
            }

            obj = null;
        } else {
            //光影的礼花
            Object obj = null;
            if(Go_Firework1 == null || Go_Firework2 == null) {
                obj = PrefabLoader.loadFromUnPack("Allen/FXFireworkShowLooper", false, false);
            }

            if(Go_Firework1 == null) {
                Go_Firework1 = Instantiate(obj) as GameObject;
                RED.AddChild(Go_Firework1, gameObject);
                Go_Firework1.transform.localPosition += new Vector3(-447f, 44f, 0f);
            } 

            if(Go_Firework2 == null) {
                Go_Firework2 = Instantiate(obj) as GameObject;
                RED.AddChild(Go_Firework2, gameObject);
                Go_Firework2.transform.localPosition += new Vector3(407f, 44f, 0f);
            }

            obj = null;
        }
    }

    public void DestoryFireWork() {
        //默认的礼花
        if(defaultFirework) {
            if(Go_DefaultFirework != null) {
                Destroy(Go_DefaultFirework);
                Go_DefaultFirework = null;
            }
        } else {
            if(Go_Firework1 != null) {
                Destroy(Go_Firework1);
                Go_Firework1 = null;
            }

            if(Go_Firework2 != null) {
                Destroy(Go_Firework2);
                Go_Firework2 = null;
            }
        }
    }
}
