using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private Button _submitButton;

        [SerializeField] private GameObject _mainScreen;
        [SerializeField] private GameObject _joinScreen;
        [SerializeField] private TextMeshProUGUI _codeText;

        // Start is called before the first frame update
        void OnEnable()
        {
            _hostButton.onClick.AddListener(onHostClick);
            _joinButton.onClick.AddListener(onJoinClick);
            _submitButton.onClick.AddListener(onSubmitCodeCliked);
        }
        void OnDisable()
        {
            _hostButton.onClick.RemoveListener(onHostClick);
            _joinButton.onClick.RemoveListener(onJoinClick);
            _submitButton.onClick.RemoveListener(onSubmitCodeCliked);
        }
        private async void onHostClick()
        {
            //Eðer lobby doðru bir þekilde oluþturulduysa sahneyi yükle.
            bool succeded = await GameLobbyManager.Instance.CreateLobby();
            if (succeded)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }

        }
        void onJoinClick()
        {
            _mainScreen.SetActive(false);
            _joinScreen.SetActive(true);
        }
        private async void onSubmitCodeCliked()
        {
            string code = _codeText.text.Substring(startIndex: 0, length: _codeText.text.Length - 1);
            bool success = await GameLobbyManager.Instance.JoinLobby(code);
            if (success)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        }
    }
}

