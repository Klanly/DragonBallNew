using UnityEngine;
using System.Collections;

public class MouseEventFilter
{
	public static MouseEventFilter mInstance;
	public static MouseEventFilter GetInstance()
	{
		if (mInstance == null) 
		{
			mInstance = new MouseEventFilter();
		}
		return mInstance;
	}

	private MouseEventFilter()
	{

	}


}


