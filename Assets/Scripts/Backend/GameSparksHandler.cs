using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using Facebook.Unity;
using GameSparks.Core;


public class GameSparksHandler : MonoBehaviour {

    public void StartFacebookLogin()
    {
        if(!FB.IsInitialized)
        {
            PlayerData.instance.IsLoggedInToFacebook = false;
            PlayerData.instance.IsLoggedInToGameSparks = false;
            EventManager.TriggerEvent(GameSettings.ONLINE_BUTTON_PRESSED);
            FB.Init(FacebookLogin);
        }
    }

    public void SilentLogin(Action<bool> callback)
    {
        if(!FB.IsInitialized)
        {
            FB.Init(FacebookLogin);
        }
            
    }

    public bool CheckFacebookAvailability()
    {
        if (FB.IsLoggedIn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckGSAvailability()
    {
        if(GS.Authenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FacebookLogin()
    {
        List<string> perms = new List<string>() {"public_profile", "user_friends"};
        if(!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(perms, FacebookLoginCallback);
        }
        else
        {
            EventManager.TriggerEvent(GameSettings.FACEBOOK_INIT_DONE);
            print("Is logged in, but somehow here.");
        }
    }

    private void FacebookLoginCallback(ILoginResult result)
    {
        print("" + result.RawResult.ToString());
        if(result.Error == null && result.Cancelled == false)
        {
            PlayerData.instance.IsLoggedInToFacebook = true;
            GameSparksLogin();
            GetFacebookData();
        }
        else
        {
            print("FB login error: " + result.Error.ToString() + " - Cancelled: " + result.Cancelled.ToString());

            //TODO: Retry login?
        }
    }


    private void GameSparksLogin()
    {
        
        new FacebookConnectRequest().SetAccessToken(AccessToken.CurrentAccessToken.TokenString).Send((response) => {
            if(response.HasErrors)
            {
                print("GS FB login error: " + response.Errors.ToString());
            }
            else
            {
                print("GS FB login succesful");
                PlayerData.instance.IsLoggedInToGameSparks = true;
                PlayerData.instance.GSUserId = response.UserId;
                EventManager.TriggerEvent(GameSettings.LOGGED_IN_TO_GAMESPARKS);
                
            }
        });   
    }

    private void GetFacebookData()
    {
        FB.API("/me", HttpMethod.GET, (response) => {
            if (response.Error != null) {
                print(response.Error);
            }
            else
            {
                print("found something");
                string str;
                response.ResultDictionary.TryGetValue("name", out str);
                PlayerData.instance.FBName = str;
                response.ResultDictionary.TryGetValue("id", out str);
                PlayerData.instance.FBUserId = str;
            }
        });
    }
}
