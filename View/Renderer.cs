using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.SceneManagement;

namespace Graphics2026.View
{
    internal class Renderer
    {
        public readonly Dictionary<SurfaceType, bool> drawSurfaces = new();

        public Renderer()
        {
            foreach(SurfaceType surfaceType in Enum.GetValues(typeof(SurfaceType)))
            {
                drawSurfaces.Add(surfaceType, false);
            }
        }

        public virtual void RenderScene()
        {
            Scene scene = SceneManager.CurrentScene();
            DrawRenderableFamily(scene.GetTransform());
        }

        protected virtual void DrawRenderableFamily(Transform transform)
        {
            foreach (Transform child in transform.GetChildren())
            {
                DrawRenderable(child.GetRenderable());
                DrawRenderableFamily(child);
            }
        }

        protected virtual void DrawRenderable(IRenderable renderable)
        {
            if (!renderable.GetRenderStatus())
                return;

            if (!DrawSurface(renderable))
                return;

            renderable.Render();
        }

        private bool DrawSurface(IRenderable renderable)
        {
            if(!(renderable is Surface))
                return true;

            Surface surface = (Surface)renderable;

            bool draw = false;
            foreach (SurfaceType surfaceType in surface.GetTypes())
            {
                if (drawSurfaces[surfaceType])
                    draw = true;
            }

            return draw;
        }
    }
}
