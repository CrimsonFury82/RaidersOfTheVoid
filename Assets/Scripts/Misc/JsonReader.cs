using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader {

    public static string LoadJsonAsResource(string path)
    {
        string jsonfilepath = path.Replace(".json", "");
        TextAsset loadedJsonfile = Resources.Load<TextAsset>(jsonfilepath);
        return loadedJsonfile.text;
    }
}
