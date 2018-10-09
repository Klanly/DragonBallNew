using UnityEngine;
using System.Collections;

public class CRLuo_ShowPlayAnim : MonoBehaviour {

	public int ID;
	public GameObject Default_Charactor;
	GameObject Man_GameObj;
	public CRLuoAnim_Main.Type ShowName;
	public float ShowTime;
	public float DeleteTime;
	public CRLuo_PlayAnim_FX Rival;
	public CRLuo_PlayAnim_FX Master;

	CRLuoAnim_Main.Type myNowAnim;
	// Use this for initialization
	void Start () {

		Object temp_Obj = ModelLoader.get3DModel(ID);

		//如果载入成功
		if (temp_Obj != null)
		{
			//  临时游戏对象 =（转换后方物体类型为GameObject）游戏对象.实例化当前读取物体（实例物体，初始位置，（优先转换（转换括号外对象为GameObject）obj）初始旋转）
			Man_GameObj = (GameObject)GameObject.Instantiate(temp_Obj,this.gameObject.transform.position, this.gameObject.transform.rotation);
		}
		else
		{
			//  临时游戏对象 =（转换后方物体类型为GameObject）游戏对象.实例化当前读取物体（实例物体，初始位置，初始旋转）
			Man_GameObj = (GameObject)GameObject.Instantiate(Default_Charactor, this.gameObject.transform.position, this.gameObject.transform.rotation);
		}
		Man_GameObj.GetComponent<CRLuo_PlayAnim_FX> ().Pos_L = Master.Pos_L;

		CRLuo_PlayAnim_FX Man_L = Man_GameObj.GetComponent<CRLuo_PlayAnim_FX>();

		Man_L.MyCamera_L.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));
		Man_L.MyCamera_L.depth = -1;

		Man_L.MyCamera_R.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));
		Man_L.MyCamera_R.depth = -1;

		Invoke("Show", ShowTime);
		Destroy(Man_GameObj, DeleteTime);
		Destroy(this.gameObject, DeleteTime);

	}


	void Update()
	{
		if (Rival == null && myNowAnim != Master.NowAnimType) {
			Man_GameObj.GetComponent<CRLuo_PlayAnim_FX> ().Injure_Key = true;
			myNowAnim = Master.NowAnimType;
			Man_GameObj.GetComponent<CRLuo_PlayAnim_FX>().HandleTypeAnim(Master.NowAnimType);
		}
	}
	void Show()
	{
		if (Rival != null) {
			Man_GameObj.GetComponent<CRLuo_PlayAnim_FX> ().RivalOBJ = Rival;
		} else {
			Man_GameObj.GetComponent<CRLuo_PlayAnim_FX> ().Injure_Key = true;
		}
		Man_GameObj.GetComponent<CRLuo_PlayAnim_FX>().HandleTypeAnim(ShowName);
	
	}

//	//物体销毁运行部分
	void OnDestroy()
	{
		//added by zhangqiang to release Assets
		Resources.UnloadUnusedAssets ();
	}
}
