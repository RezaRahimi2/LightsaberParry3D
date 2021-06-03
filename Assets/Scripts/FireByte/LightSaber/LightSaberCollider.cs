using System.Collections;
using System.Collections.Generic;
using FireByte;
using UnityEngine;

public class LightSaberCollider : MonoBehaviour
{
   private LightSaber m_lightSaber;
   
   public void Initialize(LightSaber lightSaber)
   {
      m_lightSaber = lightSaber;
   }
   
   private void OnCollisionEnter(Collision other)
   {
      Debug.unityLogger.Log("Hitted");
      m_lightSaber.OnHitEnter(other.contacts[0].point);
   }

   private void OnCollisionExit(Collision other)
   {
      m_lightSaber.OnHitExit();
   }
}
