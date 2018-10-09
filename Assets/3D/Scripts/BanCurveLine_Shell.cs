using UnityEngine;
using System.Collections;

public class BanCurveLine_Shell : MonoBehaviour
{

	public GameObject CreateOBJ;
	[HideInInspector]
	public GameObject GoPos;
	public int AllNum;
	public float LongTime;

	void CreateBanLine()
	{
		GameObject temp_Screen = EmptyLoad.CreateObj(CreateOBJ, this.gameObject.transform.position, this.gameObject.transform.rotation);
		temp_Screen.GetComponent<BanCurveLine>().target = GoPos;
	}

	public void GoCarete()
	{
		if (CreateOBJ != null && GoPos != null)
		{
			for (int i = 0; i < AllNum; i++)
			{
				if (LongTime == 0)
				{
					CreateBanLine();
				}
				else
				{
					Invoke("CreateBanLine", Random.Range(0, LongTime));

				}

			}


		}
	}

}
