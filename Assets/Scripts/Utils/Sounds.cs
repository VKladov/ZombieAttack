using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class Sounds : MonoBehaviour
    {
        private readonly GameObjectsPool<AudioSource> _pool = new();
        [SerializeField] private AudioClip[] _clips;
        [SerializeField] private AudioSource _audioSourcePrefab;

        private Dictionary<string, AudioClip> _clipsByName;

        private void Awake()
        {
            _clipsByName = new Dictionary<string, AudioClip>();
            for (var i = 0; i < _clips.Length; i++)
            {
                var clip = _clips[i];
                _clipsByName.Add(clip.name, clip);
            }
        }

        public async UniTask PlayClip(string clipName, CancellationToken cancellationToken)
        {
            if (!_clipsByName.TryGetValue(clipName, out var clip)) return;

            var source = _pool.GetInstance(_audioSourcePrefab);
            source.clip = clip;
            source.Play();
            await UniTask.Yield();
            await UniTask.WaitWhile(() => source.isPlaying, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();
            if (source != null) source.Stop();
            _pool.Recycle(source);
        }
    }
}