using System.Linq;

using Attack;

using Items;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using System;
using Tags.UI.Class;

public class MageUI : MonoBehaviour {
    private Inventory inventory;
    private Entity.Player.PlayerController playerController;
    private Image[] spellHotBarSlots;
    private CanvasGroup canvas;

    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private SpellData spellSelection = null;

    private void Start() {
        canvas = GetComponent<CanvasGroup>();
        playerController = FindFirstObjectByType<Entity.Player.PlayerController>();
        inventory = playerController.GetComponent<Inventory>();
        spellHotBarSlots = FindFirstObjectByType<SpellHotbar>().GetComponentsInChildren<Image>().Where((Image image) => image.gameObject.HasComponent<SpellSlot>()).ToArray();
        Utilities.Input.instance.playerControls.UI.SpellMenu.started += (InputAction.CallbackContext context) => Toggle();
    }

    private void Toggle() {
        if (canvas.alpha == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    public void SetSlot(int index, SpellData spell) {
        spellHotBarSlots[index].sprite = spell.icon;
        spellHotBarSlots[index].color = Color.white;
        if (playerController.getAttackSystem == null || playerController.getAttackSystem is not MageSystem) {
            Debug.LogError("Player not intialised for mage state!");
            return;
        }
        (playerController.getAttackSystem as MageSystem).SetSpell(index, spell);
        spellSelection = null;
    }

    public void Show() {
        spellSelection = null;
        foreach (SpellData spell in inventory.spells) {
            GameObject spellInstance = Instantiate(spellPrefab, canvas.transform);
            spellInstance.GetComponentInChildren<Button>().onClick.AddListener(() => SetSpellSelection(spell));
            spellInstance.GetComponentInChildren<TMP_Text>().text = spell.itemName;
            spellInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<SpellSlot>()).sprite = spell.icon;
        }
        canvas.FadeCanvas(0.1f, false, this);
    }

    private void SetSpellSelection(SpellData spell) {
        spellSelection = spell;
    }

    public void Hide() {
        canvas.FadeCanvas(0.1f, true, this);
        foreach (Transform child in canvas.transform) {
            if (!child.gameObject.HasComponent<Image>()) {
                continue;
            }
            Destroy(child.gameObject);
        }
    }

    private void Update() {
        if (spellSelection == null) {
            return;
        }
        if (Utilities.Input.instance.playerControls.UI.BindSpellOne.IsPressed()) {
            Debug.Log($"Set slot 0 to {spellSelection.itemName}");
            SetSlot(0, spellSelection);
        }
        if (Utilities.Input.instance.playerControls.UI.BindSpellTwo.IsPressed()) {
            Debug.Log($"Set slot 1 to {spellSelection.itemName}");
            SetSlot(1, spellSelection);
        }
        if (Utilities.Input.instance.playerControls.UI.BindSpellThree.IsPressed()) {
            Debug.Log($"Set slot 2 to {spellSelection.itemName}");
            SetSlot(2, spellSelection);
        }
    }
}