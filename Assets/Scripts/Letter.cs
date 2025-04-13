using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractiveHintAndImageWithChildren : MonoBehaviour
{
    [Header("Player Settings")]
    public string playerTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Hint (TextMeshPro)")]
    public TMP_Text hintText;
    public string hintMessage = "Press E";
    public TMP_FontAsset customFont;

    [Header("Image Settings")]
    public Image displayImage;
    public bool hideOnExit = true;
    public bool includeChildren = true; // Показывать дочерние объекты

    private bool isInTrigger = false;
    private bool isImageVisible = false;

    void Start()
    {
        ApplyCustomFont();
        SetImageVisibility(false); // Скрываем изображение при старте
    }

    void Update()
    {
        UpdateHintVisibility();
        
        if (isInTrigger && Input.GetKeyDown(interactKey))
        {
            ToggleImageVisibility();
        }
    }

    private void ApplyCustomFont()
    {
        if (customFont != null && hintText != null)
        {
            hintText.font = customFont;
        }
    }

    private void UpdateHintVisibility()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(isInTrigger && !isImageVisible);
            hintText.text = hintMessage;
        }
    }

    private void ToggleImageVisibility()
    {
        isImageVisible = !isImageVisible;
        SetImageVisibility(isImageVisible);
    }

    private void SetImageVisibility(bool visible)
    {
        if (displayImage != null)
        {
            displayImage.gameObject.SetActive(visible);
            
            // Показываем/скрываем дочерние объекты
            if (includeChildren)
            {
                foreach (Transform child in displayImage.transform)
                {
                    child.gameObject.SetActive(visible);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isInTrigger = false;
            isImageVisible = false;
            
            if (hideOnExit)
            {
                SetImageVisibility(false);
            }
        }
    }
}