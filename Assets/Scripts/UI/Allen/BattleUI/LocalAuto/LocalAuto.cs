using UnityEngine;
using System.Collections;

public class LocalAuto : MonoBehaviour {

	//大的怒气技
	public GameObject AsIcon;
	//小的怒气技
	public GameObject AsIconEx;

	public SkillIcon Icon;

	public ShowSkillMgr AttackSkMgr;

	// Use this for initialization
	void Start () {
		float Y = this.transform.localPosition.y;

		#if LOCAL_AUTO
		AsIcon.gameObject.SetActive(false);
		AsIconEx.gameObject.SetActive(true);
		AttackSkMgr.Ang = Icon;

		this.transform.localPosition = new Vector3(-394F, Y, 0F);
		#else
		AsIcon.gameObject.SetActive(true);
		AsIconEx.gameObject.SetActive(false);

		this.transform.localPosition = new Vector3(-352F, Y, 0F);
		#endif

	}
	

}
