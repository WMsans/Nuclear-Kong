using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour, IResetable
{
    public UnityEvent onPressed;
    public UnityEvent onReset;
    public bool IsPressed { get; private set; } = false;

    public void PressButton()
    {
        if(IsPressed) return;
        IsPressed = true;
        onPressed.Invoke();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PressButton();
        }
    }

    public void OnReset()
    {
        onReset.Invoke();
        IsPressed = false;
    }
}