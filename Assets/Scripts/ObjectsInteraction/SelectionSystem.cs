using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Interfaces;
using UnityEngine.UIElements.Experimental;

public class SelectionSystem : MonoBehaviour
{
    public static SelectionSystem instance;
    private BoardAwareness _awareness;
    public CreatureCard selectedCard
    {
        get { return _selectedCard; }
        set
        {
            _selectedCard = value;
            _awareness.SelectedCard = value;
        }
    }

    private CreatureCard _selectedCard;

    public CreatureCard targetCard
    {
        get { return _targetCard; }
        set
        {
            _targetCard = value;
            _awareness.TargetCard = value;
        }
    }

    private CreatureCard _targetCard;

    public IHealth effectTarget;
    public bool selectingEffectTarget;

    public bool cardSelected = false;

    public AttackCursor AttackCursor;

    public GameObject Canvas;
    private GraphicRaycaster raycaster;

    private PointerEventData clickData;
    private List<RaycastResult> clickResults;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        _awareness = BoardAwareness.Instance;
        Canvas = GameObject.Find("Main Canvas");
        raycaster = Canvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();

        AttackCursor = AttackCursor != null
            ? AttackCursor
            : GameObject.Find("AttackCursor").GetComponent<AttackCursor>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GetUiElementsClicked();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && cardSelected)
        {
            GetUiElementsClicked();
        }
    }

    public void StartEffectTargetSelection()
    {
        AttackCursor.ToggleCursor(true);
        selectingEffectTarget = true;
    }

    public void TrySelectEffectTarget(Card card, IHealth target)
    {
        
    }


    private void GetUiElementsClicked()
    {
        clickData.position = Mouse.current.position.ReadValue();
        clickResults.Clear();

        raycaster.Raycast(clickData, clickResults);

        CreatureCard tempCard = null;
        HeroBehaviour tempHero = null;

        foreach (RaycastResult result in clickResults)
        {
            GameObject uiElement = result.gameObject;

            tempCard = uiElement.GetComponent<CreatureCard>();

            if (tempCard != null)
            {
                if (cardSelected)
                {
                    TryImplement(tempCard);
                }
                else
                {
                    TrySelect(tempCard);
                }

                break;
            }

            tempHero = uiElement.GetComponent<HeroBehaviour>();

            if (tempHero != null && cardSelected)
            {
                TryImplement(tempHero);
                break;
            }
        }

        // In case if we wont choose to attack anything
        if (cardSelected && tempCard == null)
        {
            selectedCard = null;
            cardSelected = false;
            targetCard = null;
            AttackCursor.ToggleCursor(false);
        }
    }

    private void TrySelect(CreatureCard tempCard)
    {
        if (tempCard.State == CardState.OnTable && tempCard.Team == Team.Player && !cardSelected && tempCard.CanAttack)
        {
            // selecting a card to perform an attack
            selectedCard = tempCard;
            cardSelected = true;
            AttackCursor.ToggleCursor(true);
        }
    }

    private void TryImplement(CreatureCard tempCard)
    {
        if (tempCard.State == CardState.OnTable && tempCard.Team == Team.Opponent && cardSelected && selectedCard.CanAttack)
        {
            // selecting enemy card as a target
            targetCard = tempCard;
            // attack 
            selectedCard.AttackCard(targetCard);    
            selectedCard.CardEffects.ImplementOnTargetEffect(targetCard.GetComponent<IHealth>());
            // selectedCard.ImplementTargetEffect(targetCard);
            // resetting variables
            ResetVariables();
            
        }
    }

    private void ResetVariables()
    {
        selectedCard.CanAttack = false;
        selectedCard = null;
        cardSelected = false;
        targetCard = null;
        AttackCursor.ToggleCursor(false);
    }

    private void TryImplement(HeroBehaviour tempHero)
    {
        if (selectedCard.State == CardState.OnTable && selectedCard.CanAttack)
        {
            if (tempHero.Team == Team.Opponent)
            {
                // selecting enemy card as a target
                selectedCard.AttackHero(tempHero);
                // resetting variables
                ResetVariables();
            }
        }
    }


}
