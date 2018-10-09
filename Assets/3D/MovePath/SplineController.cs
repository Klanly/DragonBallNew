using UnityEngine;
using System.Collections;

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
	public GameObject SplineRoot;
	public float Duration = 10;
	public eWrapMode WrapMode = eWrapMode.ONCE;
	public bool AutoStart = true;
	public bool AutoClose = true;
	public bool HideOnExecute = true;
	public bool RotateEnable = true;
	public float RotateSpeed = 0.5f;

	SplineInterpolator mSplineInterp;
	Transform[] mTransforms;



	void OnDrawGizmos()
	{
		Transform[] trans = GetTransforms();
		if (trans.Length < 2)
			return;

		SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
		SetupSplineInterpolator(interp, trans);
		interp.StartInterpolation(null, WrapMode);


		Vector3 prevPos = trans[0].position;
		for (int c = 1; c <= 100; c++)
		{
			float currTime = c * Duration / 100;
			Vector3 currPos = interp.GetHermiteAtTime(currTime);
			float mag = (currPos - prevPos).magnitude * 2;
			Gizmos.color = new Color(mag, 0, 0, 1);
			Gizmos.DrawLine(prevPos, currPos);
			prevPos = currPos;
		}
	}


	void Start()
	{
		mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

		mSplineInterp.mSplineController = this;

		Setup();

		if (AutoStart)
			FollowSpline();
	}

	public Quaternion LookRotation(int fromIndex, int toIndex)
	{
		return Quaternion.LookRotation(mTransforms[toIndex].transform.position -
			mTransforms[fromIndex].transform.position, Vector3.up);
	}

	public Quaternion LookRotation(Vector3 from, Vector3 to)
	{
		return Quaternion.LookRotation(to - from, Vector3.up);
	}

	void Setup()
	{
		mTransforms = GetTransforms();

		if (RotateEnable)
		{
			this.transform.rotation = LookRotation(0, 1);
			mSplineInterp.targetRotation = this.transform.rotation;
		}

		if (HideOnExecute)
			DisableTransforms();
	}

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		interp.Reset();

		float step = (AutoClose) ? Duration / trans.Length :
			Duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
			interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(0, 1));
		}

		if (AutoClose)
			interp.SetAutoCloseMode(step * c);
	}


	/// <summary>
	/// Returns children transforms, sorted by name.
	/// </summary>
	Transform[] GetTransforms()
	{
		if (SplineRoot != null)
		{
			ArrayList transforms = new ArrayList(SplineRoot.GetComponentsInChildren(typeof(Transform)));

			transforms.Remove(SplineRoot.transform);

			transforms.Sort(new GameObjectNameComparer());

			return (Transform[])transforms.ToArray(typeof(Transform));
		}

		return null;
	}

	/// <summary>
	/// Disables the spline objects, we don't need them outside design-time.
	/// </summary>
	void DisableTransforms()
	{
		if (SplineRoot != null)
		{
			SplineRoot.SetActiveRecursively(false);
		}
	}


	/// <summary>
	/// Starts the interpolation
	/// </summary>
	void FollowSpline()
	{
		if (mTransforms.Length > 0)
		{
			SetupSplineInterpolator(mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation(null, WrapMode);
		}
	}

	/// <summary>
	/// Starts the interpolation and calls the delegate specified
	/// </summary>
	void FollowSpline(OnEndCallback callback)
	{
		if (mTransforms.Length > 0)
		{
			SetupSplineInterpolator(mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation(callback, WrapMode);
		}
	}

	public void Play(OnEndCallback callback)
	{
		Setup();
		FollowSpline(callback);
	}

	public void Play()
	{
		Setup();
		FollowSpline();
	}
}


public class GameObjectNameComparer : IComparer
{
	int IComparer.Compare(object a, object b)
	{
		return ((Transform)a).name.CompareTo(((Transform)b).name);
	}
}