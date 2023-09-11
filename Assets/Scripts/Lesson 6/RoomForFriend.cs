using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomForFriend : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private Button _createRoomButton;

    private void Start()
    {
        _createRoomButton.onClick.AddListener(()=>CreateAndCopyRoomName());
    }

    private void CreateAndCopyRoomName()
    {
        string roomName = _roomNameInputField.text;

        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 }, null);
            GUIUtility.systemCopyBuffer = roomName;

            Debug.Log($"Created room: {roomName}");
            Debug.Log($"Copied room name to clipboard: {roomName}");
        }
        else
        {
            Debug.LogError("Room name cannot be empty!");
        }
    }
}