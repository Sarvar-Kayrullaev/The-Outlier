using System;
using System.Collections;
using UnityEngine;

namespace Actors.Player.Actions
{
    /// <summary>
    /// IPlayerAction'larni ishga tushiruvchi yagona MonoBehaviour-host.
    /// Unity'ning "faqat MonoBehaviour Coroutine ishga tushira oladi" cheklovi shu yerda
    /// izolyatsiya qilingan - IPlayerAction implementatsiyalarining o'zi MonoBehaviour
    /// bo'lishi shart emas (toza, test qilinadigan C# klasslar bo'lib qoladi).
    /// </summary>
    public class PlayerActionRunner : MonoBehaviour
    {
        private IPlayerAction _activeAction;
        private PlayerActionContext _activeContext;

        public bool IsBusy => _activeAction != null;

        public void Run(IPlayerAction action, PlayerActionContext context, Action onComplete)
        {
            _activeAction?.Cancel(context);

            _activeAction = action;
            _activeContext = context;

            if (action.LocksCharacterController)
            {
                context.character.enabled = false;
            }

            action.Enter(context, () =>
            {
                if (action.LocksCharacterController)
                {
                    context.character.enabled = true;
                }

                _activeAction = null;
                _activeContext = null;
                onComplete?.Invoke();
            });
        }

        public Coroutine StartActionCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        private void Update()
        {
            _activeAction?.Tick(_activeContext);
        }
    }
}