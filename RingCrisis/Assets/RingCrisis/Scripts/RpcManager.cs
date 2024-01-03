using System;
using Photon.Pun;
using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// PhotonのRPC送受信を担うコンポーネント
    /// RequireComponent属性を使うと他のコンポーネントが必須であることを示すことができ、アタッチ時にその存在がチェックされるようになる
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
    public class RpcManager : MonoBehaviour
    {
        public event Action OnReceiveStartGame;
        public event Action<TeamColor,Vector3> OnReceiveShootRing;
        public event Action<Vector3,float> OnReceiveTargetCreated;

        private PhotonView _photonView;

        public void SendStartGame()
        {
            // "StartGame" と書く代わりに nameof(StartGame) とすると、メソッド名の変更に対して保守性が高まります
            // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/nameof
            _photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
        }

        public void SendShootRing(TeamColor teamColor, Vector3 dir)
        {
            _photonView.RPC(nameof(ShareShootRing), RpcTarget.All, teamColor, dir);
        }

        public void SendTargetCreated(Vector3 pos, float span)
        {
            _photonView.RPC(nameof(ShareTargetCreated), RpcTarget.All, pos, span);
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        [PunRPC]
        private void StartGame()
        {
            OnReceiveStartGame?.Invoke();
        }


        [PunRPC]
        private void ShareShootRing(TeamColor teamColor, Vector3 dir)
        {
            OnReceiveShootRing?.Invoke(teamColor, dir);
        }

        [PunRPC]
        private void ShareTargetCreated(Vector3 pos, float span)
        {
            OnReceiveTargetCreated?.Invoke(pos, span);
        }
    }
}
