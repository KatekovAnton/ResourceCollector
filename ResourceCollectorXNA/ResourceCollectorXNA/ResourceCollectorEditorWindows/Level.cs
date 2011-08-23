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

        // удаляет из таблицы строку в соответствии с переданным идом
        public void RemoveObject(uint id)
        {
            int length = dataGridView_ObjectsList.Rows.Count;
            for(int i = 0; i < length; ++i) {
                int curValue = Convert.ToInt32(dataGridView_ObjectsList.Rows[i].Cells[0].Value);
                if (curValue == id) {
                    dataGridView_ObjectsList.Rows.RemoveAt(i);
                    i = length;
                }
            }
            
        }
    }
}