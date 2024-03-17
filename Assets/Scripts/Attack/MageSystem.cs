using System;

using Entity.Player;

using Entity;

using Items;

using UnityEngine;

using Utilities;
using System.Collections.Generic;

namespace Attack {
    [Serializable] public class MageSystem : IAttackSystem {
        private CountDownTimer cooldown = new CountDownTimer(0f);
        private Stats stats;
        private Mana mana;
        private Transform origin;
        private WeaponController weaponController;
        private SpellData[] spells = new SpellData[3]; 
        private MageStaffData staff;
        private int spellIndex = 0;

        public MageSystem(Stats stats, Transform origin, WeaponController weaponController, MageStaffData staff, Mana mana) {
            this.stats = stats;
            this.mana = mana;
            this.origin = origin;
            this.weaponController = weaponController;
            if (staff && staff is MageStaffData) {
                this.staff = staff;
            } else {
                Debug.LogError("Mage System was initialised incorrectly!");
            }
        }

        public void SetSpell(int index, SpellData spell) {
            if (index >= spells.Length || index < 0) {
                Debug.LogWarning($"Tried to set spell at invalid index: {index}");
                return;
            }
            for (int i = 0; i < spells.Length; i++) {
                if (spells[i] == spell) {
                    spells[i] = null;
                }
            }
            spells[index] = spell;
        }

        public float CooldownProgress => cooldown.Progress();

        public void FixedUpdate() {
            cooldown.Update(Time.fixedDeltaTime);
            if (!cooldown.isFinished || !UILock.instance.allowGameplay) {
                return;
            }
            if (Utilities.Input.instance.playerControls.Gameplay.UseSpellOne.IsPressed()) {
                spellIndex = 0;
                if (HasSpell() && mana.UseMana(spells[spellIndex].manaCost)) {
                    Debug.Log($"Casting spell: {spells[spellIndex].name}");
                    Attack(origin);
                    ResetSpellCooldown();
                }
            }
            if (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.IsPressed()) {
                spellIndex = 1;
                if (HasSpell() && mana.UseMana(spells[spellIndex].manaCost)) {
                    Debug.Log($"Casting spell: {spells[spellIndex].name}");
                    Attack(origin);
                    ResetSpellCooldown();
                }
            }
            if (Utilities.Input.instance.playerControls.Gameplay.UseSpellThree.IsPressed()) {
                spellIndex = 2;
                if (HasSpell() && mana.UseMana(spells[spellIndex].manaCost)) {
                    Debug.Log($"Casting spell: {spells[spellIndex].name}");
                    Attack(origin);
                    ResetSpellCooldown();
                }
            }
        }

        private bool HasSpell() {
            if (spells == null) {
                Debug.LogWarning($"No spells?");
                return false;
            }
            if (spells.Length <= spellIndex) {
                Debug.LogWarning($"No spell assigned in slot: {spellIndex}");
                return false;
            }
            if (!spells[spellIndex]) {
                Debug.LogWarning($"No spell assigned in slot: {spellIndex}");
                return false;
            }
            return true;
        }

        public void Attack(Transform origin) {
            if (stats.GetStat(StatType.MAGIC, out float magic)) {
                Quaternion rotation = Quaternion.AngleAxis(
                    Vector2.SignedAngle(
                        Vector2.up, 
                        (Utilities.Input.instance.main.ScreenToWorldPoint(Utilities.Input.instance.playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - origin.position).normalized),
                    Vector3.forward
                );
                spells[spellIndex].CastSpell(origin.position, rotation, magic * staff.damageAmplifier * spells[spellIndex].magicModifier);
                weaponController.Attack(spells[spellIndex].castTime);
            } else {
                Debug.LogError($"Could not find entry for Magic Stat on Stats of {stats.name}!");
            }
        }

        private void ResetSpellCooldown() {
            cooldown.Restart(spells[spellIndex].cooldown * staff.cooldownModifier);
        }

        public void SetWeapon<T>(T staff) where T : ItemData {
            if (!staff || staff is not MageStaffData) {
                Debug.LogError("Tried to pass non staff to SetWeapon on MageSystem!");
                return;
            }
            this.staff = staff as MageStaffData;
            weaponController.GetWeaponAnimator<MageStaffAnimator>().SetWeapon(staff as MageStaffData);
        }

        public ItemData GetWeapon() {
            return staff;
        }

        public void SetSpells(SpellData[] spells) {
            if (spells == null) {
                Debug.LogError("No spells?");
                return;
            }
            this.spells = spells;
        }

        public SpellData[] GetSpells() {
            return spells;
        }
    }
}