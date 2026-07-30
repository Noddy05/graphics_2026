using Graphics2026.Controller;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Shopping.Prefabs;
using Graphics2026.View;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Shopping.Tools
{
    internal class GroundTool : BuildTool
    {
        private Prefab currentPrefab;
        private Dictionary<Grid, int[][]> floors = new();
        private int prefabIndex = 1;

        public GroundTool() : base([SurfaceType.Floor])
        {
            currentPrefab = PrefabCollection.GetFloor(prefabIndex).Instantiate();
        }

        protected override void Update(float deltaTime)
        {
            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float rayLength, [SurfaceType.Floor]);
            Grid? grid = surface as Grid;

            if (grid == null)
                return;

            Vector3 position = grid.SnapToGrid(point);
            Vector2i gridPosition = (Vector2i)Vector2.Floor(grid.PointToGridSpace(position) + grid.GridSize() / 2);

            WireRenderer.DrawLine(Vector3.Zero, point);
            currentPrefab.renderable!.GetTransform().localPosition = position;
            currentPrefab.renderable!.RenderFamilyWithShader(Builder.HIGHLIGHT_SHADER);

            if (!Input.GetMouseButtonDown(MouseButton.Left))
                return;

            if (!floors.ContainsKey(grid))
            {
                int[][] floorGrid = new int[grid.GridSize().X][];
                for (int i = 0; i < floorGrid.Length; i++)
                    floorGrid[i] = new int[grid.GridSize().Y];

                floors.Add(grid, floorGrid);
            }

            int floorIndex = floors[grid][gridPosition.X][gridPosition.Y];

            int priceDiff = -PrefabCollection.GetFloor(prefabIndex).price;
            if (floorIndex > 0)
            {
                priceDiff += PrefabCollection.GetFloor(floorIndex).price;
            }
            floors[grid][gridPosition.X][gridPosition.Y] = prefabIndex;
            Player.ChangeBalance(priceDiff, position);

            Prefab placed = currentPrefab.Instantiate();
            SceneManager.AddToScene(placed.renderable);
        }
    }
}
