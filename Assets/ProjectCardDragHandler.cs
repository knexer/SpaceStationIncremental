using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles a drag session for a project card.
/// Creates invisible surrogate objects to insert into the layout of the hovered ProjectCardDropTarget to 'hold its place' in line.
/// </summary>
[RequireComponent(typeof(ProjectCard))]
[RequireComponent(typeof(LayoutElement))]
public class ProjectCardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject cardSurrogatePrefab;

    // Initial state.  Reverts to this if the drag is canceled.
    private ProjectCardDropTarget startParent;
    private int startIndex;

    // The ProjectCardDropTarget that this was hovered over last frame.
    private ProjectCardDropTarget currentHovered;
    private GameObject cardSurrogate;

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.startParent = transform.GetComponentInParent<ProjectCardDropTarget>();
        this.startIndex = transform.GetSiblingIndex();
        
        // reparent this to the root Canvas so it isn't clipped.
        this.transform.SetParent(GetComponentInParent<Canvas>().transform, false);
        GetComponent<LayoutElement>().ignoreLayout = true;

        this.startParent.OnCardRemoved(GetComponent<ProjectCard>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        ProjectCardDropTarget nextHovered = GetHoveredDropTarget(eventData);
        
        // Update the hovered container.
        if (nextHovered != currentHovered)
        {
            Destroy(cardSurrogate);

            currentHovered = nextHovered;
            if (currentHovered != null)
            {
                cardSurrogate = Instantiate(cardSurrogatePrefab);
                cardSurrogate.transform.SetParent(currentHovered.CardContainer, false);

                // We need fresh position data when we are determining the surrogate's child index later.
                LayoutRebuilder.ForceRebuildLayoutImmediate(currentHovered.CardContainer);
            }
        }

        if (currentHovered == null) return;

        // Move the surrogate to the index of the closest object within the hovered container.
        // There is guaranteed to be at least one child - the surrogate - which simplifies the loop a bit.
        int minDistanceIndex = 0;
        for (int i = 1; i < currentHovered.CardContainer.childCount; i++)
        {
            float childDistance = (currentHovered.CardContainer.GetChild(i).transform.position - transform.position)
                .sqrMagnitude;
            float minDistance = (currentHovered.CardContainer.GetChild(minDistanceIndex).transform.position - transform.position)
                .sqrMagnitude;

            if (childDistance < minDistance)
            {
                minDistanceIndex = i;
            }
        }

        cardSurrogate.transform.SetSiblingIndex(minDistanceIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentHovered != null)
        {
            // Complete the drag, inserting into the current target.
            transform.SetParent(currentHovered.CardContainer, false);
            transform.SetSiblingIndex(cardSurrogate.transform.GetSiblingIndex());
            Destroy(cardSurrogate);
            currentHovered = null;
        }
        else
        {
            // Revert the drag if we were not dropped onto a target.
            transform.SetParent(startParent.CardContainer, false);
            transform.SetSiblingIndex(startIndex);
        }

        GetComponentInParent<ProjectCardDropTarget>().OnCardAdded(GetComponent<ProjectCard>());
        GetComponent<LayoutElement>().ignoreLayout = false;
    }

    private ProjectCardDropTarget GetHoveredDropTarget(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        GetComponentInParent<GraphicRaycaster>().Raycast(eventData, results);
        return results.FirstOrDefault(hit => hit.gameObject.GetComponent<ProjectCardDropTarget>() != null)
            .gameObject?.GetComponent<ProjectCardDropTarget>();
    }
}
