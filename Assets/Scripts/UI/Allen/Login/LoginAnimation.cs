using UnityEngine;
using System.Collections;

public class LoginAnimation : MonoBehaviour {

    public GameObject Logo;

    public float ScaleTime = 0.2f;

    public float ScaleShakeTime = 0.05f;

    public MiniItween.EasingType ScaleType;

    public GameObject CameraUI;

    public Vector3 shakeV3;

    public float shakeTime;
    public MiniItween.EasingType ShakeType;

    public GameObject go_Dust;

    public float Dust_DelayTime = 0.1f;
    // 
    public string m = "------------------------------------------";

    public GameObject Door;
    public GameObject DoorLeft;
    public GameObject DoorRight;

    public Vector3 DoorShakeV3;
    public Vector3 DoorMoveV3;

    public float DoorShakeTime = 0.2f;

    public Vector3 LeftDoorDisappear;
    public Vector3 RightDoorDisappear;
    public float DoorDisTime;
    public float CloseDoorTime;

    public float LightTime = 0.3f;

    public float RepeatTime = 2.0f;

    // ------ for scale Logo -----
    public string mm = "------------------------------------------";

    public Vector3 SmallLogoPos = new Vector3(-236f, 52.5f, 0f);
    public Vector3 SmalllScale = new Vector3(0.4f, 0.4f, 0.4f);
    public float ToSmallTime = 0.2f;

    public static LoginAnimation Instance;
	// Use this for initialization
	void Start () {
        Instance = this;

        oldDoorLeft = DoorLeft.transform.localPosition;
        oldDoorRight = DoorRight.transform.localPosition;

        Invoke("LogoDrop", 0.8f);
	}

    void LogoDrop() {
        StartCoroutine(PlayShake());

        Invoke("OpenBattleDoor", 2.7f);
    }

    void OpenBattleDoor() {
        StartCoroutine(OpenDoor());
    }


    IEnumerator PlayShake() {
        UITexture tex = Logo.GetComponent<UITexture>();
        tex.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Logo.transform.localScale = new Vector3(15f, 15f, 15f);

        MiniItween.ScaleTo(Logo, new Vector3(0.95f, 0.95f, 0.95f), ScaleTime, ScaleType);
        Invoke("ShakeCamera", ScaleTime - Dust_DelayTime);

        yield return new WaitForSeconds(ScaleTime * 0.5f);
        MiniItween.ColorTo(Logo, new V4( new Color(1.0f, 1.0f, 1.0f, 1.0f)), ScaleTime * 0.5f, MiniItween.EasingType.EaseInOutQuart, MiniItween.Type.ColorWidget);

        yield return new WaitForSeconds(ScaleTime);
        MiniItween.ScaleTo(Logo, new Vector3(1.05f, 1.05f, 1.05f), ScaleShakeTime, ScaleType);
        yield return new WaitForSeconds(0.05f);
        MiniItween.ScaleTo(Logo, new Vector3(1f, 1f, 1f), ScaleShakeTime, ScaleType);

    }

    void ShakeCamera() {
        Instantiate(go_Dust);
        MiniItween.Shake(CameraUI, shakeV3, shakeTime, ShakeType, false);
    }


    // ----------------------------  Open Door -------------------------------------

    private Vector3 oldDoorLeft;
    private Vector3 oldDoorRight;

    IEnumerator OpenDoor() {
        DoorLeft.transform.localPosition = oldDoorLeft;
        DoorRight.transform.localPosition = oldDoorRight;

        MiniItween.Shake(DoorLeft, DoorShakeV3, DoorShakeTime, MiniItween.EasingType.EaseInOutQuart, false);
        MiniItween.Shake(DoorRight, DoorShakeV3, DoorShakeTime, MiniItween.EasingType.EaseInOutQuart, false);

        yield return new WaitForSeconds(DoorShakeTime);
        StartCoroutine(DoorShift());
    }

    IEnumerator DoorShift() {
        MiniItween.MoveTo(DoorLeft, LeftDoorDisappear, DoorDisTime, false);
        MiniItween.MoveTo(DoorRight, RightDoorDisappear, DoorDisTime, false);

        MiniItween.Shake(Door, DoorMoveV3, DoorDisTime, MiniItween.EasingType.Linear, false);

        //Create CRLuo_Scene
        Object obj = PrefabLoader.loadFromPack("CRLuo/System/ShowVS", false, false);
        Instantiate(obj);

        yield return new WaitForSeconds(LightTime);
        ScaleToSmall();
        //Create Light
        //Instantiate(go_Light);
        obj = null;

    }

    void ScaleToSmall() {
        MiniItween.MoveTo(Logo, SmallLogoPos, ToSmallTime, MiniItween.EasingType.EaseOutCirc);
        MiniItween.ScaleTo(Logo, SmalllScale, ToSmallTime, MiniItween.EasingType.EaseOutCirc);
    }

    public void CloseDoor() {
        MiniItween.MoveTo(DoorLeft, oldDoorLeft, CloseDoorTime, MiniItween.EasingType.EaseOutCirc, false);
        MiniItween.MoveTo(DoorRight, oldDoorRight, CloseDoorTime, MiniItween.EasingType.EaseOutCirc, false);

        MiniItween.Shake(Door, DoorMoveV3, DoorDisTime, MiniItween.EasingType.Linear, false);

        Invoke("OpenBattleDoor", RepeatTime);
    }

}
