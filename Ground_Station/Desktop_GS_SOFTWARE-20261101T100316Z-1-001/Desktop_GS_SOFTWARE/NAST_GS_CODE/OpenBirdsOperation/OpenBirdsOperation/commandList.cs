using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualBasic.FileIO;

namespace OpenBirdsOperation
{
    public partial class commandList : Form
    {
        public operationMain formMain;
        public commandList()
        {
            InitializeComponent();
        }

        public void addCommand(string command, string comments, string remark)
        {
            missingPacketDataGridView.Rows.Add(command, comments, remark);
            tabControl1.SelectedTab = missingTabPage;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv file(*.csv)|*.csv|all file(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "Select command file";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(ofd.FileName);

                TextFieldParser parser = new TextFieldParser(ofd.FileName, Encoding.UTF8);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                if (tabControl1.SelectedIndex == 0)
                {
                    dataGridView1.Rows.Clear();
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    dataGridView2.Rows.Clear();
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    dataGridView3.Rows.Clear();
                }
                else
                {
                    missingPacketDataGridView.Rows.Clear();
                }

                if (!parser.EndOfData) parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    if (row == null || row.Length < 2 || string.IsNullOrWhiteSpace(row[0])) continue;

                    if (tabControl1.SelectedIndex == 0)
                    {
                        dataGridView1.Rows.Add(row);
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {
                        dataGridView2.Rows.Add(row);
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        dataGridView3.Rows.Add(row);
                    }
                    else
                    {
                        missingPacketDataGridView.Rows.Add(row);
                    }
                }
            }
        }

        private static string FormatHexForMaskedBox(string rawHex)
        {
            if (string.IsNullOrWhiteSpace(rawHex)) return "";
            string clean = rawHex.Replace(" ", "").Replace("-", "").Trim().ToUpperInvariant();
            if (clean.Length == 26)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 26; i += 2)
                {
                    if (i > 0) sb.Append(' ');
                    sb.Append(clean.Substring(i, 2));
                }
                return sb.ToString();
            }
            return rawHex;
        }

        private void SelectFromGrid(DataGridView dgv)
        {
            if (formMain == null || dgv.CurrentRow == null || dgv.CurrentRow.Cells[0].Value == null)
            {
                return;
            }
            string rawCmd = dgv.CurrentRow.Cells[0].Value.ToString();
            string comment = dgv.CurrentRow.Cells[1].Value != null ? dgv.CurrentRow.Cells[1].Value.ToString() : "";
            formMain.CommandString = FormatHexForMaskedBox(rawCmd);
            formMain.CommentsString = comment;

            if (dgv.CurrentCell != null && dgv.CurrentCell.RowIndex + 1 < dgv.Rows.Count)
            {
                dgv.CurrentCell = dgv.Rows[dgv.CurrentCell.RowIndex + 1].Cells[0];
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (formMain != null)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    SelectFromGrid(dataGridView1);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    SelectFromGrid(dataGridView2);
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    SelectFromGrid(dataGridView3);
                }
                else
                {
                    SelectFromGrid(missingPacketDataGridView);
                }
            }
        }

        private void DataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Button3_Click(sender, e);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv file(*.csv)|*.csv|all file(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "Select command file";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(ofd.FileName);

                TextFieldParser parser = new TextFieldParser(ofd.FileName, Encoding.UTF8);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                if (!parser.EndOfData) parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    if (row == null || row.Length < 2 || string.IsNullOrWhiteSpace(row[0])) continue;

                    if (tabControl1.SelectedIndex == 0)
                    {
                        dataGridView1.Rows.Add(row);
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {
                        dataGridView2.Rows.Add(row);
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        dataGridView3.Rows.Add(row);
                    }
                    else
                    {
                        missingPacketDataGridView.Rows.Add(row);
                    }
                }
            }
        }

        private void CommandList_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

            bool loaded = false;
            string resolvedCsv = operationMain.FindRepoDataFile(
                "Utils/command_list/Slipper2Sat_CMD_LIST.csv",
                "Slipper2Sat_CMD_LIST.csv"
            );

            if (!string.IsNullOrEmpty(resolvedCsv) && File.Exists(resolvedCsv))
            {
                try
                {
                    LoadCommandFile(resolvedCsv);
                    loaded = true;
                }
                catch { }
            }

            if (!loaded || dataGridView1.Rows.Count <= 1)
            {
                PopulateDefaultGsCommunicatorCommands();
            }
        }

        private void PopulateDefaultGsCommunicatorCommands()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add("53011DD1F200F2000000000001", "Request Live HK Telemetry (HK1 - 1 Pkt)", "OBC");
            dataGridView1.Rows.Add("53011DD2F200F2000000000001", "Request HK2 Telemetry (Currents & Flags - 1 Pkt)", "OBC");
            dataGridView1.Rows.Add("53011DD1F200F2000000000005", "Download Stored HK Data (5 Pkts @ 128B)", "OBC");
            dataGridView1.Rows.Add("53011DD1F80000000000000005", "Request Flash Storage Telemetry (5 Chunks @ 128B)", "OBC");
            dataGridView1.Rows.Add("5304CC5EBE00D0000000000045", "Transmit Dummy Image Data (69 Chunks / DC)", "Camera");
            dataGridView1.Rows.Add("5304CC5EBD0000000000000000", "Real Camera Data Downlink (All Chunks / CAM)", "Camera");
            dataGridView1.Rows.Add("5304CC5EBC0000000000000000", "Real Camera Capture to Flash (CAMON / SNAP)", "Camera");
            dataGridView1.Rows.Add("53011DD2F50000000000000045", "Downlink Camera Photo Packets (cam.txt - 69 Pkts)", "Camera");
            dataGridView1.Rows.Add("53011DD1F50000000000000001", "Ping Satellite OBC (PING -> ACK)", "OBC");
            dataGridView1.Rows.Add("5303A053CF0000000000000001", "ADCS ON: Run ADCS Mission (Opcode A0 53 CF)", "ADCS");
            dataGridView1.Rows.Add("5305ECCFCF0000000000000001", "EPDM ON: Run EPDM Mission (Opcode EC CF CF)", "EPDM");
        }

        private void LoadCommandFile(string filePath)
        {
            dataGridView1.Rows.Clear();
            using (TextFieldParser parser = new TextFieldParser(filePath, Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                if (!parser.EndOfData) parser.ReadFields(); // skip header
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    if (row != null && row.Length >= 2 && !string.IsNullOrWhiteSpace(row[0]))
                    {
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                dataGridView1.Rows.Clear();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                dataGridView2.Rows.Clear();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                dataGridView3.Rows.Clear();
            }
            else
            {
                missingPacketDataGridView.Rows.Clear();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                removeSelectedRows(dataGridView1);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                removeSelectedRows(dataGridView2);
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                removeSelectedRows(dataGridView3);
            }
            else
            {
                removeSelectedRows(missingPacketDataGridView);
            }
        }

        private void removeSelectedRows(DataGridView dataGridView)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                for (int i = dataGridView.SelectedRows.Count - 1; i >= 0; i--)
                {
                    var row = dataGridView.SelectedRows[i];
                    if (row.IsNewRow)
                    {
                        continue;
                    }
                    dataGridView.Rows.RemoveAt(row.Index);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to remove.");
            }
        }
    }
}
