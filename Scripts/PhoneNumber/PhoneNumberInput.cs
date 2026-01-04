using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Phone number input with optional country code dropdown
    /// Returns full phone number as a string
    /// </summary>
    public class PhoneNumberInput : FormInput
    {
        [Header("Phone Number Components")]
        [Tooltip("Country code dropdown (optional)")]
        public TMP_Dropdown CountryCodeDropdown;
        
        [Tooltip("Phone number input field")]
        public TMP_InputField PhoneField;

        [Header("Phone Settings")]
        [Tooltip("Default country code (e.g., '+1')")]
        public string DefaultCountryCode = "+1";
        
        [Tooltip("Minimum digits required (excluding country code)")]
        public int MinDigits = 7;
        
        [Tooltip("Maximum digits allowed (excluding country code)")]
        public int MaxDigits = 15;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            string countryCode = "";
            if (CountryCodeDropdown != null && CountryCodeDropdown.value >= 0 && 
                CountryCodeDropdown.value < CountryCodeDropdown.options.Count)
            {
                countryCode = CountryCodeDropdown.options[CountryCodeDropdown.value].text;
            }
            else
            {
                countryCode = DefaultCountryCode;
            }
            
            string phoneNumber = PhoneField != null ? PhoneField.text : "";
            
            // Remove any non-digit characters from phone number for storage
            string cleanNumber = Regex.Replace(phoneNumber, @"\D", "");
            
            return countryCode + cleanNumber;
        }

        /// <summary>
        /// Gets just the phone number without the country code
        /// </summary>
        public string PhoneNumber => PhoneField != null ? PhoneField.text : "";

        /// <summary>
        /// Gets the selected country code
        /// </summary>
        public string CountryCode
        {
            get
            {
                if (CountryCodeDropdown != null && CountryCodeDropdown.value >= 0 && 
                    CountryCodeDropdown.value < CountryCodeDropdown.options.Count)
                {
                    return CountryCodeDropdown.options[CountryCodeDropdown.value].text;
                }
                return DefaultCountryCode;
            }
        }

        public override void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (PhoneField != null) PhoneField.text = "";
                return;
            }

            // Try to parse country code from the beginning of the value
            if (value.StartsWith("+"))
            {
                // Find where the country code ends (after digits following the +)
                int codeEndIndex = 1;
                while (codeEndIndex < value.Length && char.IsDigit(value[codeEndIndex]))
                {
                    codeEndIndex++;
                    if (codeEndIndex > 4) break; // Country codes are typically 1-4 digits
                }
                
                string countryCode = value.Substring(0, codeEndIndex);
                string phoneNumber = value.Substring(codeEndIndex);
                
                // Set country code in dropdown if available
                if (CountryCodeDropdown != null)
                {
                    for (int i = 0; i < CountryCodeDropdown.options.Count; i++)
                    {
                        if (CountryCodeDropdown.options[i].text == countryCode)
                        {
                            CountryCodeDropdown.value = i;
                            break;
                        }
                    }
                }
                
                if (PhoneField != null) PhoneField.text = phoneNumber;
            }
            else
            {
                if (PhoneField != null) PhoneField.text = value;
            }
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            if (PhoneField != null) PhoneField.readOnly = isReadOnly;
            if (CountryCodeDropdown != null) CountryCodeDropdown.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            string phoneNumber = PhoneField != null ? PhoneField.text : "";
            string cleanNumber = Regex.Replace(phoneNumber, @"\D", "");
            
            if (isRequired && string.IsNullOrEmpty(cleanNumber))
            {
                ShowError("Phone number is required.");
                return false;
            }

            if (!string.IsNullOrEmpty(cleanNumber))
            {
                if (cleanNumber.Length < MinDigits)
                {
                    ShowError($"Phone number must have at least {MinDigits} digits.");
                    return false;
                }

                if (cleanNumber.Length > MaxDigits)
                {
                    ShowError($"Phone number cannot exceed {MaxDigits} digits.");
                    return false;
                }
            }

            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            if (PhoneField == null)
            {
                PhoneField = GetComponent<TMP_InputField>();
                if (PhoneField == null)
                {
                    PhoneField = GetComponentInChildren<TMP_InputField>();
                }
            }

            if (CountryCodeDropdown == null)
            {
                CountryCodeDropdown = GetComponent<TMP_Dropdown>();
                if (CountryCodeDropdown == null)
                {
                    CountryCodeDropdown = GetComponentInChildren<TMP_Dropdown>();
                }
            }

            if (PhoneField != null)
            {
                PhoneField.contentType = TMP_InputField.ContentType.Custom;
                PhoneField.characterValidation = TMP_InputField.CharacterValidation.Digit;
                PhoneField.interactable = !IsReadOnly;
            }

            if (CountryCodeDropdown != null)
            {
                CountryCodeDropdown.interactable = !IsReadOnly;
            }
        }
    }
}