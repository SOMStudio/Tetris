using System.Collections.Generic;
using UnityEngine;

namespace Base.UI.Content
{
    public sealed class UiChildContentManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private RectTransform parentUi;
        [SerializeField] private List<GameObject> childList;
        [SerializeField] private GameObject refChild;

        public GameObject AddChild()
        {
            GameObject newChild = Instantiate(refChild, parentUi);

            childList.Add(newChild);

            return newChild;
        }

        public void RemoveChild(int numberLevel)
        {
            GameObject removeChild = childList[numberLevel];

            childList.Remove(removeChild);

            Destroy(removeChild.gameObject);
        }

        public void RemoveChildLast()
        {
            int childCount = childList.Count;

            if (childCount > 0)
                RemoveChild(childList.Count - 1);
        }
    }
}
