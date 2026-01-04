using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Double slider (range input) with minimum and maximum handles
    /// Returns value as "min,max" string format
    /// </summary>
    public class DoubleSlider : FormInput
    {
        [Header("Range Slider Components")]
        public Slider MinSlider;
        public Slider MaxSlider;
        
        [Header("Range Settings")]
        public float RangeMin = 0f;
        public float RangeMax = 100f;
        public float DefaultMin = 25f;
        public float DefaultMax = 75f;
        public bool WholeNumbers = false;
        
        [Header("Display")]
        public TMP_Text MinValueDisplay;
        public TMP_Text MaxValueDisplay;
        public string ValueFormat = "{0:F0}";

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            return $"{MinSlider.value},{MaxSlider.value}";
        }

        /// <summary>
        /// Gets the current minimum value
        /// </summary>
        public float MinValue => MinSlider.value;

        /// <summary>
        /// Gets the current maximum value
        /// </summary>
        public float MaxValue => MaxSlider.value;

        public override void SetValue(string value)
        {
            string[] parts = value.Split(',');
            if (parts.Length == 2)
            {
                if (float.TryParse(parts[0], out float min) && float.TryParse(parts[1], out float max))
                {
                    SetRange(min, max);
                }
            }
        }

        /// <summary>
        /// Sets both min and max values
        /// </summary>
        public void SetRange(float min, float max)
        {
            min = Mathf.Clamp(min, RangeMin, RangeMax);
            max = Mathf.Clamp(max, RangeMin, RangeMax);
            
            // Ensure min <= max
            if (min > max)
            {
                (min, max) = (max, min);
            }

            MinSlider.value = min;
            MaxSlider.value = max;
            UpdateDisplays();
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            if (MinSlider != null) MinSlider.interactable = !isReadOnly;
            if (MaxSlider != null) MaxSlider.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            // Validate that min is less than or equal to max
            if (MinSlider.value > MaxSlider.value)
            {
                ShowError("Minimum value cannot be greater than maximum value.");
                return false;
            }
            
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            // Auto-find sliders if not assigned
            if (MinSlider == null || MaxSlider == null)
            {
                Slider[] sliders = GetComponentsInChildren<Slider>();
                if (sliders.Length >= 2)
                {
                    MinSlider = sliders[0];
                    MaxSlider = sliders[1];
                }
            }

            if (MinSlider != null)
            {
                MinSlider.minValue = RangeMin;
                MinSlider.maxValue = RangeMax;
                MinSlider.wholeNumbers = WholeNumbers;
                MinSlider.value = DefaultMin;
                MinSlider.interactable = !IsReadOnly;
                MinSlider.onValueChanged.AddListener(OnMinSliderChanged);
            }

            if (MaxSlider != null)
            {
                MaxSlider.minValue = RangeMin;
                MaxSlider.maxValue = RangeMax;
                MaxSlider.wholeNumbers = WholeNumbers;
                MaxSlider.value = DefaultMax;
                MaxSlider.interactable = !IsReadOnly;
                MaxSlider.onValueChanged.AddListener(OnMaxSliderChanged);
            }

            UpdateDisplays();
        }

        private void OnMinSliderChanged(float value)
        {
            // Clamp min to not exceed max
            if (value > MaxSlider.value)
            {
                MinSlider.value = MaxSlider.value;
            }
            UpdateDisplays();
        }

        private void OnMaxSliderChanged(float value)
        {
            // Clamp max to not be less than min
            if (value < MinSlider.value)
            {
                MaxSlider.value = MinSlider.value;
            }
            UpdateDisplays();
        }

        private void UpdateDisplays()
        {
            if (MinValueDisplay != null && MinSlider != null)
            {
                MinValueDisplay.text = string.Format(ValueFormat, MinSlider.value);
            }
            if (MaxValueDisplay != null && MaxSlider != null)
            {
                MaxValueDisplay.text = string.Format(ValueFormat, MaxSlider.value);
            }
        }

        private void OnDestroy()
        {
            if (MinSlider != null) MinSlider.onValueChanged.RemoveListener(OnMinSliderChanged);
            if (MaxSlider != null) MaxSlider.onValueChanged.RemoveListener(OnMaxSliderChanged);
        }
    }
}
