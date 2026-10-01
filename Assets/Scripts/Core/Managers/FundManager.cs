using System;
using Core.Initialization;
using Data.Models.World;
using Interfaces;
using UnityEngine;

namespace Core.Managers
{
    public class FundManager : MonoBehaviour, ISingle
    {
        private FundModel _model;
        
        public static event Action<int> OnBalanceChanged;
        public static event Action<int> OnSkillPointChanged;

        public void Awake()
        {
            _model = Hub.dataService.GetWorldData().funds;
            NotifyFund();
        }

        public int GetBalance() => _model.Balance;
        public int GetSkillPoints() => _model.SkillPoints;

        public void AddBalance(int amount)
        {
            if (amount <= 0) return;
            
            _model.Balance += amount;
            NotifyFund();
        }
        
        public void AddSkillPoint(int amount)
        {
            if (amount <= 0) return;
            
            _model.SkillPoints += amount;
            NotifyFund();
        }

        public bool SpendBalance(int amount)
        {
            if (amount <= 0 || _model.Balance < amount)
            {
                //Balance not enough
                return false;
            }

            _model.Balance -= amount;
            NotifyFund();
            return true;
        }
        
        public bool SpendSkillPoint(int amount)
        {
            if (amount <= 0 || _model.SkillPoints < amount)
            {
                //SkillPoint not enough
                return false;
            }

            _model.SkillPoints -= amount;
            NotifyFund();
            return true;
        }

        private void NotifyFund()
        {
            OnBalanceChanged?.Invoke(_model.Balance);
            OnSkillPointChanged?.Invoke(_model.SkillPoints);
        }
    }
}