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
    public Image[] displayImages; // ������ ����������� ��� ����������������� ������
    public bool hideOnExit = true;
    public bool includeChildren = true;

    private bool isInTrigger = false;
    private int currentImageIndex = -1; // -1 ��������, ��� �� ���� ����������� �� ��������

    void Start()
    {
        // ������������� �������� ��������� � ��� ����������� ��� ������
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }

        // �������� ��� ����������� � �� �������� �������
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

        // ��������� ��������� �����, ���� �� �����
        ApplyCustomFont();
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(interactKey))
        {
            CycleThroughImages();

            // �������� ��������� ��� ������ �������
            if (hintText != null && hintText.gameObject.activeSelf)
            {
                hintText.gameObject.SetActive(false);
            }
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
        // �������� ������� �����������
        if (currentImageIndex >= 0 && currentImageIndex < displayImages.Length)
        {
            SetImageVisibility(displayImages[currentImageIndex], false);
        }

        // ��������� � ���������� ����������� ��� ��������� �����
        currentImageIndex++;

        if (currentImageIndex < displayImages.Length)
        {
            // ���������� ��������� �����������
            SetImageVisibility(displayImages[currentImageIndex], true);
        }
        else
        {
            // ��� ����������� ��������
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
        if (other.CompareTag(playerTag))
        {
            isInTrigger = true;
            if (hintText != null && currentImageIndex == -1)
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

            // �������� ��������� ��� ������
            if (hintText != null)
            {
                hintText.gameObject.SetActive(false);
            }

            // �������� ��� ����������� ��� ������ �� ��������
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