using System;
using UnityEngine;

namespace __Scripts._Services.InputService
{
    public interface IInputService : IDisposable
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool IsSprintHeld { get; }

        void EnablePlayerActions();
        void DisablePlayerActions();
        bool ConsumeJumpPressed();
    }
}