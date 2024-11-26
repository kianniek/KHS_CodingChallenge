using System;
using TMPro;
using UnityEngine;

namespace Helper
{
    public class TMP_TextChanger : MonoBehaviour
    {
        private TMP_Text textComponent;
        private string originalText;

        [SerializeField] private string textMask = "{AmmoAmount}";
        
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

        /// <summary>
        /// Update the text of the TMP_Text component with the new text.
        /// </summary>
        /// <param name="newText">Takes a string as input.</param>
        public void UpdateText(string newText)
        {
            textComponent.text = originalText.Replace(textMask, newText);
        }
        
        /// <summary>
        /// Update the text of the TMP_Text component with the new text.
        /// </summary>
        /// <param name="newText">Takes a int as input.</param>
        public void UpdateText(int newText)
        {
            UpdateText(newText.ToString());
        }
        
        /// <summary>
        /// Update the text of the TMP_Text component with the new text.
        /// </summary>
        /// <param name="newText">Takes a float as input.</param>
        public void UpdateText(float newText)
        {
            UpdateText(newText.ToString());
        }
        
        /// <summary>
        /// Update the text of the TMP_Text component with the new text.
        /// </summary>
        /// <param name="newText">Takes a double as input.</param>
        public void UpdateText(double newText)
        {
            UpdateText(newText.ToString());
        }
    }
}