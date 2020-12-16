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
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            var path = EditorGUILayout.TextField("Path", model.assetBundlePath);
            if (EditorGUI.EndChangeCheck())
            {
                model.assetBundlePath = path;
            }

            if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.Width(40)))
            {
                path = EditorUtility.OpenFolderPanel("Select AssetBundle Output Path", Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    model.assetBundlePath = path;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            var name = EditorGUILayout.TextField("Name", model.manifestAssetBundleName);
            if (EditorGUI.EndChangeCheck())
            {
                model.manifestAssetBundleName = name;
            }

            if (GUILayout.Button("Gen Manifest"))
            {
                ctrl.Run();
            }

            if (GUILayout.Button("Save As"))
            {
                var root = Directory.GetParent(Application.dataPath).ToString();
                var file = "assetbundle_manifest.txt";

                var full = Path.Combine(root, file).Replace("\\", "/");
                ctrl.Save(full);
            }
        }
    }
}