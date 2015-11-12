using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TriangleGUI
{
    public class Triangle
    {
        public Triangle(Point first, Point second, Point third)
        {
            vertices[0] = first;
            vertices[1] = second;
            vertices[2] = third;
        }
        public Point[] vertices = new Point[3];
    }

    class Gui
    {
        private const int pictureWidth = 500;
        private const int pictureHeight = 500;
        private const int labelHeight = 40;
        private string pointLocation = "";

        private Triangle triangle = new Triangle(new Point(0, 0), new Point (0, 0), new Point(0, 0));
        private Point point = new Point(0, 0);

        private void setCoordinates()
        {
            string projectPath = System.IO.Directory.GetParent(
                System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
            XDocument coordsXML = XDocument.Load(projectPath + @"\coordinates.xml");
            //parsing XML
            foreach (XElement shape in coordsXML.Root.Elements())
            {
                if (shape.Name == "point")
                {
                    foreach (XElement coord in shape.Elements())
                    {
                        if (coord.Name == "x")
                            point.X = int.Parse(coord.Value);
                        else if (coord.Name == "y")
                            point.Y = int.Parse(coord.Value);
                    }
                }
                else if (shape.Name == "triangle")
                {
                    foreach (XElement vertex in shape.Elements())
                    {
                        int? vertexIndex = null;
                        if (vertex.Name == "first-vertex") vertexIndex = 0;
                        else if (vertex.Name == "second-vertex") vertexIndex = 1;
                        else if (vertex.Name == "third-vertex") vertexIndex = 2;
                        if (vertexIndex != null)
                            foreach (XElement coord in vertex.Elements())
                            {
                                if (coord.Name == "x")
                                    triangle.vertices[vertexIndex.Value].X = int.Parse(coord.Value);
                                if (coord.Name == "y")
                                    triangle.vertices[vertexIndex.Value].Y = int.Parse(coord.Value);
                            }
                    }
                }
            }
        }

        private void setPointLocation()
        {
            var location = CoreLib.TriangleCheck.CalcPointLocation(
                new CoreLib.Point(triangle.vertices[0].X, triangle.vertices[0].Y),
                new CoreLib.Point(triangle.vertices[1].X, triangle.vertices[1].Y),
                new CoreLib.Point(triangle.vertices[2].X, triangle.vertices[2].Y),
                new CoreLib.Point(point.X, point.Y));
            if (location == CoreLib.PointLocation.Inside)
                pointLocation = "Inside";
            else if (location == CoreLib.PointLocation.Outside)
                pointLocation = "Outside";
            else if (location == CoreLib.PointLocation.OnBorder)
                pointLocation = "On border";
            else if (location == CoreLib.PointLocation.OnVertex)
                pointLocation = "On vertex";
        }

        //scales coordinates of points so that they fit the picture
        private void scaleCoordinates()
        {
            var minX = System.Math.Min(System.Math.Min(triangle.vertices[0].X, triangle.vertices[1].X), 
                System.Math.Min(triangle.vertices[2].X, point.X));
            if (minX < 0)
            {
                var xIncreaseTo = -minX + pictureWidth;
                triangle.vertices[0].X += xIncreaseTo;
                triangle.vertices[1].X += xIncreaseTo;
                triangle.vertices[2].X += xIncreaseTo;
                point.X += xIncreaseTo;
            }

            var minY = System.Math.Min(System.Math.Min(triangle.vertices[0].Y, triangle.vertices[1].Y),
                System.Math.Min(triangle.vertices[2].Y, point.Y));
            if (minY < 0)
            {
                var yIncreaseTo = -minY + pictureWidth;
                triangle.vertices[0].Y += yIncreaseTo;
                triangle.vertices[1].Y += yIncreaseTo;
                triangle.vertices[2].Y += yIncreaseTo;
                point.Y += yIncreaseTo;
            }

            var maxX = System.Math.Max(System.Math.Max(triangle.vertices[0].X, triangle.vertices[1].X),
                System.Math.Max(triangle.vertices[2].X, point.X));
            if (maxX > pictureWidth)
            {
                var xDivideInto = maxX / (pictureWidth + 1);
                triangle.vertices[0].X /= xDivideInto;
                triangle.vertices[1].X /= xDivideInto;
                triangle.vertices[2].X /= xDivideInto;
                point.X /= xDivideInto;
            }

            var maxY = System.Math.Max(System.Math.Max(triangle.vertices[0].Y, triangle.vertices[1].Y),
                System.Math.Max(triangle.vertices[2].Y, point.Y));
            if (maxY > pictureHeight)
            {
                var yDivideInto = maxY / (pictureHeight + 1);
                triangle.vertices[0].Y /= yDivideInto;
                triangle.vertices[1].Y /= yDivideInto;
                triangle.vertices[2].Y /= yDivideInto;
                point.Y /= yDivideInto;
            }
        }

        //handles picture painting event
        private void picture_paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var pen = new Pen(Color.Red);
            pen.Width = 3;
            g.DrawLine(pen, triangle.vertices[0].X, triangle.vertices[0].Y,
                triangle.vertices[1].X, triangle.vertices[1].Y);
            g.DrawLine(pen, triangle.vertices[1].X, triangle.vertices[1].Y,
                triangle.vertices[2].X, triangle.vertices[2].Y);
            g.DrawLine(pen, triangle.vertices[2].X, triangle.vertices[2].Y,
                triangle.vertices[0].X, triangle.vertices[0].Y);
            pen.Dispose();
        
            SolidBrush brush = new SolidBrush(Color.Blue);
            g.FillEllipse(brush, new Rectangle(point.X, point.Y, 8, 8));
            brush.Dispose();
        }

        private Form getMainForm()
        {
            var form = new Form();
            form.BackColor = Color.PaleTurquoise;
            form.ClientSize = new Size(pictureWidth, pictureHeight + labelHeight);
            form.Text = "Triangle";
            form.Font = new Font(form.Font.Name, 20F, form.Font.Style, form.Font.Unit);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;

            //adding to main form a label displaying location of point relatively to a triangle
            var pointLocationLabel = new Label();
            pointLocationLabel.ClientSize = new Size(pictureWidth, labelHeight);
            pointLocationLabel.BackColor = Color.Silver;
            pointLocationLabel.Text = pointLocation;
            form.Controls.Add(pointLocationLabel);

            //adding a picture displaying a triangle and a point
            var TriangleAndPointPicture = new PictureBox();
            TriangleAndPointPicture.ClientSize = new Size(pictureWidth, pictureHeight);
            TriangleAndPointPicture.Paint += new PaintEventHandler(picture_paint);
            TriangleAndPointPicture.Location = new Point(0, labelHeight);
            form.Controls.Add(TriangleAndPointPicture);

            return form;
        }

        static void Main(string[] args)
        {
            var gui = new Gui();
            gui.setCoordinates();
            gui.scaleCoordinates();
            gui.setPointLocation();
            var mainForm = gui.getMainForm();
            mainForm.ShowDialog();
        }
    }
}