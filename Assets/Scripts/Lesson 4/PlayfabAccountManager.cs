using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

internal class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private TMP_Text _usernameLabel;
    [SerializeField] private TMP_Text _createdDataLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"PlayFab ID: {result.AccountInfo.PlayFabId}";
        _usernameLabel.text = $"PlayFab Username: {result.AccountInfo.Username}";
        _createdDataLabel.text = $"PlayFab DataOfCreated: {result.AccountInfo.Created}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
