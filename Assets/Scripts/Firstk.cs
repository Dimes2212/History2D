using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // 1. Добавляем пространство имен TMPro

public class Firstk : MonoBehaviour
{
    public TMP_Text uiText; // 2. Меняем тип с Text на TMP_Text
    public float typingSpeed = 0.6f;
    public GameObject nextButton;

    private string fullText;
    private bool isTyping;
    private bool isCoroutineStopped = false;

    private void Start()
    {
        fullText = uiText.text;
        uiText.text = string.Empty;
        isTyping = true;
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        for(int i = 0; i < fullText.Length && !isCoroutineStopped; i++)
        {
            uiText.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(isTyping)
            {
                isCoroutineStopped = true;
                StopCoroutine(TypeText());
                uiText.text = fullText;
                isTyping = false;
                nextButton.SetActive(true);
            }
            

            else if(nextButton.activeInHierarchy)
            {
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
