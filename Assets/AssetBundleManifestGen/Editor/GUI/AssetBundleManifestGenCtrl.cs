using System.IO;
using System.Collections.Generic;

namespace XH
{
    class AssetBundleManifestGenCtrl
    {
        private AssetBundleManifestGenModel model = null;

        public AssetBundleManifestGenCtrl(AssetBundleManifestGenModel model)
        {
            this.model = model;
        }

        public void Run()
        {
            model.manifest.Clear();

            Dictionary<string, string> manifest = AssetBundleManifestGen.Gen();
            foreach (var m in manifest)
            {
                AB ab = new AB();
                ab.name = m.Key;
                ab.hash = m.Value;
                model.manifest.Add(ab);
            }
        }

        public void Save(string path)
        {
            var text = string.Empty;
            foreach (var m in model.manifest)
            {
                text = text + $"{m.name}|{m.hash}\n";
            }
            File.WriteAllText(path, text);
        }
    }
}