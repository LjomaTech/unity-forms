using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Simplified color picker input using a hex input field and/or sliders
    /// Returns value as hex color string
    /// </summary>
    public class ColorPickerInput : FormInput
    {
        [Header("Color Input Components")]
        [Tooltip("Hex color input field (e.g., #FF5500)")]
        public TMP_InputField HexInput;
        
        [Tooltip("Red slider (0-255)")]
        public Slider RedSlider;
        
        [Tooltip("Green slider (0-255)")]
        public Slider GreenSlider;
        
        [Tooltip("Blue slider (0-255)")]
        public Slider BlueSlider;
        
        [Tooltip("Alpha slider (0-255) - optional")]
        public Slider AlphaSlider;
        
        [Tooltip("Preview of the selected color")]
        public Image PreviewImage;

        [Header("Settings")]
        public Color DefaultColor = Color.white;
        public bool IncludeAlpha = false;

        private Color _selectedColor;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            return IncludeAlpha 
                ? "#" + ColorUtility.ToHtmlStringRGBA(_selectedColor)
                : "#" + ColorUtility.ToHtmlStringRGB(_selectedColor);
        }

        /// <summary>
        /// Gets the currently selected color
        /// </summary>
        public Color SelectedColor => _selectedColor;

        public override void SetValue(string value)
        {
            if (ColorUtility.TryParseHtmlString(value.StartsWith("#") ? value : "#" + value, out Color color))
            {
                SetColor(color, false);
            }
        }

        /// <summary>
        /// Sets the color and updates all UI components
        /// </summary>
        public void SetColor(Color color, bool updateHexInput = true)
        {
            _selectedColor = color;
            UpdateSliders();
            if (updateHexInput) UpdateHexInput();
            UpdatePreview();
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            if (HexInput != null) HexInput.readOnly = isReadOnly;
            if (RedSlider != null) RedSlider.interactable = !isReadOnly;
            if (GreenSlider != null) GreenSlider.interactable = !isReadOnly;
            if (BlueSlider != null) BlueSlider.interactable = !isReadOnly;
            if (AlphaSlider != null) AlphaSlider.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            // Color input always has a valid color
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            // Setup sliders range
            SetupSlider(RedSlider);
            SetupSlider(GreenSlider);
            SetupSlider(BlueSlider);
            SetupSlider(AlphaSlider);

            // Subscribe to changes
            if (HexInput != null)
            {
                HexInput.onEndEdit.AddListener(OnHexInputChanged);
                HexInput.interactable = !IsReadOnly;
            }

            if (RedSlider != null) RedSlider.onValueChanged.AddListener(_ => OnSliderChanged());
            if (GreenSlider != null) GreenSlider.onValueChanged.AddListener(_ => OnSliderChanged());
            if (BlueSlider != null) BlueSlider.onValueChanged.AddListener(_ => OnSliderChanged());
            if (AlphaSlider != null) AlphaSlider.onValueChanged.AddListener(_ => OnSliderChanged());

            // Set default color
            SetColor(DefaultColor);
        }

        private void SetupSlider(Slider slider)
        {
            if (slider != null)
            {
                slider.minValue = 0;
                slider.maxValue = 255;
                slider.wholeNumbers = true;
                slider.interactable = !IsReadOnly;
            }
        }

        private void OnHexInputChanged(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex.StartsWith("#") ? hex : "#" + hex, out Color color))
            {
                _selectedColor = color;
                UpdateSliders();
                UpdatePreview();
            }
            else
            {
                // Revert to current color
                UpdateHexInput();
            }
        }

        private void OnSliderChanged()
        {
            float r = RedSlider != null ? RedSlider.value / 255f : _selectedColor.r;
            float g = GreenSlider != null ? GreenSlider.value / 255f : _selectedColor.g;
            float b = BlueSlider != null ? BlueSlider.value / 255f : _selectedColor.b;
            float a = AlphaSlider != null && IncludeAlpha ? AlphaSlider.value / 255f : _selectedColor.a;

            _selectedColor = new Color(r, g, b, a);
            UpdateHexInput();
            UpdatePreview();
        }

        private void UpdateSliders()
        {
            if (RedSlider != null) RedSlider.SetValueWithoutNotify(_selectedColor.r * 255);
            if (GreenSlider != null) GreenSlider.SetValueWithoutNotify(_selectedColor.g * 255);
            if (BlueSlider != null) BlueSlider.SetValueWithoutNotify(_selectedColor.b * 255);
            if (AlphaSlider != null && IncludeAlpha) AlphaSlider.SetValueWithoutNotify(_selectedColor.a * 255);
        }

        private void UpdateHexInput()
        {
            if (HexInput != null)
            {
                HexInput.SetTextWithoutNotify(IncludeAlpha 
                    ? "#" + ColorUtility.ToHtmlStringRGBA(_selectedColor)
                    : "#" + ColorUtility.ToHtmlStringRGB(_selectedColor));
            }
        }

        private void UpdatePreview()
        {
            if (PreviewImage != null)
            {
                PreviewImage.color = _selectedColor;
            }
        }

        private void OnDestroy()
        {
            if (HexInput != null) HexInput.onEndEdit.RemoveListener(OnHexInputChanged);
            if (RedSlider != null) RedSlider.onValueChanged.RemoveAllListeners();
            if (GreenSlider != null) GreenSlider.onValueChanged.RemoveAllListeners();
            if (BlueSlider != null) BlueSlider.onValueChanged.RemoveAllListeners();
            if (AlphaSlider != null) AlphaSlider.onValueChanged.RemoveAllListeners();
        }
    }
}