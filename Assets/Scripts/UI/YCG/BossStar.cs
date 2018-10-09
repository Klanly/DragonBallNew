using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BossStar : MonoBehaviour 
{

    public List<UISprite> listSp = new List<UISprite>();
	// Use this for initialization
	void Start ()
    {
	
	}
    public void initStar(int index)
    {
        for(int i =0 ; i < index ;i++)
        {
            listSp[i].color =Color.white;
        }


    }
	// Update is called once per frame
	void Update ()
    {
	
	}
}
