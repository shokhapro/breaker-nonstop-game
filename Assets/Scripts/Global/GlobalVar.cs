using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVar
{
    private static Dictionary<string, object> vars = new Dictionary<string, object>();

    public static void Set<T>(string key, T var)
    {
        if (vars.ContainsKey(key))
            vars[key] = var;
        else
            vars.Add(key, var);
    }

    public static T Get<T>(string key)
    {
        if (vars.ContainsKey(key))
            return (T)vars[key];
        else
            return default;
    }


    private static Dictionary<string, int> ints = new Dictionary<string, int>();

    public static void SetInt(string key, int var)
    {
        if (ints.ContainsKey(key))
            ints[key] = var;
        else
            ints.Add(key, var);
    }

    public static int GetInt(string key)
    {
        if (ints.ContainsKey(key))
            return ints[key];
        else
            return default;
    }


    private static Dictionary<string, string> strs = new Dictionary<string, string>();

    public static void SetString(string key, string var)
    {
        if (strs.ContainsKey(key))
            strs[key] = var;
        else
            strs.Add(key, var);
    }

    public static string GetString(string key)
    {
        if (strs.ContainsKey(key))
            return strs[key];
        else
            return default;
    }


    private static Dictionary<string, bool> bools = new Dictionary<string, bool>();

    public static void SetBool(string key, bool var)
    {
        if (bools.ContainsKey(key))
            bools[key] = var;
        else
            bools.Add(key, var);
    }

    public static bool GetBool(string key)
    {
        if (bools.ContainsKey(key))
            return bools[key];
        else
            return default;
    }


    private static Dictionary<string, float> floats = new Dictionary<string, float>();

    public static void SetFloat(string key, float var)
    {
        if (floats.ContainsKey(key))
            floats[key] = var;
        else
            floats.Add(key, var);
    }

    public static float GetFloat(string key)
    {
        if (floats.ContainsKey(key))
            return floats[key];
        else
            return default;
    }
}
