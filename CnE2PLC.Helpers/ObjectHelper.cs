using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CnE2PLC.Helpers;

public static class ObjectHelper
{
    public static T? DeepCopy<T>(this T source)
    {
        if (source == null) return default;

        // Configuration to handle complex objects
        var options = new JsonSerializerOptions
        {
            IncludeFields = true, // Ensures private/internal fields are included if needed
            PropertyNameCaseInsensitive = true
        };

        var jsonString = JsonSerializer.Serialize(source, options);
        return JsonSerializer.Deserialize<T>(jsonString, options);
    }
}
