// =================================
// Namespaces.
// =================================

using UnityEngine;

// =================================
// Define namespace.
// =================================

// =================================
// Classes.
// =================================

//[ExecuteInEditMode]
[System.Serializable]

//[RequireComponent(typeof(TrailRenderer))]

public class FollowMouse : MonoBehaviour
{
    // =================================
    // Nested classes and structures.
    // =================================

    // ...

    // =================================
    // Variables.
    // =================================

    // ...

    public float Speed = 8.0f;
    public float DistanceFromCamera = 5.0f;

    // =================================
    // Functions.
    // =================================

    // ...

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = DistanceFromCamera;

        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-Speed * Time.deltaTime));

        transform.position = position;
    }

    // =================================
    // End functions.
    // =================================

}

// =================================
// End namespace.
// =================================


// =================================
// --END-- //
// =================================