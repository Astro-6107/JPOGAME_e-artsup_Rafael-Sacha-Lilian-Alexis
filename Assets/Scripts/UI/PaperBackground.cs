using UnityEngine;
using UnityEngine.UI;

namespace JPO
{
    public class PaperBackground : MonoBehaviour
    {
        [SerializeField] private Sprite[] frames;
        [SerializeField] private int frameRate;
        private Image _image;
        private Sprite _currentFrame;
        private float _frameTimer;
        private float _timePerFrame;

        private void Start()
        {
            _image = GetComponent<Image>();
            _timePerFrame = 1f / frameRate;
            PickRandomFrame();
        }

        private void Update()
        {
            _frameTimer += Time.deltaTime;
            if (_frameTimer >= _timePerFrame)
            {
                _frameTimer -= _timePerFrame;
                PickRandomFrame();
            }
        }

        private void PickRandomFrame()
        {
            Sprite frame;
            do
            {
                frame = frames[Random.Range(0, frames.Length)];
            } while (frame == _currentFrame);
            _currentFrame = frame;
            _image.sprite = frame;
        }
    }
}