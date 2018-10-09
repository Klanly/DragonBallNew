//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Ever wanted to be able to auto-center on an object within a draggable panel?
/// Attach this script to the container that has the objects to center on as its children.
/// </summary>

//[AddComponentMenu("NGUI/Interaction/Center Panel On Child")]
public class JCUICenterOnChild : MonoBehaviour
{
	/// <summary>
	/// The strength of the spring.
	/// </summary>

	public float springStrength = 8f;

	/// <summary>
	/// If set to something above zero, it will be possible to move to the next page after dragging past the specified threshold.
	/// </summary>

	public float nextPageThreshold = 0f;

	/// <summary>
	/// Callback to be triggered when the centering operation completes.
	/// </summary>

	public SpringPanel.OnFinished onFinished;
	
	#region AddbyJC
	public System.Action onStartMove;
	public bool DragGently = false;
	Vector3 mousePosition = new Vector3();	
	void OnDragStarted()
	{
		mousePosition = Input.mousePosition;
	}
	#endregion
	
	
	UIScrollView mDrag;
	GameObject mCenteredObject;

	/// <summary>
	/// Game object that the draggable panel is currently centered on.
	/// </summary>

	public GameObject centeredObject { get { return mCenteredObject; } }

	void OnEnable ()
	{
		Recenter(true); 
	}
	void OnDragFinished ()
	{
		if (enabled)
			Recenter(false); 
	}

	/// <summary>
	/// Ensure that the threshold is always positive.
	/// </summary>

	void OnValidate ()
	{
		nextPageThreshold = Mathf.Abs(nextPageThreshold);
	}

	/// <summary>
	/// Recenter the draggable list on the center-most child.
	/// </summary>
	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }

	public void Recenter (bool isEnableRefresh)
	{
		if (mDrag == null)
		{
			mDrag = NGUITools.FindInParents<UIScrollView>(gameObject);

			if (mDrag == null)
			{
				Debug.LogWarning(GetType() + " requires " + typeof(UIScrollView) + " on a parent object in order to work", this);
				enabled = false;
				return;
			}
			else
			{
				mDrag.onDragStarted = OnDragStarted;
				mDrag.onDragFinished = OnDragFinished;

				if (mDrag.horizontalScrollBar != null)
					mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;

				if (mDrag.verticalScrollBar != null)
					mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (mDrag.panel == null) return;

		// Calculate the panel's center in world coordinates
		Vector3[] corners = mDrag.panel.worldCorners;
		Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

		// Offset this value by the momentum
		Vector3 pickingPoint = panelCenter - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
		mDrag.currentMomentum = Vector3.zero;

		float min = float.MaxValue;
		Transform closest = null;
		Transform trans = transform;
		int index = 0;

#region Change By JC
		if(!DragGently)
		{
			// Determine the closest child
			for (int i = 0, imax = trans.childCount; i < imax; ++i)
			{
				Transform t = trans.GetChild(i);
				float sqrDist = Vector3.SqrMagnitude(t.position - pickingPoint);

				if (sqrDist < min)
				{
					//Debug.LogError("sqrDist="+sqrDist.ToString() +"    min="+min.ToString() + "    name="+t.name);
					min = sqrDist;
					closest = t;
					index = i;
				}
			}
		}
		else
		{
			if(!isEnableRefresh)
			{
				List<Transform> list = new List<Transform>();
				for (int i = 0; i < trans.childCount; ++i)
				{
					Transform t = trans.GetChild(i);
					if (t.gameObject.activeSelf) list.Add(t);
				}
				list.Sort(SortByName);

				int count = list.Count;
				if(mCenteredObject == null)
				{
					if(count > 0)
					{
						mCenteredObject = list[0].gameObject;
						for (int i = 0; i < count; ++i)
						{
							if(trans.gameObject.activeSelf && list[i].localPosition == Vector3.zero)
							{
								mCenteredObject = list[i].gameObject;
							}
						}
					}
				}

				if(mCenteredObject != null)
				{
					#region 寻找当前显示子件
					index = 0;
					for (int i = 0; i < count; ++i)
					{
						Transform t = list[i];
						if(t == mCenteredObject.transform)
						{
							index = i;
						}
					}
					#endregion
					if(Input.mousePosition.x < mousePosition.x)
					{
						int targetIndex = index;
						if(targetIndex < count-1)
						{
							targetIndex++;
							if( list[targetIndex].gameObject.activeSelf )
								index = targetIndex;
						}
						closest = list[index];
					}
					else if(Input.mousePosition.x > mousePosition.x)
					{
						int targetIndex = index;
						if(targetIndex > 0)
						{
							targetIndex --;
							if( list[targetIndex].gameObject.activeSelf )
								index = targetIndex;
						}
						closest = list[index];
					}
				}
			}
		}
#endregion
		// If we have a touch in progress and the next page threshold set
		if (nextPageThreshold > 0f && UICamera.currentTouch != null)
		{
			// If we're still on the same object
			if (mCenteredObject != null && mCenteredObject.transform == trans.GetChild(index))
			{
				Vector2 delta = UICamera.currentTouch.totalDelta;

				if (delta.x > nextPageThreshold)
				{
					// Next page
					if (index > 1)
						closest = trans.GetChild(index - 1);
				}
				else if (delta.x < -nextPageThreshold)
				{
					// Previous page
					if (index < trans.childCount - 1)
						closest = trans.GetChild(index + 1);
				}
			}
		}

		CenterOn(closest, panelCenter);
	}

	/// <summary>
	/// Center the panel on the specified target.
	/// </summary>
	
	
	
	Vector3 oldPos = Vector3.zero;
	bool isMoveFinished = true;
	void CenterOn (Transform target, Vector3 panelCenter)
	{
		if (target != null && mDrag != null && mDrag.panel != null)
		{
			Transform panelTrans = mDrag.panel.cachedTransform;
			mCenteredObject = target.gameObject;

			// Figure out the difference between the chosen child and the panel's center in local coordinates
			Vector3 cp = panelTrans.InverseTransformPoint(target.position);
			Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
			Vector3 localOffset = cp - cc;

			// Offset shouldn't occur if blocked
			if (!mDrag.canMoveHorizontally) localOffset.x = 0f;
			if (!mDrag.canMoveVertically) localOffset.y = 0f;
			localOffset.z = 0f;
			
			
			#region Add by jc
			Vector3 targetPos = panelTrans.localPosition - localOffset;
			if( Vector3.Distance(targetPos,oldPos) >1f )
			{
			     if(onStartMove != null) onStartMove();						
				 oldPos = targetPos;
			}

			#endregion
			// Spring the panel to this calculated position
			SpringPanel.Begin(mDrag.panel.cachedGameObject,
				panelTrans.localPosition - localOffset, springStrength).onFinished = onFinished;	
			
		}
		else mCenteredObject = null;
	}
	
	/// <summary>
	/// Center the panel on the specified target.
	/// </summary>

	public void CenterOn (Transform target)
	{
		if (mDrag != null && mDrag.panel != null)
		{
			Vector3[] corners = mDrag.panel.worldCorners;
			Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;
			CenterOn(target, panelCenter);
		}
	}
}
