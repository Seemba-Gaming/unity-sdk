using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigFileHelper : MonoBehaviour {
{
    private const string CONFIG_FILE_NAME = "seemba-services";
    private static string PATH = Application.dataPath + "/Seemba/Resources/";
    private static string RELATIVE_PATH = "Assets/Seemba/Resources/";
    public  static string FILE_PATH = PATH + CONFIG_FILE_NAME + ".json";
    public  static string RELATIVE_FILE_PATH = RELATIVE_PATH + CONFIG_FILE_NAME + ".json";
}
