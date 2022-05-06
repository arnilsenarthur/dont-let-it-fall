using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DontLetItFall.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private static string sceneName = "SampleScene";
    
    [Header("Texts")]
    [SerializeField]
    private LoadingTexts loadingTexts;
    [SerializeField] 
    private TextMeshProUGUI loadingTitle;
    [SerializeField] 
    private TextMeshProUGUI loadingSubtitle;
    [SerializeField] 
    private Label subtitleLabel;
    
    [Header("Images")]
    [SerializeField]
    private Sprite[] conceptSprites;
    [SerializeField]
    private Image conceptImage;
    
    [Header("Loading Assets")]
    [SerializeField]
    private RectTransform loadingIcon;    
    [SerializeField]
    private Image loadingBar;
    
    void Start()
    {
        conceptImage.sprite = conceptSprites[Random.Range(0, conceptSprites.Length)];
        
        loadingTexts.LoadingTitle.OrderBy(x => Random.Range(0,100));
        loadingTitle.text = loadingTexts.LoadingTitle[0];
        loadingSubtitle.text = loadingTexts.LoadingSubtitle[Random.Range(0, loadingTexts.LoadingSubtitle.Count)];
        subtitleLabel.enabled = true;
        
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            loadingTitle.text = loadingTexts.LoadingTitle[1+(int)(async.progress*10)];
            loadingBar.fillAmount = async.progress;
            loadingIcon.Rotate(Vector3.forward * Time.deltaTime * -250);
            
            Debug.Log(async.progress);
            
                if(async.progress >= 0.9f) 
                    //async.allowSceneActivation = true;
                    
            yield return null;
        }
    }
}
}
