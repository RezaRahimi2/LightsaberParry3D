///////////////////////////////////////////////////////////
//  GameManager.cs
//  Implementation of the Class GameManager
//  Generated by Enterprise Architect
//  Created on:      30-May-2021 2:56:55 PM
//  Original author: Reza
///////////////////////////////////////////////////////////

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace FireByte
{
    public class GameManager : Manager
    {
        //Singleton pattern of GameManger class
        #region Singelton

        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameManager>();
                return instance;
            }
        }

        #endregion

        //Instance of scriptable object of design data
        #region Data

        [SerializeField] private DesignData m_DesignData;

        #endregion

        //Instances of managers
        #region Managers
        [SerializeField] private EventManager m_eventManager;
        [SerializeField] private UIManager m_uiManager;
        [SerializeField] private ParticleManager m_particleManager;
        #endregion

        //Instances of controllers        
        #region Controllers
        [SerializeField] private LightSaberController m_lightSaberController;
        [SerializeField] private AIController m_aiController;
        #endregion

        //flag for define AI is enabled
        public bool AIEnabled;

        private void Start()
        {
            Initialize();
        }
        
        //Initialize all managers and controllers
        public override void Initialize()
        {
            AIEnabled = m_DesignData.AIEnabled;

            m_eventManager.Initialize();
            m_lightSaberController.Initialize(m_DesignData);
            m_aiController.Initialize(m_DesignData);
            m_uiManager.Initialize(m_DesignData.FadeDuration, AIEnabled);
            m_particleManager.Initialize();

            m_eventManager.OnLightSaberRotateEvent += m_lightSaberController.RotateLightSaber;
            m_eventManager.OnHitEvent += m_particleManager.StartHitParticle;

            m_eventManager.OnGameOverEvent += m_uiManager.ShowGameOverPopup;
            m_eventManager.OnGameOverEvent += m_uiManager.ChangeSimulateButtonToResetButton;

            if (AIEnabled)
                m_eventManager.OnSimulateEvent += m_aiController.RandomRotation;

            Action simulate = () => DOVirtual.DelayedCall(m_DesignData.RotateAnimationDuration,
                () => m_lightSaberController.AnimateLightSaber());

            if (AIEnabled)
                m_eventManager.OnSimulateEvent += () => DOVirtual.DelayedCall(m_DesignData.RotateAnimationDuration,
                    () => m_lightSaberController.AnimateLightSaber());
            else
                m_eventManager.OnSimulateEvent += m_lightSaberController.AnimateLightSaber;
        }

        //for Simulation called the simulate event
        public void Simulate()
        {
            m_eventManager.OnSimulateEvent?.Invoke();
        }

        //called after light sabers rotation are finished
        public void SimulateFinishedWithoutHit()
        {
            GameOver();
        }

        //call Initialize method for reset everything to initial state
        public void ResetToStart()
        {
            Initialize();
        }

        //called when slider value changed
        public void OnRotateChange(LightSaberName lightSaberName, PlayerType playerType,
            RotateDirection rotateDirection, float value)
        {
            m_eventManager.OnLightSaberRotateEvent?.Invoke(lightSaberName, playerType, rotateDirection, value);
        }

        //called when light sabers hitted to each other
        public void LightSabersHitted(Vector3 hittedPoint)
        {
            m_eventManager.OnHitEvent?.Invoke(hittedPoint);
            GameOver();
        }

        //called when game is over, after light sabers hit or not hit
        public void GameOver()
        {
            m_eventManager.OnGameOverEvent?.Invoke();
        }
    } //end GameManager
} //end namespace FireByte