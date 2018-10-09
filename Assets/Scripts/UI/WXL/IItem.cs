using UnityEngine;
using System.Collections;
/// <summary>
/// Interface item.
/// </summary>
  interface IItem {
	
	void SetItemValue(object obj);
	
	object ReturnValue();
	
	void Refresh();
	
}
