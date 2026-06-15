using UnityEngine;

public class StudentCard : MonoBehaviour
{
    private EscapeDoor escapeDoor;

    void Start()
    {
        // Автоматически находим скрипт финальной двери на сцене
        escapeDoor = FindObjectOfType<EscapeDoor>();
    }

    // Этот метод теперь вызывается напрямую из скрипта камеры
    public void Collect()
{
    if (escapeDoor != null)
    {
        escapeDoor.AddNote(); // Вызываем новый метод с обновлением UI
        Debug.Log("[Билет] Успешно собран!");
        Destroy(gameObject); 
    }
    else
    {
        Debug.LogError("[Билет] Скрипт EscapeDoor не найден на сцене!");
    }
}
}