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
    private MageSystem mageSystem;
    private Image[] spellHotBarSlots;
    private CanvasGroup canvas;

    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private SpellData spellSelection = null;

    private void Start() {
        canvas = GetComponent<CanvasGroup>();
        Entity.Player.PlayerController playerController = FindFirstObjectByType<Entity.Player.PlayerController>();
        inventory = playerController.GetComponent<Inventory>();
        if (playerController.getAttackSystem is not MageSystem) {
            Debug.LogError("Player is not currently a mage!");
            enabled = false;
            return;
        }
        mageSystem = playerController.getAttackSystem as MageSystem;
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

    private void FixedUpdate() {
        foreach (Image spellSlot in spellHotBarSlots) {
            spellSlot.fillAmount = mageSystem.CooldownProgress;
        }
    }

    public void SetSlot(int index, SpellData spell) {
        foreach (Image image in spellHotBarSlots) {
            if (image.sprite == spellSelection.icon) {
                image.color = Color.clear;
            }
        }
        spellHotBarSlots[index].sprite = spell.icon;
        spellHotBarSlots[index].color = Color.white;
        if (mageSystem == null) {
            Debug.LogError("Player not intialised for mage state!");
            enabled = false;
            return;
        }
        
        mageSystem.SetSpell(index, spell);
    }

    public void Show() {
        spellSelection = null;
        foreach (SpellData spell in inventory.spells) {
            GameObject spellInstance = Instantiate(spellPrefab, canvas.transform);
            spellInstance.GetComponentInChildren<Button>().onClick.AddListener(() => SetSpellSelection(spell));
            spellInstance.GetComponentInChildren<TMP_Text>().text = spell.itemName;
            spellInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<SpellSlot>()).sprite = spell.icon;
            spellHotBarSlots[Array.IndexOf(inventory.spells, spell)].sprite = spell.icon;
        }
        canvas.FadeCanvas(0.1f, false, this);
    }

    private void SetSpellSelection(SpellData spell) {
        spellSelection = spell;
    }

    public void Hide() {
        spellSelection = null;
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