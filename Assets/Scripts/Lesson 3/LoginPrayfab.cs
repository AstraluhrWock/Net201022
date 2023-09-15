using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

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
        SetUserData(result.PlayFabId);
        //MakePurchase();    
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), 
            result => ShowInventory(result.Inventory), OnLoginError);
    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        foreach (var item in inventory)
        {
            ConsumePosition(item.ItemInstanceId);
        }
    }

    private void ConsumePosition(string item)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        { 
            ConsumeCount = 1,
            ItemInstanceId = item
        }, 
        result =>
        {
            Debug.Log("Use item");
        },
        OnLoginError         
        );
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        { 
            CatalogVersion = "Main",
            ItemId = "Health_potion",
            Price = 3,
            VirtualCurrency = "SC"
        },
          result =>
          {
              Debug.Log("Complete purshase item");
          },
            OnLoginError
        );
    }

    private void SetUserData(string playfabID)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "time_recieve_dairy_reward", DateTime.UtcNow.ToString()}
            }
        },
        result =>
        {
            Debug.Log("Set user data");
            GetUserData(playfabID, "time_recieve_dairy_reward");
        },
        OnLoginError
        );
    }

    private void GetUserData(string playfabID, string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = playfabID
        },
        result =>
        {
            if (result.Data.ContainsKey(keyData))
                Debug.Log($"{keyData} : {result.Data[keyData].Value}");
        }, OnLoginError
        );
    }

    private void OnLoginFailure(PlayFabError error)
    {
        _labelStatus.text = "Login failed. Error: " + error.ErrorMessage;
        _labelStatus.color = Color.red;
        Debug.LogError("Login failed. Error: " + error.ErrorMessage);
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }
}
