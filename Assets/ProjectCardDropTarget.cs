using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectCardDropTarget : MonoBehaviour
{
    [SerializeField] private RectTransform cardContainer;
    public RectTransform CardContainer => cardContainer;

    public event Action<Project> onCardAdded;
    public event Action<Project> onCardRemoved;

    public void OnCardAdded(ProjectCard card)
    {
        onCardAdded?.Invoke(card.Project);
    }

    public void OnCardRemoved(ProjectCard card)
    {
        onCardRemoved?.Invoke(card.Project);
    }
}
