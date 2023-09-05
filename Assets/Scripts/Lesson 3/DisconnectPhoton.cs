using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

internal class DisconnectPhoton : MonoBehaviour
{
    [SerializeField] private Button _disconnectButton;

    private void Start()
    {
        _disconnectButton.onClick.AddListener(()=> Disconnect());
    }

    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Disconnected from Photon");
        }
        else 
        {
            Debug.LogWarning("Already disconnected from Photon");
        }
    }
}
