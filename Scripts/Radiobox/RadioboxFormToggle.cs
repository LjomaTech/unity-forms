using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Individual radio button option within a RadioboxFormInput
    /// </summary>
    public class RadioboxFormToggle : MonoBehaviour
    {
        [Header("Option Settings")]
        [Tooltip("The value returned when this option is selected")]
        public string OptionValue;
        
        [Tooltip("Optional label text component")]
        public TMP_Text Label;

        [HideInInspector]
        public Toggle Toggle;

        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
            if (Toggle == null)
            {
                Toggle = GetComponentInChildren<Toggle>();
            }
        }

        /// <summary>
        /// Initialize this option with the parent toggle group
        /// </summary>
        public void Initialize(ToggleGroup group, bool interactable)
        {
            if (Toggle == null)
            {
                Toggle = GetComponent<Toggle>();
                if (Toggle == null)
                {
                    Toggle = GetComponentInChildren<Toggle>();
                }
            }

            if (Toggle != null)
            {
                Toggle.group = group;
                Toggle.interactable = interactable;
            }
        }

        /// <summary>
        /// Sets the label text
        /// </summary>
        public void SetLabel(string text)
        {
            if (Label != null)
            {
                Label.text = text;
            }
        }

        /// <summary>
        /// Sets whether this option is selected
        /// </summary>
        public void SetSelected(bool selected)
        {
            if (Toggle != null)
            {
                Toggle.isOn = selected;
            }
        }
    }
}
