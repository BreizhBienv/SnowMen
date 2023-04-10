using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private static LoadingScene _instance;

    [SerializeField] private GameObject _displayLoading;
    [SerializeField] private Image _progressBar;
    [SerializeField] private int _fillSpeed;

    private float _target;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_progressBar != null)
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, _fillSpeed * Time.deltaTime);
    }

    public static LoadingScene Instance
    {
        get
        {
            return _instance;
        }

        set => _instance = value;
    }

    public async void LoadScene(string p_sceneName)
    {
        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(p_sceneName);

        scene.allowSceneActivation = false;

        _displayLoading.SetActive(true);

        do
        {
            await Task.Delay(100);

            _target = scene.progress;

        } while (scene.progress < 0.9f);


        scene.allowSceneActivation = true;
        _displayLoading.SetActive(false);
    }
}
