using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct JsonDateTime
{
    public long Value;
    
    public static implicit operator DateTime(JsonDateTime jdt) {
//        Debug.Log("Converted to time");
        return DateTime.FromFileTimeUtc(jdt.Value);
    }
    
    public static implicit operator JsonDateTime(DateTime dt) {
        Debug.Log("Converted to JDT");
        JsonDateTime jdt = new JsonDateTime();
        jdt.Value = dt.ToFileTimeUtc();
        return jdt;
    }

    public override string ToString()
    {
        var dt = (DateTime) this;
        return dt.ToString(CultureInfo.InvariantCulture);
    }
}
