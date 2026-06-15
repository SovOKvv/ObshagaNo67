using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ОБЯЗАТЕЛЬНО добавляем для работы с текстом TextMeshPro

public class EscapeDoor : MonoBehaviour
{
    [Header("Настройки побега")]
    public int totalNotesNeeded = 5; 
    public string victorySceneName = "VictoryScene"; 

    [Header("Интерфейс")]
    public TextMeshProUGUI counterText; // Сюда мы перетащим наш текст счетчика

    [HideInInspector]
    public int collectedNotes = 0; 

    void Start()
    {
        // Обновляем текст в самом начале игры, чтобы там сразу было "0 / 5"
        UpdateCounterUI();
    }

    // Метод, который увеличивает счетчик (его вызывает студентческий билет)
    public void AddNote()
    {
        collectedNotes++;
        UpdateCounterUI(); // Обновляем текст на экране
    }

    // Метод, который просто меняет буквы на экране
    void UpdateCounterUI()
    {
        if (counterText != null)
        {
            counterText.text = $"Частей студенческого: {collectedNotes} / {totalNotesNeeded}";
        }
    }

    public void TryEscape()
    {
        if (collectedNotes >= totalNotesNeeded)
        {
            Debug.Log("Победа! Загружаем экран победы.");
            SceneManager.LoadScene(victorySceneName); 
        }
        else
        {
            Debug.Log($"Еще не время. Собрано: {collectedNotes} из {totalNotesNeeded}");
        }
    }
}