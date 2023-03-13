using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;
    // Start is called before the first frame update
    void Start()
    {
        _hostButton.onClick.AddListener(onHostClick);
        _joinButton.onClick.AddListener(onJoinClick);
    }
    private async void onHostClick()
    {
        bool succeded =await GameLobbyManager.Instance.CreateLobby();
        if(succeded)
        {
            SceneManager.LoadSceneAsync("Lobby");
        }

    }
    void onJoinClick()
    {
        Debug.Log(message:"Join");
    }
}
