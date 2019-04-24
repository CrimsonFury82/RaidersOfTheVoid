using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public List<Item> items;

    private void Awake()
    {
        items = new List<Item>();
        ItemDictionary dictionary = JsonUtility.FromJson<ItemDictionary>(JsonReader.LoadJsonAsResource("Items/ItemDictionary.json"));
        foreach (string dictionaryItem in dictionary.items)
        {
            LoadItem(dictionaryItem);
        }
    }

    public void LoadItem(string path)
    {
        string myLoadedItem = JsonReader.LoadJsonAsResource(path);
        Item myItem = JsonUtility.FromJson<Item>(myLoadedItem);
        items.Add(myItem);
    }

}
