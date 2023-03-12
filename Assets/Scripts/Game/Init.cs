using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class Init : MonoBehaviour
    {
        async void Start()
        {
            //Kullanýlan tüm hizmetleri baþlatmak için tek giriþ noktasý.
            await UnityServices.InitializeAsync();

            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                //Oturum açma giriþimi baþarýyla tamamlandýðýnda çaðrýlýr.
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                //Anonim olarak mevcut oyuncuya giriþ yapar. Hiçbir kimlik bilgisi gerekmez ve oturum mevcut cihazla sýnýrlýdýr.
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    string username = PlayerPrefs.GetString(key: "username");
                    if (username == "")
                    {
                        username = "Player";
                        PlayerPrefs.SetString(key: "username", value: username);
                    }

                    SceneManager.LoadSceneAsync("MainMenu");
                }
            }
        }

        private void OnSignedIn()
        {
            Debug.Log(message: $"Player Id: {AuthenticationService.Instance.PlayerId}");
            Debug.Log(message: $"Token: {AuthenticationService.Instance.AccessToken}");
        }
    }
}

