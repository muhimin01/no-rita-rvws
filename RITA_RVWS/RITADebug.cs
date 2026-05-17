using System;

namespace RITA_RVWS.Dev
{
    internal static class RITADebug
    {
        internal static void LogObjectContents(object obj)
        {
            if (obj == null)
            {
                RITA.Log.LogInfo("Object is null.");
                return;
            }

            var type = obj.GetType();
            RITA.Log.LogInfo($"Type: {type.FullName}");

            // Log all properties
            foreach (var prop in type.GetProperties())
            {
                try
                {
                    RITA.Log.LogInfo($"  Property: {prop.Name} = {prop.GetValue(obj)}");
                }
                catch (Exception ex)
                {
                    RITA.Log.LogInfo($"  Property: {prop.Name} = (error: {ex.Message})");
                }
            }

            // Log all fields
            foreach (var field in type.GetFields())
            {
                try
                {
                    RITA.Log.LogInfo($"  Field: {field.Name} = {field.GetValue(obj)}");
                }
                catch (Exception ex)
                {
                    RITA.Log.LogInfo($"  Field: {field.Name} = (error: {ex.Message})");
                }
            }
        }
    }
}
