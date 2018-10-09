using UnityEngine;
using System.Collections;

public class CRLuo_FX_Sound_Rand : MonoBehaviour {
	public SoundsFx_Element [] RandSoundGroup ;
	int i ;
	int TryNum = 0;
	// Use this for initialization
	void Start () {

		if (RandSoundGroup != null && RandSoundGroup.LongLength != 0) {
			PlayGo ();
		}
	}


	void PlayGo() { 
        if (TryNum > 5) {
			Debug.Log(this.gameObject.name + "FX No Set Sound ");
		}
		i = (int)Random.Range (0, RandSoundGroup.LongLength);
		if (RandSoundGroup [i].Sound_Key) {
			Invoke ("PlaySound", RandSoundGroup [i].StartTime);
		} else {
			TryNum++;
            PlayGo ();
		}
	}

	void PlaySound() {
		Core.Data.soundManager.SoundFxPlay(RandSoundGroup[i].sounds);
	}
}
