using UnityEngine;
using System.Collections;

public class ProduceMoneyTip : MonoBehaviour {

	public TextMesh[] txt_Coin;
	public TextMesh[] txt_Dia;
	public GameObject coinObj;
	public GameObject diaObj;
	public float upLength;
	Color txtYellowColor = new Color(253f/255f,236f/255f,0,1f);
	Color txtBlackColor = new Color (0, 0, 0, 1f);
	Color txtBAlphaColor = new Color (0, 0, 0, 0);
	Color txtYAlphaColor = new Color(253f/255f,236f/255f,0,1f);
	float c_Alpha =1;
	float d_Alpha =1;
//	public float a_interval =0.1f;
//	public float t_BeginTime = 0.1f;
	//int[coin,diamond]
	public void InitInfo(int[] tMoney){
		diaObj.SetActive (false);
		if (tMoney != null) {
			if (tMoney.Length>=2) {
				if (tMoney [0] != 0) {
//					coinObj.gameObject.SetActive (true);
					if (txt_Coin != null) {
						txt_Coin [0].text = "+" + tMoney [0].ToString ();
						txt_Coin [1].text = "+" + tMoney [0].ToString ();
					} else {
						if (coinObj != null) {
							txt_Coin [0] = coinObj.GetComponent<TextMesh> ();
							txt_Coin [1] = coinObj.transform.FindChild ("CoinBg").GetComponent<TextMesh> (); 
						} else {
							txt_Coin [0] = gameObject.transform.FindChild("CoinMoney").GetComponent<TextMesh> ();
							txt_Coin [1] = gameObject.transform.FindChild ("CoinBg").GetComponent<TextMesh> (); 
						}
						txt_Coin [0].text = "+" + tMoney [0].ToString ();
						txt_Coin [1].text = "+" + tMoney [0].ToString ();
					}
				}
				if (tMoney [1] != 0 ) {
//					diaObj.gameObject.SetActive (true);
					if (txt_Dia != null) {
						txt_Dia [0].text = "+" + tMoney [1].ToString ();
						txt_Dia [1].text = "+" + tMoney [1].ToString ();
					} else {
						if (coinObj != null) {
							txt_Dia [0] = diaObj.GetComponent<TextMesh> ();
							txt_Dia [1] = diaObj.transform.FindChild ("DiaBg").GetComponent<TextMesh> (); 
						} else {
							txt_Dia [0] = gameObject.transform.FindChild("DiaMoney").GetComponent<TextMesh> ();
							txt_Dia [1] = gameObject.transform.FindChild ("DiaBg").GetComponent<TextMesh> (); 
						}
						txt_Dia [0].text = "+" + tMoney [1].ToString ();
						txt_Dia [1].text = "+" + tMoney [1].ToString ();
					}
				}
			}
		}

		ShowCoinEffect ();
	}

	//飞升方法 
	void ShowCoinEffect(){
		gameObject.SetActive (true);
		TweenScale.Begin (gameObject,0.1f,Vector3.one) ;
		Invoke ("ShowDiaEffect",0.1f);
	}

	void ShowDiaEffect(){
		MiniItween.MoveTo (coinObj.gameObject,coinObj.transform.localPosition + upLength*Vector3.up,0.8f );	
		MiniItween.MoveTo (diaObj.gameObject,diaObj.transform.localPosition + upLength*Vector3.up,0.8f );
		MiniItween.ColorTo (coinObj.gameObject,new V4(1,1,1,0),0.8f,MiniItween.Type.Color);
		MiniItween.ColorTo (diaObj.gameObject,new V4(1,1,1,0),0.8f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Coin[1].gameObject,new V4(txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,0),0.8f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Dia[1].gameObject,new V4(txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,0),0.8f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Coin[0].gameObject,new V4(txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,0),0.8f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Dia[0].gameObject,new V4(txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,0),0.8f,MiniItween.Type.Color).myDelegateFunc += HideEffect;
		Invoke ("HideEffect",1f);
	}

//	void CAlphaTo( ){
//		if (c_Alpha > 0) {
//			c_Alpha -= 0.15f;
//		} else {
//			c_Alpha = 0;
//			CancelInvoke ("CAlphaTo");
//		}
//
//		txt_Coin [0].color = new Color (txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,c_Alpha);
//		txt_Coin [1].color = new Color (txtBlackColor.r,txtBlackColor.g,txtBlackColor.b,c_Alpha);
//
//	}
//	void DAlphaTo( ){
//		if (d_Alpha > 0) {
//			d_Alpha -= 0.15f;
//		} else {
//			d_Alpha = 0;
//			CancelInvoke ("DAlphaTo");
//		}
//		txt_Dia [0].color = new Color (txtYellowColor.r,txtYellowColor.g,txtYellowColor.b,d_Alpha);
//		txt_Dia [1].color = new Color (txtBlackColor.r,txtBlackColor.g,txtBlackColor.b,d_Alpha);
//	}

	void HideEffect(){

		gameObject.transform.localScale = Vector3.zero;
		MiniItween.ColorTo (txt_Coin[0].gameObject,new V4(txtYellowColor),0.05f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Dia[0].gameObject,new V4(txtYellowColor),0.05f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Coin[1].gameObject,new V4(txtBlackColor),0.05f,MiniItween.Type.Color);
		MiniItween.ColorTo (txt_Dia[1].gameObject,new V4(txtBlackColor),0.05f,MiniItween.Type.Color);
//		txt_Dia [1].color = txtBlackColor;
//		txt_Coin [1].color = txtBlackColor;
		coinObj.renderer.material.color = Color.white;
		diaObj.renderer.material.color = Color.white;
		diaObj.gameObject.transform.localPosition = Vector3.up*0.7f;
		coinObj.gameObject.transform.localPosition = Vector3.zero;
	}





}
