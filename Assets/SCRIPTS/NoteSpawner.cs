using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab; // Префаб записки
    public int notesToSpawn = 5;  // Сколько штук нужно заспавнить из 100

    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        // Автоматически находим все дочерние объекты (точки спавна)
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        Debug.Log($"[Spawner] Найдено точек для спавна: {spawnPoints.Count}");

        // Запускаем спавн
        SpawnNotes();
    }

    void SpawnNotes()
    {
        if (spawnPoints.Count < notesToSpawn)
        {
            Debug.LogError("Точек спавна на объекте меньше, чем нужно записок!");
            return;
        }

        // Делаем копию списка, чтобы безопасно удалять использованные точки
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < notesToSpawn; i++)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform chosenPoint = availablePoints[randomIndex];

            // 1. Берем поворот из настроек самого префаба и добавляем случайный наклон по X и Z, чтобы билет падал плашмя
            Quaternion randomRotation = notePrefab.transform.rotation * Quaternion.Euler(Random.Range(-15f, 15f), Random.Range(0f, 360f), Random.Range(-15f, 15f));

            // 2. Спавним билет в точке chosenPoint.position с нашим случайным поворотом
            Instantiate(notePrefab, chosenPoint.position, randomRotation);

            // Удаляем точку, чтобы на ней не заспавнилась вторая записка
            availablePoints.RemoveAt(randomIndex);
        }
    }
}