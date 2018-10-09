using UnityEngine;
using System.Collections;

public class UIGuidanceHand : MonoBehaviour
{
	public enum Direction
	{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	public Direction currentDirection = Direction.UP;
	public GameObject handObj;

	public Direction CurrentDirection
	{
		get
		{
			return this.currentDirection;
		}
		set
		{
			this.currentDirection = value;
			if(this.currentDirection == Direction.UP || this.currentDirection == Direction.LEFT)
			{
				handObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
			}
			else if(this.currentDirection == Direction.DOWN)
			{
				handObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 135));
			}
			else if(this.currentDirection == Direction.RIGHT)
			{
				handObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 180,0));
			}
		}
	}

	private static UIGuidanceHand instance = null;

	static public UIGuidanceHand Instance
	{
		get
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.FindObjectOfType(typeof(UIGuidanceHand)) as UIGuidanceHand;

				if (instance == null)
				{
					GameObject prb = PrefabLoader.loadFromPack("WHY/pbGuidanceHand") as GameObject;
					GameObject prbObj = GameObject.Instantiate(prb) as GameObject;


					instance = prbObj.GetComponent<UIGuidanceHand>();
				}
			}
			return instance;
		}
	}

	public void setRoot(GameObject root)
	{
		gameObject.transform.parent = root.transform;
		gameObject.transform.localPosition = Vector3.back * 10;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;

	}

	// Use this for initialization
	void Start ()
	{
		TweenPosition tp = TweenPosition.Begin(handObj, 0.5f, new Vector3(10, 10, 0));
		tp.style = UITweener.Style.PingPong;
	}
}
