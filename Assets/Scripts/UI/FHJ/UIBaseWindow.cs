using UnityEngine;
using System.Collections;

public class UIBaseWindow : MonoBehaviour
{

    void Awake()
    {
        InitWidget();
    }
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        OnUpdate();
	}

    protected virtual void OnUpdate()
    {

    }

    protected virtual void InitWidget()
    {

    }
}
