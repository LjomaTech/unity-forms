using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Helper class to manage country code dropdown population and selection
    /// </summary>
    public class CountryCodeHandler : MonoBehaviour
    {
        [Serializable]
        public class CountryCode
        {
            public string Name;
            public string Code;
            public string DialCode;
        }

        [Header("Component")]
        public TMP_Dropdown Dropdown;

        [Header("Settings")]
        [Tooltip("Default country dial code (e.g., '+1')")]
        public string DefaultDialCode = "+1";

        /// <summary>
        /// Common country codes list
        /// </summary>
        public static readonly List<CountryCode> CommonCountryCodes = new List<CountryCode>
        {
            new CountryCode { Name = "United States", Code = "US", DialCode = "+1" },
            new CountryCode { Name = "United Kingdom", Code = "GB", DialCode = "+44" },
            new CountryCode { Name = "Canada", Code = "CA", DialCode = "+1" },
            new CountryCode { Name = "Australia", Code = "AU", DialCode = "+61" },
            new CountryCode { Name = "Germany", Code = "DE", DialCode = "+49" },
            new CountryCode { Name = "France", Code = "FR", DialCode = "+33" },
            new CountryCode { Name = "Italy", Code = "IT", DialCode = "+39" },
            new CountryCode { Name = "Spain", Code = "ES", DialCode = "+34" },
            new CountryCode { Name = "Netherlands", Code = "NL", DialCode = "+31" },
            new CountryCode { Name = "Belgium", Code = "BE", DialCode = "+32" },
            new CountryCode { Name = "Switzerland", Code = "CH", DialCode = "+41" },
            new CountryCode { Name = "Austria", Code = "AT", DialCode = "+43" },
            new CountryCode { Name = "Sweden", Code = "SE", DialCode = "+46" },
            new CountryCode { Name = "Norway", Code = "NO", DialCode = "+47" },
            new CountryCode { Name = "Denmark", Code = "DK", DialCode = "+45" },
            new CountryCode { Name = "Finland", Code = "FI", DialCode = "+358" },
            new CountryCode { Name = "Poland", Code = "PL", DialCode = "+48" },
            new CountryCode { Name = "Ireland", Code = "IE", DialCode = "+353" },
            new CountryCode { Name = "Portugal", Code = "PT", DialCode = "+351" },
            new CountryCode { Name = "Japan", Code = "JP", DialCode = "+81" },
            new CountryCode { Name = "China", Code = "CN", DialCode = "+86" },
            new CountryCode { Name = "South Korea", Code = "KR", DialCode = "+82" },
            new CountryCode { Name = "India", Code = "IN", DialCode = "+91" },
            new CountryCode { Name = "Brazil", Code = "BR", DialCode = "+55" },
            new CountryCode { Name = "Mexico", Code = "MX", DialCode = "+52" },
            new CountryCode { Name = "Argentina", Code = "AR", DialCode = "+54" },
            new CountryCode { Name = "Russia", Code = "RU", DialCode = "+7" },
            new CountryCode { Name = "South Africa", Code = "ZA", DialCode = "+27" },
            new CountryCode { Name = "Israel", Code = "IL", DialCode = "+972" },
            new CountryCode { Name = "United Arab Emirates", Code = "AE", DialCode = "+971" },
            new CountryCode { Name = "Saudi Arabia", Code = "SA", DialCode = "+966" },
            new CountryCode { Name = "Singapore", Code = "SG", DialCode = "+65" },
            new CountryCode { Name = "New Zealand", Code = "NZ", DialCode = "+64" },
        };

        private void Start()
        {
            if (Dropdown == null)
            {
                Dropdown = GetComponent<TMP_Dropdown>();
            }
            
            if (Dropdown != null)
            {
                PopulateDropdown();
            }
        }

        /// <summary>
        /// Populates the dropdown with country codes
        /// </summary>
        public void PopulateDropdown()
        {
            if (Dropdown == null) return;

            Dropdown.ClearOptions();
            
            var options = new List<string>();
            int defaultIndex = 0;

            for (int i = 0; i < CommonCountryCodes.Count; i++)
            {
                var country = CommonCountryCodes[i];
                options.Add($"{country.DialCode} ({country.Code})");
                
                if (country.DialCode == DefaultDialCode)
                {
                    defaultIndex = i;
                }
            }

            Dropdown.AddOptions(options);
            Dropdown.value = defaultIndex;
            Dropdown.RefreshShownValue();
        }

        /// <summary>
        /// Gets the currently selected dial code
        /// </summary>
        public string SelectedDialCode
        {
            get
            {
                if (Dropdown != null && Dropdown.value >= 0 && Dropdown.value < CommonCountryCodes.Count)
                {
                    return CommonCountryCodes[Dropdown.value].DialCode;
                }
                return DefaultDialCode;
            }
        }

        /// <summary>
        /// Sets the dropdown to a specific dial code
        /// </summary>
        public void SetDialCode(string dialCode)
        {
            if (Dropdown == null) return;

            for (int i = 0; i < CommonCountryCodes.Count; i++)
            {
                if (CommonCountryCodes[i].DialCode == dialCode)
                {
                    Dropdown.value = i;
                    Dropdown.RefreshShownValue();
                    return;
                }
            }
        }
    }
}
