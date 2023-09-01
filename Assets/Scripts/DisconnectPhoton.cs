using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

internal class DisconnectPhoton : MonoBehaviour
{
    [SerializeField] private Button _disconnectPhoton;

    private void Start()
    {
        _disconnectPhoton.onClick.AddListener(()=> Disconnect());
    }

    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Dissconnected from Photon");
        }
        else 
        {
            Debug.LogWarning("Already disconnected from Photon");
        }
    }
}
