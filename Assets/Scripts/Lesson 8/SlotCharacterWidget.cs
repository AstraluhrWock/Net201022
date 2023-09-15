using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class SlotCharacterWidget : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _emptySlot;
    [SerializeField] private GameObject _infoSlot;

    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _levelText;
    [SerializeField] TMP_Text _goldText;
    [SerializeField] TMP_Text _damageText;
    [SerializeField] TMP_Text _healthText;
    [SerializeField] TMP_Text _expText;


    public Button SlotButton => _button;

    public void ShowInfoCharacter(string name, string level, string gold, string damage, string health, string experience) 
    {
        _nameText.text = name;
        _levelText.text = $"Level: {level}";
        _goldText.text = $"Gold: {gold}";
        _damageText.text = $"Damage: {damage}";
        _healthText.text = $"Health: {health}";
        _expText.text = $"Exp: {experience}";

        _infoSlot.SetActive(true);
        _emptySlot.SetActive(false);
    }

    public void ShowEmptySlot()
    {
        _infoSlot.SetActive(false);
        _emptySlot.SetActive(true);
    }
}
