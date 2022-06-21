using UnityEngine;

public class Skybox_CamUpdate : MonoBehaviour
{
    [SerializeField] private float x_RotationValue = 0;
    [SerializeField] private float y_RotationValue = 0;
    [SerializeField] private float z_RotationValue = 0;
    [SerializeField] private float x_Axis, y_Axis, z_Axis;

    // Update is called once per frame
    private void Update()
    {
        x_Axis = x_RotationValue * Time.deltaTime;
        y_Axis = y_RotationValue * Time.deltaTime;
        z_Axis = z_RotationValue * Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(x_Axis, y_Axis, z_Axis));
    }
}
