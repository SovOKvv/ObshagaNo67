using UnityEngine;
using System.Collections;

public class miganye : MonoBehaviour
{
    private Light pointLight;
    private float originalIntensity;
    
    public float minFlickerDuration = 2f;
    public float maxFlickerDuration = 3f;
    public float flickerSpeed = 0.05f; // скорость смены интенсивности
    
    void Start()
    {
        pointLight = GetComponent<Light>();
        originalIntensity = pointLight.intensity;
        StartCoroutine(FlickerRoutine());
    }
    
    IEnumerator FlickerRoutine()
    {
        while (true) // бесконечный цикл
        {
            // Ждём перед следующим миганием (опционально)
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            
            // Выбираем длительность мигания
            float flickerDuration = Random.Range(minFlickerDuration, maxFlickerDuration);
            float endTime = Time.time + flickerDuration;
            
            // Мигаем
            while (Time.time < endTime)
            {
                // Рандомная интенсивность от 0 до оригинальной
                pointLight.intensity = Random.Range(0f, originalIntensity);
                yield return new WaitForSeconds(flickerSpeed);
            }
            
            // Возвращаем исходную интенсивность
            pointLight.intensity = originalIntensity;
        }
    }
}