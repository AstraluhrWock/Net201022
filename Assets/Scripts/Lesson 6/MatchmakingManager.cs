using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class MatchmakingManager : MonoBehaviour, IConnectionCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _roomListText;
    [SerializeField] private Button _joinButton;

    private TypedLobby _defaultLobby = new TypedLobby("DefaultLobby", LobbyType.Default);
    private Dictionary<string, RoomInfo> _cacheRoomList = new Dictionary<string, RoomInfo>();

    private LoadBalancingClient _loadBalancingClient;

    private void Start()
    {
        _loadBalancingClient = new LoadBalancingClient();
        _loadBalancingClient.AddCallbackTarget(this);
        _loadBalancingClient.ConnectUsingSettings(_serverSettings.AppSettings);
        _joinButton.onClick.AddListener(() => OnJoinRoomButton());
    }

    private void Update()
    {
        if (_loadBalancingClient == null)
            return;

        _loadBalancingClient.Service();    
    }

    private void OnDestroy()
    {
        _loadBalancingClient.RemoveCallbackTarget(this);
        _joinButton.onClick.RemoveAllListeners();
    }

    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        Debug.Log("On Connected to master");
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true
        };

        EnterRoomParams enterRoomParams = new EnterRoomParams
        {
            RoomName = "First Room",
            RoomOptions = roomOptions,
            ExpectedUsers = new[] { "UserID" },
            Lobby = _defaultLobby
        };

       _loadBalancingClient.OpCreateRoom(enterRoomParams);
    } 

    public void OnCustomAuthenticationFailed(string debugMessage)
    {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnDisconnected(DisconnectCause cause)
    {
        _cacheRoomList.Clear();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {

    }

    public void OnCreatedRoom()
    {
        Debug.Log("On created room");
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    private void OnJoinRoomButton()
    {
        Debug.Log("On JOINDED room");
        _loadBalancingClient.CurrentRoom.IsOpen = false;
    }

  

    public void OnJoinRoomFailed(short returnCode, string message)
    {

    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("On join random failed");
        _loadBalancingClient.OpCreateRoom(new EnterRoomParams());
    }

    public void OnLeftRoom()
    {

    }

    public void OnJoinedLobby()
    {
        Debug.Log("On join NEW LOBBY");
        EnterRoomParams enterRoom = new EnterRoomParams();
        _loadBalancingClient.OpJoinRoom(enterRoom);
        _cacheRoomList.Clear();
    }

    public void OnLeftLobby()
    {
        _cacheRoomList.Clear();
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("On Room List Update");
        UpdateCachedRoomList(roomList);
        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount < room.MaxPlayers && room.IsOpen)
            {
                _cacheRoomList.Add(room.Name, room);
            }
        }   
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {

    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        Debug.Log("Update cached room list");
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i]; 
            if (info.RemovedFromList)
            {
                _cacheRoomList.Remove(info.Name);
            }
            else
            {
                _cacheRoomList[info.Name] = info;
            }
        }
        _roomListText.text = string.Join("\n", _cacheRoomList.Keys);
    } 
}