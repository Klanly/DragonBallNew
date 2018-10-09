using UnityEngine;
using System.Collections.Generic;


public class BattleDamageText: MonoBehaviour {

    public UIGrid _numberGrid; 
    private const string _numPrefab = "DamageNumber/pbEffectNumber";

	//返回伤害数值
	private GameObject _hurtGo = null;
	public GameObject GoHurtNum {
		get {
			if(_hurtGo == null)
				_hurtGo = PrefabLoader.loadFromPack (_numPrefab) as GameObject;
			return _hurtGo;
		}
	}

    public Vector3 _startPos;
    public Vector3 _midPos;
    public Vector3 _endPos;

    public int _curBzPoint = 1;
    public float _beginAlpha = 0.8f;
    public int _totalPoint = 50;

    private Bezier _bezier;
    private bool _moving = false;
    private float _startTime = 0f;
    private List<UISprite> _numberSp;

    void Start() {
        _numberSp = new List<UISprite>();
        _bezier = new Bezier (Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero); 
    }

	public void reserveGrid() {
		_numberGrid.transform.localScale = new Vector3(-1F, 1F, 1F);
	}
    // Update is called once per frame
    void Update () {

        if (_moving) {
            _startTime += Time.deltaTime;
            if (_startTime > 0.01f) {
                if (_curBzPoint / (float)_totalPoint >= _beginAlpha) {
                    if (_numberSp != null && _numberSp.Count > 0) {
                        foreach (UISprite item in _numberSp) {
							if(item) {
								item.alpha -= 0.1f;
							}
                        }
                    }

                }
                _startTime = 0f;
				_curBzPoint ++;
                this.transform.localPosition = _bezier.GetPointAtTime (_curBzPoint * 1.1f / _totalPoint);

                if (_curBzPoint > _totalPoint) {
                    _curBzPoint = 8;
                    _moving = false;
                }
            }
        }

    }

    public void SetBezier(Vector3 start, Vector3 end, Vector3 mid) {
        _bezier.p0 = start;
        _bezier.p1 = mid;
        _bezier.p2 = mid;
        _bezier.p3 = end;

        _moving = true;
        _startTime = 0f;
        _curBzPoint = 10;
    }


    public void SetDamage (int num) {
        if (num == 0)
            return;
        SetBezier(_startPos, _endPos, _midPos);

        _numberSp.Clear();

        gameObject.SetActive (true);
        _numberGrid.gameObject.SetActive (true);
        RED.RemoveChildsImmediate(_numberGrid.transform);

        string numstr = num > 0 ? "+" + num : num.ToString();
        char[] numstrlist = numstr.ToCharArray();
        string pr = num > 0 ? "g" : "r";

        int j = 0;

        foreach(char i in numstrlist) {
			GameObject numObj = NGUITools.AddChild(_numberGrid.gameObject, GoHurtNum);
            UISprite item = numObj.GetComponent<UISprite> ();
            item.spriteName = pr + i.ToString();
            item.MakePixelPerfect ();
			item.transform.localScale = Vector3.one * 2;
            _numberSp.Add (item);

            item.depth = 10;
            numObj.name = "0" + j.ToString();
            j++;
        }
        _numberGrid.Reposition();


    }

}
