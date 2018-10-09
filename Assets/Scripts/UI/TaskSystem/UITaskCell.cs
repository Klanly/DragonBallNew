using UnityEngine;
using System.Collections;

public class UITaskCell : MonoBehaviour {
	
	
	public TaskData m_data{get;set;}
	

	
	public UISprite Spr_Selected;
	
	private UITaskType uitype;
	
	public UILabel Lab_Title;
	
	public UISprite Spr_Sign;
	void Start () {
	
	}
	
	/*设置每个元素的显示
	 * */
	public void SetCell(TaskData data)
	{
		this.m_data =  data;

		uitype = (UITaskType)data.Type;
		isSelected = false;
		
		string progress = "";
		if(data.curProgress != data.Progress)
			progress = "("+data.curProgress.ToString()+"/"+data.Progress.ToString()+")";
		
		Lab_Title.text = data.Title.ToString()+progress;
		Spr_Sign.enabled = data.curProgress == data.Progress;
	}
	
	
	
	public void OnClick()
	{
//		if(m_data != null){
//			string tName = m_data.Title.ToString ();
//			ControllerEventData ctrl = new ControllerEventData (name,"UITaskCell");
//			ActivityNetController.GetInstance ().SendCurrentUserState (ctrl);
//		}
		if( isSelected ) return;
		isSelected = true;


	}
	
	
	public void SetSeleted(bool Value)
	{
		if(Value)
		{
			this.isSelected = Value;
			Spr_Selected.enabled = Value;
		}
	}
	
	
	
	//设置选中状态并自动清除最后一次选中状态
	private bool _isselected;
	public bool isSelected 
	{
		set
		{
			if(_isselected == value) return; 
			Spr_Selected.enabled = value;
			_isselected = value;
			if(value)
			{				
				if(UITask.Instance !=null )
				{		
					UITaskCell lasttask = null;
					if(uitype== UITaskType.EveryData)
					{
						lasttask = UITask.Instance.CurSelected_EveryDayCell;
						if(lasttask != null && lasttask != this)
							lasttask.isSelected = false;
						UITask.Instance.CurSelected_EveryDayCell = this;
						Core.Data.taskManager.LastDayTaskID = m_data.ID;
					}
					else if(uitype== UITaskType.MainLine)
					{
						lasttask = UITask.Instance.CurSelected_MainLineCell;
						if(lasttask != null && lasttask != this)
							lasttask.isSelected = false;
						UITask.Instance.CurSelected_MainLineCell = this;
						Core.Data.taskManager.LastMainTaskID = m_data.ID;
					}
					if(this.m_data != null)
					UITask.Instance._view.ShowRightPanel(this.m_data);
					else
					Debug.LogError("m_data==null");
				}
			}
		}
		get{return _isselected;}
	}
	
}
