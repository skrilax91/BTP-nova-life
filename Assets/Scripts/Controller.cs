using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerEngine))]
public class Controller : NetworkBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private Camera cam;
    private bool freeze = false;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private float mouseXSensitivity = 3f;
    [SerializeField]
    private float mouseYSensitivity = 3f;

    private PlayerEngine pEngine;

    private void Start()
    {
        pEngine = GetComponent<PlayerEngine>();
        if (!cam) {
            Debug.LogError("System need a camera in order to work !");
            this.enabled = false;
            return;
        }
    }
    private void Update()
    {
        RaycastHit hit;
        Vector3 horizontal = transform.right * Input.GetAxisRaw("Horizontal");
        Vector3 verticale = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 velocity = (horizontal + verticale).normalized * speed;
        Vector3 rotation = new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * mouseXSensitivity;
        float camRotation = Input.GetAxisRaw("Mouse Y") * mouseYSensitivity;

        // Set Player velocity and rotation in Engine
        if (!freeze) {
            pEngine.SetVelocity(velocity);
            pEngine.SetRotation(rotation, camRotation);
        } else {
            pEngine.SetVelocity(Vector3.zero);
            pEngine.SetRotation(Vector3.zero, 0);
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5, mask) && Input.GetKeyDown(KeyCode.E)) {
            GameObject lamp = hit.collider.gameObject;
            Light light = lamp.transform.Find("Area Light").GetComponent<Light>();
            GameObject task = gameObject.transform.Find("Canvas/" + lamp.gameObject.GetComponent<LightController>()._task).gameObject;
            
            if (!task || light.enabled || task.activeInHierarchy)
                return;

            //Prepare user for interface
            freeze = true;
            Cursor.visible = true;
            task.SetActive(true);

            // Wait for finish
            StartCoroutine(waitComplete(lamp, task));
        }

    }
    private IEnumerator waitComplete(GameObject lamp, GameObject task)
    {
        while (task.activeInHierarchy) {
            yield return new WaitForSeconds(0.5f);
        }
        ChangeLightMode(lamp.GetComponent<LightController>(), true);
        freeze = false;
        Cursor.visible = false;
    }

    [Command]
    private void ChangeLightMode(LightController light, bool mode)
    {
        light.ChangeLightMode(mode);
    }
}
