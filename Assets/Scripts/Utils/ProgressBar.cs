using UnityEngine;

namespace Utils
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bar;

        public void SetValue(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _bar.transform.localScale = new Vector3(value, 1f, 1f);
        }
    }
}