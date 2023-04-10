using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private static Timer _instance;

    [SerializeField] private TextMeshProUGUI _canvasTimer;

    [SerializeField] private float _baseTimeOfGame;
    private float _currTimeLeft;

    private int _minutesLeft;
    private int _secondsLeft;

    private Animator m_textAnim;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_textAnim = GameObject.Find("TimerText").GetComponent<Animator>();
        _currTimeLeft = _baseTimeOfGame;
    }

    // Update is called once per frame
    void Update()
    {
        _currTimeLeft -= Time.deltaTime;

        if (_currTimeLeft <= 0)
            _currTimeLeft = 0;

        UpdtateTimer();

        DisplayTimeLeft();

        if(_currTimeLeft <= 30)
        {
            m_textAnim.enabled = true;
        }
        if (_currTimeLeft == 0)
        {
            m_textAnim.enabled = false;
        }
    }

    public static Timer Instance
    {
        get
        {
            return _instance;
        }

        set => _instance = value;
    }

    private void UpdtateTimer()
    {
        _minutesLeft = Mathf.FloorToInt(_currTimeLeft / 60f);
        _secondsLeft = Mathf.FloorToInt(_currTimeLeft % 60f);
    }

    private void DisplayTimeLeft()
    {
        string StringMinLeft = _minutesLeft.ToString();

        string StringSecLeft;

        if (_secondsLeft < 10)
        {
            StringSecLeft = "0" + _secondsLeft.ToString();
        }
        else
        {
            StringSecLeft = _secondsLeft.ToString();
        }

        _canvasTimer.text = StringMinLeft + ":" + StringSecLeft;
    }

    public int MinutesLeft { get => _minutesLeft; }

    public int SecondsLeft { get => _secondsLeft; }

    public float TimeLeft { get => _currTimeLeft; set => _currTimeLeft = value; }
    public float BaseTime { get => _baseTimeOfGame; }
}
