using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SingleAudioItem {
	//每个延时播放的声音
	[SerializeField]
	public AudioClip clip;
	//延迟的时间
	[SerializeField]
	public float AudioDelay;
}

/// <summary>
/// 这个可以提供单一的声源，但是有多个声音（声效不会重叠）
/// </summary>
public class UISingleSource : MonoBehaviour {
	[SerializeField]
	public SingleAudioItem[] SoundList;
    //动态生成声源
    private AudioSource innerSource;

    private int layer = 0;
    void Awake() {
		if(SoundList != null && SoundList.Length > 0) {
            innerSource = gameObject.AddComponent<AudioSource>();
        }
        layer = 0;
    }

    void Start() {
		if(SoundList != null && SoundList.Length > 0) {
			foreach(SingleAudioItem item in SoundList) {
				if(item != null) {
					Invoke("PlayShot", item.AudioDelay);
				}
                
            }
        }
    }

    void PlayShot() {
		if (SoundList != null && layer < SoundList.Length) {
			innerSource.clip = SoundList[layer ++].clip;
			innerSource.Play();
		}

    }

}
