using UnityEngine;
using UnityEngine.InputSystem;

public class HW23_PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 90f;

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float h = 0f, v = 0f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  h = -1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h =  1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v = -1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v =  1f;

        transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime, Space.World);

        if (kb.qKey.isPressed)
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        if (kb.eKey.isPressed)
            transform.Rotate(Vector3.up,  rotateSpeed * Time.deltaTime);
    }
}
