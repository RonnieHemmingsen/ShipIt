using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using Facebook.Unity;
using GameSparks.Core;


public class OnlineManager : MonoBehaviour {

    private static OnlineManager instance = null;

    private bool isConnectedToInterwebs;

    void Awake()
    {
        if (instance != null) {
            Destroy (this.gameObject);
            print ("Dupe OnlineManager self-destructing!");
        } else 
        {
            instance = this;
            GameObject.DontDestroyOnLoad (this.gameObject);
        }


    }

    public void InitializeFacebook()
    {

            if(!FB.IsInitialized)
            {

                print("Initializing facebook");
                FB.Init(OnInitComplete);

            }
            else
            {
                print("Facebook already initialized");
                OnInitComplete();
            }    
    }

    public void FacebookLogin()
    {
        print("FB Token: " + AccessToken.CurrentAccessToken);

        //Debug!!
        if(!FB.IsLoggedIn && AccessToken.CurrentAccessToken != null)
        {
            foreach (var perm in AccessToken.CurrentAccessToken.Permissions)
            {
                print("Permission: " + perm);
            }

            FB.Mobile.RefreshCurrentAccessToken((response) =>{
                print("Refreshed accesstoken: " + response.AccessToken);
                AccessToken.CurrentAccessToken = response.AccessToken;
            });
        }

        List<string> perms = new List<string>() {"public_profile", "user_friends"};

        if(!FB.IsLoggedIn && AccessToken.CurrentAccessToken == null)
        {
            FB.LogInWithReadPermissions(perms, FacebookLoginCallback);
        }
        else
        {
            print("No interwebs connection");
            EventManager.TriggerEvent(OnlineStrings.ONLINE_FALLTHROUGH);
        }    

    }


    private void OnInitComplete()
    {
        print("FB Token: " + AccessToken.CurrentAccessToken);


        if(!FB.IsLoggedIn && AccessToken.CurrentAccessToken == null)
        {
            //Kontroller at der ikke er et login til gamesparks som kan bruges.
            string id = PersistentDataManager.LoadGSUserId();
            {
                if(string.IsNullOrEmpty(id))
                {
                    print("id: " + id);
                    EventManager.TriggerEvent(OnlineStrings.FACEBOOK_NEW_USER);        
                }

            }

            print("Not logged in to facebook, is initted");
        }
        else
        {
            EventManager.TriggerEvent(OnlineStrings.FACEBOOK_INIT_DONE);
            //GetFacebookData();
            print("Is logged in, facebook initted");
        }    


    }

    private void FacebookLoginCallback(ILoginResult result)
    {
        print("FB login callback" + result.RawResult.ToString());
        if(result.Error == null && result.Cancelled == false)
        {
            EventManager.TriggerEvent(OnlineStrings.LOGGED_IN_TO_FACEBOOK);
        }
        else if(result.Cancelled)
        {
            print("User cancelled");

            EventManager.TriggerEvent(OnlineStrings.ONLINE_FALLTHROUGH);
        } 
        else
        {
            EventManager.TriggerEvent(OnlineStrings.ONLINE_FALLTHROUGH);
            print(result.Error.ToString());
        }
    }

    public void GameSparksLogout()
    {
        GS.Reset();
        PersistentDataManager.SaveGSUserId("");
        FBLogout();
        print("Player logged out of gamesparks"); 

    }

    public void FBLogout()
    {
        if(FB.IsLoggedIn)
        {
            FB.LogOut();    
        }
        EventManager.TriggerEvent(OnlineStrings.OFFLINE_BUTTON_PRESSED);
        print("PlayerData logged out of facebook");
    }

    public void GameSparksLogin()
    {
        
        new FacebookConnectRequest().SetAccessToken(AccessToken.CurrentAccessToken.TokenString).Send((response) => {
            if(response.HasErrors)
            {
                print("GS FB login error: " + response.Errors.ToString());
                EventManager.TriggerEvent(OnlineStrings.ONLINE_FALLTHROUGH);
            }
            else
            {
                print("GS FB login succesful");
                PersistentDataManager.SaveGSUserId(response.UserId);
                EventManager.TriggerEvent(OnlineStrings.LOGGED_IN_TO_GAMESPARKS);

            }
        });   
    }
    //Not currently in use. Men det virker.
//    private void GetFacebookData()
//    {
//        FB.API("/me", HttpMethod.GET, (response) => {
//            if (response.Error != null) {
//                print(response.Error);
//            }
//            else
//            {
//                print("found something");
//                string str;
//                response.ResultDictionary.TryGetValue("name", out str);
//                PlayerData.instance.UserName = str;
//                response.ResultDictionary.TryGetValue("id", out str);
//                PlayerData.instance.FBUserId = str;
//            }
//        });
//    }
}
