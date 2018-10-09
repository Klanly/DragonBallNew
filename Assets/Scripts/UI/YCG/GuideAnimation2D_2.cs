using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
//yangchenguang
public class GuideAnimation2D_2 : MonoBehaviour 
{


		public List<UISprite> listSprite2D = new List<UISprite>() ;
		Vector3 anima_2  = new Vector3(98,190,0) ;  //动画第二张图片位置
		Vector3 anima_3  = new Vector3 (407 , 158 ,0) ;// 动画第三张图片位置
		Vector3 anima_4  = new Vector3(117,-186,0) ; 
		Vector3 anima_4_1  = new Vector3(-319,-186,0) ; 
		Vector3 anima_5 = new Vector3(94,-120,0);
		Vector3 anima_6 = new Vector3(406,-157,0);

		public static GameObject _instence  ;

		public System.Action OnFinished  ;

		// Use this for initialization
		void Start () 
		{
				listSprite2D[4].gameObject.SetActive(false);
		}
		//
		public static GuideAnimation2D_2 OpenUI(Transform tfParent)
		{
				if (_instence != null ) return null  ;
				Object obj  = PrefabLoader.loadFromPack("YCG/GuideAnimation2D_2");

				if(obj != null)
				{
						_instence = Instantiate(obj) as GameObject;
						GuideAnimation2D_2 cc = _instence.GetComponent<GuideAnimation2D_2>();

						RED.AddChild (_instence ,tfParent.gameObject);


						_instence.layer = 	tfParent.gameObject.layer ; 
						return cc;
				}
				return null;
		}
		//动画一 缩 放完成
		public void animation_1()
		{
				//Debug.Log("动画一 缩 放完成 "); 
				Invoke("animation_2" ,0.4f); 
		}
		public void animation_2()
		{
				//Debug.Log("动画二 开始 ") ; 

		}
	
		public void animation_4()
		{
			
				GameObject go  = 	listSprite2D[3].gameObject ;

				MiniItween.MoveTo(go.gameObject, anima_4, 0.3f).myDelegateFunc += animation_4_1;
		}
		void animation_4_1()
		{
				GameObject go  = 	listSprite2D[3].gameObject ;

				MiniItween.MoveTo(go.gameObject, anima_4_1, 0.2f).myDelegateFunc += animation_4_1End;	
		}
		void  animation_4_1End()
		{
				Invoke("animation_5_Wait" , 0.1f);

		}
		void animation_5_Wait()
		{
				listSprite2D[4].gameObject.SetActive(true);
		}

		void animation_5()
		{
				//Debug.Log("动画五结束");
				//Debug.Log("动画六开始");


				GameObject go2  =	listSprite2D[1].gameObject;
				GameObject go3  =	listSprite2D[2].gameObject;
				MiniItween.Shake(go2,new Vector3(5f,5f,0) ,0.2f, MiniItween.EasingType.EaseInCubic,false ); 
				MiniItween.Shake(go3,new Vector3(5f,5f,0) ,0.2f, MiniItween.EasingType.EaseInCubic,false) ; 


				Invoke("animation_6_Wait" , 0.4f);

		}
		void animation_6_Wait()
		{
				listSprite2D[5].gameObject.SetActive(true);
		}
		void animation_6()
		{

				GameObject go3  =	listSprite2D[2].gameObject;
				GameObject go5  =	listSprite2D[4].gameObject;
				MiniItween.Shake(go5,new Vector3(5f,5f,0) ,0.2f, MiniItween.EasingType.EaseInCubic,false ); 
				MiniItween.Shake(go3,new Vector3(5f,5f,0) ,0.2f, MiniItween.EasingType.EaseInCubic,false) ; 

				//Debug.Log("动画结束");

				Invoke("animation_Mask_Wait" , 1f); 
		}
		void animation_Mask_Wait()
		{
				listSprite2D[6].gameObject.SetActive(true);
		}
		void animation_Mask()
		{
		        if (OnFinished !=null )
				{
			        OnFinished();
				}
				closeUI();
		}
		public void closeUI()
		{
				Destroy(gameObject);
		}
		void OnDestroy()
		{
				_instence = null ;
		}

}
