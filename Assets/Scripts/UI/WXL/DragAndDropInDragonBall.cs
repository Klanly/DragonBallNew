using UnityEngine;
using System.Collections;

public class DragAndDropInDragonBall : UIDragDropItem
{

    public GameObject parentObj;

    private Vector3 orgPos;
   
    protected override void OnDragDropRelease(GameObject surface)
    {
        if (mTrans.gameObject.GetComponent<AoYiSlot>().aoYi == null)
            return;

        if (surface != null)
        {
            //当前位置互换
            DragAndDropInDragonBall dds = surface.GetComponent<DragAndDropInDragonBall>();
            if (dds != null)
            {//判断 

                if (dds.gameObject.GetComponent<AoYiSlot>().aoYi != null)
                {
                    gameObject.transform.localPosition = surface.transform.localPosition;
                    surface.gameObject.transform.localPosition = orgPos;

                    this.ChangeAoyiPosInData(gameObject.GetComponent<AoYiSlot>().aoYi,surface.gameObject.GetComponent<AoYiSlot>().aoYi);

                    Debug.Log(" pos  " + surface.gameObject.GetComponent<AoYiSlot>().aoYi.Pos );
                }
                else
                {
                    gameObject.transform.localPosition = orgPos;
                }

            }
            else
            {
                gameObject.transform.localPosition = orgPos;
            }
             base.OnDragDropRelease(surface);
        }

        UIShenLongManager.Instance.gameObject.GetComponent<UIDragonAltar>().ReCollectAoYiSlot();
    }

    protected override void OnDragDropStart()
    {
        if (mTrans.gameObject.GetComponent<AoYiSlot>().aoYi == null)
            return;
        orgPos = mTrans.transform.localPosition;
        base.OnDragDropStart();
    }

//    protected override void OnDragDropMove(Vector3 delta)
//    {
//        if (mTrans.gameObject.GetComponent<AoYiSlot>().aoYi == null)
//            return;
//        base.OnDragDropMove(delta);
//    }

    void ChangeAoyiPosInData( AoYi curAoYi, AoYi targetAoYi){
        Debug.Log(" change aoyi  pos  = " + curAoYi.ID + " pos  == " + (targetAoYi.Pos +1) );
        if (curAoYi != null && targetAoYi.Pos != -1)
        {
            int tP = curAoYi.Pos;
            Core.Data.dragonManager.equipAoYiRequest(curAoYi.ID, targetAoYi.Pos+1 );
            curAoYi.RTAoYi.wh = targetAoYi.Pos + 1;
            targetAoYi.RTAoYi.wh = tP + 1;


            foreach(AoYi aoyiTemp in Core.Data.dragonManager.AoYiDic.Values)
            {
                if(aoyiTemp.Pos == targetAoYi.Pos)
                {
                    aoyiTemp.RTAoYi.wh = tP +1;
                }
                else if(aoyiTemp.AoYiDataConfig.ID == curAoYi.ID)
                {
                    aoyiTemp.RTAoYi.wh = targetAoYi.Pos + 1;
                }
            }
        }

    }
}
