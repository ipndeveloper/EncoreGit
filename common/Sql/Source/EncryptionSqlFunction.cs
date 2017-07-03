using System;
using System.Data.SqlTypes;
using NetSteps.Sql.Security;

public partial class UserDefinedFunctions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: SqlFunctions to encrypt and decrypt data in the DB through CLR function.
    /// Created: 01-21-2011
    /// </summary>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString nsEncrypt(string value)
    {
        try
        {
            return Encryption.EncryptTripleDES(value);
        }
        catch (Exception ex)
        {
            // TODO: Log errors for verification that this is working and to resolve problems - JHE
            return string.Empty;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString nsDecrypt(string value)
    {
        try
        {
            return Encryption.DecryptTripleDES(value);
        }
        catch (Exception ex)
        {
            // TODO: Log errors for verification that this is working and to resolve problems - JHE
            return string.Empty;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString ComputeHash(string value)
    {
        try
        {
            return SimpleHash.ComputeHash(value, NetSteps.Sql.Security.SimpleHash.Algorithm.SHA512, null);
        }
        catch (Exception ex)
        {
            // TODO: Log errors for verification that this is working and to resolve problems - JHE
            return string.Empty;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString ComputeMd5Hash(string value)
    {
        try
        {
            return SimpleHash.ComputeHash(value, NetSteps.Sql.Security.SimpleHash.Algorithm.MD5, null);
        }
        catch (Exception ex)
        {
            // TODO: Log errors for verification that this is working and to resolve problems - JHE
            return string.Empty;
        }
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean IsHashValid(string value, string hash)
    {
        try
        {
            return SimpleHash.VerifyHash(value, SimpleHash.Algorithm.SHA512, hash);
        }
        catch (Exception ex)
        {
            // TODO: Log errors for verification that this is working and to resolve problems - JHE
            return false;
        }
    }
};

