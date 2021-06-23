using System;
using UnityEngine;
namespace SeembaSDK
{
    public class CurrencyManager : MonoBehaviour
    {
        public const string CURRENCY_SYMBOL_TYPE_EURO = "€";
        public const string CURRENCY_SYMBOL_TYPE_DOLLAR = "$";
        public const string CURRENCY_SYMBOL_TYPE_POUND = "£";
        public const string CURRENCY_SYMBOL_TYPE_TND = "TND";

        public const string CURRENCY_CODE_TYPE_EURO = "EUR";
        public const string CURRENCY_CODE_TYPE_TND = "TND";
        public const string CURRENCY_CODE_TYPE_DOLLAR = "USD";
        public const string CURRENCY_CODE_TYPE_POUND = "EGP";

        public const int CURRENCY_MUTLIPLIER_FACTOR_TND = 25;
        public const int CURRENCY_MUTLIPLIER_FACTOR_EURO = 100;

        public static string CURRENT_CURRENCY;
        public static int CURRENT_MULTIPLIER_FACTOR;
    }
}
