using System;
using System.Windows.Forms;
using ResourceCollectorXNA.Engine;

using ResourceCollector;
namespace ResourceCollectorXNA
{
    public partial class LevelWindow : Form
    {

        public LevelWindow()
        {
            GameEngine.levelController = new RCViewControllers.LevelWindowVC(this);
            InitializeComponent();
            dataGridView_ObjectsList.SelectionChanged += DataGridViewSelectionChanged;
        }


        // обработчик, который вызыватся при выделении строчки, сразу делает активными элементы на сцене
        private void DataGridViewSelectionChanged(object sender, EventArgs e)
        {
            int length = dataGridView_ObjectsList.SelectedRows.Count;
            if (length == 0)
            {
                return;
            }
            var resultIds = new uint[length];
            for (int i = 0; i < length; i++)
            {
                resultIds[i] = Convert.ToUInt32(dataGridView_ObjectsList.SelectedRows[i].Cells[0].Value);
            }
            GameEngine.Instance.editor.SetActiveObjects(resultIds, wassbavk);
            wassbavk = false;
        }


        private void LevelResize(object sender, EventArgs e)
        {
            Width = 470;
            Height = 674;
        }


        public void AddObject(String id, String name, String description, Boolean isActive = false)
        {
            String[] text = { id, name, description };
            int curRow = dataGridView_ObjectsList.Rows.Add(text);
            dataGridView_ObjectsList.Rows[curRow].Selected = isActive;
        }


        public void ClearAllObjectsInGrid()
        {
            dataGridView_ObjectsList.Rows.Clear();
        }

        bool wassbavk = false;
        // метод, выделяющий элементы в таблице в соответствии с переданными идами
        public void SetActiveObjects(uint[] ids, bool back = false)
        {
            if (back)
                wassbavk = true;
            int gridLen = dataGridView_ObjectsList.Rows.Count;
            int isSelectedNow = 0;
            int idsLen = ids.Length;
            for (int i = 0; i < gridLen; i++)
            {
                uint curId = Convert.ToUInt32(dataGridView_ObjectsList.Rows[i].Cells[0].Value);
                // ids.contains почему-то отказывается работать
                bool flag = false;
                for (int j = 0; j < idsLen; ++j)
                {
                    if (ids[j] == curId)
                    {
                        flag = true;
                    }
                    j = idsLen;
                }

                if (flag)
                {
                    dataGridView_ObjectsList.Rows[i].Selected = true;
                    if (++isSelectedNow == idsLen)
                    {
                        i = gridLen; // return analog
                    }
                }
                else
                {
                    dataGridView_ObjectsList.Rows[i].Selected = false;
                }
            }

        }


        // удаляет из таблицы строку в соответствии с переданным идом
        public void RemoveObject(uint id)
        {
            int length = dataGridView_ObjectsList.Rows.Count;
            for (int i = 0; i < length; ++i)
            {
                int curValue = Convert.ToInt32(dataGridView_ObjectsList.Rows[i].Cells[0].Value);
                if (curValue == id)
                {
                    dataGridView_ObjectsList.Rows.RemoveAt(i);
                    i = length;
                }
            }
        }

        //save
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5)
            {
                MessageBox.Show("too short nbame");
                return;
            }
            if (PackList.Instance.packs.Count == 0)
            {
                MessageBox.Show("no packs loaded!");
                return;
            }
            ResourceCollectorXNA.Engine.Level.EngineLevel level = GameEngine.Instance.gameLevel;
            if (level.levelContent.pack == null)
            {

                level.levelContent.pack = PackList.Instance.packs[0];
                PackList.Instance.packs[0].Attach(level.levelContent, FormMainPackExplorer.Instance.treeView1);
            }
            level.levelContent.name = textBox1.Text + "\0";
            level.FillContent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (PackList.Instance.packs.Count == 0)
            {
                MessageBox.Show("no packs loaded!");
                return;
            }
            FormObjectPicker fop = new FormObjectPicker(PackList.Instance.packs[0], ElementType.LevelContent);
            if (fop.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            LevelContent lc = PackList.Instance.packs[0].getobject(fop.PickedContent[0]) as LevelContent;
            Engine.Level.EngineLevel el = new Engine.Level.EngineLevel(lc);
            textBox1.Text = lc.name.Substring(0,lc.name.Length-1);
            GameEngine.Instance.LoadNewLevel(el);
            GameEngine.Instance.UpdateLevelPart();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("all not saved data will be lost! shure?", "attention", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                return;
            GameEngine.Instance.CreateNewLevel();
        }
    }
}