using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniGamePanel : MonoBehaviour
{
    public GameObject toggleParentObject;   // Toggle���� ��� �ִ� �θ� ������Ʈ
    public GameObject panelParentObject;    // �гε��� ��� �ִ� �θ� ������Ʈ

    private Toggle[] toggles;               // �������� ã�� Toggle���� ������ �迭
    private GameObject[] panels;            // �������� ã�� �гε��� ������ �迭

    private void Start()
    {
        FindTogglesAndPanels();

        // �� ��ۿ� ������ �߰�
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    OpenPanel(index);
                }
            });
        }

        // ��� �г� ��Ȱ��ȭ �� ù ��° �г� Ȱ��ȭ
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        if (toggles.Length > 0 && panels.Length > 0)
        {
            toggles[0].isOn = true;       // ù ��° ��� Ȱ��ȭ
            panels[0].SetActive(true);    // ù ��° �г� Ȱ��ȭ
        }
        Debug.Log("�̴ϰ��� Start()");
    }

    void FindTogglesAndPanels()
    {
        toggles = toggleParentObject.GetComponentsInChildren<Toggle>();
        panels = new GameObject[panelParentObject.transform.childCount];

        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = panelParentObject.transform.GetChild(i).gameObject;
        }
    }

    // ������ �гθ� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
    void OpenPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);  // ���õ� �гθ� Ȱ��ȭ, �������� ��Ȱ��ȭ
        }
    }

    // �̴ϰ��� ���� ��ư Ŭ�� �� ����
    public void OnClickMinigameButton(int index)
    {
        // ����� ���������� ���� ���̸� �̴ϰ����� �������� ����
        if (GameManager.isDungeonActive)
        {
            Debug.Log("����� ���������� ���� ���̹Ƿ� �̴ϰ����� ������ �� �����ϴ�.");
            return;
        }

        // �̴ϰ����� �̹� ���� ������ Ȯ��
        if (GameManager.isMiniGameActive)
        {
            Debug.Log("�ٸ� �̴ϰ����� �̹� ���� ���Դϴ�.");
            return;
        }


        switch (index)
        {
            case 0:
                SceneManager.LoadScene("MiniGame1", LoadSceneMode.Additive);
                Debug.Log("�̴ϰ��� 1 ���۹�ưŬ��");
                GameManager.isMiniGameActive = true;
                break;
            case 1:
                // FindObjectOfType<MiniGame2>().StartGame();
                Debug.Log("�̴ϰ��� 2 ���۹�ưŬ��");
                break;
            case 2:
                // FindObjectOfType<MiniGame3>().StartGame();
                Debug.Log("�̴ϰ��� 3 ���۹�ưŬ��");
                break;
            default:
                Debug.LogWarning("�ùٸ��� ���� �ε����Դϴ�.");
                break;
        }
    }
}
