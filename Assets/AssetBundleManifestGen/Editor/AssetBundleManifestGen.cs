using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace XH
{
    public class AssetBundleManifestGen
    {
        public static Dictionary<string, string> Gen()
        {
            Dictionary<string, string> manifest = new Dictionary<string, string>();

            var names = GetAllAssetBundleNames();
            foreach (var name in names)
            {
                var assets = AssetDatabase.GetAssetPathsFromAssetBundle(name);
                if (assets.Length == 0)
                {
                    Debug.LogWarning($"AssetBundle [{name}] is Empty!");
                    continue;
                }

                var bytes = new byte[16]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                foreach (var asset in assets)
                {
                    var path = Path.GetFullPath(asset).Replace("\\", "/");
                    var md5 = MD5.GetFileHash(path);

                    for (int i=0; i<md5.Length; i++)
                    {
                        bytes[i] ^= md5[i];
                    }
                }
                var hash = BytesToString(bytes);

                manifest.Add(name, hash);
            }

            Debug.Log("Gen AssetBundle Manifest Done!");

            return manifest;
        }

        // 获取所有AssetBundle的名字
        private static string[] GetAllAssetBundleNames()
        {
            var files = AssetDatabase.GetAllAssetBundleNames();
            return files;
        }

        // 字节数组转字符串
        private static string BytesToString(byte[] bytes)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}