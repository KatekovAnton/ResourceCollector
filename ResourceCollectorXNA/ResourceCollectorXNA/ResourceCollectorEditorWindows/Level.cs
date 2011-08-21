using System;
using System.Windows.Forms;



namespace ResourceCollectorXNA.ResourceCollectorEditorWindows
{
    public partial class Level : Form
    {
        public Level() {
            InitializeComponent();
        }

        private void Level_Resize(object sender, EventArgs e) {
            Width = 470;
            Height = 674;
        }

        public void AddObject(String id, String name, String description, Boolean isActive = false) {
            String[] text = {id, name, description};
            int curRow = dataGridView_ObjectsList.Rows.Add(text);
            dataGridView_ObjectsList.Rows[curRow].Selected = isActive;
        }

    
        public void ClearAllObjectsInGrid() {
            dataGridView_ObjectsList.Rows.Clear();
        }
    }
}