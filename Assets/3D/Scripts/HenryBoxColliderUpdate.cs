using UnityEngine;
using System.Collections;

/// <summary>
/// Herny box collider update. 
/// </summary>
namespace Henry
{
    public class HenryBoxColliderUpdate : MonoBehaviour
    {
        BoxCollider bc;
        /// <summary>
        /// 准备附加boxcollider的游戏对。如UIlabel
        /// </summary>
        public GameObject obj;
        private UILabel label;
        private UISprite uispirte;
        private float centerOrgy;
        // Use this for initialization
        void Awake()
        {
           
            bc = this.gameObject.GetComponent<BoxCollider>();
           
            if (obj != null)
            {
                label = obj.GetComponent<UILabel>();

                if (label == null)
                    uispirte = obj.GetComponent<UISprite>();
            }


            centerOrgy = bc.center.y;     
        }

        private Vector3 temp = Vector3.one;
        void FixedUpdate(){
            updateBoxCollider();
        }


        void updateBoxCollider()
        {
            if (bc != null)
            {
                if (label != null)
                {
                    temp.y = label.height;
                    temp.x = label.width;
                } else if (uispirte != null)
                {
                    temp.y = uispirte.height;
                    temp.x = uispirte.width;
                }

                bc.size = temp;
        
              
                bc.center = new Vector3(bc.center.x, -this.transform.localPosition.y + centerOrgy, bc.center.z);
            }   
        }

        void Start()
        {
        
        }
        
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}