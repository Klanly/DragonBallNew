using UnityEngine;
using System.Collections;


/// <summary>
/// 原本用于展示“怒气技”三个文字，现已经没用了。
/// </summary>
public class ShowAngry : MonoBehaviour {

	public GameObject AngryWord;
	public UISprite[] Items;
	//初始的大小
	public Vector3 InitialScale;
	//初始的Alpha
	public float InitAlpha;
	public float time;

	// Use this for initialization
	void Awake () {
		AngryWord.SetActive(false);
	}

	public IEnumerator showWord(int wordLength) {
		AngryWord.SetActive(true);

		foreach(UISprite item in Items) {
			item.transform.localScale = InitialScale;
			item.alpha = InitAlpha;

			TweenAlpha.Begin(item.gameObject, time, 1.0f);
			TweenScale.Begin(item.gameObject, time, Vector3.one);
		}

		float totalTime = wordLength * 0.2f + 1f;
		yield return new WaitForSeconds(totalTime);
		AngryWord.SetActive(false);
	}

}
