using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.U2D;
using UnityEditor;

namespace XH
{
    public class AssetBundleManifestGen
    {
        // 根据BuildTarget获取AssetBundle的Manifest表
        // AssetBundleName:Hash
        // 因为暂时无法准确判断在所有AssetBundle的引用关系是否改变，所以每一次都计算ManifestAssetBundle的Hash值
        // 不管是什么原因引起ManifestAssetBundle的Hash发生变化，（只要变化了）都让它进行热更，避免引起AssetBundle的引用关系错误
        public static Dictionary<string, string> Gen(string assetBundlePath, string manifestBundleName)
        {
            Dictionary<string, string> manifest = new Dictionary<string, string>();

            var fullname = Path.Combine(assetBundlePath, manifestBundleName).Replace("\\", "/");
            if (!File.Exists(fullname))
            {
                Debug.LogError($"Error: manifest assetbundle [{manifestBundleName}] not be found!\nfullname is [{fullname}]");
            }
            else
            {
                var hash = MD5.GetFileHash(fullname);
                if (hash != null)
                {
                    var text = BytesToString(hash);
                    manifest.Add(manifestBundleName, text);
                }
            }

            var names = GetAllAssetBundleNames();
            foreach (var name in names)
            {
                var hash = GetAssetBundleHash(name);
                if (hash != null)
                {
                    var text = BytesToString(hash);
                    manifest.Add(name, text);
                }
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

        // 获取AssetBundle的hansh值
        private static byte[] GetAssetBundleHash(string assetbundleName)
        {
            var assets = AssetDatabase.GetAssetPathsFromAssetBundle(assetbundleName);
            if (assets.Length == 0)
            {
                Debug.LogWarning($"AssetBundle [{assetbundleName}] is Empty!");
                return null;
            }

            var hash = new byte[16]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            foreach (var asset in assets)
            {
                HashAsset(asset, hash);
            }

            return hash;
        }

        // 获取Asset的hansh值
        private static void HashAsset(string assetname, byte[] hash)
        {
            XOR(assetname, hash);

            var depends = AssetDatabase.GetDependencies(assetname, true);
            foreach (var depend in depends)
            {
                if (depend == assetname)
                    continue;

                if (IsDirectory(depend))
                    continue;

                if (IsValidAsset(depend))
                {
                    XOR(depend, hash);
                }
            }
        }

        private static void XOR(string assetname, byte[] hash)
        {
            var path = Path.GetFullPath(assetname).Replace("\\", "/");
            var temp = GetFileHash(path);
            for (int i=0; i<temp.Length; i++)
            {
                hash[i] ^= temp[i];
            }
        }

        private static bool IsDirectory(string path)
        {
            if (File.Exists(path))
                return false;

            if (Directory.Exists(path))
                return true;

            Debug.LogWarning($"unknown path - {path}");
            return false;
        }

        private static bool IsValidAsset(string path)
        {
            if (!path.StartsWith("Assets/"))
                return false;

            string ext = Path.GetExtension(path);
            if (ext == ".dll" || ext == ".cs" || ext == ".meta" || ext == ".js" || ext == ".boo")
                return false;

            return true;
        }

        // 获取文件的hansh值
        //   计算方法：Asset文件的hash和对应meta文件的hash进行异或运算
        //   计算meta的原因：目前诸如Texture的一些设置，仅能从meta的变化感知它们
        private static byte[] GetFileHash(string filename)
        {
            var hash = MD5.GetFileHash(filename);
            var meta = MD5.GetFileHash(filename + ".meta");
            
            for (int i=0; i<meta.Length; i++)
            {
                hash[i] ^= meta[i];
            }

            return hash;
        }

        // 获取spriteatlas中所有sprite的guid值
        private static List<string> GetSpriteGUIDsIncludedInSpriteAtals(string path, string assetname)
        {
            List<string> guids = new List<string>();

            int index = 0;
            var lines = File.ReadAllLines(path);
            for (int i=0; i<lines.Length; i++)
            {
                var line = lines[i];
                if (line.TrimStart(' ').StartsWith("m_PackedSprites:"))
                {
                    index = i + 1;
                    break;
                }
            }

            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetname);
            for (; index<atlas.spriteCount; index++)
            {
                var match = Regex.Match(lines[index], @"guid: ([\da-f]{32})");

                foreach (var group in match.Groups)
                {
                    var guid = group.ToString();
                    if (!guid.StartsWith("guid:"))
                    {
                        guids.Add(guid);
                    }
                }
            }

            return guids;
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