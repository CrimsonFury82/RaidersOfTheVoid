using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoader : MonoBehaviour
{
    public List<BaseCardData> items;

    private void Awake()
    {
        items = new List<BaseCardData>();
        ItemDictionary dictionary = JsonUtility.FromJson<ItemDictionary>(JsonReader.LoadJsonAsResource("Items/ItemDictionary.json"));
        foreach (string dictionaryItem in dictionary.items)
        {
            LoadItem(dictionaryItem);
        }
    }

    public void LoadItem(string path)
    {
        string myLoadedItem = JsonReader.LoadJsonAsResource(path);
        BaseCardData myItem = JsonUtility.FromJson<BaseCardData>(myLoadedItem);
        items.Add(myItem);
    }

}
