using UnityEngine;
using UnityEngine.UI;

namespace LjomaAssets.FormManagement.Toggle
{
    public class FormToggleInput : FormInput
    {
        private UnityEngine.UI.Toggle _toggle;

        [Header("Toggle Settings")]
        [Tooltip("If true, the toggle must be checked to pass validation")]
        public bool MustBeChecked;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            return _toggle.isOn.ToString().ToLower();
        }

        public override void SetValue(string value)
        {
            if (bool.TryParse(value, out bool result))
            {
                _toggle.isOn = result;
            }
            else
            {
                _toggle.isOn = value == "1" || value.ToLower() == "true";
            }
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            _toggle.interactable = !isReadOnly;
        }

        public override bool Validate()
        {
            if (isRequired && MustBeChecked && !_toggle.isOn)
            {
                ShowError("This field must be checked.");
                return false;
            }
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            _toggle = GetComponent<UnityEngine.UI.Toggle>();
            if (_toggle == null)
            {
                _toggle = GetComponentInChildren<UnityEngine.UI.Toggle>();
            }
            
            if (_toggle != null)
            {
                _toggle.interactable = !IsReadOnly;
            }
        }
    }
}