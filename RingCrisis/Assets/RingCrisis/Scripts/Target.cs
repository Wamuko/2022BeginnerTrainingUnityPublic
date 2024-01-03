using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// ターゲットそのものの処理を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class Target : MonoBehaviour
    {
        [SerializeField]
        private int _score = 0;

        public int Score => _score;
        public float LifeTime { get; set; } = 8.0f;

        private void Update()
        {
            LifeTime -= Time.deltaTime;
            if(LifeTime < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
