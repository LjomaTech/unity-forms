using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace LjomaAssets.FormManagement.Inputs
{
    /// <summary>
    /// Color picker using a color wheel image
    /// Returns value as hex color string (e.g., "#FF5500")
    /// </summary>
    public class ColorPicker : FormInput, IPointerDownHandler, IDragHandler
    {
        [Header("Color Picker Components")]
        [Tooltip("The color wheel image to pick colors from")]
        public Image ColorWheelImage;
        
        [Tooltip("Image to display the selected color (optional)")]
        public Image PreviewImage;
        
        [Tooltip("Cursor image that follows the picked position (optional)")]
        public RectTransform Cursor;

        [Header("Settings")]
        public Color DefaultColor = Color.white;

        [Header("Events")]
        public UnityEvent<Color> OnColorChanged;

        private Color _selectedColor;

        private void Awake()
        {
            SetupFormInput();
        }

        public override string Value()
        {
            return ColorUtility.ToHtmlStringRGBA(_selectedColor);
        }

        /// <summary>
        /// Gets the currently selected color
        /// </summary>
        public Color SelectedColor => _selectedColor;

        public override void SetValue(string value)
        {
            if (ColorUtility.TryParseHtmlString(value.StartsWith("#") ? value : "#" + value, out Color color))
            {
                SetColor(color);
            }
        }

        /// <summary>
        /// Sets the selected color directly
        /// </summary>
        public void SetColor(Color color)
        {
            _selectedColor = color;
            UpdatePreview();
        }

        public override void SetIsReadOnly(bool isReadOnly)
        {
            base.SetIsReadOnly(isReadOnly);
            // When readonly, pointer events will still fire but we'll ignore them
        }

        public override bool Validate()
        {
            // Color picker always has a valid color
            HideError();
            return true;
        }

        protected override void SetupFormInput()
        {
            if (ColorWheelImage == null)
            {
                ColorWheelImage = GetComponent<Image>();
            }

            _selectedColor = DefaultColor;
            UpdatePreview();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsReadOnly) return;
            PickColorFromPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsReadOnly) return;
            PickColorFromPosition(eventData);
        }

        private void PickColorFromPosition(PointerEventData eventData)
        {
            if (ColorWheelImage == null || ColorWheelImage.sprite == null || 
                ColorWheelImage.sprite.texture == null) return;

            RectTransform imageRect = ColorWheelImage.rectTransform;

            // Convert the screen position to local position within the image rect
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imageRect, eventData.position, eventData.pressEventCamera, out Vector2 localPos))
            {
                return;
            }

            // Normalize the local position to get UV coordinates
            Vector2 uv = new Vector2(
                Mathf.InverseLerp(imageRect.rect.x, imageRect.rect.xMax, localPos.x),
                Mathf.InverseLerp(imageRect.rect.y, imageRect.rect.yMax, localPos.y)
            );

            // Clamp UV to valid range
            uv.x = Mathf.Clamp01(uv.x);
            uv.y = Mathf.Clamp01(uv.y);

            // Get the color from the texture at the UV coordinates
            Texture2D texture = ColorWheelImage.sprite.texture;
            if (texture.isReadable)
            {
                _selectedColor = texture.GetPixelBilinear(uv.x, uv.y);
                UpdatePreview();
                UpdateCursor(localPos, imageRect);
                OnColorChanged?.Invoke(_selectedColor);
            }
            else
            {
                Debug.LogWarning("ColorPicker: Texture is not readable. Enable Read/Write in import settings.");
            }
        }

        private void UpdatePreview()
        {
            if (PreviewImage != null)
            {
                PreviewImage.color = _selectedColor;
            }
        }

        private void UpdateCursor(Vector2 localPos, RectTransform imageRect)
        {
            if (Cursor != null)
            {
                // Clamp cursor position to image bounds
                Vector2 clampedPos = new Vector2(
                    Mathf.Clamp(localPos.x, imageRect.rect.x, imageRect.rect.xMax),
                    Mathf.Clamp(localPos.y, imageRect.rect.y, imageRect.rect.yMax)
                );
                Cursor.anchoredPosition = clampedPos;
            }
        }
    }
}