# LjomaTech Forms

[![Unity 2020.3+](https://img.shields.io/badge/Unity-2020.3%2B-black.svg?logo=unity)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.md)

A comprehensive Unity UI forms management system with validation, multiple input types, and form submission handling.

## Installation

### Unity Package Manager (Recommended)

Add to your `manifest.json`:

```json
{
  "dependencies": {
    "com.ljomatech.forms": "https://github.com/LjomaTech/unity-forms.git"
  }
}
```

Or via Package Manager UI: **Window → Package Manager → + → Add package from git URL**, enter:

`https://github.com/LjomaTech/unity-forms.git`

## Features

- **Form Manager** - Centralized form handling with submit/reset
- **Text Input** - Text fields with regex validation
- **Toggle Input** - Checkboxes with required validation
- **Dropdown** - TMP_Dropdown integration
- **Slider** - Single value slider with display
- **Double Slider** - Range selection (min/max)
- **Radio Buttons** - Single-select option groups
- **Phone Input** - International phone with country codes
- **DateTime Picker** - Date/time selection via dropdowns
- **Color Picker** - Color wheel and RGB slider inputs

## Quick Start

1. Add `FormManager` component to your form parent
2. Add input components (e.g., `FormTextInput`) to child objects
3. Add `FormButton` with type `Submit` or `Reset`
4. Subscribe to `OnSubmitJson` or `OnSubmitDictionary` events

```csharp
formManager.OnSubmitJson.AddListener(json => {
    Debug.Log("Form data: " + json);
});
```

## Input Types

| Component | Value Format | Validation |
|-----------|--------------|------------|
| `FormTextInput` | String | Required, regex, min/max length |
| `FormToggleInput` | "true"/"false" | Required checked |
| `FormDropdown` | Selected text | Required selection |
| `FormSliderInput` | Float string | Always valid |
| `DoubleSlider` | "min,max" | Min ≤ Max |
| `RadioboxFormInput` | Selected value | Required selection |
| `PhoneNumberInput` | "+1234567890" | Digit count |
| `DateTimeInput` | ISO 8601 | Valid date |
| `ColorPicker` | "#RRGGBB" | Always valid |
| `ColorPickerInput` | "#RRGGBB(AA)" | Always valid |

## Dependencies

- TextMeshPro 3.0+
- Newtonsoft.Json (for JSON serialization)

## License

[MIT License](LICENSE.md) © 2025 LjomaTech
