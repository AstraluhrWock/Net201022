using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

internal class CloudScriptManager : MonoBehaviour
{
    private void GetCharacterCloudScript()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "getBaseCharacter",
            FunctionParameter = new { PlayerName = "Player1", Health = "150"}
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptCallback, OnErrorCallback);
    }

    private void OnCloudScriptCallback(ExecuteCloudScriptResult result)
    { 
    
    }

    private void OnErrorCallback(PlayFabError error)
    {
        Debug.Log("Cloud scripts error: " + error.GenerateErrorReport()); 
    }
}
