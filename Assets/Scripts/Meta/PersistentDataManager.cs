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

    public static void SaveGSUserId(string userId)
    {
        PlayerPrefs.SetString(OnlineStrings.GS_USER_ID, userId);
    }

    public static string LoadGSUserId()
    {
        if(PlayerPrefs.HasKey(OnlineStrings.GS_USER_ID))
        {
            return PlayerPrefs.GetString(OnlineStrings.GS_USER_ID);
        } else
        {
            return "";
        }
    }

    public static IEnumerator LoadPlayerName(string userId, Action<string> callback)
    {
        string userName = ""; 
        new AccountDetailsRequest().Send((response) => {
            if (response.HasErrors) {
                print(response.HasErrors.ToString());
                userName = response.HasErrors.ToString();
            }
            else 
            {
                userName = response.DisplayName;
            }
        });

        while (userName == "")
        {
            yield return new WaitForEndOfFrame();
        }

        callback(userName);
    }

    public static void SavePlayerData(Data scores)
    {
        scores.timeStamp = DateTime.Now;
        if(IsNewTravelScoreHigher(scores.highestTravelScore, scores.lastTravelScore))
        {
            scores.highestTravelScore = scores.lastTravelScore;
        }

        SaveOfflinePlayerData(scores);
        instance.StartCoroutine(SaveOnlinePlayerData(scores));

        if(GS.Authenticated)
        {
            SaveToLeaderBoard(scores.highestTravelScore);
        }

    }
        
    public static IEnumerator LoadPlayerData(string userId, Action<Data> callback)
    {
        Data onlineData = null;
        Data offlineData = null;
        bool isOnlineDataLegit = false;
        bool hasFinished = false;

        print("loading player data " + userId);

        offlineData = new Data(LoadOfflinePlayerData());

        if (GS.Authenticated && !string.IsNullOrEmpty(userId))
        {
            new LogEventRequest_GET_DATA().Set_PLAYER_ID(userId).Send((response) => {
                if(!response.HasErrors)
                {
                    try {
                        onlineData = new Data();
                        onlineData.timeStamp = new DateTime((long)response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                            .GetNumber(BackendVariables.PLAYER_DATA_TIMESTAMP));
                        

                        onlineData.globalCoinScore = (int) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                            .GetNumber(BackendVariables.PLAYER_DATA_TOTAL_COINS);
                        onlineData.lastCoinScore = (int) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                            .GetNumber(BackendVariables.PLAYER_DATA_LAST_COINS);
                        onlineData.lastTravelScore = (float) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                            .GetNumber(BackendVariables.PLAYER_DATA_LAST_TRAVEL);
                        onlineData.highestTravelScore = (float) response.ScriptData.GetGSData(BackendVariables.PLAYER_DATA)
                            .GetNumber(BackendVariables.PLAYER_DATA_MAX_TRAVEL);    

                        print("Online data retrieved - TimeStamp: " + onlineData.timeStamp);
                    } catch (Exception ex) {
                        print("Load Exception: " +ex.ToString());
                    }

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
        
    private static IEnumerator SaveOnlinePlayerData(Data scores)
    {
        string message = "";
        //We create a GSRequestData variable
        //by using jsonDataToSend.Add() we can add in any variable we choose
        //and they will be converted to JSON
        GSRequestData jsonDataToSend = new GSRequestData();
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_TOTAL_COINS, scores.globalCoinScore);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_LAST_COINS, scores.lastCoinScore);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_MAX_TRAVEL, scores.highestTravelScore);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_LAST_TRAVEL, scores.lastTravelScore);
        jsonDataToSend.Add(BackendVariables.PLAYER_DATA_TIMESTAMP, scores.timeStamp.Ticks);

        if(GS.Authenticated)
        {
            //We then send our LogEventRequest with the event shortcode and the event attrirbute
            new LogEventRequest_SET_DATA().Set_PLAYER_DATA(jsonDataToSend).Send((response) =>
            {
                if (response.HasErrors) {
                    print("Error Sending Data: " + response.HasErrors.ToString());
                }
                else
                {
                    print("Player data sent");
                    message = "Player Data Sent";
                }

            }); 

            while(message == "")
            {
                yield return new WaitForEndOfFrame();    
            }

        }
        else
        {
            print("User not connected to GS, cant send data");
        }

    }
        

    #region offline data
    private static void SaveOfflinePlayerData(Data scores)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + GameSettings.SAVE_PATH, FileMode.OpenOrCreate);

        Data data = new Data();
        data.timeStamp = scores.timeStamp;
        data.globalCoinScore = scores.globalCoinScore;
        data.lastCoinScore = scores.lastCoinScore;
        data.highestTravelScore = scores.highestTravelScore;
        data.lastTravelScore = scores.lastTravelScore;

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

    private static bool IsOnlineTimeStampNewer(DateTime offlineStamp, DateTime onlineStamp)
    {
        return (onlineStamp >= offlineStamp) ? true : false;
    }

    private static bool IsNewTravelScoreHigher(float prevScore, float thisScore)
    {
        return (thisScore > prevScore) ? true : false;
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
        lastCoinScore = dat.lastCoinScore;
        lastTravelScore = dat.lastTravelScore;
        highestTravelScore = dat.highestTravelScore;

    }
    public DateTime timeStamp;
    public int globalCoinScore;
    public int lastCoinScore;
    public float lastTravelScore;
    public float highestTravelScore;
    public string userName;
}
