using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;

public class TopBannerAd : MonoBehaviour
{
    [SerializeField] private string appId = "ca-app-pub-3940256099942544~3347511713";
    
#if UNITY_ANDROID
    private const string BannerId = "ca-app-pub-1983845649073603/7088117137";
    private const string InterId = "ca-app-pub-1983845649073603/1405180075";
    private const string RewardedId = "ca-app-pub-1983845649073603/9838203869";
    private const string NativeId = "ca-app-pub-3940256099942544/2247696110";
    
#elif UNITY_IOS
    private string bannerId = "ca-app-pub-3940256099942544/2934735716";
    private string interId = "ca-app-pub-3940256099942544/4411468910";
    private string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    private string nativeId = "ca-app-pub-3940256099942544/3986624511";
    
#endif

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;
    private NativeAd _nativeAd;

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            //print("Ads Initialized !!");
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }
#region Banner

    private void LoadBannerAd()
    {
        //load the banner
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        
        //listen to banner events
        ListenToBannerEvents();
        
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        
        print("Loading Banner Ad");
        _bannerView.LoadAd(adRequest);
        _bannerView.Hide();
    }

    private void CreateBannerView()
    {
        if (_bannerView != null)
        {
            DestroyBannerAd();
        }

        _bannerView = new BannerView(BannerId, AdSize.Banner, AdPosition.Bottom);
    }

    private void ListenToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                      + _bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                           + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    private void DestroyBannerAd()
    {
        if (_bannerView != null)
        {
            //print("Destroying Banner Ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void ShowBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Show();
        }
        else
        {
            Debug.Log("Banner view not loaded yet");
        }
    }

    public void HideBannerAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Hide();
            Debug.Log("Banner ad hidden.");
        }
        else
        {
            Debug.LogError("Banner view not loaded yet");
        }
    }
#endregion

#region Interstitial

    private void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        InterstitialAd.Load(InterId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Interstitial ad failed to load");
                return;
            }
        
            print("Interstitial ad loaded. " + ad.GetResponseInfo());
            
            _interstitialAd = ad;
            InterstitialEvents(_interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            print("Interstitial ad not ready.");
        }
        LoadInterstitialAd();
    }

    private void InterstitialEvents(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            StartCoroutine(DelayedTimeScaleChange());

            IEnumerator DelayedTimeScaleChange()
            {
                yield return new WaitForEndOfFrame();
                Time.timeScale = 0;
            }
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

#endregion

#region Rewarded

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        
        RewardedAd.Load(RewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                print("Rewarded ad failed to load " + error);
                return;
            }
            
            print("Rewarded ad loaded. " + ad.GetResponseInfo());
            _rewardedAd = ad;
            RewardedAdEvents(_rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((reward =>
            {
                print("Rewarded ad was clicked.");
                GameManager gameManager = (GameManager)GameObject.FindFirstObjectByType(typeof(GameManager));
                gameManager.NewLife();
            }));
            
            LoadRewardedAd();
        }
        else
        {
            print("Rewarded ad not ready.");
        }
    }

    private void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
#endregion
}
