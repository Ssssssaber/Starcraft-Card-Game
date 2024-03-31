using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DatabaseLoader.Flyweight;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
using Random = System.Random;


    [Serializable]
    public class CardData
    {
        public CardStats cardStats;
        public StatsLoadError error;
        public CardData()
        {
            cardStats = new CardStats();
            error = new StatsLoadError("", false);
        }
    }
    
    [Serializable]
    public class StatsLoadError
    {
        public string errorText;
        public bool isError;

        public StatsLoadError(string errorText, bool isError)
        {
            this.errorText = errorText;
            this.isError = isError;
        }
    }

public class CardStatsManager : MonoBehaviour, ICardStatsLoader
{
    [FormerlySerializedAs("targetURL")] public string TargetURL;

    [FormerlySerializedAs("AllCards")] public List<CardStats> LoadedCards = new List<CardStats>();

    public static CardStatsManager instance;

    private int _cardsCount = 0;

    [SerializeField] private CardStats _testStats;

    private CardStats _downloadedCardStats;

    private void HandleDownload(CardStats downloadedCardStats)
    {
        EventManager.CardLoaded(downloadedCardStats);
        AddNewCard(downloadedCardStats);

        if (LoadedCards.Count == _cardsCount)
        {
            EventManager.AllCardsLoaded(LoadedCards);
        }
    }

    public void AddNewCard(CardStats cardStats)
    {
        if (!IsAlreadyContained(cardStats))
        {
            LoadedCards.Add(cardStats);
        }
    }

    private bool IsAlreadyContained(CardStats cardStats)
    {
        bool found = false;

        foreach (var tempCardStats in LoadedCards)
        {
            if (cardStats.id == tempCardStats.id)
            {
                found = true;
                break;
            }
        }

        return found;
    }

    public UnityEvent OnDownload = new UnityEvent();
    public UnityEvent OnUpload = new UnityEvent();
    public UnityEvent OnCheckCount = new UnityEvent();

    public enum RequestType
    {
        Download,
        Upload,
        DownloadAll,
        CheckCardsCount
    }

    public string GetCardData(CardData cardData)
    {
        return JsonUtility.ToJson(cardData);
    }

    public static CardData SetCardData(string data)
    {
        try
        {
            return JsonUtility.FromJson<CardData>(data);
        }
        catch (Exception)
        {
            throw new Exception("Invalid JSON file");
        }
    }

    private void Awake()
    {
        instance = this;
        // CheckCardsCount();
    }

    public int GetCardsCount()
    {
        return _cardsCount;
    }

    public void LoadCards()
    {
        if (_cardsCount == 0)
        {
            CheckCardsCount();
        }
        for (int i = 1; i <= _cardsCount; i++)
        {
            // RoutinesQueue.coroutineQueue.Enqueue(LoadCard(i));
            // EnqueueLoad(i);
            LoadCard(i);
        }
    }

        // private void EnqueueLoad(int id)
        // {
        //     RoutinesQueue.coroutineQueue.Enqueue(LoadCard(id));
        // }
        
        public void LoadCard(int i)
        {
            WWWForm form = new WWWForm();
            form.AddField("request-type", RequestType.Download.ToString());
            form.AddField("id", i);
            RoutinesQueue.coroutineQueue.Enqueue(Loading(form, RequestType.Download));
            // yield return new WaitForSeconds(0f);
        }

        public void UploadCard(string _name, string desc,
            int manaCost, int health, int attack, Race race, CardType type, PlayStyle style)
        {
            WWWForm form = new WWWForm();
            form.AddField("request-type", RequestType.Upload.ToString());
            form.AddField("name", _name);
            form.AddField("desc", desc);
            form.AddField("mana-cost", manaCost);
            form.AddField("health", health);
            form.AddField("attack", attack);
            form.AddField("race", race.ToString());
            form.AddField("card-type", type.ToString());
            form.AddField("play-style", style.ToString());
            RoutinesQueue.coroutineQueue.Enqueue(Loading(form, RequestType.Upload));
        }

        // public void DownloadCard(int id)
        // {
        //     WWWForm form = new WWWForm();
        //     form.AddField("request-type", RequestType.Download.ToString());
        //     form.AddField("id", id);
        //     RoutinesQueue.coroutineQueue.Enqueue(Loading(form, RequestType.Download));
        // }

        public void CheckCardsCount()
        {
            StopAllCoroutines();
            WWWForm form = new WWWForm();
            form.AddField("request-type", RequestType.CheckCardsCount.ToString());
            RoutinesQueue.coroutineQueue.Enqueue(Loading(form, RequestType.CheckCardsCount));
        }

        IEnumerator Loading(WWWForm form, RequestType type)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(TargetURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (type == RequestType.CheckCardsCount)
                    {
                        // Debug.Log(www.downloadHandler.text);
                        _cardsCount = Int32.Parse(www.downloadHandler.text);
                        OnCheckCount?.Invoke();
                    }
                    else if (type == RequestType.DownloadAll)
                    {
                        // Debug.Log(www.downloadHandler.text);
                        // Debug.Log(www.downloadHandler.text);
                        _cardsCount = Int32.Parse(www.downloadHandler.text);
                        OnCheckCount?.Invoke();
                        LoadCards();
                    }
                    else
                    {
                        CardData temp = SetCardData(www.downloadHandler.text);
                        // Debug.Log(www.downloadHandler.text);
                        if (!temp.error.isError)
                        {
                            switch (type)
                            {
                                case RequestType.Download:
                                    _downloadedCardStats = temp.cardStats;
                                    // _downloadedCardStats.PrintStats();
                                    HandleDownload(_downloadedCardStats);
                                    OnDownload?.Invoke();
                                    break;
                                case RequestType.Upload:
                                    OnUpload?.Invoke();
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
                            }
                        }
                        else
                        {
                            Debug.Log(temp.error.errorText);
                        }
                    }
                   
                }
            }
        }

    }