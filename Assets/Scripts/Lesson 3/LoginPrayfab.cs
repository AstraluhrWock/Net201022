using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

internal class LoginPrayfab : MonoBehaviour
{
    [SerializeField] private Button _logInButton;
    [SerializeField] private TMP_Text _labelStatus;
    private const string AUTH_GUID_KEY = "auth_guid_key";

    private void Start()
    {
        _logInButton.onClick.AddListener(() => LogIn());
    }

    private void LogIn()
    {
        var needCreation = PlayerPrefs.HasKey(AUTH_GUID_KEY);
        var uniqueID = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = uniqueID,
            CreateAccount = !needCreation
        };
        PlayFabClientAPI.LoginWithCustomID(request, 
            result =>
            {
                PlayerPrefs.SetString(AUTH_GUID_KEY, uniqueID);
                OnLoginSuccess(result);
            },
            OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _labelStatus.text = "Login successful";
        _labelStatus.color = Color.green;
        Debug.Log("Login successful");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        _labelStatus.text = "Login failed. Error: " + error.ErrorMessage;
        _labelStatus.color = Color.red;
        Debug.LogError("Login failed. Error: " + error.ErrorMessage);
    }
}
