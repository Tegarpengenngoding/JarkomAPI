// Assets/Scripts/JsonHelper.cs
using System;
using UnityEngine; // <-- TAMBAHKAN BARIS INI

public static class JsonHelper
{
    public static T[] GetJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        
        // Sekarang Unity tahu apa itu 'JsonUtility'
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}