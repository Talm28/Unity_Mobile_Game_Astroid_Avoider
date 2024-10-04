using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _IOSGameId;

    [SerializeField] private string _androidAdUnitId;
    [SerializeField] private string _IOSAdUnitId;

    [SerializeField] private bool _testMode;

    private string _gameId;
    private string _adUnitId;
    private GameOverHandler _gameOverHandler;

    public static AdManager Instance;

    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            InitializeAds();
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _IOSGameId;
        _adUnitId = _IOSAdUnitId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
#endif

        if(!Advertisement.isInitialized)
            Advertisement.Initialize(_gameId, _testMode, this);
    }

    private bool adLoaded;
    private bool awaitingLoad;
    private string cachedPlacementID = "";

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        Advertisement.Load(_adUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Unity Ads initialization failed.");
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        adLoaded = true;
        cachedPlacementID = placementId;
        if (awaitingLoad)
        {
            Advertisement.Show(placementId, this);
            adLoaded = false;
            awaitingLoad = false;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad unity {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad unity {_adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId.Equals(_adUnitId) 
            && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Advertisement.Load(_adUnitId, this);
            _gameOverHandler.ContinueGame();
        }
    }

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this._gameOverHandler = gameOverHandler;
        if (adLoaded)
        {
            Advertisement.Show(cachedPlacementID, this);
            adLoaded = false;
        } else
        {
            awaitingLoad = true;
            Advertisement.Load(_adUnitId, this);
        }
    }
}
