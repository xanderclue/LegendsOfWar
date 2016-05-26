using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class KeyboardMenuNavigate : MonoBehaviour
{

    public GameObject startButton = null;
    public GameObject tutorialButton = null;
    public GameObject optionsButton = null;
    public GameObject creditButton = null;
    public GameObject exitButton = null;

    public Sprite buttonOut = null;
    public Sprite buttonOver = null;
    public Sprite buttonPushed = null;
    
    private bool mouseUse = false;
    enum CurrentMenu { START, TUTORIAL, OPTIONS, CREDITS, EXIT };
    private CurrentMenu _currentmenu;


    // Use this for initialization
    void Start()
    {
        _currentmenu = CurrentMenu.START;

    }


    //void StopClear(GameObject ha)
    //{
    //    ha.GetComponent<HoverEvent>().button.Stop();
    //    ha.GetComponent<HoverEvent>().button.Clear();
    //}

    // Update is called once per frame
    void Update()
    {
        switch (_currentmenu)
        {
            case CurrentMenu.START:

                if (_currentmenu == CurrentMenu.START)
                {
                    startButton.GetComponent<Image>().sprite = buttonOver;
                    if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                    {
                        startButton.GetComponent<Image>().sprite = buttonPushed;
                        ApplicationManager.Instance.ChangeAppState(StateID.STATE_SELECTION);
                        FindObjectOfType<menuEvents>().PlayClickSound();
                    }
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        startButton.GetComponent<Image>().sprite = buttonOut;
                        _currentmenu = CurrentMenu.TUTORIAL;
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        startButton.GetComponent<Image>().sprite = buttonOut;
                        _currentmenu = CurrentMenu.EXIT;
                    }
                }
                break;
            case CurrentMenu.TUTORIAL:
                tutorialButton.GetComponent<Image>().sprite = buttonOver;
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                {
                    tutorialButton.GetComponent<Image>().sprite = buttonPushed;
                    ApplicationManager.Instance.ChangeAppState(StateID.STATE_INTRODUCTION);
                    FindObjectOfType<menuEvents>().PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    tutorialButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.OPTIONS;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    tutorialButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.START;
                }
                break;
            case CurrentMenu.OPTIONS:
                optionsButton.GetComponent<Image>().sprite = buttonOver;
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                {
                    optionsButton.GetComponent<Image>().sprite = buttonPushed;
                    ApplicationManager.Instance.ChangeAppState(StateID.STATE_OPTIONS_MENU);
                    FindObjectOfType<menuEvents>().PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    optionsButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.CREDITS;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    optionsButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.TUTORIAL;
                }
                break;

            case CurrentMenu.CREDITS:
                creditButton.GetComponent<Image>().sprite = buttonOver;
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                {
                    creditButton.GetComponent<Image>().sprite = buttonPushed;
                    ApplicationManager.Instance.ChangeAppState(StateID.STATE_CREDITS);
                    FindObjectOfType<menuEvents>().PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    creditButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.EXIT;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    creditButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.OPTIONS;
                }
                break;
            case CurrentMenu.EXIT:
                exitButton.GetComponent<Image>().sprite = buttonOver;
                if (Input.GetKeyDown("return") || Input.GetKeyDown("enter"))
                {
                    exitButton.GetComponent<Image>().sprite = buttonPushed;
                    ApplicationManager.Instance.ChangeAppState(StateID.STATE_EXIT);
                    FindObjectOfType<menuEvents>().PlayClickSound();
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    exitButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.START;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    exitButton.GetComponent<Image>().sprite = buttonOut;
                    _currentmenu = CurrentMenu.CREDITS;
                }
                break;
        }

        if (startButton.GetComponent<HoverEvent>().OnOff || exitButton.GetComponent<HoverEvent>().OnOff || tutorialButton.GetComponent<HoverEvent>().OnOff
            || creditButton.GetComponent<HoverEvent>().OnOff || optionsButton.GetComponent<HoverEvent>().OnOff)
        {
            startButton.GetComponent<Image>().sprite = buttonOut;
            tutorialButton.GetComponent<Image>().sprite = buttonOut;
            optionsButton.GetComponent<Image>().sprite = buttonOut;
            creditButton.GetComponent<Image>().sprite = buttonOut;
            exitButton.GetComponent<Image>().sprite = buttonOut;

            //StopClear(startButton);
            //StopClear(tutorialButton);
            //StopClear(optionsButton);
            //StopClear(creditButton);
            //StopClear(exitButton);
            mouseUse = true;
        }
        if (mouseUse)
        {
            MouseUse();
            mouseUse = false;
        }
    }

    void MouseUse()
    {
        if (startButton.GetComponent<HoverEvent>().OnOff)
        {
            _currentmenu = CurrentMenu.START;
            startButton.GetComponent<Image>().sprite = buttonOver;
            //startButton.GetComponent<HoverEvent>().button.Play();
            startButton.GetComponent<HoverEvent>().OnOff = false;
        }
        else if (tutorialButton.GetComponent<HoverEvent>().OnOff)
        {
            _currentmenu = CurrentMenu.TUTORIAL;
            tutorialButton.GetComponent<Image>().sprite = buttonOver;
            //tutorialButton.GetComponent<HoverEvent>().button.Play();
            tutorialButton.GetComponent<HoverEvent>().OnOff = false;
        }
        else if (optionsButton.GetComponent<HoverEvent>().OnOff)
        {
            _currentmenu = CurrentMenu.OPTIONS;
            optionsButton.GetComponent<Image>().sprite = buttonOver;
            //optionsButton.GetComponent<HoverEvent>().button.Play();
            optionsButton.GetComponent<HoverEvent>().OnOff = false;
        }
        else if (creditButton.GetComponent<HoverEvent>().OnOff)
        {
            _currentmenu = CurrentMenu.CREDITS;
            creditButton.GetComponent<Image>().sprite = buttonOver;
            //creditButton.GetComponent<HoverEvent>().button.Play();
            creditButton.GetComponent<HoverEvent>().OnOff = false;

        }
        else if (exitButton.GetComponent<HoverEvent>().OnOff)
        {
            _currentmenu = CurrentMenu.EXIT;
            exitButton.GetComponent<Image>().sprite = buttonOver;
            //exitButton.GetComponent<HoverEvent>().button.Play();
            exitButton.GetComponent<HoverEvent>().OnOff = false;
        }


    }
}
