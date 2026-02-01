using System;

namespace Shop.Web.Utility;

public class SD
{
    public static string CouponApiBase { get; set; }
    public static string AuthApiBase { get; set; }
    public static string ProductApiBase { get; set; }
    public static string CartApiBase { get; set; }
    public static string TokenCookie = "AuthToken";
    public enum APITYPE
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum Role
    {
        Admin,
        Customer
    }
}
