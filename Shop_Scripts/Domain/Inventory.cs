using System;
using System.Collections.Generic;

namespace CleanRefactor
{
    public sealed class Inventory
    {
        private readonly Dictionary<string, int> m_items;

        public Inventory() : this(new Dictionary<string, int>()) { }

        public Inventory(IDictionary<string, int> i_initialItems)
        {
            if (i_initialItems == null)
            {
                throw new ArgumentNullException(nameof(i_initialItems));
            }

            m_items = new Dictionary<string, int>(i_initialItems);
        }

        public int GetQuantity(string i_itemId)
        {
            return m_items.TryGetValue(i_itemId, out int quantity) ? quantity : 0;
        }

        public bool Has(string i_itemId) => GetQuantity(i_itemId) > 0;

        public void Add(string i_itemId, int i_quantity = 1)
        {
            if (string.IsNullOrEmpty(i_itemId))
            {
                throw new ArgumentException("Item id cannot be null or empty.", nameof(i_itemId));
            }

            if (i_quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(i_quantity), "Quantity must be greater than zero.");
            }

            if (m_items.ContainsKey(i_itemId))
            {
                m_items[i_itemId] += i_quantity;
            }
            else
            {
                m_items[i_itemId] = i_quantity;
            }
        }
    }
}
