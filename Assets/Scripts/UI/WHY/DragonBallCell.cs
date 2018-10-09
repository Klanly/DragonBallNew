using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DragonBallCell : MonoBehaviour {

	public int ballIndex;

    public UIWidget[] Widgets;

    private float ShowTime = 0.3f;

	public void qiangDuoDragonBall()
	{
        //  List<Soul> items = null;
        int Number = 0;

        if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon) {
            Number = 150001 + ballIndex;
        } else if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon) {
            Number = 150008 + ballIndex;
		}

        ComLoading.Open();

		Core.Data.temper.DragonballNum = Number;

        Core.Data.dragonManager.isGetQiangDuoDragonOpponentsCompleted = false;
        Core.Data.dragonManager.isGetSuDiListCompleted = false;
        Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Clear ();

        Core.Data.dragonManager.getQiangDuoDragonBallOpponentsRequest(Number);
		
	}


    public void showFull() {
        if(Widgets != null && Widgets.Length > 0) {

            Color colorFrom = new Color(1.0f, 1.0f, 1.0f , 0.5f);
            Color colorTo = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            // V4 from = new V4( colorFrom );
            //    V4 to = new V4( colorTo );

            foreach(UIWidget widget in Widgets) {
                if(widget != null) {
                    TweenColor.Begin(widget.gameObject, ShowTime, colorFrom, colorTo);
                    //MiniItween.ColorFromTo(widget.gameObject, from, to, ShowTime, MiniItween.EasingType.Linear, MiniItween.Type.ColorWidget);
                }
            }
        }
    }

    public void HideFull() {
        if(Widgets != null && Widgets.Length > 0) {
            Color colorTo = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            foreach(UIWidget widget in Widgets) {
                if(widget != null) {
                    TweenColor.Begin(widget.gameObject, ShowTime, colorTo );
                    //MiniItween.ColorTo(widget.gameObject, new V4(new Color(1.0f, 1.0f, 1.0f, 0.0f)), ShowTime, MiniItween.Type.ColorWidget);
                }
            }
        }
    }

}
