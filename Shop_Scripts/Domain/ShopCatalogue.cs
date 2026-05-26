using System;
using System.Collections.Generic;

namespace CleanRefactor
{
    public sealed class ShopCatalogue
    {
        private readonly Dictionary<string, ShopItem> m_items;

        public ShopCatalogue(IReadOnlyList<ShopItem> i_items)
        {
            if (i_items == null)
            {
                throw new ArgumentNullException(nameof(i_items));
            }

            if (i_items.Count == 0)
            {
                throw new ArgumentException("Catalogue must contain at least one item.", nameof(i_items));
            }

            m_items = new Dictionary<string, ShopItem>();

            foreach (ShopItem item in i_items)
            {
                if (item == null)
                {
                    throw new ArgumentException("Catalogue cannot contain null items.", nameof(i_items));
                }

                if (m_items.ContainsKey(item.Id))
                {
                    throw new ArgumentException($"Duplicate item id in catalogue: {item.Id}", nameof(i_items));
                }

                m_items[item.Id] = item;
            }
        }

        public ShopItem GetById(string i_itemId)
        {
            if (!m_items.TryGetValue(i_itemId, out ShopItem item))
            {
                throw new KeyNotFoundException($"Item not found in catalogue: {i_itemId}");
            }

            return item;
        }

        public IEnumerable<ShopItem> GetAll() => m_items.Values;
    }
}
