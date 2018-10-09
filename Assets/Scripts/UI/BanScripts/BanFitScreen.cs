using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//适应屏幕
public class BanFitScreen : MonoBehaviour {

  public bool b_ShowBlack = true;
	public static BanFitScreen Instance = null;

	public const float F_Standard_Height_Divide_Width = 9f/16;

	public const float F_Cull_Plane_Width = 1f;

	public const float F_Distance_To_Plane = 0.1f;

	public bool PresentInstance = true;

	public int layer = 0;

	//显示上方
	public bool b_ShowUpper = true;

	//显示下方
	public bool b_ShowBelow = true;

	public bool b_ShowLeft = false;
	
	public bool b_ShowRight = false;

	//需要调整位置的物体
	public GameObject [] obj;

	//以下方为锚点需要调整的物体
	public GameObject [] obj_Bottom;

	private float [] posRate;

	//旧的最高点
	private float oldTop;

	//旧的最低点
	private float oldDown;

	//新的最高点
	private float newTop;

	//新的最低点
	private float newDown;

	//设置全局的点
	private GameObject go_Gobal;

	//计算上面和下面
	void CalculateTopDown(ref float top,ref float down){
		top = this.camera.ScreenToWorldPoint( new Vector3(0,Screen.height,0) ).y;
		down = this.camera.ScreenToWorldPoint( new Vector3(0,0,0) ).y;
	}

	//计算位置比例
	float CalculatePosRate(float pos,float top,float down){
		return (top - pos) / (top - down);
	}

	//根据比例计算位置
	float CalculatePos(float rate,float top,float down){
		return top - rate * (top - down );
	}

	//一次便能够适应完成 (1是旧的,2是新的)
	public void FitOnce( ref Vector3 pos,ref Vector3 scale ,float top1,float down1,float top2,float down2){
		//Pos
		float posYRate = CalculatePosRate(pos.y,top1,down1);
		pos.y = CalculatePos(posYRate,top2,down2);
		//Scale
		scale = Mathf.Abs(top2 - down2) / Mathf.Abs(top1 - down1) * scale;
	}

	//给上屏用的
	public float GetScale(){
		float temp = 5.04f;
		return 	(Mathf.Abs(newTop - newDown) - temp) / 
				(Mathf.Abs(oldTop - oldDown) - temp);
	}

	//给红框用的
	public float GetScale2(){
		float temp = 0f;
		float result = (Mathf.Abs(newTop - newDown) - temp) / 
						(Mathf.Abs(oldTop - oldDown) - temp);
		//Debug.Log("result:"+result);
		return 	result;
	}

	[System.NonSerialized]
	public float mainResult = 0;
		
