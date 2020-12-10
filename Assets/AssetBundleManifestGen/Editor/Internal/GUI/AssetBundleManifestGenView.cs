using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetBundleManifestGenView
{
    private AssetBundleManifestGenModel model = null;
    private AssetBundleManifestGenCtrl ctrl = null;

    public AssetBundleManifestGenView(AssetBundleManifestGenModel model,
                                      AssetBundleManifestGenCtrl ctrl)
    {
        this.model = model;
        this.ctrl = ctrl;
    }

    public void OnGUI()
    {

    }
}
