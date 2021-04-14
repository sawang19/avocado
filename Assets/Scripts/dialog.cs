using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Subtegral.DialogueSystem.DataContainers;

namespace Subtegral.DialogueSystem.Runtime
{
    public class Dialog : MonoBehaviour
    {
        public GameObject dialogBox;
        [SerializeField] private DialogueContainer dialogue;
        [SerializeField] private Text dialogueText;
        [SerializeField] private Button choicePrefab;
        [SerializeField] private Transform buttonContainer;
        private int POTION_HP = 1;
        private int POTION_RANDOM = 2;
        private int SHIELD = 3;
        private int ROCKET = 4;
        private int BOOTS = 5;

        public PlayerMovement playerMovement;

        private void Start()
        {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
        }
        public void SetToStart()
        {
            Start();
        }

        private void ProceedToNarrative(string narrativeDataGUID)
        {
            var text = dialogue.DialogueNodeData.Find(x => x.NodeGUID == narrativeDataGUID).DialogueText;
            var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGUID == narrativeDataGUID);
            dialogueText.text = ProcessProperties(text);
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
            int isItem = GetItemNumber(dialogueText.text);

            float xPos = buttonContainer.position.x;
            buttonContainer.position = new Vector2(xPos, 0);

            foreach (var choice in choices)
            {
                var button = Instantiate(choicePrefab, buttonContainer);
                button.GetComponentInChildren<Text>().text = ProcessProperties(choice.PortName);
                if (isItem == 0 || choice.PortName == "no")
                {
                    button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGUID));
                }else if (isItem == POTION_HP)
                {
                    button.onClick.AddListener(() => ProceedToTrading(POTION_HP));

                }
                else if (isItem == POTION_RANDOM)
                {
                    button.onClick.AddListener(() => ProceedToTrading(POTION_RANDOM));
                }
                else if (isItem == SHIELD)
                {
                    button.onClick.AddListener(() => ProceedToTrading(SHIELD));
                }
                else if (isItem == ROCKET)
                {
                    button.onClick.AddListener(() => ProceedToTrading(ROCKET));
                }else if (isItem == BOOTS)
                {
                    button.onClick.AddListener(() => ProceedToTrading(BOOTS));
                }
                

            }

            
        }
        private void ProceedToTrading(int item)
        {
            var buttons = buttonContainer.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                Destroy(buttons[i].gameObject);
            }
            bool enoughCoins = FindObjectOfType<PlayerMovement>().BuyItem(item);
            if (enoughCoins == false)
            {
                dialogueText.text = "You don't have enough money";
                var button = Instantiate(choicePrefab, buttonContainer);
                button.GetComponentInChildren<Text>().text = "BACK";
                button.onClick.AddListener(() => ProceedToNarrative("9b8aff7e-294d-4369-88c1-916a9454dc84"));
            }else
            {
                dialogueText.text = "Bought Successfully";
                var button = Instantiate(choicePrefab, buttonContainer);
                button.GetComponentInChildren<Text>().text = "BACK";
                button.onClick.AddListener(() => ProceedToNarrative("9b8aff7e-294d-4369-88c1-916a9454dc84"));
            }
        }
        private int GetItemNumber(string title)
        {
            if (title == "Do you wanna buy HP Potion?")
            {
                return POTION_HP;

            }
            else if (title == "Do you wanna buy Random Potion?")
            {
                return POTION_RANDOM;

            }
            else if (title == "Do you wanna buy Shield?")
            {
                return SHIELD;
            }else if (title == "Do you wanna buy Rocket?")
            {
                return ROCKET;
            }else if (title == "Do you wanna buy Boots?")
            {
                return BOOTS;
            }
            return 0;
        }

        private string ProcessProperties(string text)
        {
            foreach (var exposedProperty in dialogue.ExposedProperties)
            {
                text = text.Replace($"[{exposedProperty.PropertyName}]", exposedProperty.PropertyValue);
            }
            return text;
        }

        public void closeDialogue()
        {
            // if (dialogBox.activeInHierarchy) 
            // {
            var narrativeData = dialogue.NodeLinks.First(); //Entrypoint node
            ProceedToNarrative(narrativeData.TargetNodeGUID);
            dialogBox.SetActive(false);
            Time.timeScale = 1;
            // }
        }
    }
}