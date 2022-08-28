using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MouseInputDisplayer : MonoBehaviour
    {
        [SerializeField] private Image baseImage;

        [SerializeField] private GameObject lmbGameObject;
        [SerializeField] private Image lmbImage;
        [Space]
        [SerializeField] private GameObject rmbGameObject;
        [SerializeField] private Image rmbImage;
        [Space]
        [SerializeField] private GameObject mmbGameObject;
        [SerializeField] private Image mmbImage;
        [Space]
        [SerializeField] private GameObject mb4GameObject;
        [SerializeField] private Image mb4Image;
        [Space]
        [SerializeField] private GameObject mb5GameObject;
        [SerializeField] private Image mb5Image;
        [Space]
        [SerializeField] private GameObject wheelUpDownGameObject;

        public void Setup(MouseButtonConfig mouseButtonConfig)
        {
            lmbGameObject.SetActive(mouseButtonConfig.lmbIsActive);
            rmbGameObject.SetActive(mouseButtonConfig.rmbIsActive);

            mmbGameObject.SetActive(mouseButtonConfig.mmbIsActive);

            mb4GameObject.SetActive(mouseButtonConfig.mb4IsActive);
            mb5GameObject.SetActive(mouseButtonConfig.mb5IsActive);

            wheelUpDownGameObject.SetActive(mouseButtonConfig.wheelUpDownIsActive);
        }

        public void SetColor(Color color)
        {
            baseImage.color = color;
            lmbImage.color = color;
            rmbImage.color = color;
            mmbImage.color = color;
            mb4Image.color = color;
            mb5Image.color = color;
        }
    }
}
