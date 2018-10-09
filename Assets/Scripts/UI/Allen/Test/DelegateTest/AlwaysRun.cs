using UnityEngine;
using System;
using System.Collections.Generic;

public class AlwaysRun : MonoBehaviour {
    private List<Action> callbacks = null;

    private static AlwaysRun _instance;
    public static AlwaysRun instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
        callbacks = new List<Action>();
    }

    void Start() {
        Invoke("CallBacks", 15f);
    }

    void CallBacks() {
        foreach(Action work in callbacks) {
            if(work != null) {
                work();
            }
        }
    }

    public void register(Action work) {
        callbacks.Add(work);
    }
}
