using System;

namespace NetSteps.Testing.Integration
{
    /// <summary>
    /// Browse Products Status.
    /// </summary>
    public enum BrowseProductsStatus
    {
        Active = 1,
        Inactive
    }


    /// <summary>
    /// Browser types.
    /// </summary>
    public enum BrowserType
    {
        IE,
        Firefox,
        Chrome
    }

    /// <summary>
    /// DWS order table columns
    /// </summary>
    public enum DWS_Order_Columns
    {
        ID,
        First,
        Last
    }

    /// <summary>
    /// DWS order table columns
    /// </summary>
    public enum DWS_DownLine_Columns
    {
        Account = 4,
        First,
        Last
    }
}
