using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LjomaAssets.FormManagement
{
    public class FormDropdown : FormInput
    {
        private TMP_Dropdown _dropdown;

        [Header("Dropdown Settings")]
        [Tooltip("List of options for the dropdown")]
        public List<string> Options = new List<string>();
        
        [Tooltip("Default selected index (-1 for none)")]
        public int DefaultIndex = 0;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            if (_dropdown.value >= 0 && _dropdown.value < _dropdown.options.Count)
            {
                return _dropdown.options[_dropdown.value].text;
            }
            return "";
        }

        /// <summary>
        /// Gets the currently selected index
        /// </summary>
        public int SelectedIndex => _dropdown.value;

        public override void SetValue(string value)
        {
            // Try to find the option by text
            for (int i = 0; i < _dropdown.options.Count; i++)
            {
                if (_dropdown.options[i].text == value)
                {
                    _dropdown.value = i;
                    _dropdown.RefreshShownValue();
                    return;
                }
            }

            // Try to parse as index
            if (int.TryParse(value, out int index) && index >= 0 && index < _dropdown.options.Count)
            {
                _dropdown.value = index;
                _dropdown.RefreshShownValue();
            }
        }

        /// <summary>
        /// Sets the dropdown value by index
        /// </summary>
        public void SetValueByIndex(int index)
        {
            if (index >= 0 && index < _dropdown.options.Count)
            {
                _dropdown.value = index;
                _dropdown.RefreshShownValue();
            }
        }

        /// <summary>
        /// Populates the dropdown with new options
        /// </summary>
        public void SetOptions(List<string> options)
        {
            Options = options;
            _dropdown.ClearOptions();
            _dropdown.AddOptions(options);
            
            if (DefaultIndex >= 0 && DefaultIndex < options.Count)
            {
                _dropdown.value = DefaultIndex;
            }
            _dropdown.RefreshShownValue();
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            _dropdown.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            if (isRequired && _dropdown.value < 0)
            {
                ShowError("Please select an option.");
                return false;
            }

            // Optionally validate that a meaningful selection was made
            if (isRequired && _dropdown.options.Count > 0 && string.IsNullOrEmpty(_dropdown.options[_dropdown.value].text))
            {
                ShowError("Please select a valid option.");
                return false;
            }

            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            if (_dropdown == null)
            {
                _dropdown = GetComponentInChildren<TMP_Dropdown>();
            }

            if (_dropdown != null)
            {
                _dropdown.interactable = !IsReadOnly;

                // Populate options if provided in inspector
                if (Options != null && Options.Count > 0)
                {
                    _dropdown.ClearOptions();
                    _dropdown.AddOptions(Options);
                }

                if (DefaultIndex >= 0 && DefaultIndex < _dropdown.options.Count)
                {
                    _dropdown.value = DefaultIndex;
                    _dropdown.RefreshShownValue();
                }
            }
        }
    }
}
