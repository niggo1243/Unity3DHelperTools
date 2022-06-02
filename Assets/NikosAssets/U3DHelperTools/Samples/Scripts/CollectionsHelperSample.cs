using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace NikosAssets.Helpers.Samples
{
    public class CollectionsHelperSample : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public List<int> integersList = new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public List<int> otherIntegersList = new List<int>() {100, 200, 1};

        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public CollectionHelper.ItemMatching matching = CollectionHelper.ItemMatching.MatchAtLeastOne;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_DESCRIPTIONS)]
        [ReadOnly]
        public int currentIndexOfIntegersList = 0;
        
        [Button("Log the integers list")]
        public void LogCollection()
        {
            CollectionHelper.LogCollection(integersList);
        }

        [Button("Increase (loop) index within bounds")]
        public void CountIndexUp()
        {
            currentIndexOfIntegersList =
                CollectionHelper.PointerHandler(true, currentIndexOfIntegersList, integersList.Count);
        }

        [Button("Lower (loop) index within bounds")]
        public void CountIndexDown()
        {
            currentIndexOfIntegersList =
                CollectionHelper.PointerHandler(false, currentIndexOfIntegersList, integersList.Count);
        }

        [Button("Do the lists match based on the desired matching?")]
        public void MatchCheck()
        {
            Debug.Log("Matching: " + CollectionHelper.CollectionsMatcher(matching, integersList, otherIntegersList));
        }
    }
}
