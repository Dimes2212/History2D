using UnityEngine;
using TMPro;

public class ShowTextWhileInTrigger : MonoBehaviour
{
    [Header("Настройки")]
    public string playerTag = "Player"; // Тег объекта игрока
    public bool hideOnExit = true; // Скрывать текст при выходе из триггера

    [Header("Текст (TextMeshPro)")]
    public TMP_Text displayText; // Компонент TextMeshPro
    public string message = "Текст подсказки"; // Отображаемый текст

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Показываем текст при входе
            displayText.text = message;
            displayText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Можно добавить дополнительные проверки/действия
            // пока игрок находится в триггере
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && hideOnExit)
        {
            // Скрываем текст при выходе
            displayText.gameObject.SetActive(false);
        }
    }
}