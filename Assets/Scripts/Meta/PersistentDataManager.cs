using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class PersistentDataManager : MonoBehaviour {

    private static PersistentDataManager instance = null;



    void Awake()
    {
        if (instance != null) {
            Destroy (this.gameObject);
            print ("Dupe PersistentDataManager self-destructing!");
        } else 
        {
            instance = this;
            GameObject.DontDestroyOnLoad (this.gameObject);
        }

        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    public static void SaveToLeaderBoard(float maxTravel)
    {
        if(GS.Authenticated)
        {
            new LogEventRequest_SCORE_EVENT()
                .Set_TRAVEL_ATTR((long)maxTravel)
                .Send((response) => 
                {
                    if(response.HasErrors)
                    {
                        Console.WriteLine("score not posted");
                    }
                    else
                    {
                        Console.WriteLine("score posted");
                    }
                });

        }
    }

    public static void SavePlayerData(int coins, float maxTravel, float lastTravel)
    {
        SaveOfflinePlayerData(coins, maxTravel, lastTravel);
        instance.StartCoroutine(SaveOnlinePlayerDataAsync(coins, maxTravel, lastTravel));
    }


    public static IEnumerator LoadPlayerData(string userId, Action<Data> callback)
    {
        Data onlineData = null;
        Data offlineData = null;
        bool isOnlineDataLegit = false;
        bool hasFinished = false;

        print("loading stuff!");

        offlineData = new Data(LoadOfflinePlayerData());

        if (GS.Authenticated && userId != null)
        {
            new LogEventRequest_GET_DATA().Set_PLAYER_ID(userId).Send((response) => {
                if(!response.HasErrors)
                {
                    onlineData = new Data();
                    onlineData.timeStamp = new DateTime((long)response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                        .GetNumber(BackendVariables.PLAYER_DATA_TIMESTAMP));
                    onlineData.globalCoinScore = (int) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                        .GetNumber(BackendVariables.PLAYER_DATA_COINS);
                    onlineData.lastTravelScore = (float) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                        .GetNumber(BackendVariables.PLAYER_DATA_LAST_TRAVEL);
                    onlineData.highestTravelScore = (float) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                        .GetNumber(BackendVariables.PLAYER_DATA_MAX_TRAVEL);    

                    print("Online TimeStamp: " + onlineData.timeStamp);
                    hasFinished = true;
                }
                else
                {
                    print("Error retrieving player data: " + response.Errors.ToString());
                    hasFinished = true;
                }


            });


            while (!hasFinished)
            {
                
                yield return new WaitForEndOfFrame();
            }


        }
        else
        {
            //Player not signed in. use offlinedata
            isOnlineDataLegit = false;
            hasFinished = true;
            print("User Offline, using offline data");   
        }


        //assuming data was found both online and off. Check which is the newest.
        if (onlineData != null && offlineData != null)
        {
            if (IsOfflineTimeStampNewer(offlineData.timeStamp, onlineData.timeStamp))
            {
                //if the offline data is newer, save it to online.
                instance.StartCoroutine(SaveOnlinePlayerDataAsync(offlineData.globalCoinScore, offlineData.lastTravelScore, offlineData.highestTravelScore));
                isOnlineDataLegit = false;
                print("Offline data newer. Using that.");

            } else
            {
                SaveOfflinePlayerData(onlineData.globalCoinScore, onlineData.lastTravelScore, onlineData.highestTravelScore);
                isOnlineDataLegit = true;

            }
        }

        //print("returning " + onlineData.timeStamp + " - " + onlineData.globalCoinScore + "Is online data: " + isOnlineDataLegit);
        if(hasFinished && isOnlineDataLegit)
        {
            callback(onlineData);    
        }
        else if(hasFinished && !isOnlineDataLegit)
        {
            callback(offlineData);
        }

    }
        

    private static IEnumerator SaveOnlinePlayerDataAsync(int coins, float maxTravel, float lastTravel)
    {
        string message = "";
        //We create a GSRequestData variable
        //by using jsonDataToSend.Add() we can add in any variable we choose
        //and they will be converted to JSON
        GSRequestData jsonDataToSend = new GSRequestData();
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_COINS, coins);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_MAX_TRAVEL, maxTravel);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_LAST_TRAVEL, lastTravel);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_TIMESTAMP, DateTime.Now.Ticks);

        if(GS.Authenticated)
        {
            //We then send our LogEventRequest with the event shortcode and the event attrirbute
            new LogEventRequest_SET_DATA().Set_PLAYER_DATA(jsonDataToSend).Send((response) =>
            {
                if (response.HasErrors) {
                    Console.WriteLine(response.Errors.ToString());
                    message = response.Errors.ToString();
                }
                else
                {
                    Console.WriteLine("Player Data sent");
                    message = "Player Data Sent";
                }

            }); 

            while(message == "")
            {
                yield return new WaitForEndOfFrame();    
            }

        }

    }

    #region offline data
    private static void SaveOfflinePlayerData(int coins, float lastTravel, float highestTravel)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + GameSettings.SAVE_PATH, FileMode.OpenOrCreate);

        Data data = new Data();
        data.timeStamp = DateTime.Now;
        data.globalCoinScore = coins;
        data.highestTravelScore = highestTravel;
        data.lastTravelScore = lastTravel;

        bf.Serialize(file, data);
        file.Close();

    }

    private static Data LoadOfflinePlayerData()
    {
        if(File.Exists(Application.persistentDataPath + GameSettings.SAVE_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + GameSettings.SAVE_PATH, FileMode.Open);
            Data data = (Data) bf.Deserialize(file);

            file.Close();

            return data;
        }
        else
        {
            return new Data();
        }
    }
    #endregion

    private static bool IsOfflineTimeStampNewer(DateTime offlineStamp, DateTime onlineStamp)
    {
        return (offlineStamp > onlineStamp) ? true : false;
    }

}
    
[Serializable]
public class Data
{
    public Data()
    {}

    public Data(Data dat)
    {
        timeStamp = dat.timeStamp;
        globalCoinScore = dat.globalCoinScore;
        lastTravelScore = dat.lastTravelScore;
        highestTravelScore = dat.highestTravelScore;

    }
    public DateTime timeStamp;
    public int globalCoinScore;
    public float lastTravelScore;
    public float highestTravelScore;
}
