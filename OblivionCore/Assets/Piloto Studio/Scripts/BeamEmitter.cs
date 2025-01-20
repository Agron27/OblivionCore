using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PilotoStudio
{
    [ExecuteAlways]
    public class BeamEmitter : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private List<LineRenderer> beams = new List<LineRenderer>();
        [Space]
        [SerializeField]
        private List<ParticleSystem> beamSystems = new List<ParticleSystem>();
        [SerializeField]
        [Space]
        private float beamLifetime;
        [SerializeField]
        private float beamFormationTime;

        [SerializeField]
        private Vector3 beamTarget; // Target point of the beam
        [SerializeField]
        private GameObject beamTargetHitFX;
        [SerializeField]
        private List<float> desiredWidth = new List<float>();

        [SerializeField]
        private List<UnityEngine.ParticleSystem.MinMaxCurve> defaultDensity = new List<UnityEngine.ParticleSystem.MinMaxCurve>();

        [SerializeField]
        private Transform firePoint; // The starting point of the beam

        private void OnEnable()
        {
            PlayBeam();
        }

        public void SetBeamTarget(Vector3 newBeamTarget)
        {
            beamTarget = newBeamTarget;
        }

        private IEnumerator BeamStart()
        {
            float elapsedTime = 0f;

            while (elapsedTime <= 1)
            {
                for (int i = 0; i < beams.Count; i++)
                {
                    beams[i].widthMultiplier = Mathf.Lerp(0, desiredWidth[i], elapsedTime / 1);
                    elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            for (int i = 0; i < beams.Count; i++)
            {
                beams[i].widthMultiplier = desiredWidth[i];
            }
        }

        private void CacheParticleDensity()
        {
            defaultDensity.Clear();

            for (int i = 0; i < beamSystems.Count; i++)
            {
                defaultDensity.Add(beamSystems[i].emission.rateOverTime.constant);
            }
        }

        private void UpdateParticleDensity()
        {
            float distance = Vector3.Distance(firePoint.position, beamTarget);
            distance -= 5f;

            if (distance > 0)
            {
                float distanceMultiplier = 1 + (distance / 5);
                for (int i = 0; i < beamSystems.Count; i++)
                {
                    var emission = beamSystems[i].emission;
                    emission.rateOverTime = defaultDensity[i].constant * distanceMultiplier;
                }
            }
            else
            {
                for (int i = 0; i < beamSystems.Count; i++)
                {
                    var emission = beamSystems[i].emission;
                    emission.rateOverTime = defaultDensity[i].constant;
                }
            }
        }

        private void UpdateImpactFX()
        {
            beamTargetHitFX.transform.position = beamTarget;
            beamTargetHitFX.transform.LookAt(firePoint.position);
        }

        public void PlayBeam()
        {
            StopAllCoroutines();
            PlayEdgeSystems();
            PlayLineRenderers();

            if (beamLifetime == 0)
            {
                StartCoroutine(nameof(BeamStart));
            }
            else
            {
                StartCoroutine(nameof(BeamPlayComplete));
            }
        }

        private IEnumerator BeamPlayComplete()
        {
            float elapsedTime = 0f;

            while (elapsedTime <= beamFormationTime)
            {
                for (int i = 0; i < beams.Count; i++)
                {
                    beams[i].widthMultiplier = Mathf.Lerp(0, desiredWidth[i], elapsedTime / beamFormationTime);
                    elapsedTime += Time.deltaTime;
                }
                yield return null;
            }

            for (int i = 0; i < beams.Count; i++)
            {
                beams[i].widthMultiplier = desiredWidth[i];
            }

            yield return new WaitForSeconds(beamLifetime);

            float dissipationTime = 0f;

            while (dissipationTime <= beamFormationTime)
            {
                for (int i = 0; i < beams.Count; i++)
                {
                    beams[i].widthMultiplier = Mathf.Lerp(desiredWidth[i], 0, dissipationTime / beamFormationTime);
                    dissipationTime += Time.deltaTime;
                }
                yield return null;
            }

            for (int i = 0; i < beams.Count; i++)
            {
                beams[i].widthMultiplier = 0;
            }
        }

        private void PlayLineRenderers()
        {
            foreach (LineRenderer _line in beams)
            {
                _line.useWorldSpace = true;
                _line.SetPosition(0, firePoint.position); // Start beam at firePoint
                _line.SetPosition(1, beamTarget);         // End beam at target
            }
        }

        private void PlayEdgeSystems()
        {
            foreach (ParticleSystem _ps in beamSystems)
            {
                Quaternion _lookRotation = Quaternion.LookRotation(beamTarget - firePoint.position).normalized;
                _ps.gameObject.transform.rotation = _lookRotation;

                var sh = _ps.shape;
                sh.rotation = new Vector3(0, 90, 0);
                float beamLength = Vector3.Distance(beamTarget, firePoint.position) / 2;
                sh.radius = beamLength;
                sh.position = new Vector3(0, 0, beamLength);
            }
        }

        private void Update()
        {
            PlayEdgeSystems();
            PlayLineRenderers();
            UpdateParticleDensity();
            UpdateImpactFX();
        }
    }
}