using System;

namespace WLX.Utils.WebUtils
{
    public static class MachHelper
    {

        public static double Round(this double d, int digits)
        {
            int digitsN = 2;

            return Convert.ToDouble(Math.Round(d, digitsN + 2).ToString("F" + digitsN.ToString()));
        }
    }
}
