using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab;

public class PhotonSetting : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField email;
    [SerializeField] InputField userID;
    [SerializeField] InputField password;


    public void LoginSuccess(LoginResult result)
    {
        PhotonNetwork.AutomaticallySyncScene = false;   // AutomaticallySyncScene : 자동 동기화 장면
        PhotonNetwork.GameVersion = "1.0f";
        PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
        PhotonNetwork.LoadLevel("Photon Lobby");
    }

    public void LoginFailure(PlayFabError error)
    {
        NotificationManager.NotificationWindow
        (
            "Login failed",
            "There are currently no accounts registered on the server." +
            "\n\n Please enter your ID and password correctly."
        );
    }

    public void SignUpSuccess(RegisterPlayFabUserResult result)
    {
        NotificationManager.NotificationWindow
        (
            "Membership successful",
            "Congratulations on becoming a member." +
            "\n\n Your email account has been registered on the game server."
        );
    }

    public void SignUpFailure(PlayFabError error)
    {
        NotificationManager.NotificationWindow
        (
            "Failed to Sign Up",
            "Membership registration failed due to a current server error." +
            "\n\n Please try to register as a member again."
        );
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email.text,
            Password = password.text,
        };

        PlayFabClientAPI.LoginWithEmailAddress
        (
            request,
            LoginSuccess,
            LoginFailure
        );

    }

    public void SignUp()
    {
        // RegisterPlayFabUserRequest : 서버에 유저를 등록하기 위한 클래스
        var request = new RegisterPlayFabUserRequest
        {
            Email = email.text,
            Password = password.text,
            Username = userID.text
        };

        PlayerPrefs.SetString("Name", userID.text);

        PlayFabClientAPI.RegisterPlayFabUser
        (
            request,
            SignUpSuccess,
            SignUpFailure
        );
    }
}
