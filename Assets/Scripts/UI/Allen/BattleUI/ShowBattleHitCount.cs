using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// 
/// 展示OverSkill1之后的连击次数以及由此带来的金币
/// 
public class ShowBattleHitCount : MonoBehaviour {
    //个位
    public UISprite Count_1;
    //十位
    public UISprite Count_2;
    //独立的一位
    public UISprite Count_Single;

    //个位
    public UISprite Count_Hundred_1;
    //十位
    public UISprite Count_Hundred_2;
    //百位
    public UISprite Count_Hundred_3;

    public GameObject Hundred;

	public GameObject Decade;

    // -- 显示Sprite
    private List<UISprite> workingSprite = new List<UISprite>();
    private List<int> workingNum = new List<int>();

    //初始的大小
    public float Scale = 2.0f;

    public float ScaleTime = 0.12f;

	//金币的数量
	public UILabel CoinNum;
	//有时候这个金币不需要展示
	public GameObject GoCoin;

    //展示数字 --支持到百位
	public void showCount(int count, int price) {
		if(price == 0) GoCoin.SetActive(false);
		else GoCoin.SetActive(true);

        if(count >= 1000) count = 999;

        List<int> separate = new List<int>();
        int howmany = MathHelper.howMany(count, separate);

        workingSprite.Clear();
        workingNum.Clear();

        if(howmany == 1) {
			Decade.SetActive(false);
            Hundred.SetActive(false);
            Count_Single.gameObject.SetActive(true);

            workingSprite.Add(Count_Single);
            workingNum.Add(count);
        } else if(howmany == 2) {
            Count_Single.gameObject.SetActive(false);
            Hundred.SetActive(false);
			Decade.SetActive(true);
            Count_1.gameObject.SetActive(true);
            Count_2.gameObject.SetActive(true);

            workingSprite.Add(Count_1);
            workingSprite.Add(Count_2);
            workingNum.Add(count % 10);
            workingNum.Add(count / 10);
        } else if(howmany == 3) {
			Decade.SetActive(false);
            Count_Single.gameObject.SetActive(false);

            Hundred.SetActive(true);

			workingSprite.Add(Count_Hundred_1);
			workingSprite.Add(Count_Hundred_2);
			workingSprite.Add(Count_Hundred_3);
            
            workingNum = separate;
        }

        int length = workingSprite.Count;

        for(int i = 0; i < length; ++ i){
            UISprite sp = workingSprite[i];
            int number = workingNum[i];
            number = number == 0 ? 10 : number;

            sp.spriteName = "hit_" + number.ToString();
            //sp.MakePixelPerfect();

            sp.transform.localScale = new Vector3(Scale, Scale, Scale);
            MiniItween.ScaleTo(sp.gameObject, Vector3.one, ScaleTime);
        }

		showCoinCount(count * price);
    }

	//显示金币 --支持到6位数字
	void showCoinCount(int num) {
		CoinNum.text  = "+" + num.ToString();

		CoinNum.color = new Color(1f, 1f, 1f, 0f);
		CoinNum.transform.localScale = Vector3.one * 3;

		MiniItween.ScaleTo(CoinNum.gameObject, Vector3.one, 0.1f);
		MiniItween.ColorTo(CoinNum.gameObject, new V4(Color.yellow), 0.1f, MiniItween.Type.ColorWidget);
	}

	/*
	private int test = 0;
	void OnGUI() {
		if(GUI.Button(new Rect(0, 0, 100, 100), "test")){
			//showCount(++test);
			showCoinCount(++test);
		}
	}*/

}
