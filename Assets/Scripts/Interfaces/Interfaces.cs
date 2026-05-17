using UnityEngine;

namespace Interfaces
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }
    public interface IActor
    {
        string Id { get; }
        Transform Transform { get; }
        void OnInitialize();
    }
    
    public interface IDamageable
    {
        float CurrentHealth { get; }
        void TakeDamage(float amount, IActor attacker);
        void Die();
    }

    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void Interact(IActor interactor);
    }
    
    public interface IProvider<out T>
    {
        T Get();
    }
}
