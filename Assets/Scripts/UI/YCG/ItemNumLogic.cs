using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//yangcg 
public class ItemNumLogic : MonoBehaviour 
{
   // List<string>  listStr = new List<string>()["25_25" ,"32_25","44_25","55_25"];
	// Use this for initialization
	void Start ()
    {
	
	}
    //
    public static string setItemNum(int value ,UILabel NumLab, UISprite NumBg)
    {
        if (value <= 0)return  "";
        if (NumLab == null || NumBg == null )return  "";
        string[] listStr = {"25_25" ,"32_25","44_25","55_25"}; // 万以下的字符串显示
        string[] listStr1 = {"40_25" ,"50_25" , "55_25"};  // 万以上的字符串显示
        string strValue = value.ToString() ;
        float Num  = 0 ; 
        bool point =false ; // 是否有小数点
        if (strValue.Length > 4 )
        {
            if (value % 10000 > 0 ) point = true  ;
            Num = (float) value/10000 ;

            if (Num >= 10)
            {
                string[] str ;
                if(Num >= 100)
                {
                    str = listStr1[2].ToString().Split('_');
                }else
                {
                    str = listStr1[1].ToString().Split('_');
                }
                NumBg.width    = int.Parse(str[0]);
                NumBg.height   = int.Parse(str[1]);
               

            }else
            {
                string[] str ;
                if (point == true )
                {
                    str = listStr1[2].ToString().Split('_');

                }else
                {
                    str = listStr1[0].ToString().Split('_');

                }
                NumBg.width    = int.Parse(str[0]);
                NumBg.height   = int.Parse(str[1]);
            }
            NumLab.width  = 55 ;
            NumLab.height = 25 ;
            string[] strPoint;
            string strNum ;

            //yangcg 10月29日 小数点后保存一位
            if (point == true )
            {

                strPoint =Num.ToString().Split('.');
                if (strPoint != null  && strPoint.Length > 0 ){
                    if (strPoint.Length>=2)
                    {
                        strNum = strPoint[0] +"."+ strPoint[1][0];
                    }else
                    {
                        strNum = strPoint[0] ;
                    }

                    Num =  float.Parse(strNum);

                }
                 
            }


            strValue =Num.ToString() + Core.Data.stringManager.getString(6117);


            //return strValue ;


        }
        else
        {
            int  index = strValue.Length -1 ;

//            if (index >= 4 )
//            {
//                index = 3;
//            }
            string[] str = listStr[index].ToString().Split('_');

            NumLab.width  = int.Parse(str[0]);
            NumLab.height = int.Parse(str[1]);

            NumBg.width    = int.Parse(str[0]);
            NumBg.height   = int.Parse(str[1]);


            strValue = value.ToString()  ;



            //return strValue;

        }



        return strValue ; 

      
       
    }
 
  
	// Update is called once per frame
	void Update ()
    {
	
	}
}
