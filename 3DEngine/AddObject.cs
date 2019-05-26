using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (mainForm.scene.GetObjectByName(ObjectName.Text) == null)
            {
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
                Close();
            }
            else
            {
                MessageBox.Show("Объект с таким именем уже существует!", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ObjectName.Clear();
                ObjectName.Focus();
            }
        }

        private void ObjectName_TextChanged(object sender, EventArgs e)
        {
            AddObj.Enabled = ObjectName.TextLength >= 2;
        }
    }
}
