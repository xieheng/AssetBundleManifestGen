using System;
using System.IO;
using UnityEngine;

namespace XH
{
    class MD5
    {
        public static byte[] GetFileHash(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    FileStream file = new FileStream(filename, FileMode.Open);
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] bytes = md5.ComputeHash(file);
                    file.Close();

                    return bytes;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Compute file hash failed. err: " + ex.Message);
            }

            return null;
        }
    }
}