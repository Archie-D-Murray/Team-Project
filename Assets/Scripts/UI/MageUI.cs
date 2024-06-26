using System.Linq;

using Attack;

using Items;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using System;
using Tags.UI.Class;
using Entity.Player;
using Tags.UI.Item;

public class MageUI : MonoBehaviour {
    private Inventory inventory;
    private MageSystem mageSystem;
    private Image[] spellHotBarSlots;
    private PlayerController playerController;
    private CanvasGroup canvas;

    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private SpellData spellSelection = null;
    [SerializeField] private Sprite noSpellIcon;

    private void OnEnable() {
        canvas = GetComponentInChildren<CanvasGroup>();
        playerController = FindFirstObjectByType<Entity.Player.PlayerController>();
        inventory = playerController.GetComponent<Inventory>();
        mageSystem = playerController.getAttackSystem as MageSystem;
        spellHotBarSlots = FindFirstObjectByType<SpellHotbar>().GetComponentsInChildren<Image>().Where((Image image) => image.gameObject.HasComponent<SpellSlot>()).ToArray();
        Array.ForEach(spellHotBarSlots, (Image image) => image.sprite = noSpellIcon);
        Utilities.Input.instance.playerControls.UI.SpellMenu.started += (InputAction.CallbackContext context) => Show();
        Utilities.Input.instance.playerControls.UI.Cancel.started += (InputAction.CallbackContext context) => Hide();
        if (playerController.getAttackSystem is not MageSystem) {
            gameObject.SetActive(false);
            return;
        }
    }

    private void FixedUpdate() {
        foreach (Image spellSlot in spellHotBarSlots) {
            spellSlot.fillAmount = mageSystem.CooldownProgress;
        }
    }

    public void SetSlot(int index, SpellData spell) {
        foreach (Image image in spellHotBarSlots) {
            if (image.sprite == spellSelection.sprite) {
                image.sprite = noSpellIcon;
            }
        }
        spellHotBarSlots[index].sprite = spell.sprite;
        spellHotBarSlots[index].color = Color.white;
        if (mageSystem == null) {
            Debug.LogError("Player not intialised for mage state!");
            enabled = false;
            return;
        }
        
        mageSystem.SetSpell(index, spell);
    }

    public void Show() {
        UILock.instance.OpenUI();
        for (int i = 0; i < mageSystem.GetSpells().Length; i++) {
            spellHotBarSlots[i].sprite = mageSystem.GetSpells()[i] ? mageSystem.GetSpells()[i].icon : noSpellIcon;
        }
        spellSelection = null;
        foreach (SpellData spell in inventory.spells) {
            GameObject spellInstance = Instantiate(spellPrefab, canvas.transform);
            spellInstance.GetComponentInChildren<Button>().onClick.AddListener(() => SetSpellSelection(spell));
            spellInstance.GetComponentInChildren<TMP_Text>().text = spell.itemName;
            spellInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<ItemSprite>()).sprite = spell.sprite;
            spellInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<ItemIcon>()).sprite = spell.icon;

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
        UILock.instance.CloseUI();
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