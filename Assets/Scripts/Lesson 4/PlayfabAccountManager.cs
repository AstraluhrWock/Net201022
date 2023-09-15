using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

internal class PlayfabAccountManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleLabel;
    [SerializeField] private TMP_Text _usernameLabel;
    [SerializeField] private TMP_Text _createdDataLabel;

    [SerializeField] private GameObject _newCharacterPanel;
    [SerializeField] private Button _createCharacterButton;
    [SerializeField] private TMP_InputField _inputCharacterField;

    [SerializeField] List<SlotCharacterWidget> _slots;

    private string _characterName;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);

        GetCharacters();

        foreach (var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(OpenNewCharacterPanel);
        }

        _inputCharacterField.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Character count: {result.Characters.Count}");
                ShowCharactersInSlot(result.Characters);
            }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0)
        {
            foreach (var slot in _slots)
            {
                slot.ShowEmptySlot();
            }
        }
        else if (characters.Count > 0 && characters.Count <= _slots.Count)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = characters.First().CharacterId,
            },
            result =>
            {
                var level = result.CharacterStatistics["Level"].ToString();
                var gold = result.CharacterStatistics["Gold"].ToString();
                var damage = result.CharacterStatistics["Damage"].ToString();
                var health = result.CharacterStatistics["Health"].ToString();
                var exp = result.CharacterStatistics["Exp"].ToString();
                _slots.First().ShowInfoCharacter(characters.First().CharacterName, level, gold, damage, health, exp);   

            }, OnError);
        }
        else
        {
            Debug.LogError($"Add slot for new character");
        }
    }

    private void OpenNewCharacterPanel()
    {
        _newCharacterPanel.SetActive(true);
    }

    private void CloseNewCharacterPanel()
    {
        _newCharacterPanel.SetActive(false);
    }

    private void OnNameChanged(string name)
    {
        _characterName = name;
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        { 
            CharacterName = _characterName,
            ItemId = "character_token"
        },
        result =>
        {
            UpdateCharacterStatics(result.CharacterId);
        }, OnError);
    }

    private void UpdateCharacterStatics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"Gold", 0},
                {"Damage", 10},
                {"Health", 100},
                {"Exp", 100}
            }
        },
            result =>
            {
                Debug.Log("Character success");
                CloseNewCharacterPanel();
                GetCharacters();
            }, OnError);
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

    private void OnGetCatalog(GetCatalogItemsResult result)
    {
        Debug.Log("Catalog success");
        ShowItems(result.Catalog);
    }

    private void ShowItems(List<CatalogItem> catalog)
    {
        foreach (var item in catalog)
        {
            Debug.Log(item.ItemId);
        }
    }
}
