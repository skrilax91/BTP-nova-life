using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] toDisable;
    Camera mainCam;

    private void Start()
    {
        Cursor.visible = false;
        // Disable other clients scrypt to avoid networking problems
        if (isLocalPlayer) {
            mainCam = Camera.main;
            if (mainCam)
                mainCam.gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < toDisable.Length; i++)
            toDisable[i].enabled = false;
    }

    private void OnDisable()
    {
        if (mainCam)
            mainCam.gameObject.SetActive(true);
        Cursor.visible = true;
    }
}
