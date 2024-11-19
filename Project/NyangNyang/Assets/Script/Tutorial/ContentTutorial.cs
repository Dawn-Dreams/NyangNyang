using System;
using System.Collections;
using System.Collections.Generic;
using Ricimi;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialStep
{
    public Vector2 xMaskPos;
    public Vector2 yMaskPos;
    public string tutorialText;

    public Vector2 GetXMask()
    {
        Vector2 retVec2 = new Vector2
        {
            [0] = xMaskPos[0] / 900.0f,
            [1] = (900.0f - xMaskPos[1]) / 900.0f
        };
        return retVec2;
    }

    public Vector2 GetYMask()
    {
        float currentScreenHeight = (float)Screen.height / Screen.width * 900; 
        Vector2 retVec2 = new Vector2
        {
            [0] = (yMaskPos[0]) / currentScreenHeight,
            [1] = (currentScreenHeight - yMaskPos[1]) / currentScreenHeight
        };
        return retVec2;
    }
}

public class TutorialMaskObject
{
    public GameObject obj = null;
    public Image tutorialMaskImage = null;
    public Material tutorialMaskMaterial = null;
    public Button tutorialNextButton = null;
    public Button tutorialSkipButton = null;
    public TextMeshProUGUI tutorialTextMeshPro = null;
    public TutorialMaskObject Load(Image getObj)
    {
        obj = getObj.gameObject;
        tutorialMaskImage = getObj;
        tutorialMaskMaterial = tutorialMaskImage.material;
        tutorialNextButton = obj.transform.GetChild(1).GetComponent<Button>();
        tutorialSkipButton = obj.transform.GetChild(0).GetComponent<Button>();
        tutorialTextMeshPro = obj.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>(true);
        return this;
    }
}

public class ContentTutorial : MonoBehaviour
{
    public static TutorialMaskObject tutorialMaskObject = null;

    public List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    private int _currentStep = 0;
    public Image tutorialMaskImage;

    void Awake()
    {
        LoadTutorialMaskObj();
    }

    void LoadTutorialMaskObj()
    {
        if (tutorialMaskImage != null && tutorialMaskObject == null)
        {
            tutorialMaskObject = new TutorialMaskObject().Load(tutorialMaskImage);
        }
    }

    public void OpenTutorialMaskObject()
    {
        LoadTutorialMaskObj();

        _currentStep = 0;
        tutorialMaskObject.obj.SetActive(true);

        tutorialMaskObject.tutorialSkipButton.onClick.AddListener(SkipTutorial);
        tutorialMaskObject.tutorialNextButton.onClick.AddListener(NextTutorial);

        ShowTutorialStep();
    }
    void NextTutorial()
    {
        ++_currentStep;
        ShowTutorialStep();
    }
    void SkipTutorial()
    {
        _currentStep = tutorialSteps.Count;
        ShowTutorialStep();
    }

    void ShowTutorialStep()
    {
        if (tutorialSteps.Count <= _currentStep)
        {
            tutorialMaskObject.tutorialNextButton.onClick.RemoveAllListeners();
            tutorialMaskObject.tutorialSkipButton.onClick.RemoveAllListeners();
            tutorialMaskObject.obj.SetActive(false);
            return;
        }

        tutorialMaskObject.obj.SetActive(true);
        tutorialMaskObject.tutorialMaskMaterial.SetVector("_MaskX", tutorialSteps[_currentStep].GetXMask());
        tutorialMaskObject.tutorialMaskMaterial.SetVector("_MaskY", tutorialSteps[_currentStep].GetYMask());

        string tutorialText = tutorialSteps[_currentStep].tutorialText;
        if (tutorialText != "")
        {
            tutorialMaskObject.tutorialTextMeshPro.gameObject.SetActive(true);
            tutorialMaskObject.tutorialTextMeshPro.text = tutorialText;
        }
        else
        {
            tutorialMaskObject.tutorialTextMeshPro.gameObject.SetActive(false);
        }
    }

}
