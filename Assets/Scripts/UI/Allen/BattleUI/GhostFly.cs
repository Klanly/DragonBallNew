using UnityEngine;
using System.Collections;

public class GhostFly : MonoBehaviour {

	private BanCurveLine_Shell Curve;
	private GameObject _ghost;

	private Vector3 whoDieTransPos;

	//3D 和 2D 的照相机
	public Camera Main3DCamera;
	public Camera Main2DCamera;

	private GameObject _go;
	//先调用这个
	public void createGhostly() {
		if(_ghost == null) {
			Object obj = PrefabLoader.loadFromUnPack("Ban/Ghost_2D", false);
			_ghost = Instantiate(obj) as GameObject;
			Curve = _ghost.GetComponent<BanCurveLine_Shell>();
		}
	}

	//再次调用这个
	public void ShowGhostFly(Transform whoDie, Vector3 AngryPointPos) {
		whoDieTransPos = whoDie.position;
		Vector3 pos = Main2DCamera.ScreenToWorldPoint(Main3DCamera.WorldToScreenPoint(whoDie.position));
		pos.z = 0;

		if(_go == null)
			_go = new GameObject();
		_go.transform.position = AngryPointPos;

		Curve.gameObject.transform.position = pos;
		Curve.GoPos = _go;

		Curve.GoCarete();
		InvokeRepeating ("mUpdate", 0.0f, 0.3f);
		Invoke("cancelUpdate", 2.1f);
	}

	void cancelUpdate() {
		CancelInvoke("mUpdate");
	}

	void mUpdate() {
		Vector3 pos = Main2DCamera.ScreenToWorldPoint(Main3DCamera.WorldToScreenPoint(whoDieTransPos));
		pos.z = 0;
		_ghost.transform.position = pos;
	}


}
