using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using ExitGames.Client.Photon;

internal class ConnectAndJoinRandomLobby : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField] private ServerSettings _serverSettings;
    [SerializeField] private TMP_Text _stateUiText;

    private const string GAME_MODE_KEY = "gamemode";
    private const string AI_MODE_KEY = "AImode";

    private const string MAP_PROB_KEY = "C0";
    private const string GOLD_PROB_KEY = "C1";

    private TypedLobby _sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);

    private LoadBalancingClient _loadBalancingClient;

    private void Start()
    {
        _loadBalancingClient = new LoadBalancingClient();
        _loadBalancingClient.AddCallbackTarget(this);
        _loadBalancingClient.ConnectUsingSettings(_serverSettings.AppSettings);
    }

    private void Update()
    {
        if (_loadBalancingClient == null)
            return;

        _loadBalancingClient.Service();
        var state = _loadBalancingClient.State.ToString();
        _stateUiText.text = state;
    }

    private void OnDestroy()
    {
        _loadBalancingClient.RemoveCallbackTarget(this);
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
            PublishUserId = true,
            CustomRoomPropertiesForLobby = new[] { MAP_PROB_KEY, GOLD_PROB_KEY },
            CustomRoomProperties = new Hashtable { { GOLD_PROB_KEY, 400}, { MAP_PROB_KEY, "Map3"} }
        };

        EnterRoomParams enterRoomParams = new EnterRoomParams 
        { 
            RoomName = "First Room", 
            RoomOptions = roomOptions,
            ExpectedUsers = new[] {"UserID"},
            Lobby = _sqlLobby
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

    public void OnJoinedRoom()
    {
        Debug.Log("On joined room");
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
        string sqlLobbyFilter = $"{MAP_PROB_KEY} = Map3 AND {GOLD_PROB_KEY} BETWEEN 300 AND 500";
        var opJoinRandomRoomParams = new OpJoinRandomRoomParams
        {
            SqlLobbyFilter = sqlLobbyFilter
        };
        _loadBalancingClient.OpJoinRandomRoom(opJoinRandomRoomParams);
    }

    public void OnLeftLobby()
    {

    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {

    }
}
