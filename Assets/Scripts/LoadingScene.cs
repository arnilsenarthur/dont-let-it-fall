using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DontLetItFall.UI;
using DontLetItFall.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DontLetItFall
{
    public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private VariableString sceneName;
    
    [SerializeField]
    private bool isTestScene = false;
    
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
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName.Value);
        async.allowSceneActivation = false;

        while(!async.isDone)
        {
            loadingTitle.text = loadingTexts.LoadingTitle[1+(int)(async.progress*10)];
            Debug.Log(loadingTitle.text);
            loadingBar.fillAmount = async.progress;
            loadingIcon.Rotate(Vector3.forward * Time.deltaTime * -250);

            if(async.progress >= 0.9f && !isTestScene) 
                    async.allowSceneActivation = true;
                    
            yield return null;
        }
    }
}
}
