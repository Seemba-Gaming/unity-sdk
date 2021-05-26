using UnityEngine;

namespace SeembaSDK
{
    public class ConfigFileHelper : MonoBehaviour
    {
        public const string CONFIG_FILE_NAME = "seemba-services";
        public static string PATH = Application.dataPath + "/Seemba/Resources/";
        public static string RELATIVE_PATH = "Assets/Seemba/Resources/";
        public static string FILE_PATH = PATH + CONFIG_FILE_NAME + ".json";
        public static string RELATIVE_FILE_PATH = RELATIVE_PATH + CONFIG_FILE_NAME + ".json";
    }
}
