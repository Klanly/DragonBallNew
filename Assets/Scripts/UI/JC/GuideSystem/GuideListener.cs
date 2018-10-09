using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GuideListener  {

	private List<EventReceiver> list_Receivers = null;
		
	public GuideListener () 
	{
	     list_Receivers = new List<EventReceiver>();
		 registered();
	}
	
	/*此处创建观察者
	 * */
	void registered()
	{
#if NewGuide
		list_Receivers.Add(new NewJCReceiver());
		list_Receivers.Add (new NewZQReceiver ());
		list_Receivers.Add(new NewBattleListener());
		list_Receivers.Add (new NewWXLReceiver ());
		list_Receivers.Add(new NewLSReceiver());
#else
		list_Receivers.Add(new JCReceiver());
		list_Receivers.Add (new ZQReceiver ());
        list_Receivers.Add(new BattleListener());
		list_Receivers.Add (new WXLReceiver ());
		list_Receivers.Add(new LSReceiver());
#endif
	}
	
	/*以下为各观察者实例返回
	 * */
#if NewGuide
	public NewJCReceiver jcReceiver
	{
		get {
			return list_Receivers[0] as NewJCReceiver;
		}
	}
#else
	public JCReceiver jcReceiver
	{
        get {
			return list_Receivers[0] as JCReceiver;
		}
	}
#endif	

#if NewGuide
	//返回战斗观察者的实例
	public NewBattleListener battleReceiver {
		get { 
			return list_Receivers[2] as NewBattleListener; 
		}
	}
#else
    //返回战斗观察者的实例
    public BattleListener battleReceiver {
        get { 
            return list_Receivers[2] as BattleListener; 
        }
    }
#endif
	
}
