using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

internal class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] private InputField _emailField;
    [SerializeField] private Button _createAccountButton;

    private string _email;

    protected override void SubscriptionsElementsUI()
    {
        base.SubscriptionsElementsUI();

        _emailField.onValueChanged.AddListener(UpdateEmail);
        _createAccountButton.onClick.AddListener(()=>CreateAccount());
    }

    private void UpdateEmail(string email)
    {
        _email = email;
    }

    private void CreateAccount()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _email,
            Password = _password
        },
          result =>
          {
              Debug.Log($"Success: {_username}");
              LoadGameScene();
          },
          error =>
          {
              Debug.LogError($"Fail : {error.ErrorMessage}");
          }
        );
    }
    
}
