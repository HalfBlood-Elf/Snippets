using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI.Ingame.Hud.EnvironmentInteraction;

namespace UI
{
    [System.Serializable]
    public class InputButtonDisplayConfig
    {
        public bool isBorderActive = true;

        public bool isImageUsed;
        public Sprite image;

        public string text;

        public bool isMouseConfigUsed;
        public MouseButtonConfig mouseButtonConfig;

    }

    [System.Serializable]
    public class MouseButtonConfig
    {
        public bool lmbIsActive;
        public bool rmbIsActive;

        public bool mmbIsActive;

        public bool mb4IsActive;
        public bool mb5IsActive;

        public bool wheelUpDownIsActive;
    }


    public class InputButtonDisplayer : MonoBehaviour, IEnvironmentInteractionCombinationItem
    {
        [SerializeField] private GameObject imageHolder;
        [SerializeField] private Image image;

        [SerializeField] private GameObject buttonTextHolder;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Image borderImage;
        [SerializeField] private LayoutGroup layoutGroup;
        [SerializeField] private MouseInputDisplayer mouseInputDisplayer;
        [SerializeField] private Color startColor = Color.white;

        public void Setup(InputButtonDisplayConfig buttonConfig)
        {
            if(buttonConfig is null)
            {
                borderImage.gameObject.SetActive(false);
                imageHolder.SetActive(false);
                buttonTextHolder.SetActive(false);
                mouseInputDisplayer.gameObject.SetActive(false);
                return;
            }

            borderImage.gameObject.SetActive(buttonConfig.isBorderActive);
            imageHolder.SetActive(buttonConfig.isImageUsed);
            buttonTextHolder.SetActive(!buttonConfig.isImageUsed && !buttonConfig.isMouseConfigUsed);
            mouseInputDisplayer.gameObject.SetActive(buttonConfig.isMouseConfigUsed);
            if (buttonConfig.isImageUsed)
            {
                image.sprite = buttonConfig.image;
            }
            else if (buttonConfig.isMouseConfigUsed)
            {
                mouseInputDisplayer.Setup(buttonConfig.mouseButtonConfig);
            }
            else
            {
                buttonText.text = buttonConfig.text;
            }
            SetColor(startColor);

            if(gameObject.activeInHierarchy)
                CoroutineWorker.Instance.StartCoroutine(RebuildLayout());
        }

        public void SetColor(Color color)
        {
            mouseInputDisplayer.SetColor(color);
            image.color = color;
            buttonText.color = color;
            borderImage.color = color;
        }

        private IEnumerator RebuildLayout()
        {
            yield return null;
            layoutGroup.CalculateLayoutInputHorizontal();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}
