using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class ButtonController : MonoBehaviour
{
    [SerializeField]
    private List<ButtonBehaviour> buttonsToPress;

    public UnityEvent onAllButtonsPressed;

    private bool allButtonsWerePressed = false;

    void Update()
    {
        if (!allButtonsWerePressed && AllButtonsArePressed())
        {
            onAllButtonsPressed.Invoke();
            allButtonsWerePressed = true;
        }
    }

    private bool AllButtonsArePressed()
    {
        return buttonsToPress.All(b => b.IsPressed);
    }
}