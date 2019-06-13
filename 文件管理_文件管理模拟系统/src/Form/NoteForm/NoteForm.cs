using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManageSystem
{
    public partial class NoteForm : Form
    {
        public static bool ischanged = false;       //状态位 -> 用户打开记事本是否进行编辑
        public MainForm mainForm;
        public string filename;

        public NoteForm(string Name, MainForm parent)
        {
            mainForm = parent;
            filename = Name;

            InitializeComponent();
        }

        /*关闭记事本*/
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (ischanged == true)  //如果用户更改记事本 => 弹出提示框提醒保存
            {
                DialogResult result = MessageBox.Show("是否进行保存？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    FCB nowFCB = mainForm.category.search(mainForm.category.root, filename, 0).fcb;
                    int oldSize = nowFCB.size;
                    int oldStart = nowFCB.start;

                    string content = textBox1.Text.Trim();
                    if (content != "")
                        { MessageBox.Show("保存成功！"); }

                    nowFCB.size = textBox1.Text.Trim().Length;
                    nowFCB.lastModify = DateTime.Now.ToLocalTime().ToString();    //获取当前时间

                    //在内存上给文件分配空间
                    if (nowFCB.size>0)
                    {
                        if(mainForm.MyDisk.remain<=textBox1.Text.Trim().Length)
                        {
                            MessageBox.Show("磁盘空间不足！");
                        }
                        else
                        {
                            if(oldStart==-1)    //如果该文本文件之前为空(第一次修改)
                            {
                                mainForm.MyDisk.giveSpace(nowFCB, textBox1.Text.Trim());
                            }
                            else                //更新
                            {
                                mainForm.MyDisk.fileUpdate(oldStart, oldSize,nowFCB, textBox1.Text.Trim());
                            }
                        }
                        
                            
                    }
                    mainForm.fileFormInit(mainForm.currentRoot);
                }
                else
                {
                    e.Cancel = false;  //不保存直接退出
                }
            }
            else
                e.Cancel = false;   //用户未编辑直接退出
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ischanged = true;
        }

        private void NoteForm_Load(object sender, EventArgs e)
        {
            this.Text = filename + ".txt";
            FCB nowFCB = mainForm.category.search(mainForm.category.root, filename, 0).fcb;
            if (mainForm.MyDisk.getFileContent(nowFCB) != "")
            {
                textBox1.AppendText(mainForm.MyDisk.getFileContent(nowFCB));    //读取保存的文本文件信息
            }
            ischanged = false;
        }
    }
}
