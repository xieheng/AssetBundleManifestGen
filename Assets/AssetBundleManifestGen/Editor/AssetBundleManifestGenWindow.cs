using UnityEngine;
using UnityEditor;

public class AssetBundleManifestGenWindow : EditorWindow
{
    private AssetBundleManifestGenView view = null;

    [MenuItem("Window/Gen AssetBundle Manifest")]
    static void Create()
    {
        var window = EditorWindow.GetWindow(typeof(AssetBundleManifestGenWindow)) as AssetBundleManifestGenWindow;
        window.Init();
        window.Show();
    }

    void Init()
    {
        var model = new AssetBundleManifestGenModel();
        var ctrl = new AssetBundleManifestGenCtrl(model);
        view = new AssetBundleManifestGenView(model, ctrl);
    }

    void OnGUI()
    {
        view.OnGUI();
    }
}
