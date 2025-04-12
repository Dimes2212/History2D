using UnityEngine;
using TMPro;

public class ShowTextWhileInTrigger : MonoBehaviour
{
    [Header("���������")]
    public string playerTag = "Player"; // ��� ������� ������
    public bool hideOnExit = true; // �������� ����� ��� ������ �� ��������

    [Header("����� (TextMeshPro)")]
    public TMP_Text displayText; // ��������� TextMeshPro
    public string message = "����� ���������"; // ������������ �����

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // ���������� ����� ��� �����
            displayText.text = message;
            displayText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // ����� �������� �������������� ��������/��������
            // ���� ����� ��������� � ��������
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && hideOnExit)
        {
            // �������� ����� ��� ������
            displayText.gameObject.SetActive(false);
        }
    }
}