using Microsoft.Msagl.Drawing;

namespace Expert_System_21.Visualizer
{
    public class NodeInfoVisualise: NodeAttr
    {
        public NodeInfoVisualise(Color color, Shape shape)
        {
            Color = color;
            Shape = shape;
        }
        
        public NodeInfoVisualise(Color color)
        {
            Color = color;
        }
        
        public NodeInfoVisualise(Shape shape)
        {
            Shape = shape;
        }
    }
}