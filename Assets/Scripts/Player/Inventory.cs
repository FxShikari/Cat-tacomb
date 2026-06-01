using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    [SerializeField]
    private List<Item> items = new List<Item>();
    [SerializeField]
    private int itemCount = 0;

    // Getter
    public List<Item> getItems()
    {
        return items;
    }

    public int getItemCount()
    {
        return itemCount;
    }

    // Méthode spécifique
    public void AddItem(Item item)
    {
        items.Add(item);
        itemCount++;
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
        itemCount--;
    }
}