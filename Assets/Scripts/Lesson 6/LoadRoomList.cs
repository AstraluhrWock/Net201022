using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadRoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomListText;
    [SerializeField] private Button _joinButton;

    private Dictionary<string, RoomInfo> _cachedRoomList = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        _joinButton.onClick.AddListener(()=>JoinSelectedRoom());
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _cachedRoomList.Clear();

        foreach (RoomInfo room in roomList)
        {
            if (room.IsOpen && room.PlayerCount < room.MaxPlayers)
            {
                _cachedRoomList.Add(room.Name, room);
            }
        }
        UpdateRoomListUI();
    }

    private void UpdateRoomListUI()
    {
        _roomListText.text = "";

        foreach (var room in _cachedRoomList.Values)
        {
            _roomListText.text += room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")\n";
        }
    }

    private void JoinSelectedRoom()
    {
        if (_cachedRoomList.Count > 0)
        {
            foreach (var room in _cachedRoomList.Values)
            {
                if (room.IsOpen && room.PlayerCount < room.MaxPlayers)
                {
                    PhotonNetwork.JoinRoom(room.Name);
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("No available rooms to join.");
        }
    }
}