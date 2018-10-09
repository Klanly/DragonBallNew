using UnityEngine;
using System.Collections;

public class RoleIcon : MonoBehaviour {

	public UISprite headIcon;

	public GameObject[] stars;

	public float w = 17;

	public void setStars(int count)
	{
		float startX = (count / 2) * - w;
		if (count % 2 == 0)
		{
			startX += w / 2;
		}

		for (int i = 0; i < stars.Length; i++)
		{
			RED.SetActive (i < count, stars [i].gameObject);
			stars[i].transform.localPosition = new Vector3(startX + w * i ,stars[i].transform.localPosition.y, stars[i].transform.localPosition.z);
		}
	}
}
