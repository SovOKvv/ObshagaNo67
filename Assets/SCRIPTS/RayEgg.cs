using UnityEngine;

public class RayEgg : MonoBehaviour
{
    public float maxDistance = 5f;
    
    private Outline currentOutline;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if(hitObject.layer == LayerMask.NameToLayer("egg"))
            {
                Outline outline = hitObject.GetComponent<Outline>();

                if (outline!= null)
                {
                    if(currentOutline != outline)
                    {
                        if(currentOutline != null)
                        {
                            currentOutline.enabled = false;
                        }

                        currentOutline = outline;
                        
                        currentOutline.enabled = true;
                    }
                }
            }
            else if(currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
        else if(currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}

