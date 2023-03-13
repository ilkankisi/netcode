using Supercyan.AnimalPeopleSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelManager.score > 5)
        {
            gameObject.GetComponent<SimpleSampleCharacterControl>().m_jumpForce = 15;
        }
    }
}
