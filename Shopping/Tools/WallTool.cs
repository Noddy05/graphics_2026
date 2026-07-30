using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Shopping.Prefabs;
using Graphics2026.View;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Shopping
{
    internal class WallTool : BuildTool
    {
        private Prefab currentPrefab;
        private Dictionary<Grid, int[][]> xWalls = new();
        private Dictionary<Grid, int[][]> zWalls = new();
        private bool directionIsZ = true;
        private int prefabIndex = 1;

        public WallTool() : base([ SurfaceType.Floor ])
        {
            currentPrefab = PrefabCollection.GetWall(prefabIndex).Instantiate();
        }

        private Dictionary<Grid, int[][]> WallAxis() => directionIsZ ? zWalls : xWalls;

        protected override void Update(float deltaTime)
        {

            if(Input.GetKeyDown(Keys.R))
                directionIsZ = !directionIsZ;

            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float rayLength, [SurfaceType.Floor]);
            Grid? grid = surface as Grid;

            if (grid == null)
                return;


            if (WallAxis().ContainsKey(grid))
            {
                for (int x = 0; x < WallAxis()[grid].Length; x++)
                {
                    for (int y = 0; y < WallAxis()[grid][x].Length; y++)
                    {
                        if (WallAxis()[grid][x][y] != 0)
                        {
                            WireRenderer.SetColor(Color4.Red);
                            WireRenderer.DrawInFront(true);
                            WireRenderer.DrawSphere(grid.PointToWorldSpace(new Vector2(x, y) - grid.GridSize() / 2), 0.05f);
                            WireRenderer.DrawInFront(false);
                        }
                    }
                }
            }
            

            Transform prefabTransform = currentPrefab.renderable!.GetTransform();
            prefabTransform.localRotation.Y = directionIsZ ? 90f : 0;

            Vector3 position = grid.SnapToGrid(point + 0.5f *
                prefabTransform.Left());
            Vector2i gridPosition = (Vector2i)Vector2.Floor(grid.PointToGridSpace(position) 
                 + grid.GridSize() / 2) + new Vector2i(1, 0);

            WireRenderer.SetColor(Color4.White);
            WireRenderer.DrawLine(Vector3.Zero, point);
            prefabTransform.localPosition = position;
            currentPrefab.renderable!.RenderFamilyWithShader(Builder.HIGHLIGHT_SHADER);

            if (!Input.GetMouseButtonDown(MouseButton.Left))
                return;

            if (!WallAxis().ContainsKey(grid))
            {
                int[][] floorGrid = new int[grid.GridSize().X + 1][];
                for (int i = 0; i < floorGrid.Length; i++)
                    floorGrid[i] = new int[grid.GridSize().Y + 1];

                WallAxis().Add(grid, floorGrid);
            }

            Console.WriteLine(gridPosition);
            int floorIndex = WallAxis()[grid][gridPosition.X][gridPosition.Y];

            int priceDiff = -PrefabCollection.GetWall(prefabIndex).price;
            if (floorIndex > 0)
            {
                priceDiff += PrefabCollection.GetWall(floorIndex).price;
            }
            WallAxis()[grid][gridPosition.X][gridPosition.Y] = prefabIndex;
            Player.ChangeBalance(priceDiff, position);

            Prefab placed = currentPrefab.Instantiate();
            SceneManager.AddToScene(placed.renderable);
        }
    }
}
