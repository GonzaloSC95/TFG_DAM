using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CryptController : MonoBehaviour
{
    /* Atributos */
    private static string Hash = "JunDesssertPixel2023HASH15851247";
    /* Métodos */
    /* Método Encrypt */
    public static string Encrypt(string str)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(str);

        MD5 md = MD5.Create();
        TripleDES tripleDes = TripleDES.Create();
        tripleDes.Key = md.ComputeHash(UTF8Encoding.UTF8.GetBytes(Hash));
        tripleDes.Mode = CipherMode.ECB;

        ICryptoTransform transform = tripleDes.CreateEncryptor();
        byte[] result = transform.TransformFinalBlock(data,0,data.Length);

        return Convert.ToBase64String(result);
    }
    /* Método Decrypt */
    public static string Decrypt(string str)
    {
        byte[] data = Convert.FromBase64String(str);

        MD5 md = MD5.Create();
        TripleDES tripleDes = TripleDES.Create();
        tripleDes.Key = md.ComputeHash(UTF8Encoding.UTF8.GetBytes(Hash));
        tripleDes.Mode = CipherMode.ECB;

        ICryptoTransform transform = tripleDes.CreateDecryptor();
        byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

        return UTF8Encoding.UTF8.GetString(result);
    }

}
