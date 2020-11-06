using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS3009 // Le type de base n'est pas conforme CLS
public class CurrencyManager : MonoBehaviour
#pragma warning restore CS3009 // Le type de base n'est pas conforme CLS
{
    public const string CURRENCY_SYMBOL_TYPE_EURO = "€";
    public const string CURRENCY_SYMBOL_TYPE_DOLLAR = "$";
    public const string CURRENCY_SYMBOL_TYPE_POUND = "£";
    public const string CURRENT_CURRENCY= CURRENCY_SYMBOL_TYPE_EURO;
    public const string CURRENCY_CODE_TYPE_EURO = "EUR";
    public const string CURRENCY_CODE_TYPE_DOLLAR = "USD"; 
    public const string CURRENCY_CODE_TYPE_POUND = "EGP"; 
}
