using UnityEngine;

public class PhysicsDoorOpener : MonoBehaviour
{
    [Header("Настройки луча")]
    public float maxDistance = 5f; 
    public LayerMask doorLayer;   

    [Header("Настройки силы двери")]
    public float pullSpeed = 40f;  

    private Rigidbody grabbedRigidbody;
    private Vector3 grabPoint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabOrInteract(); // Изменили название метода для точности
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }
    }

    void FixedUpdate()
    {
        if (grabbedRigidbody != null)
        {
            HoldObject();
        }
    }

    void TryGrabOrInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.white, 2f);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log($"[Raycast] Попали в объект: {hit.collider.name}");

            // --- ПРОВЕРКА 1: Это студенческий билет? ---
            StudentCard card = hit.collider.GetComponent<StudentCard>();
            if (card != null)
            {
                Debug.Log("<color=yellow>[Взаимодействие] Нашли студенческий билет! Собираем...</color>");
                card.Collect(); 
                return; // Выходим из метода, чтобы код двери ниже не выполнялся
            }

            // --- ПРОВЕРКА 2: Это финальная дверь побега? ---
            // --- ПРОВЕРКА 2: Это финальная дверь побега? (Ищет и на самом объекте, и на его "родителе") ---
            EscapeDoor escapeDoor = hit.collider.GetComponentInParent<EscapeDoor>();
            if (escapeDoor != null)
            {
                Debug.Log("<color=cyan>[Взаимодействие] Кликнули по финальной двери побега!</color>");
                escapeDoor.TryEscape();
                return; // Выходим
            }

            // --- ПРОВЕРКА 3: Логика физической двери (твой оригинальный код) ---
            if (((1 << hit.collider.gameObject.layer) & doorLayer) != 0)
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                
                if (rb != null)
                {
                    if (!rb.isKinematic)
                    {
                        grabbedRigidbody = rb;
                        grabPoint = hit.point; 
                        Debug.Log($"<color=green>[Ура!] Дверь '{hit.collider.name}' успешно схвачена!</color>");
                    }
                    else
                    {
                        Debug.LogWarning("[Ошибка] У Rigidbody двери включена галочка 'Is Kinematic'.");
                    }
                }
                else
                {
                    Debug.LogError("[Ошибка] На объекте с дверью нет компонента Rigidbody!");
                }
            }
        }
    }

    void HoldObject()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 forceDirection = transform.right * mouseX + transform.forward * mouseY;

        grabbedRigidbody.AddForceAtPosition(forceDirection * pullSpeed * Time.deltaTime, grabPoint, ForceMode.VelocityChange);
        grabPoint = grabbedRigidbody.ClosestPointOnBounds(grabPoint);
    }

    void ReleaseObject()
    {
        if (grabbedRigidbody != null)
        {
            Debug.Log("[Рука] Отпустили дверь.");
        }
        grabbedRigidbody = null;
    }
}