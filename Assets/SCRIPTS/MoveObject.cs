using UnityEngine;

public class DragObject : MonoBehaviour
{
    public float grabRange = 5f; // range grab
    public float grabSpeed = 10f; // скорость перемещения
    public LayerMask grabLayer; // какие обьекты можно хватать

    private Camera playerCamera;
    private GameObject grabbedObject; // обьект который держим
    private Rigidbody grabbedRB; // физика обьекта который мы держим
    private float originalDrag; //сохраняем оригинальное сопротивление

    void Start()
    {
        playerCamera = GetComponent<Camera>();
    }
    void Update()
    {
        //захват объекта
        if (Input.GetMouseButtonDown(0)) // 0=левая кнопка
        {
            TryGrabObject();
        }
        //удержание
        if (Input.GetMouseButton(0) && grabbedObject != null)
        {
            MoveGrabbedObject();
        }
        //отпускание обьекта
        if (Input.GetMouseButtonUp(0) && grabbedObject != null)
        {
            ReleaseObject();
        }
    }
    void TryGrabObject(){
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height / 2, 0));
        RaycastHit hit;
        //проверяем попал ли луч в обьект
        if (Physics.Raycast(ray, out hit, grabRange, grabLayer))
        {
            grabbedObject = hit.collider.gameObject;
            grabbedRB = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRB != null)
            {
                originalDrag = grabbedRB.linearDamping;
                grabbedRB.linearDamping = 5f; //увеличиваем сопротивление для плавности
                grabbedRB.useGravity = false; //отключаем гравитацию
                grabbedRB.freezeRotation = true; //блокируем вращение
            }
        }
    }
    void MoveGrabbedObject()
    {
        //точка перед камерой на расстоянии grabrange/2
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * (grabRange/2);

        //двигаем обьект к цели

        Vector3 direction = targetPosition - grabbedObject.transform.position;
        grabbedRB.linearVelocity = direction * grabSpeed;
    }
    void ReleaseObject()
    {
        if (grabbedRB != null)
        {
            grabbedRB.linearDamping = originalDrag;
            grabbedRB.useGravity = true;
            grabbedRB.freezeRotation = false;
        }
        grabbedObject = null;
        grabbedRB = null;
    }
}
