using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

internal class SignWindow : AccountDataWindowBase
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _signInCanvas;

    [SerializeField] private TMP_Text _loadingText;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _signInButton.onClick.AddListener(SignIn);
        _backButton.onClick.AddListener(BackInMenu);
    }

    private void SignIn()
    {
        _loadingText.enabled = true;  
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        { 
            Username = _username,
            Password = _password
        },
          result =>
          {
              _loadingText.enabled = false;
              Debug.Log($"Success: {_username}");
              LoadGameScene();
          },
          error =>
          {
              Debug.LogError($"Fail: {error.ErrorMessage}");
          } 
        );
    }

    private void BackInMenu()
    {
        _enterInGameCanvas.enabled = true;
        _signInCanvas.enabled = false;
    }

    private void Update()
    {
        if (_loadingText.enabled)
        {
            _loadingText.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}
