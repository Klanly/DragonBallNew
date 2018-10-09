using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SQYDataManager{



	public List<string> lstProductID = new List<string>();
	public List<string> lstRecepit = new List<string>();

	private static SQYDataManager dmInstance = null;
	public static SQYDataManager getInstance()
	{
		if(dmInstance == null)
		{
			dmInstance = new SQYDataManager();
		}
		return dmInstance;
	}
	private SQYDataManager()
	{
	}
}
