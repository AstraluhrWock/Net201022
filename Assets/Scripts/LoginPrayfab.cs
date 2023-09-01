using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

internal class LoginPrayfab : MonoBehaviour
{
    [SerializeField] private Button _logInButton;
    [SerializeField] private TMP_Text _labelStatus;

    private void Start()
    {
        _logInButton.onClick.AddListener(() => LogIn());
    }

    private void LogIn()
    {
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
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
