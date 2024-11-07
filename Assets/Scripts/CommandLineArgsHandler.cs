using System;
using UnityEngine;

public class CommandLineArgsHandler : MonoBehaviour
{
    [HideInInspector]
    public string commandLineArg;
    private EventLogger logger;

    void Start()
    {
        logger = GetComponent<EventLogger>();

        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            commandLineArg = args[1];
            //Debug.Log("First command line argument: " + commandLineArg);
        }
        else
        {
           // Debug.LogWarning("No command line arguments found.");
        }

        StartSession();
    }

    void StartSession()
    {
        logger.StartSession(commandLineArg);
    }
}
