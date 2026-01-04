using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Date/Time input using dropdown selectors for year, month, day, hour, minute
    /// Returns value as ISO 8601 string (yyyy-MM-ddTHH:mm:ss)
    /// </summary>
    public class DateTimeInput : FormInput
    {
        [Header("Date Components")]
        public TMP_Dropdown YearDropdown;
        public TMP_Dropdown MonthDropdown;
        public TMP_Dropdown DayDropdown;

        [Header("Time Components (Optional)")]
        public TMP_Dropdown HourDropdown;
        public TMP_Dropdown MinuteDropdown;

        [Header("Settings")]
        [Tooltip("Include time selection")]
        public bool IncludeTime = false;
        
        [Tooltip("Minimum year in dropdown")]
        public int MinYear = 1920;
        
        [Tooltip("Maximum year in dropdown")]
        public int MaxYear = 2100;
        
        [Tooltip("Minute step (e.g., 5 for every 5 minutes)")]
        public int MinuteStep = 1;

        private DateTime _selectedDateTime = DateTime.Now;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            UpdateDateTime();
            return IncludeTime 
                ? _selectedDateTime.ToString("yyyy-MM-ddTHH:mm:ss")
                : _selectedDateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Gets the selected DateTime value
        /// </summary>
        public DateTime SelectedDateTime
        {
            get
            {
                UpdateDateTime();
                return _selectedDateTime;
            }
        }

        public override void SetValue(string value)
        {
            if (DateTime.TryParse(value, out DateTime dateTime))
            {
                SetDateTime(dateTime);
            }
        }

        /// <summary>
        /// Sets the date/time value directly
        /// </summary>
        public void SetDateTime(DateTime dateTime)
        {
            _selectedDateTime = dateTime;
            
            // Set dropdowns
            if (YearDropdown != null)
            {
                int yearIndex = dateTime.Year - MinYear;
                if (yearIndex >= 0 && yearIndex < YearDropdown.options.Count)
                {
                    YearDropdown.value = yearIndex;
                    YearDropdown.RefreshShownValue();
                }
            }

            if (MonthDropdown != null)
            {
                MonthDropdown.value = dateTime.Month - 1;
                MonthDropdown.RefreshShownValue();
                UpdateDayOptions();
            }

            if (DayDropdown != null)
            {
                int dayIndex = dateTime.Day - 1;
                if (dayIndex >= 0 && dayIndex < DayDropdown.options.Count)
                {
                    DayDropdown.value = dayIndex;
                    DayDropdown.RefreshShownValue();
                }
            }

            if (IncludeTime)
            {
                if (HourDropdown != null)
                {
                    HourDropdown.value = dateTime.Hour;
                    HourDropdown.RefreshShownValue();
                }

                if (MinuteDropdown != null)
                {
                    MinuteDropdown.value = dateTime.Minute / MinuteStep;
                    MinuteDropdown.RefreshShownValue();
                }
            }
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            if (YearDropdown != null) YearDropdown.interactable = !isReadOnly;
            if (MonthDropdown != null) MonthDropdown.interactable = !isReadOnly;
            if (DayDropdown != null) DayDropdown.interactable = !isReadOnly;
            if (HourDropdown != null) HourDropdown.interactable = !isReadOnly;
            if (MinuteDropdown != null) MinuteDropdown.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            UpdateDateTime();
            
            if (isRequired)
            {
                // Check if default/empty selection
                if (_selectedDateTime == default)
                {
                    ShowError("Please select a valid date.");
                    return false;
                }
            }

            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            // Auto-find components if not assigned
            var dropdowns = GetComponentsInChildren<TMP_Dropdown>();
            int dropdownIndex = 0;
            
            if (YearDropdown == null && dropdownIndex < dropdowns.Length)
                YearDropdown = dropdowns[dropdownIndex++];
            if (MonthDropdown == null && dropdownIndex < dropdowns.Length)
                MonthDropdown = dropdowns[dropdownIndex++];
            if (DayDropdown == null && dropdownIndex < dropdowns.Length)
                DayDropdown = dropdowns[dropdownIndex++];
            if (IncludeTime)
            {
                if (HourDropdown == null && dropdownIndex < dropdowns.Length)
                    HourDropdown = dropdowns[dropdownIndex++];
                if (MinuteDropdown == null && dropdownIndex < dropdowns.Length)
                    MinuteDropdown = dropdowns[dropdownIndex++];
            }

            PopulateDropdowns();
            SetupListeners();
        }

        private void PopulateDropdowns()
        {
            // Years
            if (YearDropdown != null)
            {
                YearDropdown.ClearOptions();
                var years = new List<string>();
                for (int y = MinYear; y <= MaxYear; y++)
                {
                    years.Add(y.ToString());
                }
                YearDropdown.AddOptions(years);
                YearDropdown.value = DateTime.Now.Year - MinYear;
                YearDropdown.interactable = !IsReadOnly;
            }

            // Months
            if (MonthDropdown != null)
            {
                MonthDropdown.ClearOptions();
                var months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", 
                                                  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                MonthDropdown.AddOptions(months);
                MonthDropdown.value = DateTime.Now.Month - 1;
                MonthDropdown.interactable = !IsReadOnly;
            }

            // Days (will be updated based on month/year)
            UpdateDayOptions();

            // Hours
            if (HourDropdown != null && IncludeTime)
            {
                HourDropdown.ClearOptions();
                var hours = new List<string>();
                for (int h = 0; h < 24; h++)
                {
                    hours.Add(h.ToString("D2"));
                }
                HourDropdown.AddOptions(hours);
                HourDropdown.value = DateTime.Now.Hour;
                HourDropdown.interactable = !IsReadOnly;
            }

            // Minutes
            if (MinuteDropdown != null && IncludeTime)
            {
                MinuteDropdown.ClearOptions();
                var minutes = new List<string>();
                for (int m = 0; m < 60; m += MinuteStep)
                {
                    minutes.Add(m.ToString("D2"));
                }
                MinuteDropdown.AddOptions(minutes);
                MinuteDropdown.value = DateTime.Now.Minute / MinuteStep;
                MinuteDropdown.interactable = !IsReadOnly;
            }
        }

        private void SetupListeners()
        {
            if (YearDropdown != null)
                YearDropdown.onValueChanged.AddListener(_ => UpdateDayOptions());
            if (MonthDropdown != null)
                MonthDropdown.onValueChanged.AddListener(_ => UpdateDayOptions());
        }

        private void UpdateDayOptions()
        {
            if (DayDropdown == null) return;

            int year = MinYear + (YearDropdown != null ? YearDropdown.value : 0);
            int month = (MonthDropdown != null ? MonthDropdown.value : 0) + 1;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            int currentDaySelection = DayDropdown.value;
            
            DayDropdown.ClearOptions();
            var days = new List<string>();
            for (int d = 1; d <= daysInMonth; d++)
            {
                days.Add(d.ToString());
            }
            DayDropdown.AddOptions(days);
            
            // Restore selection if valid
            DayDropdown.value = Mathf.Min(currentDaySelection, daysInMonth - 1);
            DayDropdown.interactable = !IsReadOnly;
        }

        private void UpdateDateTime()
        {
            int year = MinYear + (YearDropdown != null ? YearDropdown.value : 0);
            int month = (MonthDropdown != null ? MonthDropdown.value : 0) + 1;
            int day = (DayDropdown != null ? DayDropdown.value : 0) + 1;
            int hour = IncludeTime && HourDropdown != null ? HourDropdown.value : 0;
            int minute = IncludeTime && MinuteDropdown != null ? MinuteDropdown.value * MinuteStep : 0;

            try
            {
                _selectedDateTime = new DateTime(year, month, day, hour, minute, 0);
            }
            catch
            {
                _selectedDateTime = DateTime.Now;
            }
        }
    }
}
