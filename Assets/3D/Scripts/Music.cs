using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public AudioSource FX_Sound;

	public static Music St_Self = null;

	//音阶
	public AudioClip[] gamut;

	//滚珠
	public AudioClip GunZhu;

	//BGM
	public AudioClip[] BGM;

	//回合开始
	public AudioClip turnStart;

	//combo结算
	public AudioClip comboCulcu;

	public const float FPS = 30;

	public const float SwapSpeed = 1;

	private int i_BGM = 0;

	private float f_OldVolume;

	private bool b_Minus = true;

	void Start() {
		St_Self = this;
		DontDestroyOnLoad(this);
		//Application.LoadLevel("Menu");
	}

	//void Update() {
	//      if (Input.GetKey(KeyCode.W)) {
	//            audio.volume += SwapSpeed * Time.deltaTime;
	//            Debug.Log("come here");
	//      } else if (Input.GetKey(KeyCode.S)) {
	//            audio.volume -= SwapSpeed * Time.deltaTime;
	//            Debug.Log("come her2");
	//      }
	//      if(Input.GetKeyDown(KeyCode.Alpha1)){
	//            Swap(0);
	//      } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
	//            Swap(1);
	//      } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
	//            Swap(2);
	//      }
	//}

	public void Play(AudioClip sound) {
		FX_Sound.PlayOneShot(sound);
	}

	/// <summary>
	/// 0:滚珠
	/// 1:combo结算
	/// </summary>
	/// <param name="i"></param>
	public void Play(int i) {
		switch (i) {
			case 0:
				FX_Sound.PlayOneShot(GunZhu);
				break;
			case 1:
				FX_Sound.PlayOneShot(comboCulcu);
				break;
		}
	}

	/// <summary>
	/// 播放音阶
	/// </summary>
	/// <param name="i"></param>
	public void PlayGamut(int i) {
		if (i < gamut.Length) {
			FX_Sound.PlayOneShot(gamut[i]);
		} else {
			FX_Sound.PlayOneShot(gamut[gamut.Length - 1]);
		}
	}

	//更换背景音乐
	public void Swap(int i) {
		if(IsInvoking("Func")){
			return;
		}
		if (i == i_BGM) {
			return;
		} else {
			i_BGM = i;
			f_OldVolume = this.audio.volume;
			b_Minus = true;
			InvokeRepeating("Func", 0.01f, 1 / FPS);
		}
	}

	void Func() {
		if (b_Minus) {
			this.audio.volume -= Time.deltaTime * SwapSpeed;
			if (this.audio.volume <= 0) {
				this.audio.volume = 0;
				audio.clip = BGM[i_BGM];
				audio.Play();
				b_Minus = false;
			}
		} else {
			this.audio.volume += Time.deltaTime * SwapSpeed;
			if(this.audio.volume >= f_OldVolume){
				this.audio.volume = f_OldVolume;
				CancelInvoke("Func");
				b_Minus = true;
			}
		}
	}

}
