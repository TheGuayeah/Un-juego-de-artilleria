using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PEC3
{
    public class GameplayManager : MonoBehaviour
    {
        public EnemyController enemy;
        public PlayerControl hero;
        public GameObject startButton;
        public CameraFollow cameraFollow;
        public GameObject UIRoot;

        public Text mainText;
        public Text mainTextShadow;

        private int timeRemaining = 10;
        private GameObject[] ui;

        private void Start()
        {
            enemy.GetComponentInChildren<Gun>().gunFired += SwapTurn;
            hero.GetComponentInChildren<Gun>().gunFired += SwapTurn;

            UIRoot.SetActive(false);
        }

        public void StartGame()
        {
            startButton.SetActive(false);
            enemy.HasTurn = true;
            InvokeRepeating(nameof(DecreaseTime), 0, 1);
            SwapTurn();
        }

        private void SwapTurn()
        {
            StartCoroutine(SwapTurnCoroutine());
        }

        private IEnumerator SwapTurnCoroutine()
        {
            timeRemaining = 10;
            hero.HasTurn = !hero.HasTurn;

            cameraFollow.SetPlayerToFollow(hero.HasTurn ? hero.transform : enemy.transform);

            if (!enemy.HasTurn)
            {
                yield return new WaitForSeconds(2.0f);
            }

            enemy.HasTurn = !enemy.HasTurn;
        }

        private void DecreaseTime()
        {
            timeRemaining--;
            if (timeRemaining < 0)
            {
                SwapTurn();
            }

            mainText.text = mainTextShadow.text = "" + timeRemaining;
        }
    }

}