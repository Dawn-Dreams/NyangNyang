using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
    public class Popup : MonoBehaviour
    {
        public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);

        private GameObject m_background;

        public void Open()
        {
            AddBackground();
            gameObject.SetActive(true); // 팝업 활성화
        }

        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            if (m_background != null)
            {
                RemoveBackground();
            }
            else
            {
                Debug.LogWarning("Background was not found during Close.");
            }

            StartCoroutine(RunPopupDeactivate());
        }

        // 팝업 비활성화 처리 (비파괴)
        private IEnumerator RunPopupDeactivate()
        {
            yield return new WaitForSeconds(0.5f); // 닫기 애니메이션 후 대기 시간
            if (m_background != null)
            {
                m_background.SetActive(false); // 배경 비활성화
            }
            gameObject.SetActive(false); // 팝업 비활성화
        }

        private void AddBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();

            m_background = new GameObject("PopupBackground");
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material = new Material(image.material);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            image.canvasRenderer.SetAlpha(0.0f);
            image.CrossFadeAlpha(1.0f, 0.4f, false);

            var canvas = GetComponentInParent<Canvas>();
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        private void RemoveBackground()
        {
            var image = m_background.GetComponent<Image>();
            if (image != null)
            {
                image.CrossFadeAlpha(0.0f, 0.2f, false);
            }
        }
    }
}
