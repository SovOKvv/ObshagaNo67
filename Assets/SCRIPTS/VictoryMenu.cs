using UnityEngine;
using UnityEngine.SceneManagement; // Для работы со сценами

public class VictoryMenu : MonoBehaviour
{
    void Start()
    {
        // Когда игрок победил, нам СТОПРОЦЕНТНО нужен курсор, чтобы кликнуть по кнопке
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Метод для кнопки
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Напиши сюда точное название сцены своего главного меню
    }
}