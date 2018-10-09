using UnityEngine;
using System.Collections;

public class CRLuo_FX_Sound : MonoBehaviour {
	public bool Sound_Key;
	public SoundFx sounds;
	public float StartTime = 0;
	// Use this for initialization
	void Start () {

		if (Sound_Key) {
			Invoke("PlaySound",StartTime);
		}
	
	}


	void PlaySound()
	{
		if (Sound_Key)
		{
            if(Core.Data != null && Core.Data.soundManager != null)
    			Core.Data.soundManager.SoundFxPlay(sounds);
		}
	}
}
