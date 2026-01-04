using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Radio button group input - only one option can be selected at a time
    /// </summary>
    public class RadioboxFormInput : FormInput
    {
        [Header("Radiobox Settings")]
        [Tooltip("The ToggleGroup component managing exclusivity")]
        public ToggleGroup ToggleGroup;
        
        [Tooltip("List of toggle options in this radio group")]
        public List<RadioboxFormToggle> Options = new List<RadioboxFormToggle>();

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            // Find the selected toggle and return its value
            foreach (var option in Options)
            {
                if (option != null && option.Toggle != null && option.Toggle.isOn)
                {
                    return option.OptionValue;
                }
            }
            return "";
        }

        /// <summary>
        /// Gets the index of the currently selected option (-1 if none)
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < Options.Count; i++)
                {
                    if (Options[i] != null && Options[i].Toggle != null && Options[i].Toggle.isOn)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public override void SetValue(string value)
        {
            foreach (var option in Options)
            {
                if (option != null && option.Toggle != null)
                {
                    option.Toggle.isOn = option.OptionValue == value;
                }
            }
        }

        /// <summary>
        /// Sets the selected option by index
        /// </summary>
        public void SetValueByIndex(int index)
        {
            for (int i = 0; i < Options.Count; i++)
            {
                if (Options[i] != null && Options[i].Toggle != null)
                {
                    Options[i].Toggle.isOn = i == index;
                }
            }
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            foreach (var option in Options)
            {
                if (option != null && option.Toggle != null)
                {
                    option.Toggle.interactable = !isReadOnly;
                }
            }
        }

        public override bool Validate()
        {
            if (isRequired)
            {
                bool hasSelection = Options.Any(o => o != null && o.Toggle != null && o.Toggle.isOn);
                if (!hasSelection)
                {
                    ShowError("Please select an option.");
                    return false;
                }
            }
            
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            // Auto-find toggle group if not assigned
            if (ToggleGroup == null)
            {
                ToggleGroup = GetComponent<ToggleGroup>();
                if (ToggleGroup == null)
                {
                    ToggleGroup = GetComponentInChildren<ToggleGroup>();
                }
            }

            // Auto-find options if not assigned
            if (Options == null || Options.Count == 0)
            {
                Options = GetComponentsInChildren<RadioboxFormToggle>().ToList();
            }

            // Setup each option
            foreach (var option in Options)
            {
                if (option != null)
                {
                    option.Initialize(ToggleGroup, !IsReadOnly);
                }
            }
        }

        /// <summary>
        /// Parses a comma-separated value string into an array
        /// </summary>
        public static string[] FormatValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }
            return value.Split(',').Select(s => s.Trim()).ToArray();
        }
    }
}