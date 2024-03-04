using System.Linq;

using Attack;

using Items;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using System;

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
        spellHotBarSlots = GetComponentsInChildren<Image>().Where((Image image) => image.gameObject.HasComponent<SpellSlot>()).ToArray();
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
        spellHotBarSlots[index].sprite = spell.icon.ToSprite();
        if (playerController.getAttackSystem == null || playerController.getAttackSystem is not MageSystem) {
            Debug.LogError("Player not intialised for mage state!");
            return;
        }
        (playerController.getAttackSystem as MageSystem).SetSpell(index, spell);
    }

    public void Show() {
        spellSelection = null;
        foreach (SpellData spell in inventory.spells) {
            GameObject spellInstance = Instantiate(spellPrefab, canvas.transform);
            spellInstance.GetComponentInChildren<Button>().onClick.AddListener(() => SetSpellSelection(spell));
            spellInstance.GetComponentInChildren<TMP_Text>().text = spell.itemName;
            spellInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<SpellSlot>()).sprite = spell.icon.ToSprite();
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
            SetSlot(0, spellSelection);
            spellSelection = null;
        }
        if (Utilities.Input.instance.playerControls.UI.BindSpellTwo.IsPressed()) {
            SetSlot(1, spellSelection);
            spellSelection = null;
        }
        if (Utilities.Input.instance.playerControls.UI.BindSpellThree.IsPressed()) {
            SetSlot(2, spellSelection);
            spellSelection = null;
        }
    }
}