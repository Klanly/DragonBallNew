using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用来控制怒气点的动画，以及显示的。现在已经没用了。
/// </summary>

public class BanAngrySlot : MonoBehaviourEx {

	#region Indicator
	private const short UOBJ_EMPTY = 0;
	private const short UOBJ_FULL = 1;
	private const short UOBJ_EXPLODE = 2;
    private const short UOBJ_ANGRY_FLY = 3;
	#endregion

	public int maxSlot = 5;

	public float gap = 1;

	private int _curSlot= 0;

    public int curSlot {
        set {
			_curSlot = Mathf.Clamp(value,0, maxSlot);
            if(fullObjects.Count > _curSlot) {

                while(fullObjects.Count > _curSlot ) {
					BanTools.CreateObject( prefab_Explode, fullObjects[_curSlot].transform.position );
					Destroy(fullObjects[_curSlot].gameObject);
					fullObjects.RemoveAt(_curSlot);
				}

            } else if(fullObjects.Count < _curSlot) {
                StartCoroutine(showAngryPoint());
			}
		}
        get {
			return _curSlot;
		}
	}

    IEnumerator showAngryPoint() {

        yield return new WaitForSeconds(BanTimeCenter.AddOneAngry);

        while(fullObjects.Count < _curSlot) {
            fullObjects.Add( BanTools.CreateObject(prefab_Full, GetPos(fullObjects.Count) + Vector3.back * 0.01f) );
        }
    }

    public Object prefab_Empty {
        get {
			if(UObjDic.ContainsKey(UOBJ_EMPTY))
				return UObjDic[UOBJ_EMPTY];
			else {
				Object o = BanTools.LoadFromUnPack("AngrySlotEmpty");
				UObjDic.Add(UOBJ_EMPTY, o);
				return o;
			}
		}
	}

    public Object prefab_Full {
        get {
			if(UObjDic.ContainsKey(UOBJ_FULL))
				return UObjDic[UOBJ_FULL];
			else {
				Object o = BanTools.LoadFromUnPack("AngrySlotFull");
				UObjDic.Add(UOBJ_FULL, o);
				return o;
			}
		}
	}

	public Object prefab_Explode{
        get {
			if(UObjDic.ContainsKey(UOBJ_EXPLODE))
				return UObjDic[UOBJ_EXPLODE];
			else {
				Object o = BanTools.LoadFromUnPack("AngrySlotExplode");
				UObjDic.Add(UOBJ_EXPLODE, o);
				return o;
			}
		}
	}

    public Object prefab_GhostFly {
        get {
            if(UObjDic.ContainsKey(UOBJ_ANGRY_FLY)) {
                return UObjDic[UOBJ_ANGRY_FLY];
            } else {
                Object o = BanTools.LoadFromUnPack("Ghost2D");
                UObjDic.Add(UOBJ_ANGRY_FLY, o);
                return o;
            }
        }
    }

	public bool rightDir = true;

	private List<GameObject> fullObjects = null;

	private List<GameObject> emptyObjects = null;

	Vector3 GetPos(int index){
        if(rightDir) {
            return this.transform.position + Vector3.right * (index + 1) * gap;
        } else {
            return this.transform.position + Vector3.left * (index + 1) * gap;
		}
	}

	public Vector3 GetNextPos() {
		if(rightDir) {
			return this.transform.position + Vector3.right * (_curSlot + 1) * gap;
		} else {
			return this.transform.position + Vector3.left * (_curSlot + 1) * gap;
		}
	}


	void Start(){
		//Initialize the empty Angry Point
		AsyncInitial();
	}

	void AsyncInitial() {
		fullObjects = new List<GameObject>();
		emptyObjects = new List<GameObject>();

		for(int i = 0;i<maxSlot;i++){
			emptyObjects.Add(BanTools.CreateNGUIOject(prefab_Empty,GetPos(i),Quaternion.identity,this.transform));
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		for(int i = 0;i<maxSlot;i++){
			Gizmos.DrawWireCube(GetPos(i),Vector3.one * 0.5f);
		}
	}

	void OnDestory() {
		base.dealloc();
		base.destoryGo(fullObjects);
		base.destoryGo(emptyObjects);
	}
}
