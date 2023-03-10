using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void onHostClick()
    {
        Debug.Log(message:"Host");
        GameLobbyManager.Instance.CreateLobby();
    }
    void onJoinClick()
    {
        Debug.Log(message:"Join");
    }
}
