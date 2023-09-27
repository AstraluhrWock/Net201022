using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ConnectToPhoton : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField _nameInput;
    [SerializeField] Button _connectButton;

    private byte maxPlayerInRoom = 3;
    private bool isConnecting;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _connectButton.onClick.AddListener(Connect);
    }

 
    public void Connect()
    {
        isConnecting = true;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else 
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("OnConnectedMaster join random room");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No join random room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayerInRoom });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Load game scene");
            PhotonNetwork.LoadLevel("Lesson_7_2");
        }
    }
}
