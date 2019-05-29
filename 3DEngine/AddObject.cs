using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3DEngine
{
    public partial class AddObject : Form
    {
        private Main mainForm = null;
        public AddObject(Main mainForm, SceneObject triplaneObject)
        {
            this.mainForm = mainForm; 
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Point3D basePoint = new Point3D(
                (double)PositionX.Value,
                (double)PositionY.Value,
                (double)PositionZ.Value
                );
            TriplaneObject sceneObject;
            sceneObject = new TriplaneObject(ObjectName.Text);
            sceneObject.BasePoint = basePoint;
            sceneObject.AngleX = (int)RotateX.Value;
            sceneObject.AngleY = (int)RotateY.Value;
            sceneObject.AngleZ = (int)RotateZ.Value;
            sceneObject.SetScale((double)ScaleX.Value, (double)ScaleY.Value, (double)ScaleZ.Value);
            mainForm.scene.AddObject(sceneObject);
            int index = mainForm.ObjectsList.Items.Add(ObjectName.Text);
            mainForm.ObjectsList.SelectedIndex = index;
            mainForm.scene.PaintObjects();
            Close();
        }

        private void ObjectName_TextChanged(object sender, EventArgs e)
        {
            AddObj.Enabled = ObjectName.TextLength >= 2;
            if (mainForm.scene.GetObjectByName(ObjectName.Text) != null)
            {
                ObjectName.ForeColor = Color.Red;
                ErrorLabel.Text = "Объект с таким именем уже существует!";
                RotateX.Enabled = RotateY.Enabled = RotateZ.Enabled = ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = PositionX.Enabled = PositionY.Enabled = PositionZ.Enabled = AddObj.Enabled = false;
            }
            else if(ObjectName.TextLength >= 2 & mainForm.scene.GetObjectByName(ObjectName.Text) == null)
            {
                ErrorLabel.Text = null;
                ObjectName.ForeColor = Color.Green;
                RotateX.Enabled = RotateY.Enabled = RotateZ.Enabled = ScaleX.Enabled = ScaleY.Enabled = ScaleZ.Enabled = PositionX.Enabled = PositionY.Enabled = PositionZ.Enabled = AddObj.Enabled = true;
            }
        }
    }
}
