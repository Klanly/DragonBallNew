using System;
using AW.Resources;

public class AudioLoader : IAssetManager {
    #if UNITY_IPHONE
    private const int CAPACITY = 20;
    private const int MAX = 10;

    #elif UNITY_ANDROID
    private const int CAPACITY = 30;
    private const int MAX = 10;

    #elif UNITY_WP8
    private const int CAPACITY = 30;
    private const int MAX = 10;

    #else
    private const int CAPACITY = 50;
    private const int MAX = 30;

    #endif

    public AudioLoader () : base(CAPACITY, MAX) { }

    public override void RefAsset (string name) {
        int cached = lstRefAsset.Get(name);
        if(cached == 0) {
            string[] toBeRm = lstRefAsset.Add(name, 1);
            if(toBeRm != null && toBeRm.Length > 0) {
                foreach(string assetName in toBeRm) {
					PrefabLoader.CleanOneSpecial(assetName, false, false);
                }
            }
        } 
    }

}
