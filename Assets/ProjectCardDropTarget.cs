﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectCardDropTarget : MonoBehaviour {
    public void OnCardAdded(ProjectCard card)
    {
        // TODO
        Debug.Log("Card added: " + card);
    }

    public void OnCardRemoved(ProjectCard card)
    {
        // TODO
        Debug.Log("Card removed: " + card);
    }
}
