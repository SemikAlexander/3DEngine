using System;
using System.Drawing;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace _3DEngine
{
    public partial class Main : Form
    {
        public long MinCoordinate = long.MinValue;
        public long MaxCoordinate = long.MaxValue;

        public Scene scene = null;
        private int Rotation = 0;
        private bool IsPan = false;
        private Camera tempCamera = null;

        private SceneObject currentObject = null;
        private ScenePrimitive currentPrimitive = null;

        public Main()
        {
            InitializeComponent();

            scene = new Scene(RenderPicture);

            IsUpdating = true;

            CameraTargetX.Minimum = CameraTargetY.Minimum = CameraTargetZ.Minimum = MinCoordinate;
            CameraPositionX.Minimum = CameraPositionY.Minimum = CameraPositionZ.Minimum = MinCoordinate;

            CameraTargetX.Maximum = CameraTargetY.Maximum = CameraTargetZ.Maximum = MaxCoordinate;
            CameraPositionX.Maximum = CameraPositionY.Maximum = CameraPositionZ.Maximum = MaxCoordinate;

            PositionX.Minimum = PositionY.Minimum = PositionZ.Minimum = MinCoordinate;
            PositionX.Maximum = PositionY.Maximum = PositionZ.Maximum = MaxCoordinate;

            IsUpdating = false;
            RenderPicture.MouseWheel += new MouseEventHandler(Canvas_MouseWheel);
        }

        private void ResetCamera_Click(object sender, EventArgs e)
        {
            scene.ResetCamera();
            UpdateCameraValues();
            scene.PaintObjects();
        }

        bool IsUpdating = false;

        private void UpdateCameraValues()
        {
            IsUpdating = true;

            CameraTargetX.Value = (decimal)scene.Camera.Target.X;
            CameraTargetY.Value = (decimal)scene.Camera.Target.Y;
            CameraTargetZ.Value = (decimal)scene.Camera.Target.Z;

            CameraPositionX.Value = (decimal)scene.Camera.Position.X;
            CameraPositionY.Value = (decimal)scene.Camera.Position.Y;
            CameraPositionZ.Value = (decimal)scene.Camera.Position.Z;

            IsUpdating = false;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {

        }

        private bool IsCanvasAction = false;
        private bool LeftMouse = false;
        private bool RightMouse = false;
        private Point start;

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            IsCanvasAction = true;
            if (e.Button == MouseButtons.Left && !RightMouse)
            {
                LeftMouse = true;
            }
            else if (e.Button == MouseButtons.Right && !LeftMouse)
            {
                RightMouse = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsCanvasAction)
            {
                int dx = e.Location.X - start.X;
                int dy = e.Location.Y - start.Y;

                if (LeftMouse)
                {
                    if (Rotation == 0)
                    {
                        scene.Camera.RotatePositionLeftRight(-dx);
                        scene.Camera.RotatePositionUpDown(-dy);
                    }
                    else
                    {
                        scene.Camera.RotateTargetLeftRight(-dx);
                        scene.Camera.RotateTargetUpDown(dy * 25);
                    }
                }
                else if (RightMouse)
                {
                    scene.Camera.MoveCameraLeftRight(dx);
                    scene.Camera.MoveCameraUpDown(-dy);
                }

                start = e.Location;

                UpdateCameraValues();
                scene.PaintObjects();
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            IsCanvasAction = false;
            if (e.Button == MouseButtons.Left)
            {
                LeftMouse = false;
            }
            else if (e.Button == MouseButtons.Right)
            {
                RightMouse = false;
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                Rotation = Rotation == 0 ? 1 : 0;
            }
        }

        private void Canvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (scene.Camera.IsCentralProjection)
            {
                scene.Camera.MoveCameraAlongVector(e.Delta / 12);
                UpdateCameraValues();
                scene.PaintObjects();
            }
        }

        private void CoordinateBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != 8)
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar == '-')
                {
                    if (box.TextLength > 0 && (box.SelectionStart != 0 || box.SelectionLength == 0 && box.Text[0] == '-'))
                    {
                        e.Handled = true;
                    }
                }
                else if (char.IsDigit(e.KeyChar))
                {
                    if (box.TextLength > 0 && box.SelectionStart > 0 && box.Text[0] == '0')
                    {
                        int start = box.SelectionStart;
                        box.Text = box.Text.Substring(1);
                        box.SelectionStart = start - 1;
                    }
                    else if (box.TextLength > 1 && box.SelectionStart > 1 && box.Text[0] == '-' && box.Text[1] == '0')
                    {
                        int start = box.SelectionStart;
                        box.Text = "-" + box.Text.Substring(2);
                        box.SelectionStart = start - 1;
                    }
                }
            }
        }

        private void CoordinateBox_Leave(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            int value = 0;
            if (!int.TryParse(box.Text, out value))
            {
                box.Text = "0";
            }
        }

        private void ObjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPrimitive = null;
            currentObject = scene.GetObjectByName(ObjectsList.SelectedItem.ToString());
            UpdateCameraValues();
            DeleteObject.Enabled = true;
            SpacePanel.Enabled = true;
            PrimitivesList.Items.Clear();
            PrimitivesList.Items.Add("");
            PrimitivesList.SelectedIndex = 0;
            if (!currentObject.Name.Equals("Lorenz"))
            {
                if (!(currentObject is TriplaneObject))
                {
                    foreach (ScenePrimitive scenePrimitive in currentObject.ScenePrimitives)
                    {
                        PrimitivesList.Items.Add(scenePrimitive.Name);
                    }
                    ConsoleParameters.Visible = ConsoleParameters.Enabled = false;
                }
                else
                {
                    WingWidth.Value = (decimal)((TriplaneObject)currentObject).WingWidth;
                    widthLowerChassis.Value = (decimal)((TriplaneObject)currentObject).widthLowerChassis;
                    widthVerticalWingSupports.Value = (decimal)((TriplaneObject)currentObject).widthVerticalWingSupports;
                    radiusChassis.Value = (decimal)((TriplaneObject)currentObject).radiusChassis;
                    widthLop.Value = (decimal)((TriplaneObject)currentObject).widthLop;
                    Sit.Value = (decimal)((TriplaneObject)currentObject).Sit;
                    widthVerticalBackWing.Value = (decimal)((TriplaneObject)currentObject).widthVerticalBackWing;
                    ConsoleParameters.Visible = ConsoleParameters.Enabled = true;
                    ParametersPanel.Visible = ParametersPanel.Enabled = false;
                }
            }
        }

        private void PositionX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.Primitive.BasePoint.X = (double)PositionX.Value;
                }
                else
                {
                    currentObject.BasePoint.X = (double)PositionX.Value;
                }
                scene.PaintObjects();
            }
        }

        private void PositionY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.Primitive.BasePoint.Y = (double)PositionY.Value;
                }
                else
                {
                    currentObject.BasePoint.Y = (double)PositionY.Value;
                }
                scene.PaintObjects();
            }
        }

        private void PositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.Primitive.BasePoint.Z = (double)PositionZ.Value;
                }
                else
                {
                    currentObject.BasePoint.Z = (double)PositionZ.Value;
                }
                scene.PaintObjects();
            }
        }

        private void RotateX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.AngleX = (int)RotateX.Value;
                }
                else
                {
                    currentObject.AngleX = (int)RotateX.Value;
                }
                scene.PaintObjects();
            }
        }

        private void RotateY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.AngleY = (int)RotateY.Value;
                }
                else
                {
                    currentObject.AngleY = (int)RotateY.Value;
                }
                scene.PaintObjects();
            }
        }

        private void RotateZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.AngleZ = (int)RotateZ.Value;
                }
                else
                {
                    currentObject.AngleZ = (int)RotateZ.Value;
                }
                scene.PaintObjects();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                MainTimer.Enabled = false;
            }
            else
            {
                MainTimer.Enabled = true;
                if (scene != null)
                {
                    scene.Camera.ResizeFrame(RenderPicture.ClientSize.Width, RenderPicture.ClientSize.Height);
                    scene.PaintObjects();
                }
            }
        }

        private void AddObject_Click(object sender, EventArgs e)
        {
            AddObject form = new AddObject(this, null);
            form.ShowDialog();
        }

        private void AddPrimitive_Click(object sender, EventArgs e)
        {
            //CreateObject form = new CreateObject(this, currentObject);
            //form.ShowDialog();
        }

        private void PrimitivesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsUpdating = true;
            RadiusBox.Enabled = false;
            SegmentsBox.Enabled = false;
            WidthBox.Enabled = false;
            HeightBox.Enabled = false;
            LengthBox.Enabled = false;
            RadiusBox.Value = WidthBox.Value = HeightBox.Value = LengthBox.Value = 25;
            SegmentsBox.Value = 16;
            ColorBox.BackColor = Color.LightGray;
            ParametersPanel.Visible = ParametersPanel.Enabled = ColorBox.Enabled = PrimitivesList.SelectedIndex != 0;
            if (PrimitivesList.SelectedIndex == 0)
            {
                currentPrimitive = null;
            }
            else
            {
                string name = PrimitivesList.SelectedItem.ToString();
                currentPrimitive = currentObject.GetScenePrimitiveByName(name);
                if (currentPrimitive.Primitive is Box)
                {
                    Box primitive = (Box)currentPrimitive.Primitive;
                    WidthBox.Enabled = true;
                    WidthBox.Value = (decimal)primitive.Width;
                    HeightBox.Enabled = true;
                    HeightBox.Value = (decimal)primitive.Height;
                    LengthBox.Enabled = true;
                    LengthBox.Value = (decimal)primitive.Length;
                }
                else if (currentPrimitive.Primitive is Cylinder)
                {
                    Cylinder primitive = (Cylinder)currentPrimitive.Primitive;
                    RadiusBox.Enabled = true;
                    RadiusBox.Value = (decimal)primitive.Radius;
                    HeightBox.Enabled = true;
                    HeightBox.Value = (decimal)primitive.Height;
                    SegmentsBox.Enabled = true;
                    SegmentsBox.Value = (decimal)primitive.SegmentsCount;
                }
                else if (currentPrimitive.Primitive is Sphere)
                {
                    Sphere primitive = (Sphere)currentPrimitive.Primitive;
                    RadiusBox.Enabled = true;
                    RadiusBox.Value = (decimal)primitive.Radius;
                    SegmentsBox.Enabled = true;
                    SegmentsBox.Value = (decimal)primitive.SegmentsCount;
                }
                else if (currentPrimitive.Primitive is Hemisphere)
                {
                    Hemisphere primitive = (Hemisphere)currentPrimitive.Primitive;
                    RadiusBox.Enabled = true;
                    RadiusBox.Value = (decimal)primitive.Radius;
                    SegmentsBox.Enabled = true;
                    SegmentsBox.Value = (decimal)primitive.SegmentsCount;
                }
                ColorBox.BackColor = currentPrimitive.Primitive.Color;
            }
            RotateX.Value = currentPrimitive != null ? currentPrimitive.AngleX : currentObject.AngleX;
            RotateY.Value = currentPrimitive != null ? currentPrimitive.AngleY : currentObject.AngleY;
            RotateZ.Value = currentPrimitive != null ? currentPrimitive.AngleZ : currentObject.AngleZ;
            PositionX.Value = currentPrimitive != null ? (int)currentPrimitive.Primitive.BasePoint.X : (int)currentObject.BasePoint.X;
            PositionY.Value = currentPrimitive != null ? (int)currentPrimitive.Primitive.BasePoint.Y : (int)currentObject.BasePoint.Y;
            PositionZ.Value = currentPrimitive != null ? (int)currentPrimitive.Primitive.BasePoint.Z : (int)currentObject.BasePoint.Z;
            ScaleX.Value = currentPrimitive != null ? (decimal)currentPrimitive.ScaleX : (decimal)currentObject.ScaleX;
            ScaleY.Value = currentPrimitive != null ? (decimal)currentPrimitive.ScaleY : (decimal)currentObject.ScaleY;
            ScaleZ.Value = currentPrimitive != null ? (decimal)currentPrimitive.ScaleZ : (decimal)currentObject.ScaleZ;
            IsUpdating = false;
        }

        private void DeletePrimitive_Click(object sender, EventArgs e)
        {
            currentObject.ScenePrimitives.Remove(currentPrimitive);
            int index = PrimitivesList.SelectedIndex;
            PrimitivesList.Items.RemoveAt(index);
            PrimitivesList.SelectedIndex = index - 1;
        }

        private void DeleteObject_Click(object sender, EventArgs e)
        {
            scene.Objects.Remove(currentObject);
            int index = ObjectsList.SelectedIndex;
            ObjectsList.Items.RemoveAt(index);
            ObjectsList.SelectedIndex = index - 1;
            if (ObjectsList.SelectedIndex == -1 && ObjectsList.Items.Count > 0)
            {
                ObjectsList.SelectedIndex = 0;
            }
            if (ObjectsList.Items.Count == 0)
            {
                DeleteObject.Enabled = false;
                PrimitivesList.Items.Clear();
                SpacePanel.Enabled = false;
                PositionX.Value = PositionY.Value = PositionZ.Value = 0;
                RotateX.Value = RotateY.Value = RotateZ.Value = 0;
                ScaleX.Value = 1;
                ScaleY.Value = 1;
                ScaleZ.Value = 1;
                ParametersPanel.Visible = ParametersPanel.Enabled = false;
                ConsoleParameters.Visible = ConsoleParameters.Enabled = false;
            }
            RenderPicture.Image = null;
        }

        private void ColorBox_Click(object sender, EventArgs e)
        {
            if (ColorPicker.ShowDialog() == DialogResult.OK)
            {
                ColorBox.BackColor = ColorPicker.Color;
                currentPrimitive.Primitive.Color = ColorPicker.Color;
                scene.PaintObjects();
            }
        }

        private void RadiusBox_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive.Primitive is Cylinder)
                {
                    Cylinder primitive = (Cylinder)currentPrimitive.Primitive;
                    primitive.ModifyRadius((int)RadiusBox.Value);
                }
                else if (currentPrimitive.Primitive is Sphere)
                {
                    Sphere primitive = (Sphere)currentPrimitive.Primitive;
                    primitive.ModifyRadius((int)RadiusBox.Value);
                }
                else if (currentPrimitive.Primitive is Hemisphere)
                {
                    Hemisphere primitive = (Hemisphere)currentPrimitive.Primitive;
                    primitive.ModifyRadius((int)RadiusBox.Value);
                }
                currentPrimitive.SetScale(currentPrimitive.ScaleX, currentPrimitive.ScaleY, currentPrimitive.ScaleZ);
                scene.PaintObjects();
            }
        }

        private void SegmentsBox_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive.Primitive is Cylinder)
                {
                    Cylinder primitive = (Cylinder)currentPrimitive.Primitive;
                    primitive.ModifySegmentsCount((int)SegmentsBox.Value);
                }
                else if (currentPrimitive.Primitive is Sphere)
                {
                    Sphere primitive = (Sphere)currentPrimitive.Primitive;
                    primitive.ModifySegmentsCount((int)SegmentsBox.Value);
                }
                else if (currentPrimitive.Primitive is Hemisphere)
                {
                    Hemisphere primitive = (Hemisphere)currentPrimitive.Primitive;
                    primitive.ModifySegmentsCount((int)SegmentsBox.Value);
                }
                currentPrimitive.SetScale(currentPrimitive.ScaleX, currentPrimitive.ScaleY, currentPrimitive.ScaleZ);
                scene.PaintObjects();
            }
        }

        private void HeightBox_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive.Primitive is Box)
                {
                    Box primitive = (Box)currentPrimitive.Primitive;
                    primitive.ModifyHeight((int)HeightBox.Value);
                }
                else if (currentPrimitive.Primitive is Cylinder)
                {
                    Cylinder primitive = (Cylinder)currentPrimitive.Primitive;
                    primitive.ModifyHeight((int)HeightBox.Value);
                }
                currentPrimitive.SetScale(currentPrimitive.ScaleX, currentPrimitive.ScaleY, currentPrimitive.ScaleZ);
                scene.PaintObjects();
            }
        }

        private void WidthBox_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive.Primitive is Box)
                {
                    Box primitive = (Box)currentPrimitive.Primitive;
                    primitive.ModifyWidth((int)WidthBox.Value);
                }
                currentPrimitive.SetScale(currentPrimitive.ScaleX, currentPrimitive.ScaleY, currentPrimitive.ScaleZ);
                scene.PaintObjects();
            }
        }

        private void LengthBox_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive.Primitive is Box)
                {
                    Box primitive = (Box)currentPrimitive.Primitive;
                    primitive.ModifyLength((int)LengthBox.Value);
                }
                currentPrimitive.SetScale(currentPrimitive.ScaleX, currentPrimitive.ScaleY, currentPrimitive.ScaleZ);
                scene.PaintObjects();
            }
        }

        private void Pan_Click(object sender, EventArgs e)
        {
            if (!IsPan)
            {
                tempCamera = scene.Camera;
                scene.ResetCamera();
                scene.Camera.Target.Set(
                    currentObject.BasePoint.X,
                    currentObject.BasePoint.Y,
                    currentObject.BasePoint.Z
                    );
                scene.Camera.Position.Set(
                    currentObject.BasePoint.X,
                    currentObject.BasePoint.Y + currentObject.MaxLength * 2,
                    currentObject.BasePoint.Z - Math.Max(currentObject.MaxLength + scene.Camera.NearClippingPlaneZ, 100)
                    );
                scene.Camera.FarClippingPlaneZ = Math.Max(scene.Camera.FarClippingPlaneZ, currentObject.MaxLength * 1.5);
                RenderPicture.Enabled = false;
                scene.PanObject = currentObject;
                IsPan = true;
            }
            else
            {
                IsPan = false;
                scene.Camera = tempCamera;
                SpacePanel.Enabled = ObjectsList.SelectedIndex >= 0;
                if (!(currentObject is TriplaneObject))
                {
                    ParametersPanel.Enabled = PrimitivesList.SelectedIndex >= 0;
                }
                else
                {
                    ConsoleParameters.Enabled = true;
                }
                RenderPicture.Enabled = true;
                scene.PanObject = null;
            }
        }

        private void PositionX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionX.Value = 0;
                scene.PaintObjects();
            }
        }

        private void PositionY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionY.Value = 0;
                scene.PaintObjects();
            }
        }

        private void PositionZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PositionZ.Value = 0;
                scene.PaintObjects();
            }
        }

        private void RotateX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateX.Value = 0;
                scene.PaintObjects();
            }
        }

        private void RotateY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateY.Value = 0;
                scene.PaintObjects();
            }
        }

        private void RotateZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RotateZ.Value = 0;
                scene.PaintObjects();
            }
        }

        private void CameraTargetX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void CameraTargetY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void CameraTargetZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Target.Set(
                (double)CameraTargetX.Value,
                (double)CameraTargetY.Value,
                (double)CameraTargetZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void CameraPositionX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void CameraPositionY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void CameraPositionZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                scene.Camera.Position.Set(
                (double)CameraPositionX.Value,
                (double)CameraPositionY.Value,
                (double)CameraPositionZ.Value
                );
                scene.PaintObjects();
            }
        }

        private void ScaleX_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                else
                {
                    currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                scene.PaintObjects();
            }
        }

        private void ScaleY_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                else
                {
                    currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                scene.PaintObjects();
            }
        }

        private void ScaleZ_ValueChanged(object sender, EventArgs e)
        {
            if (!IsUpdating)
            {
                if (currentPrimitive != null)
                {
                    currentPrimitive.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                else
                {
                    currentObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
                }
                scene.PaintObjects();
            }
        }

        private void ScaleX_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleX.Value = 1;
                scene.PaintObjects();
            }
        }

        private void ScaleY_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleY.Value = 1;
                scene.PaintObjects();
            }
        }

        private void ScaleZ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ScaleZ.Value = 1;
                scene.PaintObjects();
            }
        }

        private void SecondDisplayDiagonal_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).WingWidth = (double)WingWidth.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void ManipulatorRadius_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).widthLowerChassis = (double)widthLowerChassis.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void ManipulatorBaseRadius_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).widthVerticalWingSupports = (double)widthVerticalWingSupports.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void CylindersHeight_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).radiusChassis = (double)radiusChassis.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void CylindersRadius_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).widthLop = (double)widthLop.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void CrossButtonSize_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).Sit = (double)Sit.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void VolumeSpace_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).widthVerticalBackWing = (double)widthVerticalBackWing.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void ExportScene_Click(object sender, EventArgs e)
        {
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("es-ES");

                    XDocument document = new XDocument();

                    XElement scene1 = new XElement("scene");

                    XElement cameras = new XElement("cameras");
                    for (int i = 0; i < scene.Cameras.Count; ++i)
                    {
                        Camera camera = scene.Cameras[i];
                        XElement cameraElement = new XElement("camera");

                        cameraElement.SetElementValue("central-projection", camera.IsCentralProjection);

                        XElement position = new XElement("position");
                        position.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Position.X));
                        position.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Position.Y));
                        position.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Position.Z));
                        cameraElement.Add(position);

                        XElement target = new XElement("target");
                        target.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Target.X));
                        target.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Target.Y));
                        target.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Target.Z));
                        cameraElement.Add(target);

                        XElement clippingPlanes = new XElement("clipping-planes");
                        clippingPlanes.SetAttributeValue("near", camera.NearClippingPlaneZ);
                        clippingPlanes.SetAttributeValue("far", camera.FarClippingPlaneZ);
                        cameraElement.Add(clippingPlanes);

                        cameraElement.SetElementValue("fov", camera.FOV);

                        cameras.Add(cameraElement);
                    }
                    scene1.Add(cameras);

                    XElement lights = new XElement("lights");
                    for (int i = 0; i < scene.Lights.Count; ++i)
                    {
                        Vector3D light = scene.Lights[i];
                        XElement lightElement = new XElement("light");
                        lightElement.SetAttributeValue("x", string.Format(format, "{0:0.00}", light.X));
                        lightElement.SetAttributeValue("y", string.Format(format, "{0:0.00}", light.Y));
                        lightElement.SetAttributeValue("z", string.Format(format, "{0:0.00}", light.Z));
                        lights.Add(lightElement);
                    }
                    scene1.Add(lights);

                    XElement objects = new XElement("objects");
                    foreach (SceneObject sceneObject in scene.Objects)
                    {
                        XElement objectElement;

                        if (sceneObject is TriplaneObject)
                        {
                            TriplaneObject triplaneObject = (TriplaneObject)sceneObject;
                            objectElement = new XElement("TriplaneObject-object");
                        }
                        else
                        {
                            objectElement = new XElement("scene-object");
                            XElement scenePrimitivesElement = new XElement("scene-primitives");
                            foreach (ScenePrimitive scenePrimitive in sceneObject.ScenePrimitives)
                            {
                                XElement scenePrimitiveElement = new XElement("scene-primitive");

                                XElement primitiveRotate = new XElement("rotate");
                                primitiveRotate.SetAttributeValue("x", scenePrimitive.AngleX);
                                primitiveRotate.SetAttributeValue("y", scenePrimitive.AngleY);
                                primitiveRotate.SetAttributeValue("z", scenePrimitive.AngleZ);
                                scenePrimitiveElement.Add(primitiveRotate);

                                XElement primitiveScale = new XElement("scale");
                                primitiveScale.SetAttributeValue("x", string.Format(format, "{0:0.00}", scenePrimitive.ScaleX));
                                primitiveScale.SetAttributeValue("y", string.Format(format, "{0:0.00}", scenePrimitive.ScaleY));
                                primitiveScale.SetAttributeValue("z", string.Format(format, "{0:0.00}", scenePrimitive.ScaleZ));
                                scenePrimitiveElement.Add(primitiveScale);

                                XElement primitiveElement = new XElement("primitive");

                                XElement primitiveBasePoint = new XElement("base-point");
                                primitiveBasePoint.SetAttributeValue("x", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.X));
                                primitiveBasePoint.SetAttributeValue("y", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.Y));
                                primitiveBasePoint.SetAttributeValue("z", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.Z));
                                primitiveElement.Add(primitiveBasePoint);

                                XElement primitiveColor = new XElement("color");
                                primitiveColor.SetAttributeValue("r", scenePrimitive.Primitive.Color.R);
                                primitiveColor.SetAttributeValue("g", scenePrimitive.Primitive.Color.G);
                                primitiveColor.SetAttributeValue("b", scenePrimitive.Primitive.Color.B);
                                primitiveElement.Add(primitiveColor);

                                if (scenePrimitive.Primitive is Box)
                                {
                                    Box box = (Box)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "box");
                                    primitiveElement.SetElementValue("width", string.Format(format, "{0:0.00}", box.Width));
                                    primitiveElement.SetElementValue("height", string.Format(format, "{0:0.00}", box.Height));
                                    primitiveElement.SetElementValue("length", string.Format(format, "{0:0.00}", box.Length));
                                }
                                else if (scenePrimitive.Primitive is Cylinder)
                                {
                                    Cylinder cylinder = (Cylinder)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "cylinder");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", cylinder.Radius));
                                    primitiveElement.SetElementValue("height", string.Format(format, "{0:0.00}", cylinder.Height));
                                    primitiveElement.SetElementValue("segments-count", cylinder.SegmentsCount);
                                }
                                else if (scenePrimitive.Primitive is Sphere)
                                {
                                    Sphere sphere = (Sphere)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "sphere");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", sphere.Radius));
                                    primitiveElement.SetElementValue("segments-count", sphere.SegmentsCount);
                                }
                                else if (scenePrimitive.Primitive is Hemisphere)
                                {
                                    Hemisphere hemisphere = (Hemisphere)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "hemisphere");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", hemisphere.Radius));
                                    primitiveElement.SetElementValue("segments-count", hemisphere.SegmentsCount);
                                }

                                scenePrimitiveElement.Add(primitiveElement);
                                scenePrimitiveElement.SetAttributeValue("name", scenePrimitive.Name);
                                scenePrimitivesElement.Add(scenePrimitiveElement);
                            }

                            objectElement.Add(scenePrimitivesElement);
                        }

                        objectElement.SetAttributeValue("name", sceneObject.Name);

                        XElement objectPosition = new XElement("position");
                        objectPosition.SetAttributeValue("x", string.Format(format, "{0:0.00}", sceneObject.BasePoint.X));
                        objectPosition.SetAttributeValue("y", string.Format(format, "{0:0.00}", sceneObject.BasePoint.Y));
                        objectPosition.SetAttributeValue("z", string.Format(format, "{0:0.00}", sceneObject.BasePoint.Z));
                        objectElement.Add(objectPosition);

                        XElement objectRotate = new XElement("rotate");
                        objectRotate.SetAttributeValue("x", sceneObject.AngleX);
                        objectRotate.SetAttributeValue("y", sceneObject.AngleY);
                        objectRotate.SetAttributeValue("z", sceneObject.AngleZ);
                        objectElement.Add(objectRotate);

                        XElement objectScale = new XElement("scale");
                        objectScale.SetAttributeValue("x", string.Format(format, "{0:0.00}", sceneObject.ScaleX));
                        objectScale.SetAttributeValue("y", string.Format(format, "{0:0.00}", sceneObject.ScaleY));
                        objectScale.SetAttributeValue("z", string.Format(format, "{0:0.00}", sceneObject.ScaleZ));
                        objectElement.Add(objectScale);

                        objects.Add(objectElement);
                    }
                    scene1.Add(objects);

                    document.Add(scene);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unable to export scene!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportScene_Click(object sender, EventArgs e)
        {
            if (ImportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("es-ES");

                    XDocument document = XDocument.Load(ImportDialog.FileName);

                    XElement scene1 = document.Element("scene");

                    scene.Cameras.Clear();
                    foreach (XElement cameraElement in scene1.Element("cameras").Elements("camera"))
                    {
                        scene.Camera = new Camera();
                        scene.ResetCamera();

                        scene.Camera.IsCentralProjection = bool.Parse(cameraElement.Element("central-projection").Value);

                        XElement position = cameraElement.Element("position");
                        scene.Camera.Position = new Point3DSpherical(new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            ));

                        XElement target = cameraElement.Element("target");
                        scene.Camera.Target = new Point3DSpherical(new Point3D(
                            double.Parse(target.Attribute("x").Value, format),
                            double.Parse(target.Attribute("y").Value, format),
                            double.Parse(target.Attribute("z").Value, format)
                            ));

                        scene.Camera.RotatePositionLeftRight(0);
                        scene.Camera.RotatePositionUpDown(0);

                        XElement clippingPlanes = cameraElement.Element("clipping-planes");
                        scene.Camera.NearClippingPlaneZ = double.Parse(clippingPlanes.Attribute("near").Value, format);
                        scene.Camera.FarClippingPlaneZ = double.Parse(clippingPlanes.Attribute("far").Value, format);

                        scene.Camera.FOV = double.Parse(cameraElement.Element("fov").Value, format);

                        scene.Cameras.Add(scene.Camera);
                    }

                    scene.Lights.Clear();
                    foreach (XElement lightElement in scene1.Element("lights").Elements("light"))
                    {
                        scene.Light = new Vector3D(
                            double.Parse(lightElement.Attribute("x").Value, format),
                            double.Parse(lightElement.Attribute("y").Value, format),
                            double.Parse(lightElement.Attribute("z").Value, format)
                            );

                        scene.Lights.Add(scene.Light);
                    }

                    scene.Objects.Clear();
                    ObjectsList.Items.Clear();
                    PrimitivesList.Items.Clear();
                    ParametersPanel.Visible = ParametersPanel.Enabled = ConsoleParameters.Visible = ConsoleParameters.Enabled = false;
                    foreach (XElement sceneObjectElement in scene1.Element("objects").Elements("scene-object"))
                    {
                        SceneObject sceneObject = new SceneObject(sceneObjectElement.Attribute("name").Value);

                        XElement position = sceneObjectElement.Element("position");
                        sceneObject.BasePoint = new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            );

                        XElement rotate = sceneObjectElement.Element("rotate");
                        sceneObject.AngleX = int.Parse(rotate.Attribute("x").Value);
                        sceneObject.AngleY = int.Parse(rotate.Attribute("y").Value);
                        sceneObject.AngleZ = int.Parse(rotate.Attribute("z").Value);

                        XElement scale = sceneObjectElement.Element("scale");
                        sceneObject.SetScale(
                            double.Parse(scale.Attribute("x").Value, format),
                            double.Parse(scale.Attribute("y").Value, format),
                            double.Parse(scale.Attribute("z").Value, format)
                            );

                        foreach (XElement scenePrimitiveElement in sceneObjectElement.Element("scene-primitives").Elements("scene-primitive"))
                        {
                            Primitive primitive = null;

                            XElement primitiveElement = scenePrimitiveElement.Element("primitive");
                            string type = primitiveElement.Attribute("type").Value;

                            XElement basePointElement = primitiveElement.Element("base-point");
                            Point3D basePoint = new Point3D(
                                double.Parse(basePointElement.Attribute("x").Value, format),
                                double.Parse(basePointElement.Attribute("y").Value, format),
                                double.Parse(basePointElement.Attribute("z").Value, format)
                                );

                            XElement colorElement = primitiveElement.Element("color");
                            Color color = Color.FromArgb(
                                int.Parse(colorElement.Attribute("r").Value),
                                int.Parse(colorElement.Attribute("g").Value),
                                int.Parse(colorElement.Attribute("b").Value)
                                );

                            switch (type)
                            {
                                case "box":
                                    primitive = new Box(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("width").Value, format),
                                        double.Parse(primitiveElement.Element("height").Value, format),
                                        double.Parse(primitiveElement.Element("length").Value, format),
                                        color
                                        );
                                    break;
                                case "cylinder":
                                    primitive = new Cylinder(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        double.Parse(primitiveElement.Element("height").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                                case "sphere":
                                    primitive = new Sphere(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                                case "hemisphere":
                                    primitive = new Hemisphere(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                            }

                            ScenePrimitive scenePrimitive = new ScenePrimitive(primitive, scenePrimitiveElement.Attribute("name").Value);

                            XElement primitiveRotate = scenePrimitiveElement.Element("rotate");
                            scenePrimitive.AngleX = int.Parse(primitiveRotate.Attribute("x").Value);
                            scenePrimitive.AngleY = int.Parse(primitiveRotate.Attribute("y").Value);
                            scenePrimitive.AngleZ = int.Parse(primitiveRotate.Attribute("z").Value);

                            XElement primitiveScale = scenePrimitiveElement.Element("scale");
                            scenePrimitive.SetScale(
                                double.Parse(primitiveScale.Attribute("x").Value, format),
                                double.Parse(primitiveScale.Attribute("y").Value, format),
                                double.Parse(primitiveScale.Attribute("z").Value, format)
                                );

                            sceneObject.AddScenePrimitive(scenePrimitive);
                        }

                        scene.AddObject(sceneObject);
                        ObjectsList.Items.Add(sceneObject.Name);
                    }

                    foreach (XElement triplaneObjectElement in scene1.Element("objects").Elements("console-object"))
                    {
                        TriplaneObject triplaneObject = new TriplaneObject(triplaneObjectElement.Attribute("name").Value);

                        XElement position = triplaneObjectElement.Element("position");
                        triplaneObject.BasePoint = new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            );

                        XElement rotate = triplaneObjectElement.Element("rotate");
                        triplaneObject.AngleX = int.Parse(rotate.Attribute("x").Value);
                        triplaneObject.AngleY = int.Parse(rotate.Attribute("y").Value);
                        triplaneObject.AngleZ = int.Parse(rotate.Attribute("z").Value);

                        XElement scale = triplaneObjectElement.Element("scale");
                        triplaneObject.SetScale(
                            double.Parse(scale.Attribute("x").Value, format),
                            double.Parse(scale.Attribute("y").Value, format),
                            double.Parse(scale.Attribute("z").Value, format)
                            );
                        triplaneObject.UpdateObject();

                        scene.AddObject(triplaneObject);
                        ObjectsList.Items.Add(triplaneObject.Name);
                    }

                    ObjectsList.SelectedIndex = -1;
                    if (scene.Objects.Count > 0)
                    {
                        ObjectsList.SelectedIndex = 0;
                    }
                    scene.PaintObjects();
                }
                catch (Exception exception)
                {
                    //Log.Append(exception.StackTrace);
                    MessageBox.Show("Unable to import scene! File is corrupted.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.L)
            {
                double a = 10;
                double r = 24.7;
                double b = 2.7;

                double x = 1;
                double y = 1;
                double z = 1;

                double time = 1000;
                double dt = 0.01;

                SceneObject sceneObject = new SceneObject("Lorenz");
                sceneObject.BasePoint = new Point3D(0, 0, 0);

                for (int i = 0; i < time; i++)
                {
                    double nx = x + (-a * x + a * y) * dt;
                    double ny = y + (-x * z + r * x - y) * dt;
                    double nz = z + (x * y - b * z) * dt;

                    x = nx;
                    y = ny;
                    z = nz;

                    int red = (int)((i / (double)time) * 255);
                    int green = 0;
                    int blue = (int)(((time - i) / (double)time) * 255);
                    Sphere box = new Sphere(new Point3D(x * 10, y * 10, z * 10), 5, 4, Color.FromArgb(red, green, blue));
                    ScenePrimitive scenePrimitive = new ScenePrimitive(box, $"Point-{i}");
                    sceneObject.AddScenePrimitive(scenePrimitive);
                }

                scene.AddObject(sceneObject);
                int index = ObjectsList.Items.Add(sceneObject.Name);
                ObjectsList.SelectedIndex = index;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (SaveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    RenderPicture.Image.Save(SaveImageDialog.FileName);
                }
            }
        }

        private void ResetScene_Click(object sender, EventArgs e)
        {
            scene.Cameras.Clear();
            scene.Camera = new Camera();
            scene.ResetCamera();
            scene.Cameras.Add(scene.Camera);

            scene.Lights.Clear();
            scene.Light = new Vector3D();
            scene.ResetLight();
            scene.Lights.Add(scene.Light);

            scene.Objects.Clear();
            currentObject = null;
            ObjectsList.Items.Clear();
            PrimitivesList.Items.Clear();
            ParametersPanel.Visible = ConsoleParameters.Visible = false;
            scene.PaintObjects();
        }

        private void ИмпортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("es-ES");

                    XDocument document = XDocument.Load(ImportDialog.FileName);

                    XElement scene1 = document.Element("scene");

                    scene.Cameras.Clear();
                    foreach (XElement cameraElement in scene1.Element("cameras").Elements("camera"))
                    {
                        scene.Camera = new Camera();
                        scene.ResetCamera();

                        scene.Camera.IsCentralProjection = bool.Parse(cameraElement.Element("central-projection").Value);

                        XElement position = cameraElement.Element("position");
                        scene.Camera.Position = new Point3DSpherical(new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            ));

                        XElement target = cameraElement.Element("target");
                        scene.Camera.Target = new Point3DSpherical(new Point3D(
                            double.Parse(target.Attribute("x").Value, format),
                            double.Parse(target.Attribute("y").Value, format),
                            double.Parse(target.Attribute("z").Value, format)
                            ));

                        scene.Camera.RotatePositionLeftRight(0);
                        scene.Camera.RotatePositionUpDown(0);

                        XElement clippingPlanes = cameraElement.Element("clipping-planes");
                        scene.Camera.NearClippingPlaneZ = double.Parse(clippingPlanes.Attribute("near").Value, format);
                        scene.Camera.FarClippingPlaneZ = double.Parse(clippingPlanes.Attribute("far").Value, format);

                        scene.Camera.FOV = double.Parse(cameraElement.Element("fov").Value, format);

                        scene.Cameras.Add(scene.Camera);
                    }

                    scene.Lights.Clear();
                    foreach (XElement lightElement in scene1.Element("lights").Elements("light"))
                    {
                        scene.Light = new Vector3D(
                            double.Parse(lightElement.Attribute("x").Value, format),
                            double.Parse(lightElement.Attribute("y").Value, format),
                            double.Parse(lightElement.Attribute("z").Value, format)
                            );

                        scene.Lights.Add(scene.Light);
                    }

                    scene.Objects.Clear();
                    ObjectsList.Items.Clear();
                    PrimitivesList.Items.Clear();
                    ParametersPanel.Visible = ParametersPanel.Enabled = ConsoleParameters.Visible = ConsoleParameters.Enabled = false;
                    foreach (XElement sceneObjectElement in scene1.Element("objects").Elements("scene-object"))
                    {
                        SceneObject sceneObject = new SceneObject(sceneObjectElement.Attribute("name").Value);

                        XElement position = sceneObjectElement.Element("position");
                        sceneObject.BasePoint = new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            );

                        XElement rotate = sceneObjectElement.Element("rotate");
                        sceneObject.AngleX = int.Parse(rotate.Attribute("x").Value);
                        sceneObject.AngleY = int.Parse(rotate.Attribute("y").Value);
                        sceneObject.AngleZ = int.Parse(rotate.Attribute("z").Value);

                        XElement scale = sceneObjectElement.Element("scale");
                        sceneObject.SetScale(
                            double.Parse(scale.Attribute("x").Value, format),
                            double.Parse(scale.Attribute("y").Value, format),
                            double.Parse(scale.Attribute("z").Value, format)
                            );

                        foreach (XElement scenePrimitiveElement in sceneObjectElement.Element("scene-primitives").Elements("scene-primitive"))
                        {
                            Primitive primitive = null;

                            XElement primitiveElement = scenePrimitiveElement.Element("primitive");
                            string type = primitiveElement.Attribute("type").Value;

                            XElement basePointElement = primitiveElement.Element("base-point");
                            Point3D basePoint = new Point3D(
                                double.Parse(basePointElement.Attribute("x").Value, format),
                                double.Parse(basePointElement.Attribute("y").Value, format),
                                double.Parse(basePointElement.Attribute("z").Value, format)
                                );

                            XElement colorElement = primitiveElement.Element("color");
                            Color color = Color.FromArgb(
                                int.Parse(colorElement.Attribute("r").Value),
                                int.Parse(colorElement.Attribute("g").Value),
                                int.Parse(colorElement.Attribute("b").Value)
                                );

                            switch (type)
                            {
                                case "box":
                                    primitive = new Box(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("width").Value, format),
                                        double.Parse(primitiveElement.Element("height").Value, format),
                                        double.Parse(primitiveElement.Element("length").Value, format),
                                        color
                                        );
                                    break;
                                case "cylinder":
                                    primitive = new Cylinder(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        double.Parse(primitiveElement.Element("height").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                                case "sphere":
                                    primitive = new Sphere(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                                case "hemisphere":
                                    primitive = new Hemisphere(
                                        basePoint,
                                        double.Parse(primitiveElement.Element("radius").Value, format),
                                        int.Parse(primitiveElement.Element("segments-count").Value),
                                        color
                                        );
                                    break;
                            }

                            ScenePrimitive scenePrimitive = new ScenePrimitive(primitive, scenePrimitiveElement.Attribute("name").Value);

                            XElement primitiveRotate = scenePrimitiveElement.Element("rotate");
                            scenePrimitive.AngleX = int.Parse(primitiveRotate.Attribute("x").Value);
                            scenePrimitive.AngleY = int.Parse(primitiveRotate.Attribute("y").Value);
                            scenePrimitive.AngleZ = int.Parse(primitiveRotate.Attribute("z").Value);

                            XElement primitiveScale = scenePrimitiveElement.Element("scale");
                            scenePrimitive.SetScale(
                                double.Parse(primitiveScale.Attribute("x").Value, format),
                                double.Parse(primitiveScale.Attribute("y").Value, format),
                                double.Parse(primitiveScale.Attribute("z").Value, format)
                                );

                            sceneObject.AddScenePrimitive(scenePrimitive);
                        }

                        scene.AddObject(sceneObject);
                        ObjectsList.Items.Add(sceneObject.Name);
                    }

                    foreach (XElement triplaneObjectElement in scene1.Element("objects").Elements("console-object"))
                    {
                        TriplaneObject triplaneObject = new TriplaneObject(triplaneObjectElement.Attribute("name").Value);

                        XElement position = triplaneObjectElement.Element("position");
                        triplaneObject.BasePoint = new Point3D(
                            double.Parse(position.Attribute("x").Value, format),
                            double.Parse(position.Attribute("y").Value, format),
                            double.Parse(position.Attribute("z").Value, format)
                            );

                        XElement rotate = triplaneObjectElement.Element("rotate");
                        triplaneObject.AngleX = int.Parse(rotate.Attribute("x").Value);
                        triplaneObject.AngleY = int.Parse(rotate.Attribute("y").Value);
                        triplaneObject.AngleZ = int.Parse(rotate.Attribute("z").Value);

                        XElement scale = triplaneObjectElement.Element("scale");
                        triplaneObject.SetScale(
                            double.Parse(scale.Attribute("x").Value, format),
                            double.Parse(scale.Attribute("y").Value, format),
                            double.Parse(scale.Attribute("z").Value, format)
                            );
                        triplaneObject.UpdateObject();

                        scene.AddObject(triplaneObject);
                        ObjectsList.Items.Add(triplaneObject.Name);
                    }

                    ObjectsList.SelectedIndex = -1;
                    if (scene.Objects.Count > 0)
                    {
                        ObjectsList.SelectedIndex = 0;
                    }
                    scene.PaintObjects();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unable to import scene! File is corrupted.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ЕкспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ExportDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IFormatProvider format = new System.Globalization.CultureInfo("es-ES");

                    XDocument document = new XDocument();

                    XElement scene1 = new XElement("scene");

                    XElement cameras = new XElement("cameras");
                    for (int i = 0; i < scene.Cameras.Count; ++i)
                    {
                        Camera camera = scene.Cameras[i];
                        XElement cameraElement = new XElement("camera");
                        
                        cameraElement.SetElementValue("central-projection", camera.IsCentralProjection);

                        XElement position = new XElement("position");
                        position.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Position.X));
                        position.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Position.Y));
                        position.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Position.Z));
                        cameraElement.Add(position);

                        XElement target = new XElement("target");
                        target.SetAttributeValue("x", string.Format(format, "{0:0.00}", camera.Target.X));
                        target.SetAttributeValue("y", string.Format(format, "{0:0.00}", camera.Target.Y));
                        target.SetAttributeValue("z", string.Format(format, "{0:0.00}", camera.Target.Z));
                        cameraElement.Add(target);

                        XElement clippingPlanes = new XElement("clipping-planes");
                        clippingPlanes.SetAttributeValue("near", camera.NearClippingPlaneZ);
                        clippingPlanes.SetAttributeValue("far", camera.FarClippingPlaneZ);
                        cameraElement.Add(clippingPlanes);

                        cameraElement.SetElementValue("fov", camera.FOV);

                        cameras.Add(cameraElement);
                    }
                    scene1.Add(cameras);

                    XElement lights = new XElement("lights");
                    for (int i = 0; i < scene.Lights.Count; ++i)
                    {
                        Vector3D light = scene.Lights[i];
                        XElement lightElement = new XElement("light");
                        lightElement.SetAttributeValue("name", "Light-0");
                        lightElement.SetAttributeValue("x", "0,00");
                        lightElement.SetAttributeValue("y", "150,00");
                        lightElement.SetAttributeValue("z", "-150,00");
                        lights.Add(lightElement);
                    }
                    scene1.Add(lights);

                    XElement objects = new XElement("objects");
                    foreach (SceneObject sceneObject in scene.Objects)
                    {
                        XElement objectElement;

                        if (sceneObject is TriplaneObject)
                        {
                            TriplaneObject triplaneObject = (TriplaneObject)sceneObject;
                            objectElement = new XElement("console-object");
                        }
                        else
                        {
                            objectElement = new XElement("scene-object");
                            XElement scenePrimitivesElement = new XElement("scene-primitives");
                            foreach (ScenePrimitive scenePrimitive in sceneObject.ScenePrimitives)
                            {
                                XElement scenePrimitiveElement = new XElement("scene-primitive");

                                XElement primitiveRotate = new XElement("rotate");
                                primitiveRotate.SetAttributeValue("x", scenePrimitive.AngleX);
                                primitiveRotate.SetAttributeValue("y", scenePrimitive.AngleY);
                                primitiveRotate.SetAttributeValue("z", scenePrimitive.AngleZ);
                                scenePrimitiveElement.Add(primitiveRotate);

                                XElement primitiveScale = new XElement("scale");
                                primitiveScale.SetAttributeValue("x", string.Format(format, "{0:0.00}", scenePrimitive.ScaleX));
                                primitiveScale.SetAttributeValue("y", string.Format(format, "{0:0.00}", scenePrimitive.ScaleY));
                                primitiveScale.SetAttributeValue("z", string.Format(format, "{0:0.00}", scenePrimitive.ScaleZ));
                                scenePrimitiveElement.Add(primitiveScale);

                                XElement primitiveElement = new XElement("primitive");

                                XElement primitiveBasePoint = new XElement("base-point");
                                primitiveBasePoint.SetAttributeValue("x", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.X));
                                primitiveBasePoint.SetAttributeValue("y", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.Y));
                                primitiveBasePoint.SetAttributeValue("z", string.Format(format, "{0:0.00}", scenePrimitive.Primitive.BasePoint.Z));
                                primitiveElement.Add(primitiveBasePoint);

                                XElement primitiveColor = new XElement("color");
                                primitiveColor.SetAttributeValue("r", scenePrimitive.Primitive.Color.R);
                                primitiveColor.SetAttributeValue("g", scenePrimitive.Primitive.Color.G);
                                primitiveColor.SetAttributeValue("b", scenePrimitive.Primitive.Color.B);
                                primitiveElement.Add(primitiveColor);

                                if (scenePrimitive.Primitive is Box)
                                {
                                    Box box = (Box)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "box");
                                    primitiveElement.SetElementValue("width", string.Format(format, "{0:0.00}", box.Width));
                                    primitiveElement.SetElementValue("height", string.Format(format, "{0:0.00}", box.Height));
                                    primitiveElement.SetElementValue("length", string.Format(format, "{0:0.00}", box.Length));
                                }
                                else if (scenePrimitive.Primitive is Cylinder)
                                {
                                    Cylinder cylinder = (Cylinder)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "cylinder");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", cylinder.Radius));
                                    primitiveElement.SetElementValue("height", string.Format(format, "{0:0.00}", cylinder.Height));
                                    primitiveElement.SetElementValue("segments-count", cylinder.SegmentsCount);
                                }
                                else if (scenePrimitive.Primitive is Sphere)
                                {
                                    Sphere sphere = (Sphere)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "sphere");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", sphere.Radius));
                                    primitiveElement.SetElementValue("segments-count", sphere.SegmentsCount);
                                }
                                else if (scenePrimitive.Primitive is Hemisphere)
                                {
                                    Hemisphere hemisphere = (Hemisphere)scenePrimitive.Primitive;
                                    primitiveElement.SetAttributeValue("type", "hemisphere");
                                    primitiveElement.SetElementValue("radius", string.Format(format, "{0:0.00}", hemisphere.Radius));
                                    primitiveElement.SetElementValue("segments-count", hemisphere.SegmentsCount);
                                }

                                scenePrimitiveElement.Add(primitiveElement);
                                scenePrimitiveElement.SetAttributeValue("name", scenePrimitive.Name);
                                scenePrimitivesElement.Add(scenePrimitiveElement);
                            }

                            objectElement.Add(scenePrimitivesElement);
                        }

                        objectElement.SetAttributeValue("name", sceneObject.Name);

                        XElement objectPosition = new XElement("position");
                        objectPosition.SetAttributeValue("x", string.Format(format, "{0:0.00}", sceneObject.BasePoint.X));
                        objectPosition.SetAttributeValue("y", string.Format(format, "{0:0.00}", sceneObject.BasePoint.Y));
                        objectPosition.SetAttributeValue("z", string.Format(format, "{0:0.00}", sceneObject.BasePoint.Z));
                        objectElement.Add(objectPosition);

                        XElement objectRotate = new XElement("rotate");
                        objectRotate.SetAttributeValue("x", sceneObject.AngleX);
                        objectRotate.SetAttributeValue("y", sceneObject.AngleY);
                        objectRotate.SetAttributeValue("z", sceneObject.AngleZ);
                        objectElement.Add(objectRotate);

                        XElement objectScale = new XElement("scale");
                        objectScale.SetAttributeValue("x", string.Format(format, "{0:0.00}", sceneObject.ScaleX));
                        objectScale.SetAttributeValue("y", string.Format(format, "{0:0.00}", sceneObject.ScaleY));
                        objectScale.SetAttributeValue("z", string.Format(format, "{0:0.00}", sceneObject.ScaleZ));
                        objectElement.Add(objectScale);

                        objects.Add(objectElement);
                    }
                    scene1.Add(objects);

                    document.Add(scene1);
                    document.Save(ExportDialog.FileName);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Unable to export scene!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void КаркасToolStripMenuItem_Click(object sender, EventArgs e)
        {
            объёмноеToolStripMenuItem.Image = null;
            каркасToolStripMenuItem.Image = Properties.Resources.Check;
            scene.Mode = Scene.MODE.WIREFRAME;
            scene.PaintObjects();
        }

        private void ОбъёмноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            каркасToolStripMenuItem.Image = null;
            объёмноеToolStripMenuItem.Image = Properties.Resources.Check;
            scene.Mode = Scene.MODE.SOLID;
            scene.PaintObjects();
        }

        private void ПозиционныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scene.Camera.RotatePositionLeftRight(0);
            scene.Camera.RotatePositionUpDown(0);
            UpdateCameraValues();
            Rotation = 0;
            объекToolStripMenuItem.Image = null;
            позиционныйToolStripMenuItem.Image = Properties.Resources.Check;
        }

        private void ОбъекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scene.Camera.RotateTargetLeftRight(0);
            scene.Camera.RotateTargetUpDown(0);
            UpdateCameraValues();
            Rotation = 1;
            позиционныйToolStripMenuItem.Image = null;
            объекToolStripMenuItem.Image = Properties.Resources.Check;
        }

        private void NumOfWings_ValueChanged(object sender, EventArgs e)
        {
            ((TriplaneObject)currentObject).NumOfWings = (double)NumOfWings.Value;
            ((TriplaneObject)currentObject).UpdateObject();
            scene.PaintObjects();
        }

        private void ВPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "PNG Image(*.png)|*.png";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                RenderPicture.Image.Save(saveFile.FileName, ImageFormat.Png);
            }
        }

        private void ВJPGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "JPG Image(*.jpg)|*.jpg";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                RenderPicture.Image.Save(saveFile.FileName, ImageFormat.Jpeg);
            }
        }

        private void ЦентральноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scene.Camera.IsCentralProjection = true;
        }
        private void ПараллельноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scene.Camera.IsCentralProjection = false;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            scene.PaintObjects();
        }

        private void AddCamera_Click_1(object sender, EventArgs e)
        {
            scene.Camera = new Camera();
            scene.ResetCamera();
            scene.Cameras.Add(scene.Camera);
            int index = 0;
            while (CamerasList.Items.Contains($"Camera-{index}"))
            {
                ++index;
            }
            CamerasList.Items.Add($"Camera-{index}");
            CamerasList.SelectedIndex = CamerasList.Items.Count - 1;
            DeleteCamera.Enabled = true;
            scene.PaintObjects();
        }
        private void DeleteCamera_Click(object sender, EventArgs e)
        {
            int index = CamerasList.SelectedIndex;
            CamerasList.Items.RemoveAt(index);
            scene.Cameras.RemoveAt(index);
            CamerasList.SelectedIndex = -1;
            CamerasList.SelectedIndex = index > 0 ? index - 1 : 0;
            if (scene.Cameras.Count == 1)
            {
                DeleteCamera.Enabled = false;
            }
            scene.PaintObjects();
        }
        private void CamerasList_SelectedIndexChanged(object sender, EventArgs e)
        {
            scene.Camera = scene.Cameras[CamerasList.SelectedIndex];
            UpdateCameraValues();
            scene.PaintObjects();
        }
    }
}