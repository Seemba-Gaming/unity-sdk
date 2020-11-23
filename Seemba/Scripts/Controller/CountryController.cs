public class CountryController
{
    private static string[] blockedCountry = { "af", "by", "bg", "cd", "ci", "cu", "eg", "fr", "pf", "gr", "id", "ir", "iq", "jp", "kp", "lt", "mk", "my", "mm", "ng", "ro", "ru", "sd", "ss", "sy", "tr", "ua", "vn", "zw" };
    private static string[] blockedRegion = { "arizona", "arkansas", "connecticut", "delaware", "florida", "louisiana", "maryland", "montana", "south carolina", "south dakota", "tennessee" };
    public static bool checkCountry(string country_code)
    {
        //For ALL BLOCKED COUNTRIES
        if (!country_code.ToLower().Equals("us"))
        {
            foreach (string code in blockedCountry)
            {
                if (country_code.ToLower().Equals(code))
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return checkRegion(UserManager.Get.CurrentUser.country);
        }
    }
    public static bool checkRegion(string region)
    {
        //For USA State
        foreach (string rg in blockedRegion)
        {
            if (region.ToLower().Equals(rg))
            {
                return false;
            }
        }
        return true;
    }
}
