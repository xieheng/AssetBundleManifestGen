using System.IO;
using UnityEngine;
using UnityEditor;

namespace XH
{
    class AssetBundleManifestGenView
    {
        private AssetBundleManifestGenModel model = null;
        private AssetBundleManifestGenCtrl ctrl = null;

        public AssetBundleManifestGenView(AssetBundleManifestGenModel model, AssetBundleManifestGenCtrl ctrl)
        {
            this.model = model;
            this.ctrl = ctrl;
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Gen Manifest"))
            {
                ctrl.Run();
            }

            if (GUILayout.Button("Save As"))
            {
                var root = Directory.GetParent(Application.dataPath).ToString();
                var name = "assetbundle_manifest.txt";

                var path = Path.Combine(root, name).Replace("\\", "/");
                ctrl.Save(path);
            }
        }
    }
}