using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class ConfigFileHelper : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    public const string CONFIG_FILE_NAME = "seemba-services";
    public static string PATH = Application.dataPath + "/Seemba/Resources/";
    public static string RELATIVE_PATH = "Assets/Seemba/Resources/";
    public  static string FILE_PATH = PATH + CONFIG_FILE_NAME + ".json";
    public  static string RELATIVE_FILE_PATH = RELATIVE_PATH + CONFIG_FILE_NAME + ".json";
}
