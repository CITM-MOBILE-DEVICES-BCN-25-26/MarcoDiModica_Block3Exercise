using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRefactor
{
    public sealed class ShopView : MonoBehaviour, IShopView
    {
        [Serializable]
        public sealed class ItemUIBinding
        {
            public ShopItemKey itemKey;
            public TextMeshProUGUI nameText;
            public TextMeshProUGUI costText;
            public TextMeshProUGUI statusText;
            public Button buyButton;
        }

        [Header("Common Texts")]
        [SerializeField] private TextMeshProUGUI m_coinsText;
        [SerializeField] private TextMeshProUGUI m_levelText;
        [SerializeField] private TextMeshProUGUI m_feedbackText;

        [Header("Item Bindings")]
        [SerializeField] private List<ItemUIBinding> m_itemBindings;

        [Header("Debug Buttons")]
        [SerializeField] private Button m_addCoinsButton;
        [SerializeField] private Button m_levelUpButton;
        [SerializeField] private Button m_resetButton;

        [Header("Audio")]
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_purchaseClip;
        [SerializeField] private AudioClip m_failClip;

        private static readonly Color COLOR_CAN_BUY = new Color(0.2f, 0.55f, 0.9f);
        private static readonly Color COLOR_CANNOT_BUY = new Color(0.6f, 0.2f, 0.2f);

        private readonly Dictionary<string, ItemUIBinding> m_bindingsById = new Dictionary<string, ItemUIBinding>();

        public event Action<string> BuyItemClicked;
        public event Action AddCoinsClicked;
        public event Action LevelUpClicked;
        public event Action ResetClicked;

        private void Awake()
        {
            foreach (ItemUIBinding binding in m_itemBindings)
            {
                string id = binding.itemKey.ToString();
                m_bindingsById[id] = binding;
                binding.buyButton.onClick.AddListener(() => BuyItemClicked?.Invoke(id));
            }

            m_addCoinsButton.onClick.AddListener(() => AddCoinsClicked?.Invoke());
            m_levelUpButton.onClick.AddListener(() => LevelUpClicked?.Invoke());
            m_resetButton.onClick.AddListener(() => ResetClicked?.Invoke());
        }

        public void SetCoins(int i_coins) => m_coinsText.text = "Coins: " + i_coins;
        public void SetLevel(int i_level) => m_levelText.text = "Level: " + i_level;
        public void SetFeedback(string i_message) => m_feedbackText.text = i_message;

        public void SetItemStatus(string i_itemId, string i_name, int i_cost, int i_currentQuantity, int i_maxQuantity, bool i_isUnique, bool i_canBuy)
        {
            if (!m_bindingsById.TryGetValue(i_itemId, out ItemUIBinding binding))
                return;

            if (binding.nameText != null) binding.nameText.text = i_name;
            if (binding.costText != null) binding.costText.text = "Cost: " + i_cost;

            binding.statusText.text = i_isUnique
                ? (i_currentQuantity > 0 ? "Owned" : "Not owned")
                : ("Uses: " + i_currentQuantity + " / " + i_maxQuantity);

            binding.buyButton.image.color = i_canBuy ? COLOR_CAN_BUY : COLOR_CANNOT_BUY;
        }

        public void PlayPurchaseSound()
        {
            if (m_purchaseClip != null && m_audioSource != null)
                m_audioSource.PlayOneShot(m_purchaseClip);
        }

        public void PlayFailSound()
        {
            if (m_failClip != null && m_audioSource != null)
                m_audioSource.PlayOneShot(m_failClip);
        }
    }
}
