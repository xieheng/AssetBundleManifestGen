using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleManifestGenCtrl
{
    public class AB
    {
        public string name { get; set; } = string.Empty;
        public string hash { get; set; } = string.Empty;
        public Dictionary<string, string> assets { get; } = new Dictionary<string, string>();
    }
    
    private AssetBundleManifestGenModel model = null;

    public AssetBundleManifestGenCtrl(AssetBundleManifestGenModel model)
    {
        this.model = model;
    }
}
