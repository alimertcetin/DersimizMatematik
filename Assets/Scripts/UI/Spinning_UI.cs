using UnityEngine;

namespace XIV.UI
{
    public class Spinning_UI : MonoBehaviour
    {
        private RectTransform rectComponent;
        public float rotateSpeed = 200f;

        private void Start()
        {
            rectComponent = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        }
    }
}