using UnityEngine;
using System;
using System.Collections;

public class ALaLei_Command : MonoBehaviour {


	void Command1()
	{
		switch (DateTime.Now.Hour) {
		case 0:
			ALaLei.Instance.Play ("哦呦呦！还没有睡觉啊～");
			break;
		case 1:
			ALaLei.Instance.Play ("必须休息了哦～");
			break;
		case 2:
			ALaLei.Instance.Play ("好巧啊！你也在上厕所吗？");
			break;
		case 3:
			ALaLei.Instance.Play ("睡不着了吗？");
			break;
		case 4:
			ALaLei.Instance.Play ("不要着凉呦～");
			break;
		case 5:
			ALaLei.Instance.Play ("天就要亮了呦～");
			break;
		case 6:
			ALaLei.Instance.Play ("太阳出来了！！");
			break;
		case 7:
			ALaLei.Instance.Play ("早安！");
			break;
		case 8:
			ALaLei.Instance.Play ("小吉刚才吃了一张桌子");
			break;
		case 9:
			ALaLei.Instance.Play ("我又撞翻一辆警车");
			break;
		case 10:
			ALaLei.Instance.Play ("看见猩猩博士了吗？");
			break;
		case 11:
			ALaLei.Instance.Play ("看见山吹老师了吗？");
			break;
		case 12:
			ALaLei.Instance.Play ("我饿了，有机油吃吗？");
			break;
		case 13:
			ALaLei.Instance.Play ("我是好孩子！");
			break;
		case 14:
			ALaLei.Instance.Play ("咕噼啵~");
			break;
		case 15:
			ALaLei.Instance.Play ("捅一捅，大便");
			break;
		case 16:
			ALaLei.Instance.Play ("大便厉害吗？");
			break;
		case 17:
			ALaLei.Instance.Play ("哎呦呦，怪兽呦～");
			break;
		case 18:
			ALaLei.Instance.Play ("太阳要去休息了吗？");
			break;
		case 19:
			ALaLei.Instance.Play ("扔高高喽～");
			break;
		case 20:
			ALaLei.Instance.Play ("为什么我没有胸部？");
			break;
		case 21:
			ALaLei.Instance.Play ("博士要睡觉喽！");
			break;
		case 22:
			ALaLei.Instance.Play ("好困啊~");
			break;
		case 23:
			ALaLei.Instance.Play ("月亮，晚安!");
			break;
		}
	}
}
