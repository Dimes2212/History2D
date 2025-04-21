using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveHintAndImageWithChildren2 : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Hint (TextMeshPro)")]
    public TMP_Text hintText;
    public string hintMessage = "Press E";
    public TMP_FontAsset customFont;

    [Header("Image Settings")]
    public Image[] displayImages; // Массив изображений для последовательного показа
    public bool hideOnExit = true;
    public bool includeChildren = true;

    private bool isInTrigger = false;
    private int currentImageIndex = -1; // -1 означает, что ни одно изображение не показано
    private bool hintDisabled = false; // Флаг, что подсказка отключена навсегда

    void Start()
    {
        // Принудительно скрываем подсказку и все изображения при старте
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }

        // Скрываем все изображения и их дочерние объекты
        foreach (var image in displayImages)
        {
            if (image != null)
            {
                image.gameObject.SetActive(false);
                if (includeChildren)
                {
                    foreach (Transform child in image.transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }

        // Применяем кастомный шрифт, если он задан
        ApplyCustomFont();
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(interactKey))
        {
            if (!hintDisabled && hintText != null)
            {
                // Удаляем подсказку при первом нажатии
                Destroy(hintText.gameObject);
                hintDisabled = true;
            }

            CycleThroughImages();
        }
    }

    private void ApplyCustomFont()
    {
        if (customFont != null && hintText != null)
        {
            hintText.font = customFont;
        }
    }

    private void CycleThroughImages()
    {
        // Скрываем текущее изображение
        if (currentImageIndex >= 0 && currentImageIndex < displayImages.Length)
        {
            SetImageVisibility(displayImages[currentImageIndex], false);
        }

        // Переходим к следующему изображению или завершаем показ
        currentImageIndex++;

        if (currentImageIndex < displayImages.Length)
        {
            // Показываем следующее изображение
            SetImageVisibility(displayImages[currentImageIndex], true);
        }
        else
        {
            // Все изображения показаны
            currentImageIndex = -1;
        }
    }

    private void SetImageVisibility(Image image, bool visible)
    {
        if (image != null)
        {
            image.gameObject.SetActive(visible);

            if (includeChildren)
            {
                foreach (Transform child in image.transform)
                {
                    child.gameObject.SetActive(visible);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !hintDisabled)
        {
            isInTrigger = true;
            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isInTrigger = false;

            // Скрываем все при выходе из триггера
            if (hideOnExit)
            {
                foreach (var image in displayImages)
                {
                    SetImageVisibility(image, false);
                }
                currentImageIndex = -1;
            }
        }
    }
}