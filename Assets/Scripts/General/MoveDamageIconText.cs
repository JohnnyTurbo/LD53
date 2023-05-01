using System.Collections;
using TMPro;
using UnityEngine;

namespace TMG.LD53
{
    public class MoveDamageIconText : MonoBehaviour
    {
        [SerializeField] private float _moveDuration;
        private TextMeshProUGUI _text;
        
        public Vector3 EndPosition { get; set; }

        private float _timer;
        private Vector3 _startPosition;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _startPosition = transform.position;
            StartCoroutine(MoveDamageText());
        }

        private IEnumerator MoveDamageText()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
            while (_timer < _moveDuration)
            {
                var t = EaseOutCubic(_timer, _moveDuration);

                transform.position = Vector3.Lerp(_startPosition, EndPosition, t);
                _timer += Time.deltaTime;
                yield return null;
            }

            _timer = 0f;
            while (_timer < 0.5f)
            {
                var color = _text.color;
                var t = _timer / 0.5f;
                color.a = Mathf.Lerp(1, 0, t);
                _text.color = color;
                _timer += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
        
        private static float EaseOutCubic(float time, float duration)
        {
            time /= duration;
            time--;
            return time * time * time + 1;
        }
    }
}