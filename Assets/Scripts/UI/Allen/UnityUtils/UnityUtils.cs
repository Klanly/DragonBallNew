using UnityEngine;
using System.Collections;

namespace AW.Utils {
    public class UnityUtils {

        //3D 显示区域的判定
        public static bool inScreenRect(GameObject aGo, Camera camera, Vector3 touch) {

            float left = float.MaxValue;
            //右边为Z最小
            float right = float.MinValue;
            //上面为Y最小
            float up = float.MinValue;
            //下面为Y最大
            float down = float.MaxValue;

            if (left > aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x)
            {
                //左边记录z最小值
                left = aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x;
            }

            //如果左边 小于  当前网格模型中心坐标.z + 当前网格宽度.z 
            if (right < aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x)
            {
                //右边记录z最大值
                right = aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x;
            }

            //如果上边 大于  当前网格模型中心坐标.y + 当前网格高度.y 
            if (up < aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y)
            {
                //上边记录y最大值
                up = aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y;
            }

            //如果上边 大于  当前网格模型中心坐标.y + 当前网格高度.y 
            if (down > aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y)
            {
                //下边记录y最小值
                down = aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y;
            }


            Vector3 leftBottom = new Vector3(left, down, aGo.renderer.bounds.center.z);
            Vector3 ScreenLeftBottom = camera.WorldToScreenPoint(leftBottom);

            Vector3 rightUp = new Vector3(right, up, aGo.renderer.bounds.center.z);
            Vector3 ScreenRightUp = camera.WorldToScreenPoint(rightUp);

            bool inArea = false;
            if(touch.x <= ScreenRightUp.x && touch.x >= ScreenLeftBottom.x && touch.y <= ScreenRightUp.y && touch.y >= ScreenLeftBottom.y) {
                inArea = true;
            }
            return inArea;

        }

        /// <summary>
        /// NGUI 的2D UI判定方法
        /// </summary>
        /// <returns><c>true</c>, if screen rect was ised, <c>false</c> otherwise.</returns>
        /// <param name="sprite">Sprite.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="touch">Touch.</param>
        public static bool isInScreenRect(UIWidget sprite, Camera camera, Vector3 touch) {
            bool inArea = false; 

            Vector3 pos = sprite.transform.position;
            pos = camera.WorldToScreenPoint(pos);

            float halfWidth = sprite.width * 0.5f;
            float halfHeight = sprite.height * 0.5f;

            float left = pos.x - halfWidth;
            float right = pos.x + halfWidth;
            float top = pos.y - halfHeight;
            float bottom = pos.y + halfHeight;

            if(touch.x <= right && touch.x >= left && touch.y <= bottom && touch.y >= top) {
                inArea = true;
            }

            return inArea;
        }

    }
}