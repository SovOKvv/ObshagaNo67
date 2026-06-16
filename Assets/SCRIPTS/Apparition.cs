using UnityEngine;

public class Apparition : MonoBehaviour
{
    [Header("Настройки")]
    public float disappearDistance = 3f;
    public bool isActive = false;
    public bool hasDisappeared = false;

    private GameObject player;
    private Renderer objectRenderer;
    private Collider objectCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        
        SetVisible(false);
    }

    void Update()
    {
        if (!isActive || hasDisappeared || player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= disappearDistance)
        {
            Disappear();
        }
    }

    void Disappear()
    {
        hasDisappeared = true;
        isActive = false;
        SetVisible(false);
        Debug.Log("[Apparition] Исчез!");
    }

    public void Activate()
    {
        if (!hasDisappeared)
        {
            isActive = true;
            SetVisible(true);
            Debug.Log("[Apparition] Появился!");
        }
    }

    public void SetVisible(bool visible)
    {
        if (objectRenderer != null)
            objectRenderer.enabled = visible;
        if (objectCollider != null)
            objectCollider.enabled = visible;
    }
}