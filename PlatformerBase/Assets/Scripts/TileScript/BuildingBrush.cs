/*
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

        public GameObject ChildMap(GameObject brushTarget, BuildingBrushType brushType)
        {
            string searchName;
            //convert brush type to string
            switch (brushType)
            {
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
                default:
                    searchName = "";
                    break;
            }
            //try to find the child object
            GameObject child = GameObject.Find(brushTarget.name + searchName);
            
            //make the child object if it doesn't exist
            if(child == null)
            {
                child = new GameObject();
                child.name = brushTarget.name + searchName;
                child.transform.SetParent(brushTarget.transform);
                child.transform.localPosition = Vector3.zero;

                //add required components based on brush type
                child.AddComponent<Tilemap>();
                //add Tilemap Renderer
                if(brushType == BuildingBrushType.Detail || brushType == BuildingBrushType.Ladder)
                {
                    TilemapRenderer render;
                    render = child.AddComponent<TilemapRenderer>();
                    render.sortingLayerName = "Enviro";
                    render.sortingOrder = brushType == BuildingBrushType.Detail ? 3 : 4;
                }
                //add Rigidbody2D, Composite Collider, Tilemap Collider
                if (brushType == BuildingBrushType.Ladder || brushType == BuildingBrushType.Oneway || brushType == BuildingBrushType.Solid)
                {
                    child.layer = LayerMask.NameToLayer(searchName);
                    child.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    child.AddComponent<CompositeCollider2D>();
                    child.AddComponent<TilemapCollider2D>().usedByComposite = true;
                }
            }
            return child;
        }

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;
            
            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.Paint(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.Paint(grid, brushTarget, position);
                    base.Paint(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.Paint(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position);
                    base.Paint(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.Paint(grid, brushTarget, position);
                    break;
            }
        }
        
        public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.Erase(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.Erase(grid, brushTarget, position);
                    base.Erase(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.Erase(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position);
                    base.Erase(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.Erase(grid, brushTarget, position);
                    break;
            }
        }

        
        public override void BoxFill(GridLayout grid, GameObject brushTarget, BoundsInt position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.BoxFill(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.BoxFill(grid, brushTarget, position);
                    base.BoxFill(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.BoxFill(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position);
                    base.BoxFill(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.BoxFill(grid, brushTarget, position);
                    break;
            }
        }
        
        public override void FloodFill(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.FloodFill(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.FloodFill(grid, brushTarget, position);
                    base.FloodFill(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.FloodFill(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position);
                    base.FloodFill(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.FloodFill(grid, brushTarget, position);
                    break;
            }
        }

        /*
        public override void Select(GridLayout grid, GameObject brushTarget, BoundsInt position)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.Select(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.Select(grid, brushTarget, position);
                    base.Select(grid, ChildMap(brushTarget, brushType), position);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.Select(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position);
                    base.Select(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.Select(grid, brushTarget, position);
                    break;
            }
        }

        public override void Pick(GridLayout grid, GameObject brushTarget, BoundsInt position, Vector3Int pivot)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.Pick(grid, ChildMap(brushTarget, brushType), position, pivot);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.Pick(grid, brushTarget, position, pivot);
                    base.Pick(grid, ChildMap(brushTarget, brushType), position,pivot);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.Pick(grid, ChildMap(brushTarget, BuildingBrushType.Detail), position, pivot);
                    base.Pick(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), position, pivot);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.Pick(grid, brushTarget, position, pivot);
                    break;
            }
        }

        public override void Move(GridLayout grid, GameObject brushTarget, BoundsInt from, BoundsInt to)
        {
            // Do not allow editing palettes
            if (brushTarget.layer == 31)
                return;

            switch (brushType)
            {
                //alternate visible tilemaps
                case BuildingBrushType.Detail:
                case BuildingBrushType.Ladder:
                    base.Move(grid, ChildMap(brushTarget, brushType), from, to);
                    break;
                //alternate collision tilemaps
                case BuildingBrushType.Oneway:
                case BuildingBrushType.Solid:
                    base.Move(grid, brushTarget, from, to);
                    base.Move(grid, ChildMap(brushTarget, brushType), from, to);
                    break;
                //alternate visble and collision tilemaps
                case BuildingBrushType.DetailOneway:
                    base.Move(grid, ChildMap(brushTarget, BuildingBrushType.Detail), from, to);
                    base.Move(grid, ChildMap(brushTarget, BuildingBrushType.Oneway), from, to);
                    break;
                //normnal (directly to selected tilemap)
                case BuildingBrushType.Normal:
                default:
                    base.Move(grid, brushTarget, from, to);
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

}*/