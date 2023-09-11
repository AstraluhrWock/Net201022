using Photon.Pun;
using UnityEngine.UI;

internal class CloseRoomManager : MonoBehaviourPunCallbacks
{
    private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(() => CloseRoom());
    }

    private void CloseRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }
}