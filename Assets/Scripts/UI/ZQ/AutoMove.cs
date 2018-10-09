using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour 
{
	public Transform paths;
	public float m_nSpeed = 0.05f;

	private Transform[] points;
	public Transform nextTarget;
	int m_nIndex;
	bool  m_bDirection;

	void Start()
	{
		points = new Transform[paths.childCount];
		for (int i = 0; i < paths.childCount; i++)
		{
			points [i] = paths.FindChild (i.ToString ());
		}

		this.transform.position = points [0].position;
		nextTarget = points [1];
		m_nIndex = 1;
	}


	void Update()
	{
		if (Vector3.Distance (transform.position, nextTarget.position) <= m_nSpeed * 1.5f)
		{
			if (m_bDirection)
			{
				m_nIndex++;
				if (m_nIndex >= points.Length )
				{
					m_bDirection = false;
					m_nIndex -= 2;
				}
			}
			else
			{
				m_nIndex--;
				if (m_nIndex < 0)
				{
					m_bDirection = true;
					m_nIndex += 2;
				}
			}

			nextTarget = points [m_nIndex];
		}

		this.transform.LookAt (nextTarget);
		Vector3 direction = Vector3.Normalize(nextTarget.position - this.transform.position);
		transform.Translate (direction * Time.deltaTime * m_nSpeed, Space.World);
	}
}
