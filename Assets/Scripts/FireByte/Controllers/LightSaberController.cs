///////////////////////////////////////////////////////////
//  LightSaberController.cs
//  Implementation of the Class LightSaberController
//  Generated by Enterprise Architect
//  Created on:      30-May-2021 2:56:55 PM
//  Original author: Reza
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using DG.Tweening;
using FireByte;
using Unity.Mathematics;
using UnityEngine;

namespace FireByte
{
    public class LightSaberController : Controller<DesignData>
    {
        [SerializeField] private LightSaber m_redLightSaber;
        [SerializeField] private LightSaber m_blueLightSaber;

        private Dictionary<LightSaberName, Dictionary<PlayerType, LightSaber>> _lightSabers;
        private LightSaberRotateEvent m_lightSaberRotateEvent;

        private bool m_aiEnabled;

        public override void Initialize(DesignData designData)
        {
            _lightSabers = new Dictionary<LightSaberName, Dictionary<PlayerType, LightSaber>>();

            m_redLightSaber.Initialize(this, designData.GoBackAnimationAngle, designData.RotationAnimationAngle,
                designData.BeforeAnimationDuration, designData.RotateAnimationDuration);
            m_blueLightSaber.Initialize(this, designData.GoBackAnimationAngle, designData.RotationAnimationAngle,
                designData.BeforeAnimationDuration, designData.RotateAnimationDuration);
            ;

            _lightSabers.Add(LightSaberName.Red, new Dictionary<PlayerType, LightSaber>());
            _lightSabers.Add(LightSaberName.Blue, new Dictionary<PlayerType, LightSaber>());

            _lightSabers[LightSaberName.Red].Add(PlayerType.Player, m_redLightSaber);

            m_aiEnabled = designData.AIEnabled;

            PlayerType playerType = m_aiEnabled ? PlayerType.AI : PlayerType.Player;

            _lightSabers[LightSaberName.Blue].Add(playerType, m_blueLightSaber);

            LightSabersToStartPosition();
        }

        public void LightSabersToStartPosition()
        {
            foreach (LightSaber lightSaber in GetLightSabersFromDic())
            {
                lightSaber.StartPosition();
            }
        }

        public void RotateLightSaber(LightSaberName lightSaberName, PlayerType playerType,
            RotateDirection rotateDirection, float value)
        {
            _lightSabers[lightSaberName][playerType].Rotate(rotateDirection, value);
        }

        public void LightSabersHit(Vector3 hitPoint)
        {
            GameManager.Instance.LightSabersHitted(hitPoint);
        }

        public void AnimateLightSaber()
        {
            if (!m_aiEnabled)
            {
                m_blueLightSaber.RotateAnimateY();
            }

            m_redLightSaber.RotateAnimateY();
        }

        public void Update()
        {
            if (GetLightSabersFromDic().All(x => x.IsFnishMovement))
                GameManager.Instance.SimulateFinishedWithoutHit();
        }

        private List<LightSaber> GetLightSabersFromDic()
        {
            return (from x in _lightSabers.Values
                from y in x.Values
                select y).ToList();
        }
    } //end LightSaberController
} //end namespace FireByte