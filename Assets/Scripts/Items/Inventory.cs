using System.Collections.Generic;

using UnityEngine;

namespace Items {
    public class Inventory : MonoBehaviour {
        public const int MAX_ITEMS = 10;
        public Item[] items = new Item[MAX_ITEMS];
    }
}