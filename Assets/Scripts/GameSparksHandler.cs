using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using Facebook.Unity;


public class GameSparksHandler : MonoBehaviour {

    void Awake()
    {
        if(!FB.IsInitialized)
        {
            FB.Init(FacebookLogin);
        }
        else
        {
            FacebookLogin();
        }
    }

    public void FacebookLogin()
    {
        List<string> perms = new List<string>() {"public_profile", "user_friends"};
        if(!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(perms, GameSparksLogin);
        }
    }

    public void GameSparksLogin(ILoginResult result)
    {
        if(FB.IsLoggedIn)
        {
            new FacebookConnectRequest().SetAccessToken(AccessToken.CurrentAccessToken.TokenString).Send((response) => {
                if(response.HasErrors)
                {
                    print("GS error: " + response.Errors.ToString());
                }
                else
                {
                    print("GS FB login succesful");
                }
            });
        }
        
    }

    public static void CheckGameSparksStatus()
    {
        
    }

    public static void AuthenticateUser(string userName, string password)
    {

        bool isAuthenticated = false;
        AuthenticationRequest authRequest = new AuthenticationRequest();
        authRequest.SetUserName(userName);
        authRequest.SetPassword(password);
        authRequest.Send((response) => {
            if(response.HasErrors)
            {
                print(response.Errors.ToString());
            }
            else
            {
                print("user authenticated");
                isAuthenticated = true;
            }
        });


    }
}
