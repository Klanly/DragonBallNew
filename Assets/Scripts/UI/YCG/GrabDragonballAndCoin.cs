using UnityEngine;
using System.Collections;

public class GrabDragonballAndCoin : MonoBehaviour
{
    //
    public GameObject _GrodCoin; 
    public UILabel _labCoin ; 
    public UILabel _labStone; 
	// Use this for initialization
	void Start () 
    {
       
	}
    public void GrodCoin(bool isValue)
    {
        _GrodCoin.SetActive(isValue);
    }

}
