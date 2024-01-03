using UnityEngine;
using UnityEngine.Assertions;
using Photon.Pun;

namespace RingCrisis
{
    /// <summary>
    /// ターゲットの生成を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class TargetManager : MonoBehaviour
    {
        private static readonly float SpawnInterval = 5.0f;
        private static readonly float LifeTimeBase = 10.0f;

        [SerializeField]
        private RpcManager _rpcManager = null;

        [SerializeField]
        private Target _targetPrefab = null;

        [SerializeField]
        private GameObject _fxSpawn = null;

        [SerializeField]
        private GameObject _spawnArea = null;

        private bool _activated;

        private float _timer;

        private BoxCollider baseCollider;

        public void ActivateSpawn()
        {
            _activated = true;
            RandomSpawnTarget();
        }

        public void DeactivateSpawn()
        {
            _activated = false;
        }

        private void Awake()
        {
            Assert.IsNotNull(_rpcManager);
            Assert.IsNotNull(_targetPrefab);
            Assert.IsNotNull(_fxSpawn);
            Assert.IsNotNull(_spawnArea);
        }

        private void Start()
        {
            baseCollider = _spawnArea.GetComponent<BoxCollider>();
            _rpcManager.OnReceiveTargetCreated += (pos, lifeTime) =>
            {
                SpawnTargetLocal(pos, lifeTime);
            };
        }

        private void Update()
        {
            if (!_activated || !PhotonNetwork.IsMasterClient)
            {
                return;
            }

            // 一定時間ごとにターゲットを生成する
            _timer += Time.deltaTime;
            if (_timer > SpawnInterval)
            {
                RandomSpawnTarget();
                _timer -= SpawnInterval;
            }
        }

        private void RandomSpawnTarget()
        {
            // ランダムな位置を生成
            Vector3 randomPosition = new Vector3(
                Random.Range(baseCollider.bounds.min.x, baseCollider.bounds.max.x),
                0f,
                Random.Range(baseCollider.bounds.min.z, baseCollider.bounds.max.z)
            );
            float lifeTime = LifeTimeBase * Random.Range(0.7f, 1.3f);

            _rpcManager.SendTargetCreated(randomPosition, lifeTime);
        }

        private void SpawnTargetLocal(Vector3 worldPosition, float lifeTime)
        {
            Target newTarget = Instantiate(_targetPrefab, worldPosition, Quaternion.identity);
            Instantiate(_fxSpawn, worldPosition, Quaternion.identity);

            newTarget.LifeTime = lifeTime;
        }
    }
}
