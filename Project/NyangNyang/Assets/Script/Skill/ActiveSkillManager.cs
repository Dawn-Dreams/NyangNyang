using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkillManager : MonoBehaviour
{

    private static ActiveSkillManager instance;
    public static ActiveSkillManager GetInstance() => instance;

    private void Awake()
    {
        if ( instance == null) { 
            instance = this;
        }
    }

    [Header("���� �ɰ���")]
    public GameObject UFOCat;
    public GameObject Beam;
    public AnimationManager UFOCatAnim;
    public float UFOmoveDuration = 0.5f; // �̵��� �ɸ��� �ð�
    public float BeamDuration = 0.5f; // ���� �����Ǵ� �ð�
    public Vector3 UFOstartPosition;  // ���� ��ġ
    public Vector3 UFOendPosition;    // �� ��ġ
    public float beamOffset = 1f;     // ���� ��ġ ������ ���� Y�� ������

    [Header("Ĺ�غ�")]
    public GameObject Leaf;
    public int numberOfLeaf = 20;
    public Vector2 spawnArea = new Vector2(10f, 5f); // ������ ���� ���� (����, ����)
    public float minFallSpeed = 1f; // �ּ� ���� �ӵ�
    public float maxFallSpeed = 3f; // �ִ� ���� �ӵ�
    public float minScale = 0.5f;   // �ּ� ũ��
    public float maxScale = 1.5f;   // �ִ� ũ��

    [Header("������ ����")]
    public GameObject HelpCat;
    public AnimationManager HelpCatAnim;
    public float HelpmoveDuration = 0.5f; // �̵��� �ɸ��� �ð�
    public Vector3 HelpstartPosition;  // ���� ��ġ
    public Vector3 HelpendPosition;    // �� ��ġ

    [Header("�ڸ�����")]
    public GameObject ScroogeCat;
    public AnimationManager ScroogeCatAnim;
    public float ScroogemoveDuration = 0.5f; // �̵��� �ɸ��� �ð�
    public Vector3 ScroogestartPosition;  // ���� ��ġ
    public Vector3 ScroogeendPosition;    // �� ��ġ

    [Header("��Ÿ�� ��ź")]
    public GameObject Skein;
    public Vector3 start;           // ��Ÿ���� ���� ��ǥ ��ġ
    public Vector3 target;           // ��Ÿ���� ���� ��ǥ ��ġ
    public float throwHeight = 5f;     // �������� �ְ� ����
    public float duration = 2f;        // �̵��� �ɸ��� �ð�


    public float SkillCoolTime = 10f;
    public bool isWaiting = false;
    public Slider CoolTime;

    public void CurSkillActivate(int id)
    {

        if ( !isWaiting )
        {
            switch (id)
            {
                case 0:
                    StartCoroutine(UFOCatSkill());
                    break;
                case 1:
                    StartCoroutine(SpawnLeaves());
                    break;
                case 2:
                    StartCoroutine(HelpMeSkill());
                    break;
                case 3:
                    StartCoroutine(ScroogeSkill());
                    break;
                case 4:
                    StartCoroutine(ThrowToTarget());
                    break;

            }

        }
    }

    IEnumerator HelpMeSkill()
    {

        HelpCat.SetActive(true);
        HelpCat.transform.position = HelpstartPosition;
        HelpCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < HelpmoveDuration)
        {
            // �̵� ���� ���
            float t = elapsedTime / HelpmoveDuration;

            // Ease-Out ������� �ӵ� ���� (t^2 ���)
            float easedT = Mathf.Pow(t, 2);

            // ����� ������Ʈ�� �������� �����Ͽ� �̵�
            HelpCat.transform.position = Vector3.Lerp(HelpstartPosition, HelpendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� ���� �� ��Ȯ�� ��ġ ����
        HelpCat.transform.position = HelpendPosition;


        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator ScroogeSkill()
    {

        ScroogeCat.SetActive(true);
        ScroogeCat.transform.position = ScroogestartPosition;
        ScroogeCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < ScroogemoveDuration)
        {
            // �̵� ���� ���
            float t = elapsedTime / ScroogemoveDuration;

            // Ease-Out ������� �ӵ� ���� (t^2 ���)
            float easedT = Mathf.Pow(t, 2);

            // ����� ������Ʈ�� �������� �����Ͽ� �̵�
            ScroogeCat.transform.position = Vector3.Lerp(ScroogestartPosition, ScroogeendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� ���� �� ��Ȯ�� ��ġ ����
        ScroogeCat.transform.position = ScroogeendPosition;

        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator UFOCatSkill()
    {

        UFOCat.SetActive(true);
        UFOCat.transform.position = UFOstartPosition;
        UFOCatAnim.PlayAnimation(AnimationManager.AnimationState.Run);

        float elapsedTime = 0f;

        while (elapsedTime < UFOmoveDuration)
        {
            // �̵� ���� ���
            float t = elapsedTime / UFOmoveDuration;

            // Ease-Out ������� �ӵ� ���� (t^2 ���)
            float easedT = Mathf.Pow(t, 2);

            // ����� ������Ʈ�� �������� �����Ͽ� �̵�
            UFOCat.transform.position = Vector3.Lerp(UFOstartPosition, UFOendPosition, easedT);

            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // �̵� ���� �� ��Ȯ�� ��ġ ����
        UFOCat.transform.position = UFOendPosition;

        yield return new WaitForSeconds(1f);

        // �� ���� �� ��ġ ����
        GameObject beam = Instantiate(Beam, new Vector3(UFOCat.transform.position.x, UFOCat.transform.position.y - 3f, 0), Quaternion.identity);

        // ���� �������� ���̵��� ���� ����
        // beam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // �� ���� �ð� ���� ���
        yield return new WaitForSeconds(BeamDuration);

        // �� ����
        Destroy(beam);
        UFOCat.SetActive(false);

        StartCoroutine(WaitForCoolTime());

    }

    private IEnumerator SpawnLeaves()
    {
        for (int i = 0; i < numberOfLeaf; i++)
        {
            // ������ ����
            GameObject leaf = Instantiate(Leaf);

            // ���� ��ġ ���� (���� ���� ��ġ + ���� ����)
            float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
            float startY = spawnArea.y;
            leaf.transform.position = new Vector2(randomX, startY);

            // ũ�� ����
            float randomScale = Random.Range(minScale, maxScale);
            leaf.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            // ���� �ڷ�ƾ ����
            float fallSpeed = Random.Range(minFallSpeed, maxFallSpeed);
            StartCoroutine(FallLeaf(leaf, fallSpeed));

            // ������ ���� ����
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(WaitForCoolTime());
    }

    private IEnumerator FallLeaf(GameObject leaf, float speed)
    {
        while (leaf.transform.position.y > -spawnArea.y / 2f)
        {
            // ����
            leaf.transform.position += Vector3.down * speed * Time.deltaTime;

            // ��¦ ��鸲 ȿ�� �߰� (Optional)
            float oscillation = Mathf.Sin(Time.time * speed) * 0.1f;
            leaf.transform.position += Vector3.right * oscillation;

            yield return null;
        }

        // �ٴڿ� ���� ������ ����
        Destroy(leaf);
    }

    private IEnumerator ThrowToTarget()
    {
        GameObject skein = Instantiate(Skein);

        Vector3 startPosition = start;  // ���� ��ġ
        Vector3 targetPosition = target;    // ��ǥ ��ġ

        float elapsedTime = 0f;  // ��� �ð� �ʱ�ȭ

        while (elapsedTime < duration)
        {
            // t�� ���� ���� ���� (0���� 1����)
            float t = elapsedTime / duration;

            // XZ ��鿡�� ���� ����
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Y ��ǥ�� ������ ȿ�� �߰�
            float height = Mathf.Sin(t * Mathf.PI) * throwHeight; // ������ ����
            currentPosition.y += height;

            // ��Ÿ�� ��ġ ����
            skein.transform.position = currentPosition;

            // �ð� ����
            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // ��ǥ ��ġ�� ����
        Skein.transform.position = targetPosition;
        Debug.Log("���� �Ϸ�!");

        yield return new WaitForSeconds(1f);

        Destroy(skein);

        StartCoroutine(WaitForCoolTime());
    }

    IEnumerator WaitForCoolTime()
    {
        float elapsedTime = 0f;
        
        isWaiting = true;

        while (elapsedTime < SkillCoolTime)
        {
            CoolTime.value = elapsedTime / SkillCoolTime;
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        isWaiting = false;
    }

    public void SetSkillCoolTime(float _time)
    {
        SkillCoolTime *= (_time + 1f);
    }

    public void ResetSkillCoolTime()
    {
        SkillCoolTime = 1f;
    }
}
