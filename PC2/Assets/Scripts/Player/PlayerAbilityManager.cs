using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityManager : MonoBehaviour
{
    public static PlayerAbilityManager instance;
    public bool disabled = false;
    public bool inDialogue = false;
    [SerializeField]private List<IAbility> abilities = new();

    private void Start()
    {
        instance = this;
        IAbility[] abilities = GetComponents<IAbility>();
        foreach (IAbility ability in abilities)
        {
            AddAbility(ability);
        }
    }

    public void Move(InputAction.CallbackContext c)
    {
        if (disabled) return;

        foreach (IAbility ability in abilities)
        {
            ability.Move(c);
        }
    }

    public void Jump(InputAction.CallbackContext c)
    {
        if (disabled) return;

        foreach (IAbility ability in abilities)
        {
            ability.Jump(c);
        }
    }

    public void Interact(InputAction.CallbackContext c)
    {
        if (disabled)
        {
            if (inDialogue)
            {
                DialogueManager.instance.Confirm(c);
            }
            return;
        }

        foreach (IAbility ability in abilities)
        {
            ability.Interact(c);
        }
    }

    public void AddAbility(IAbility ability)
    {
        abilities.Add(ability);
    }

    public void RemoveAbility(IAbility ability)
    {
        abilities.Remove(ability);
    }

    public void DisableAll()
    {
        disabled = true;
        foreach (IAbility ability in abilities)
        {
            ability.Disable();
        }
    }

    public void EnableAll()
    {
        disabled = false;
        foreach (IAbility ability in abilities)
        {
            ability.Enable();
        }
    }

}
