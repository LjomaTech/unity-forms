using LjomaAssets.FormManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LTAssets.FormManagament
{
    public class FormSliderInput : FormInput
    {
        private Slider _slider;
        
        [Header("Slider Settings")]
        public float MinValue = 0f;
        public float MaxValue = 100f;
        public float DefaultValue = 50f;
        public bool WholeNumbers = false;
        
        [Header("Display")]
        [Tooltip("Optional text component to display current value")]
        public TMP_Text ValueDisplay;
        [Tooltip("Format string for value display (e.g., '{0:F1}' or '{0:0}%')")]
        public string ValueFormat = "{0:F0}";

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            return _slider.value.ToString();
        }

        /// <summary>
        /// Gets the current slider value as a float
        /// </summary>
        public float FloatValue => _slider.value;

        public override void SetValue(string value)
        {
            if (float.TryParse(value, out float result))
            {
                _slider.value = Mathf.Clamp(result, MinValue, MaxValue);
                UpdateValueDisplay();
            }
        }

        /// <summary>
        /// Sets the slider value directly
        /// </summary>
        public void SetFloatValue(float value)
        {
            _slider.value = Mathf.Clamp(value, MinValue, MaxValue);
            UpdateValueDisplay();
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            _slider.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            // Slider always has a valid value within range, so validation is typically successful
            // You could add custom range validation here if needed
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            _slider = GetComponent<Slider>();
            if (_slider == null)
            {
                _slider = GetComponentInChildren<Slider>();
            }

            if (_slider != null)
            {
                _slider.minValue = MinValue;
                _slider.maxValue = MaxValue;
                _slider.wholeNumbers = WholeNumbers;
                _slider.value = DefaultValue;
                _slider.interactable = !IsReadOnly;

                // Subscribe to value changes for display updates
                _slider.onValueChanged.AddListener(OnSliderValueChanged);
                UpdateValueDisplay();
            }
        }

        private void OnSliderValueChanged(float value)
        {
            UpdateValueDisplay();
        }

        private void UpdateValueDisplay()
        {
            if (ValueDisplay != null && _slider != null)
            {
                ValueDisplay.text = string.Format(ValueFormat, _slider.value);
            }
        }

        private void OnDestroy()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            }
        }
    }
}