using UnityEngine;
using System.Collections;

public class UICityItem : MonoBehaviour 
{
    public enum CityState
    {
        None,
        Pass,
        Current,
        Unlocked,
    }

    public delegate void onClickedItem(City item);
    public onClickedItem OnClickedItem = null;

    public UISprite background;
    //public UISprite curr;
    public UILabel cityName;
    City data;
     //Color curColor = new Color(1f,215f/255f,0,1f);
     //Color norColor = new Color(243f/255f,215f/255f,190f/255f,1f);
     //Color lockColor = Color.white;
    private CityState _state = CityState.None;
	
    public UISprite Spr_unlock;
	public UISprite Spr_current;
    public CityState state
    {
        get {return _state;}
        protected set
        {
            if(_state != value)
            {
                _state = value;
#if FB2
				switch(_state)
                {
					case CityState.Pass:
					Spr_unlock.gameObject.SetActive(false);
					Spr_current.gameObject.SetActive(false);
					break;
					case CityState.Current:
					Spr_unlock.gameObject.SetActive(false);
					Spr_current.gameObject.SetActive(true);
					break;
					case CityState.Unlocked:
					Spr_unlock.gameObject.SetActive(true);
					Spr_current.gameObject.SetActive(false);
					break;
				}
#else
                switch(_state)
                {
                    case CityState.Pass:
                    background.spriteName = "Symbol 32";
                        //cityName.color = norColor;
                    break;
                    case CityState.Current:
                    background.spriteName = "Symbol 31";
                        //cityName.color = curColor;
                    break;
                    case CityState.Unlocked:
                        background.spriteName = "common-0015";
                        //cityName.color = lockColor;
                    break;
                }
#endif
            }

        }
    }

    public void InitItem(City _data)
    {
        if(_data != null)
        {
            data = _data;
            cityName.text = data.config.name;
        }
    }

    public void UpdateItem(int currID, int maxID)
    {
        if(data != null)
        {
            if(data.config.ID > maxID)
            {
                state = CityState.Unlocked;
            }
            else
            {
                if(data.config.ID != currID)
                {
                    state = CityState.Pass;
                }
                else
                {
                    state = CityState.Current;
                }
            }
        }
    }
	
    void OnClick()
    {
        if(state!=CityState.Unlocked && OnClickedItem!=null)
        {
            OnClickedItem(data);
			//add by wxl 
//			if(data != null){
//				string tName = data.config.name.ToString ();
//				ControllerEventData ctrl = new ControllerEventData (name,"OnClick");
////				ActivityNetController.GetInstance ().SendCurrentUserState (ctrl);
//			}
        }
    }
	
	public void SetImage(int cityID)
	{
		Debug.Log("cityID="+cityID.ToString());
		AtlasMgr.mInstance.SetFB2CityImage(background,cityID);
	}
	
}
