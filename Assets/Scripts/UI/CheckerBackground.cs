using UnityEngine;
using UnityEngine.UI;

namespace JPO
{
    [RequireComponent(typeof(RawImage))]
    public class CheckerBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 OffsetSpeed;
        private RawImage _image;
        private Rect _rect;

        private void Start()
        {
            _image = GetComponent<RawImage>();
            _rect = _image.uvRect;
        }

        private void Update()
        {
            _rect.x += Time.deltaTime * OffsetSpeed.x;
            _rect.y += Time.deltaTime * OffsetSpeed.y;
            _image.uvRect = _rect;
        }
    }
}