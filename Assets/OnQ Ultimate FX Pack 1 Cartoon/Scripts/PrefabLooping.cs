using UnityEngine;
using System.Collections;

public class PrefabLooping : MonoBehaviour {

    public GameObject FXLoop;
    public float intervalMin  = 0;
    public float intervalMax = 0;
    public float positionFactor = 0;
    public float killtime = 0;

    void Start() {
        InvokeRepeating("Burst", 0.0f, Random.Range(intervalMin, intervalMax));
    }

    void Burst () {
        if(gameObject.active == false) { return; }

        GameObject clone = null;
        var pos = transform.position;
        pos.x += Random.Range(-positionFactor, positionFactor);
        pos.y += Random.Range(-positionFactor, positionFactor);
        pos.z += Random.Range(-positionFactor, positionFactor);

        clone = Instantiate (FXLoop, pos, transform.rotation) as GameObject;

        Destroy (clone.gameObject, killtime);
    }

}

