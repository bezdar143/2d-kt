using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    private string Horizontal = nameof(Horizontal);
    
    public event Action<float> MovementButtonPressed;
    public event Action JumpButtonPressed;

    private void Update()
    {
        float horizontal = Input.GetAxis(Horizontal);

        if (horizontal != 0)
        {
            MovementButtonPressed?.Invoke(horizontal);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButtonPressed?.Invoke();
        }
    }
}
