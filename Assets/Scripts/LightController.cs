using UnityEngine;
using Mirror;
public class LightController : NetworkBehaviour
{
    [SerializeField]
    private Light systemLight;
    public string _task;
    [SerializeField]
    private int timer = 20;
    [SerializeField]
    private float time = 0f;
    void Start()
    {
        if (!systemLight) {
            Debug.LogError("Script need a light in order to work !");
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!isServer)
            return;
        if (systemLight.enabled)
            time += Time.deltaTime;
        
        if (time < timer) {
            ChangeLightMode(true);
        } else {
            ChangeLightMode(false);
        }
    }

    public bool IsActive()
    {
        return systemLight.enabled;
    }

    public void ChangeLightMode(bool mode)
    {
        if (isServer)
            RpcChangeLightMode(mode);
        else
            CmdChangeLightMode(mode);
    }

    [Command]
    private void CmdChangeLightMode(bool mode)
    {
        systemLight.enabled = mode;
        RpcChangeLightMode(mode);
    }

    [ClientRpc]
    private void RpcChangeLightMode(bool mode)
    {
        if (isLocalPlayer)
            return;
        time = (mode && !systemLight.enabled) ? 0f : time;
        systemLight.enabled = mode;
    }
}
