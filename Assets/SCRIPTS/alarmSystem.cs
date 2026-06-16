using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    [Header("--- Настройки Таймера ---")]
    [Tooltip("Через сколько секунд активируется сирена")]
    public float intervalInSeconds = 60f;
    [Tooltip("Сколько секунд длится тревога")]
    public float alarmDuration = 10f;

    [Header("--- Настройки Звука ---")]
    public AudioSource alarmAudioSource;
    [Range(0f, 1f)]
    public float alarmVolume = 1f;

    [Header("--- Настройки Света ---")]
    [Tooltip("Цвет, в который окрасятся все лампы")]
    public Color alarmColor = Color.red;
    [Tooltip("Скорость мигания света (чем выше, тем быстрее мигает)")]
    public float blinkSpeed = 4f;

    private List<Light> allLights = new List<Light>();
    private List<Color> originalColors = new List<Color>();
    private bool isAlarmActive = false;
    private float timer = 0f;

    void Start()
    {
        // Если AudioSource забыли прикрепить, пробуем найти его на этом же объекте
        if (alarmAudioSource == null)
            alarmAudioSource = GetComponent<AudioSource>();

        if (alarmAudioSource != null)
            alarmAudioSource.volume = alarmVolume;
    }

    void Update()
    {
        // Если тревога уже идет, таймер новой тревоги не тикает
        if (isAlarmActive) return;

        timer += Time.deltaTime;

        if (timer >= intervalInSeconds)
        {
            StartCoroutine(ActivateAlarm());
            timer = 0f; // сброс таймера
        }
    }

    IEnumerator ActivateAlarm()
    {
        isAlarmActive = true;

        // 1. Находим ВСЕ лампы на сцене перед тревогой
        Light[] sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        allLights.Clear();
        originalColors.Clear();

        foreach (Light lt in sceneLights)
        {
            if (lt.gameObject.activeInHierarchy && lt.enabled)
            {
                allLights.Add(lt);
                originalColors.Add(lt.color);
            }
        }

        // 2. Включаем звук сирены и принудительно ЗАЦИКЛИВАЕМ его
        if (alarmAudioSource != null && alarmAudioSource.clip != null)
        {
            alarmAudioSource.volume = alarmVolume;
            alarmAudioSource.loop = true; // Заставляем Unity крутить трек бесконечно по кругу
            alarmAudioSource.Play();
        }

        // 3. Цикл самой тревоги (звук и свет работают ровно столько секунд, сколько указано в alarmDuration)
        float elapsed = 0f;
        while (elapsed < alarmDuration)
        {
            elapsed += Time.deltaTime;

            // Высчитываем коэффициент мигания (плавно туда-обратно от 0 до 1)
            float wave = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            // Меняем цвет у всех сохраненных ламп
            for (int i = 0; i < allLights.Count; i++)
            {
                if (allLights[i] != null)
                {
                    allLights[i].color = Color.Lerp(originalColors[i], alarmColor, wave);
                }
            }

            yield return null; // ждем следующий кадр
        }

        // 4. Время тревоги вышло — ВЫКЛЮЧАЕМ звук (он оборвется ровно со светом)
        if (alarmAudioSource != null)
        {
            alarmAudioSource.Stop();
            alarmAudioSource.loop = false; // сбрасываем зацикливание на всякий случай
        }

        // Возвращаем лампам родные цвета
        for (int i = 0; i < allLights.Count; i++)
        {
            if (allLights[i] != null)
            {
                allLights[i].color = originalColors[i];
            }
        }

        isAlarmActive = false;
    }
    public void ForceActivateAlarm()
    {
        if (!isAlarmActive)
        {
            StartCoroutine(ActivateAlarm());
        }
    }
}