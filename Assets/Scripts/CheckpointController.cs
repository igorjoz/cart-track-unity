using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public int lap = 0;
    public int checkpoint = -1;
    int checkpointCount;
    public int nextCheckpoint;
    public GameObject lastCheckpoint;

    void Start()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpointCount = checkpoints.Length;

        for (int i = 0; i < checkpointCount; i++)
        {
            if (checkpoints[i].name == "0")
            {
                lastCheckpoint = checkpoints[i];
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Checkpoint")
        {
            int thisCheckpoint = int.Parse(other.gameObject.name);

            if (thisCheckpoint == nextCheckpoint)
            {
                lastCheckpoint = other.gameObject;
                checkpoint = thisCheckpoint;

                if (checkpoint == 0)
                {
                    lap++;
                    Debug.Log("Lap: " + lap);
                }

                nextCheckpoint++;
                nextCheckpoint = nextCheckpoint % checkpointCount;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
