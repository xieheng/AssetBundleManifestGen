using UnityEngine;
using UnityEditor;

namespace XH
{
    class AssetBundleManifestGenWindow : EditorWindow
    {
        private AssetBundleManifestGenView view = null;

        [MenuItem("Window/Gen AssetBundle Manifest")]
        static void CreateWindow()
        {
            var window = EditorWindow.GetWindow<AssetBundleManifestGenWindow>();
            window.Init();
            window.Show();
        }

        void Init()
        {
            var m = new AssetBundleManifestGenModel();
            var c = new AssetBundleManifestGenCtrl(m);
            view = new AssetBundleManifestGenView(m, c);
        }

        void OnGUI()
        {
            view.OnGUI();
        }
    }
}