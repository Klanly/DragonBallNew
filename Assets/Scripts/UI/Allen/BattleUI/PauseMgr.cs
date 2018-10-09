using UnityEngine;
using System.Collections;

public class PauseMgr : MonoBehaviour {

	public PausePanelMgr pausePanelView;

	/// <summary>
	/// 暂停游戏
	/// </summary>
	public void OnPauseClick() {
		pausePanelView.gameObject.SetActive(true);
		pausePanelView.init();

		Time.timeScale = 0f;
		TimeMgr.getInstance().WarPause = true;
	}
}
