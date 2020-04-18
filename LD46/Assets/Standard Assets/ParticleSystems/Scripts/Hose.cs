using System;
using UnityEngine;


namespace UnityStandardAssets.Effects
{
    public class Hose : MonoBehaviour
    {
        private float maxPower = 20;
        private float minPower = 10;
        private float changeSpeed = 10;
        public ParticleSystem[] hoseWaterSystems;
        public Renderer systemRenderer;

        private float m_Power;
        private bool active = false;

        private void Start()
        {
            InvokeRepeating("EnableHose", 0.5f, 0.5f);
        }

        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(0, 20 * Time.deltaTime, 0);

            m_Power = Mathf.Lerp(m_Power, active ? maxPower : minPower, Time.deltaTime * changeSpeed);
            foreach (var system in hoseWaterSystems)
            {
				ParticleSystem.MainModule mainModule = system.main;
                mainModule.startSpeed = m_Power;
                var emission = system.emission;
                emission.enabled = (m_Power > minPower*1.1f);
            }
        }

        private void EnableHose()
        {
            active = !active;
        }
    }
}
