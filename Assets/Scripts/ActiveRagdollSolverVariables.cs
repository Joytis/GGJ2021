using UnityEngine;

public class ActiveRagdollSolverVariables : MonoBehaviour
{
    [SerializeField] int _solverIterations = 10;
    [SerializeField] int _velocitySolverIterations = 10;
    [SerializeField] float _maxAngularVelocity = 25;

    void Awake() 
    {
        // Drive all the rigidbody solvers with our more-precise values. 
        foreach(var rb in GetComponentsInChildren<Rigidbody>(true))
        {
            rb.solverIterations = _solverIterations;
            rb.solverVelocityIterations = _velocitySolverIterations;
            rb.maxAngularVelocity = _maxAngularVelocity;
        }
    }
}
