using System.Collections.Generic;

namespace XH
{
    class AB
    {
        public string name = string.Empty;
        public string hash = string.Empty;
    }

    class AssetBundleManifestGenModel
    {
        public string assetBundlePath = string.Empty;
        public string manifestAssetBundleName = string.Empty;
        public List<AB> manifest = new List<AB>();
    }
}