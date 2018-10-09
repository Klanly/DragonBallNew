using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 这个可以提供单一的声音Clip,但是会同时有多个声源（声效会重叠）
/// </summary>
public class UISingleSound : MonoBehaviour {
    public AudioClip clip;
    //延迟的时间
    public float[] AudioDelay;
    private List<AudioSource> innerSource;

    private int layer = 0;

    void Awake() {
        if(AudioDelay != null) {
            innerSource = new List<AudioSource>();

            int length = AudioDelay.Length;
            for(int i = 0; i < length; ++ i) {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                innerSource.Add(source);
            }
        }
    }

    void Start() {
        if(AudioDelay != null) {
            foreach(float delay in AudioDelay) {
                Invoke("PlayShot", delay);
            }
        }
    }

    void PlayShot() {
        innerSource[layer].clip = clip;
        innerSource[layer ++].Play();
    }

}
