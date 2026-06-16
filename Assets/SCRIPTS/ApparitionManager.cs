using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparitionManager : MonoBehaviour
{
    [Header("Настройки")]
    public Apparition[] apparitionPrefabs;
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 20f;
    public int maxActiveApparitions = 2;
    public float spawnRadius = 30f;
    
    [Header("Настройки карты")]
    public LayerMask groundLayer;    // ← Слой земли (пола)
    public float heightOffset = 0.5f; // ← Поднять над землёй

    [Header("Ссылка на EscapeDoor")]
    public EscapeDoor escapeDoor;

    private bool isSystemActive = false;
    private List<Apparition> activeApparitions = new List<Apparition>();
    private Coroutine spawnRoutine;

    void Start()
    {
        if (escapeDoor == null)
            escapeDoor = FindObjectOfType<EscapeDoor>();

        // Если слой не назначен — ищем по тегу "Ground"
        if (groundLayer == 0)
        {
            groundLayer = LayerMask.GetMask("Default");
        }
    }

    void Update()
    {
        if (escapeDoor == null) return;

        int collected = escapeDoor.collectedNotes;

        if (collected >= 2 && !isSystemActive)
        {
            ActivateSystem();
        }

        if (isSystemActive && collected < 2)
        {
            DeactivateSystem();
        }
    }

    void ActivateSystem()
    {
        isSystemActive = true;
        Debug.Log("[Manager] Система активирована!");

        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    void DeactivateSystem()
    {
        isSystemActive = false;
        Debug.Log("[Manager] Система деактивирована.");

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }

        foreach (Apparition app in activeApparitions)
        {
            if (app != null)
                Destroy(app.gameObject);
        }
        activeApparitions.Clear();
    }

    IEnumerator SpawnRoutine()
    {
        while (isSystemActive)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            CleanupApparitions();

            if (activeApparitions.Count >= maxActiveApparitions)
                continue;

            if (apparitionPrefabs == null || apparitionPrefabs.Length == 0)
                continue;

            // Находим позицию на карте
            Vector3 spawnPosition = GetRandomPositionOnGround();
            
            if (spawnPosition == Vector3.zero)
            {
                Debug.Log("[Manager] Не удалось найти точку на карте!");
                continue;
            }

            // Выбираем случайный префаб
            int prefabIndex = Random.Range(0, apparitionPrefabs.Length);
            Apparition prefab = apparitionPrefabs[prefabIndex];

            // Создаём силуэт
            Apparition newApparition = Instantiate(prefab, spawnPosition, Quaternion.identity);
            newApparition.Activate();
            activeApparitions.Add(newApparition);

            Debug.Log($"[Manager] Силуэт появился в {spawnPosition}");
        }
    }

    Vector3 GetRandomPositionOnGround()
    {
        // Случайная точка в радиусе
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPos = new Vector3(randomCircle.x, 100f, randomCircle.y);

        // Бросаем луч вниз, чтобы найти землю
        RaycastHit hit;
        if (Physics.Raycast(randomPos, Vector3.down, out hit, 200f, groundLayer))
        {
            // Нашли землю — возвращаем позицию с небольшим смещением вверх
            Vector3 groundPos = hit.point;
            groundPos.y += heightOffset;
            return groundPos;
        }

        // Если не нашли — пробуем ещё раз (рекурсия)
        return GetRandomPositionOnGround();
    }

    void CleanupApparitions()
    {
        for (int i = activeApparitions.Count - 1; i >= 0; i--)
        {
            if (activeApparitions[i] == null || activeApparitions[i].hasDisappeared)
            {
                if (activeApparitions[i] != null)
                    Destroy(activeApparitions[i].gameObject);
                activeApparitions.RemoveAt(i);
            }
        }
    }

    // Визуализация зоны появления в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}