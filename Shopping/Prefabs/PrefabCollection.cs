using Graphics2026.Model.Actors;
using Graphics2026.Model.Mesh;
using Graphics2026.View.Shading.Shaders;
using Graphics2026.View.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Shopping.Prefabs
{
    internal static class PrefabCollection
    {
        private static List<Prefab> floors = new();
        private static List<Prefab> walls = new();

        static PrefabCollection()
        {
            GenerateFloorPrefabs();
            GenerateWallPrefabs();
        }

        private static void GenerateFloorPrefabs()
        {
            Actor actor = new Actor("Tile #1");
            actor.mesh = MeshGenerator.Cube();
            actor.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.5f, 0.02f, 0.5f));
            actor.shader = new TexturedProcedural(new Texture(Program.ASSETS + "tile_texture.jpg", TextureTarget.Texture2D)
                .AddParameter(new TexParameter(TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear))
                .AddParameter(new TexParameter(TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat))
                .AddParameter(new TexParameter(TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat)),
                10f
            );

            Prefab tilePrefab = new Prefab("Tile #1", actor, 15);
            floors.Add(tilePrefab);
        }

        private static void GenerateWallPrefabs()
        {
            Wall1();
            //Wall2();
        }

        private static void Wall1()
        {
            Actor wall = new Actor("Wall #1");
            wall.mesh = MeshGenerator.Cube();
            wall.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 0.5f) * Matrix4.CreateTranslation(0.5f, 0, 0));
            wall.shader = new DefaultLit();

            Actor rounding1 = new Actor("Wall rounding #1");
            rounding1.mesh = MeshGenerator.Cylinder(32);
            rounding1.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 0.05f) * Matrix4.CreateTranslation(0.5f, 0, 0.5f));
            rounding1.shader = wall.shader;
            rounding1.SetParent(wall);

            Actor rounding2 = new Actor("Wall rounding #2");
            rounding2.mesh = MeshGenerator.Cylinder(32);
            rounding2.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 0.05f) * Matrix4.CreateTranslation(0.5f, 0, -0.5f));
            rounding2.shader = wall.shader;
            rounding2.SetParent(wall);

            Prefab wallPrefab = new Prefab("Wall #1", wall, 40);
            walls.Add(wallPrefab);
        }
        private static void Wall2()
        {
            Actor wall = new Actor("Wall #2");
            wall.mesh = MeshGenerator.Cube();
            wall.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 1f) * Matrix4.CreateTranslation(0.5f, 0, 0));
            wall.shader = new DefaultLit();

            Actor rounding1 = new Actor("Wall rounding #1");
            rounding1.mesh = MeshGenerator.Cylinder(32);
            rounding1.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 0.05f) * Matrix4.CreateTranslation(0.5f, 0, 1f));
            rounding1.shader = wall.shader;
            rounding1.SetParent(wall);

            Actor rounding2 = new Actor("Wall rounding #2");
            rounding2.mesh = MeshGenerator.Cylinder(32);
            rounding2.mesh.BakeTransformation(Matrix4.CreateTranslation(Vector3.UnitY)
                * Matrix4.CreateScale(0.05f, 2f, 0.05f) * Matrix4.CreateTranslation(0.5f, 0, -1f));
            rounding2.shader = wall.shader;
            rounding2.SetParent(wall);

            Prefab wallPrefab = new Prefab("Wall #2", wall, 40);
            walls.Add(wallPrefab);
        }

        public static Prefab GetFloor(int index) => floors[index - 1];
        public static Prefab GetWall(int index) => walls[index - 1];
    }
}
