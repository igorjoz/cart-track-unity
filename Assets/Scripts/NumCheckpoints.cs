using UnityEngine;

[ExecuteInEditMode]
public class NumCheckpoints : MonoBehaviour
{
    void Start()
    {
        Transform[] checkpoints = GetComponentsInChildren<Transform>();

        for (int i = 1; i < checkpoints.Length; i++)
        {
            checkpoints[i].gameObject.name = (i - 1).ToString();
        }
    }
}
