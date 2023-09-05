using UnityEngine;
using UnityEngine.UI;

internal class EnterGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _createAccountButton;

    [SerializeField] private Canvas _enterInGameCanvas;
    [SerializeField] private Canvas _createAccountCanvas;
    [SerializeField] private Canvas _signInCanvas;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccountButton.onClick.AddListener(CreateAccountWindow);
    }

    private void OpenSignInWindow()
    {
        _signInCanvas.enabled = true;
        _enterInGameCanvas.enabled = false;
        _createAccountCanvas.enabled = false;
    }

    private void CreateAccountWindow()
    {
        _createAccountCanvas.enabled = true;
        _signInCanvas.enabled = false;
        _enterInGameCanvas.enabled = false;
    }
}
