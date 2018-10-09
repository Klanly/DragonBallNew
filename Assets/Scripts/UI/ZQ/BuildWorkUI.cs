using UnityEngine;
using System.Collections;

public class BuildWorkUI : MonoBehaviour 
{
	//建筑升级冒烟
	public ParticleSystem m_particle;

	void Start()
	{
		RED.SetActive (false, m_particle.gameObject);
	}
		
	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds (3);
		Destroy (this.gameObject);
	}
	
	public void PlayAnim()
	{
		m_particle.enableEmission = true;
		StartCoroutine("DelayDestroy");
	}
}
