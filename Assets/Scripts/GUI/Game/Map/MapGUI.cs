using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MostDanger
{

    public class MapGUI : MonoBehaviour
    {

        public static MapGUI Instance { private set; get; }

        public GameObject Background;
        public GameObject MarkPrefab;

        //private int MapWidth;
        //private int MapHeight;

        private Dictionary<Transform, Transform> Objects;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            UpdateObjects();
        }

        public void Init(int mapWidth, int mapHeight)
        {
            //MapWidth = mapWidth;
            //MapHeight = mapHeight;
            Objects = new Dictionary<Transform, Transform>();
        }

        //Регистрируем объект для отображения
        //color - каким цветом будет его значок
        public void RegisterObject(Transform go, Color color)
        {
            var newObject = Instantiate(MarkPrefab) as GameObject;
            newObject.GetComponent<Image>().color = color;
            var newObjTransform = newObject.GetComponent<Transform>();
            newObjTransform.parent = Background.transform;
            newObjTransform.localScale = Vector3.one;
            newObjTransform.localPosition = new Vector3(go.transform.localPosition.x / 500 * 200, go.transform.localPosition.z / 500 * 200, 0);
            Objects.Add(newObjTransform, go);
        }

        public void DeregisterObject(GameObject go, Color color)
        {

        }

        private void UpdateObjects()
        {
            foreach (KeyValuePair<Transform, Transform> obj in Objects)
            {
                obj.Key.localPosition = new Vector3(obj.Value.localPosition.x/500*200, obj.Value.localPosition.z/500*200, 0);
            }
        }

        //Устанавливаем объект особой слежки
        public void SetActiveObject(GameObject go)
        {

        }
    }

}