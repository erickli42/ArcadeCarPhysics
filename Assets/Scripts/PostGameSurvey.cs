﻿using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class PostGameSurvey : MonoBehaviour
{
    private String[] questions = {
        "How would you rate the overall quality of the game (i.e. graphics, smoothness, responsiveness, etc.)?",
        "How would you rate your personal, overall gameplay experience?",
        "How would you rate this game's graphical resolution?",
        "How would you rate this game's smoothness (affected by frames per second)?",
        "How would you rate this game's responsiveness to user input (aka your actions and controls)?",
        "How frustrated were you with the gameplay due to the game's quality/performance?"
    };
    int qi = 0;
    int prev_qi = -1;
    GameObject qtext;
    ArcadeCar car;
    public bool showing = true;
    public bool doneSurvey = false;
    ToggleGroup toggle_group;
    GameObject panel;
    public String surveyData = "";
    // Use this for initialization
    void Start()
    {
        qtext = GameObject.Find("PostSurveyQText");
        panel = GameObject.Find("Panel (Post-Game Survey)");
        car = GameObject.Find("Car").GetComponent<ArcadeCar>();
        car.postGameSurvey = this;
        toggle_group = GameObject.Find("PostSurveyToggleGroup").GetComponent<ToggleGroup>();
        hide();
    }

    public void hide()
    {
        showing = false;
        panel.SetActive(false);
    }

    public void show()
    {
        showing = true;
        panel.SetActive(true);
        prev_qi = -1;
        qi = 0;
        surveyData = "";
    }

    static String getIdButtonName(String btnName)
    {
        int pos = btnName.IndexOf('(');
        if (pos < 0)
        {
            return "";
        }
        int pos1 = btnName.IndexOf(')', pos);
        if (pos1 < 0)
        {
            pos1 = btnName.Length;
        }
        return btnName.Substring(pos + 1, pos1 - (pos + 1));
    }

    public void nextAction()
    {
        if (!toggle_group.AnyTogglesOn())
        {
            return;
        }
        bool first = true;
        String arrRecord = "";
        foreach (Toggle t in toggle_group.ActiveToggles())
        {
            String id = getIdButtonName(t.gameObject.name);
            if (arrRecord.Length > 0)
            {
                arrRecord += ", ";
            }
            arrRecord += "\"" + id.Replace("\"", "\\\"") + "\"";
        }
        String dataRecord = "\"Question_" + qi + "\": [" + arrRecord + "]";
        if (surveyData.Length > 0)
        {
            surveyData += ", ";
        }
        else
        {
            surveyData = "{";
        }
        surveyData += dataRecord;
        ++qi;
        if (qi >= questions.Length)
        {
            qi = 0;
            doneSurvey = true;
            surveyData += "}";
        } else
        {
            toggle_group.SetAllTogglesOff();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (prev_qi != qi)
        {
            if (qi < questions.Length && qi >= 0)
            {
                qtext.GetComponent<UnityEngine.UI.Text>().text = questions[qi];
            }
            prev_qi = qi;
        }
    }
}
