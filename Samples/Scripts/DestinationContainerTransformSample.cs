using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class DestinationContainerTransformSample : BaseDestinationContainerMono<Transform>
    {
        [Button("Clear the Destination Container", EButtonEnableMode.Playmode)]
        public void ClearChildren()
        {
            RemoveAllFromDestinations();
        }

        [Button("Add Children to the Destination Container (distinct)", EButtonEnableMode.Playmode)]
        public void AddChildrenDistinct()
        {
            foreach (Transform child in transform)
            {
                AddToDestinations(child, true);
            }
        }

        [Button("Add Children to the Destination Container (non distinct)", EButtonEnableMode.Playmode)]
        public void AddChildrenNonDistinct()
        {
            foreach (Transform child in transform)
            {
                AddToDestinations(child, false);
            }
        }
    }
}