	//主功能
    void MainFunction(){
		go_Gobal = GameObject.FindGameObjectWithTag("Gobal");

        mainResult = ((float) Screen.height/Screen.width) / F_Standard_Height_Divide_Width;
        if(mainResult == 1){
            //DoNothing
        } else {
            //generate plane up and down
            if(mainResult > 1){
                if(layer >= 0){
                    //down
                    if(b_ShowBelow){
                        Vector3 pos = this.camera.ScreenToWorldPoint(new Vector3(Screen.width/2,0,this.camera.nearClipPlane + F_Distance_To_Plane));

                        GameObject aBlack = BanTools.CreateObject(BanTools.prefab_Black);

                        aBlack.layer = layer;
						aBlack.transform.localScale = new Vector3(F_Cull_Plane_Width,F_Cull_Plane_Width ,F_Cull_Plane_Width/10);
                        aBlack.transform.position = pos + aBlack.renderer.bounds.extents.y * Vector3.down;
                        //if(go_Gobal != null) aBlack.transform.parent = go_Gobal.transform;
                        aBlack.gameObject.name = gameObject.transform.name + "Block";
                        aBlack.SetActive (b_ShowBlack);
                    }
                    //up
                    if(b_ShowUpper){
                        Vector3 pos = this.camera.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height,this.camera.nearClipPlane + F_Distance_To_Plane));

                        GameObject aBlack = BanTools.CreateObject(BanTools.prefab_Black);

                        aBlack.layer = layer;
						aBlack.transform.localScale = new Vector3(F_Cull_Plane_Width,F_Cull_Plane_Width,F_Cull_Plane_Width/10);
                        aBlack.transform.position = pos + aBlack.renderer.bounds.extents.y * Vector3.up;
                        //if(go_Gobal != null) aBlack.transform.parent = go_Gobal.transform;
                        aBlack.gameObject.name = gameObject.transform.name + "Block";
                        aBlack.SetActive (b_ShowBlack);
                    }
                }
                //Change Camera Size
                if(this.camera.orthographic){
                    this.camera.orthographicSize *= mainResult;
                }
            }else{
                if(layer >= 0){
                    if(b_ShowLeft){
                        //Left
                        Vector3 pos = this.camera.ScreenToWorldPoint(new Vector3( Screen.width/2*(1-mainResult) ,Screen.height/2,this.camera.nearClipPlane + F_Distance_To_Plane));

                        GameObject aBlack = BanTools.CreateObject( BanTools.prefab_Black);

                        aBlack.layer = layer;
                        aBlack.transform.position = pos;
						aBlack.transform.localScale = new Vector3(F_Cull_Plane_Width,F_Cull_Plane_Width,F_Cull_Plane_Width/10);
                        aBlack.transform.position = pos + aBlack.renderer.bounds.extents.x * Vector3.left;
                        // if(go_Gobal != null) aBlack.transform.parent = go_Gobal.transform;aBlack.SetActive (b_ShowBlack);
                    }
						
                    if(b_ShowRight){
                        //Right
                        Vector3 pos = this.camera.ScreenToWorldPoint(new Vector3( Screen.width - Screen.width/2*(1-mainResult) ,Screen.height/2,this.camera.nearClipPlane + F_Distance_To_Plane));

                        GameObject aBlack = BanTools.CreateObject(BanTools.prefab_Black);

                        aBlack.layer = layer;
						aBlack.transform.localScale = new Vector3(F_Cull_Plane_Width,F_Cull_Plane_Width,F_Cull_Plane_Width/10);
                        aBlack.transform.position = pos + aBlack.renderer.bounds.extents.x * Vector3.right;
                        //if(go_Gobal != null) aBlack.transform.parent = go_Gobal.transform;
                        aBlack.SetActive (b_ShowBlack);
                    }
						
                }
            }
        }
	}

    void ClearGlobal()
    {
        int count = go_Gobal.transform.childCount;
        List<GameObject> tempObj = new List<GameObject> ();
        for (int i = 0; i < count; i++) {
            Transform cd = go_Gobal.transform.GetChild (i);
            if (cd.name.Contains ("Black")) {
                //cd.gameObject.SetActive (false);
                tempObj.Add (cd.gameObject);
            }
        }

        while (tempObj.Count > 0) {
            GameObject.Destroy (tempObj [0]);
            tempObj.RemoveAt (0);
        }
    }

	//位置初始化
	void InitPos(){
		//Debug.Log("InitPos");
		if(obj != null && obj.Length > 0){
			posRate = new float[obj.Length];
			for(int i = 0;i<obj.Length;i++){
				posRate[i] = CalculatePosRate(obj[i].transform.position.y,oldTop,oldDown);
			}
		}
	}

	//位置校对
	void FitPos(){
		if(obj != null && obj.Length > 0){
			for(int i = 0;i<obj.Length;i++){
				GameObject go = obj[i];
				Vector3 oldPos = go.transform.position;
				float targetY = CalculatePos(posRate[i],newTop,newDown);
				oldPos.y = targetY;
				obj[i].transform.position = oldPos;
			}
		}
	}

	//校对底部位置
	void FitPosBottom(){
		
		if(obj_Bottom != null && obj_Bottom.Length > 0 ){
			for(int i = 0;i<obj_Bottom.Length;i++){
				GameObject go = obj_Bottom[i];
				go.transform.position += new Vector3(0,newDown - oldDown ,0);
			}
		}
	}

	private bool inited = false;

	public void Init(){
		
		Camera camera = GetComponent<Camera>();
		if(camera !=null)
		{
			if(camera !=null && !camera.isOrthoGraphic)
			{		
				camera.fieldOfView *=1136f/(640f/(float)Screen.height*(float)Screen.width);
			}
			else
			{
				if(inited == false) {
					if(this.camera != null){
						CalculateTopDown(ref oldTop,ref oldDown);
						InitPos();
		                MainFunction();
						CalculateTopDown(ref newTop,ref newDown);
						FitPos();
						FitPosBottom();
					}
					inited = true;
				}
			}
		}
	}

	void Awake(){
        if(PresentInstance){
            Instance = this;
        }
        Init();
	}

}
