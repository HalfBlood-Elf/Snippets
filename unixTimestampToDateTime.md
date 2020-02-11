[source](https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa)

    public static DateTime UnixTimeStampToDateTime( double unixTimeStamp ) //in seconds
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dtDateTime;
    }

## For the .NET Framework >= 4.6
    System.DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
