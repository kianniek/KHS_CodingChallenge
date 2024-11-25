using System;
using TMPro;
using UnityEngine;

namespace Helper
{
    public class TMP_TextChanger : MonoBehaviour
    {
        private TMP_Text textComponent;

        [SerializeField] private string textMask = "{AmmoAmount}";

        private string originalText;

        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();
            if (textComponent == null)
            {
                Debug.LogError("Text component not found.");
            }

            originalText = textComponent.text;
            
            UpdateText(0);
        }

        public void UpdateText(string newText)
        {
            textComponent.text = originalText.Replace(textMask, newText);
        }
        
        public void UpdateText(int newText)
        {
            UpdateText(newText.ToString());
        }
        
        public void UpdateText(float newText)
        {
            UpdateText(newText.ToString());
        }
        
        public void UpdateText(double newText)
        {
            UpdateText(newText.ToString());
        }
    }
}