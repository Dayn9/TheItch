using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace UnityEditor
{

    //dictates which layer(s) are currently being painted to with brush
    public enum BuildingBrushType
    {
        Normal, //paints directly to top layer
        Detail, //paints to the detail layer
        DetailOneway, //paints to detail AND oneway collider layers
        Solid, //paints to normal AND solid collider layers
        Oneway, //paints to the normal AND oneway collider layers
        Ladder //paints to the ladder collider layer
    }


    [CreateAssetMenu(fileName = "Building Brush", menuName = "Brushes/Building Brush")]
    [CustomGridBrush(false, true, false, "Building Brush")]
    public class BuildingBrush : GridBrush
    {
        public BuildingBrushType brushType;

        private GameObject detailMap;
        private GameObject onewayMap;
        private GameObject solidMap;
        private GameObject ladderMap;

        public GameObject ChildMap(GameObject brushTarget, BuildingBrushType brushType)
        {
            GameObject child;
            string searchName;
            switch (brushType)
            {
                default:
                case BuildingBrushType.Detail:
                    searchName = "Detail";
                    break;
                case BuildingBrushType.Oneway:
                    searchName = "Oneway";
                    break;
                case BuildingBrushType.Solid:
                    searchName = "Solid";
                    break;
                case BuildingBrushType.Ladder:
                    searchName = "Ladder";
                    break;
            }
            child = GameObject.Find(brushTarget.name + searchName);
            if(child == null)
            {
                child = Instantiate(new GameObject());

                child.name = brushTarget.name + searchName;
                child.transform.SetParent(brushTarget.transform);
                child.transform.localPosition = Vector3.zero;

                //add required components based on brush type
                child.AddComponent<Tilemap>();

                TilemapRenderer render;
                

                //TODO: use if(multiple types...) for rend and then for collider

                switch (brushType)
                {
                    default:
                    case BuildingBrushType.Detail:
                        render = child.AddComponent<TilemapRenderer>();
                        render.sortingLayerName = "Enviro";
                        render.sortingOrder = 3;
                        break;
                    case BuildingBrushType.Ladder:
                        render = child.AddComponent<TilemapRenderer>();
                        render.sortingLayerName = "Enviro";
                        render.sortingOrder = 4;
                        break;
                    case BuildingBrushType.Oneway:

                        break;
                    case BuildingBrushType.Solid:

                        break;
                }
            }
            return child;
        }


        public GameObject DetailMap(GameObject brushTarget)
        {
            //return the detail map if it already exists 
            if (detailMap != null && detailMap.name == brushTarget.name + "Detail") { return detailMap; }
            //find the detail map
            detailMap = GameObject.Find(brushTarget.name + "Detail");
            //make a new detail map if one doesn't already exist
            if (detailMap == null)
            {
                detailMap = Instantiate(new GameObject());

                detailMap.name = brushTarget.name + "Detail";
                detailMap.transform.SetParent(brushTarget.transform);
                detailMap.transform.localPosition = Vector3.zero;
                detailMap.AddComponent<Tilemap>();
                detailMap.AddComponent<TilemapRenderer>();
            }
            return detailMap;
        }

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            //base.Paint(grid, brushTarget, position);
            
            switch (brushType)
            {
                case BuildingBrushType.Detail:
                    //paint to the detail map 
                    base.Paint(grid, DetailMap(brushTarget), position);
                    break;
                case BuildingBrushType.Normal:
                default:
                    base.Paint(grid, brushTarget, position);
                    break;
            }

        }



        #if UNITY_EDITOR
        //create asset menu for prefab brush
        [MenuItem("Assets/Create/BuildingBrush")]
        public static void CreateBuildingBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save BuildingBrush", "New BuildingBrush", "asset", "Save BuildingBrush", "Assets");
            if (path == "") { return; }

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<BuildingBrush>(), path);
        }
        #endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BuildingBrush))]
    public class BuildingBrushEditor : GridBrushEditor
    {
        private BuildingBrush Brush { get { return (target as BuildingBrush); } }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif

}