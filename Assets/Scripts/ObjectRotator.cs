using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeed;

    // Update is called once per frame
    private void Update()
    {
        gameObject.transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
