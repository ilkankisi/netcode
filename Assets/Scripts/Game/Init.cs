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
            //Kullan�lan t�m hizmetleri ba�latmak i�in tek giri� noktas�.
            await UnityServices.InitializeAsync();

            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                //Oturum a�ma giri�imi ba�ar�yla tamamland���nda �a�r�l�r.
                AuthenticationService.Instance.SignedIn += OnSignedIn;
                //Anonim olarak mevcut oyuncuya giri� yapar. Hi�bir kimlik bilgisi gerekmez ve oturum mevcut cihazla s�n�rl�d�r.
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

