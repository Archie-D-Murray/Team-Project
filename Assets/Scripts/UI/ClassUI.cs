using Entity.Player;

using UnityEngine;

namespace UI {
    public class ClassUI : MonoBehaviour {
        [SerializeField] private GameObject mageUI, meleeUI, rangedUI;

        private void Start() {
            meleeUI = GetComponentInChildren<MeleeUI>(true).gameObject;
            rangedUI = GetComponentInChildren<RangedUI>(true).gameObject;
            mageUI = GetComponentInChildren<MageUI>(true).gameObject;
            PlayerController playerController = FindFirstObjectByType<PlayerController>();
            ActivateUI(playerController.getPlayerClass);
            playerController.onClassChange += ActivateUI;
        }

        public void ActivateUI(PlayerClass type) {
            switch (type) {
                case PlayerClass.MELEE:
                    mageUI.SetActive(false);
                    rangedUI.SetActive(false);
                    meleeUI.SetActive(true);
                    break;
                case PlayerClass.RANGED:
                    meleeUI.SetActive(false);
                    mageUI.SetActive(false);
                    rangedUI.SetActive(true);
                    break;
                case PlayerClass.MAGE:
                    meleeUI.SetActive(false);
                    rangedUI.SetActive(false);
                    mageUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}