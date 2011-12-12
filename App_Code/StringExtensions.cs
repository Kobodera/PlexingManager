using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StringExtensions
/// </summary>
public static class StringExtensions
{
    public static int ToInt(this string str)
    {
        int result = 0;
        if (int.TryParse(str, out result))
            return result;

        return 0;
    }

    public static double ToDouble(this string str)
    {
        double result = 0;
        if (double.TryParse(str, out result))
            return result;

        return 0;
    }

    //public StringExtensions()
    //{
    //    //
    //    // TODO: Add constructor logic here
    //    //
    //}
}