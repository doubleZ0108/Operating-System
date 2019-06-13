using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml;
using File_Management_System;

namespace FileManageSystem
{
    public partial class MainForm : Form
    {
        public Category category = new Category();                  //创建目录
        public Category.Node rootNode = new Category.Node();        //目录的根节点
        public Category.Node currentRoot = new Category.Node();     //当前根节点为root
        public VirtualDisk MyDisk = new VirtualDisk(1000, 2);       //申请内存空间

        public string nowPath;
        public string change;

        public MainForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            FCB root = new FCB("root", FCB.FOLDER, "", 1);
            rootNode = new Category.Node(root);
            currentRoot = rootNode;
            category.root = rootNode;

            /*恢复文件管理系统*/
            readFormDisk();        //读取目录信息文件
            readBitMap();          //读取位图文件
            readMyDisk();          //读取虚拟磁盘文件

            InitializeComponent();
        }

        //判断控件内按钮点击
        public void MyControlButtonClick(object sender, EventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Forms.Button")
            {
                Button bt = (Button)sender;  //当前点击的按钮
                if (bt.Tag.ToString() == "0") //文件
                {
                    NoteForm file = new NoteForm(bt.Text, this);        //弹出记事本窗口
                    file.Show();
                }
                else if (bt.Tag.ToString() == "1")//文件夹
                {
                    currentRoot = category.search(rootNode, bt.Text, 1);    //更改当前根节点
                    nowPath = nowPath + "> " + bt.Text;                     //更改当前路径
                    fileFormInit(currentRoot);                              //更新界面元素
                }
            }
            else
            {
                string filename = fileWindow.contextMenuStrip_FileChoose.SourceControl.Text;
                string type = fileWindow.contextMenuStrip_FileChoose.SourceControl.Tag.ToString();
                if (((ToolStripMenuItem)sender).Name == "打开ToolStripMenuItem")
                {
                    if (type == "0") //文件
                    {
                        NoteForm file = new NoteForm(filename, this);
                        file.Show();
                    }
                    else if (type == "1")//文件夹
                    {
                        currentRoot = category.search(rootNode, filename, 1);
                        nowPath = nowPath + "> " + filename;
                        fileFormInit(currentRoot);
                    }
                }
                else if (((ToolStripMenuItem)sender).Name == "删除ToolStripMenuItem")
                {
                    if (type == "0")
                    {
                        delete(filename, 0);
                    }
                    else if (type == "1")
                    {
                        delete(filename, 1);
                    }
                }
            }
        }


        //初始化当前结点下的界面
        public void fileFormInit(Category.Node now)
        {
            labelDiskSize.Text = "当前磁盘大小: " + MyDisk.size.ToString() + "B";
            labelBlockSize.Text = "当前盘块大小: " + MyDisk.blockSize.ToString() + "B";

            if (now.fcb.fileName == "root") //当前目录为root时禁用返回上一层按钮
                buttonBack.Enabled = false;
            else
                buttonBack.Enabled = true;

            textBoxSearch.Text = nowPath;   //更新路径

            string name = now.fcb.fileName;
            //窗体初始化
            this.fileWindow.Init();

            //按照左孩子-右兄弟树的结构, 依次显示该目录下的文件夹/文件
            if (now.firstChild == null)
                return;
            Category.Node temp = new Category.Node();
            temp = now.firstChild;
            fileWindow.showFiles(temp.fcb.fileName, temp.fcb.lastModify, temp.fcb.type, temp.fcb.size);
            temp = temp.nextBrother;
            while (temp != null)
            {
                FCB current = temp.fcb;
                fileWindow.showFiles(temp.fcb.fileName, temp.fcb.lastModify, temp.fcb.type, temp.fcb.size);
                temp = temp.nextBrother;
            }
        }
        //界面加载
        private void Form1_Load(object sender, EventArgs e)
        {
            //帮助界面
            HelpForm helpForm = new HelpForm();
            helpForm.StartPosition = FormStartPosition.CenterScreen;
            helpForm.Show(this);

            //左侧的目录树
            fileFormInit(rootNode);
            createTreeView(rootNode.firstChild);
            fileWindow.btnClick += new EventHandler(MyControlButtonClick);
        }

        //新建文件
        public void createFile()
        {
            string str = Interaction.InputBox("请输入文件的名称", "字符串", "", 100, 100);
            if (str != "")
            {
                if (category.noSameName(str, currentRoot, FCB.TXTFILE))
                {
                    string time = DateTime.Now.ToLocalTime().ToString();    //获取时间信息
                    if (fileWindow.showFiles(str, time, FCB.TXTFILE, 0))
                        category.createFile(currentRoot.fcb.fileName, new FCB(str, FCB.TXTFILE, time, 0));  //文件加入到目录中
                }
                else
                {
                    MessageBox.Show("已存在名为" + str + ".txt的文件，创建失败！");
                }
            }
            //目录树更新
            treeView.Nodes.Clear();
            createTreeView(rootNode.firstChild);
        }
        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFile();
        }
        private void buttonCreateFile_Click(object sender, EventArgs e)
        {
            createFile();
        }

        //新建文件夹
        public void createFolder()
        {
            string str = Interaction.InputBox("请输入文件夹的名称", "字符串", "", 100, 100);
            if (str != "")
            {
                if(str==currentRoot.fcb.fileName)   //如果跟上级根目录重名, 添加一个_做标识
                {
                    str = "_" + str;
                }
                if (category.noSameName(str, currentRoot, FCB.FOLDER))
                {
                    string time = DateTime.Now.ToLocalTime().ToString();
                    if (fileWindow.showFiles(str, time, FCB.FOLDER, 0))
                        category.createFile(currentRoot.fcb.fileName, new FCB(str, FCB.FOLDER, time, 0));  //文件夹加入到目录中
                }
                else
                {
                    MessageBox.Show("已存在名为" + str + "的文件夹，创建失败！");
                }
            }
            //目录树更新
            treeView.Nodes.Clear();
            createTreeView(rootNode.firstChild);
        }
        private void 新建文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFolder();
        }
        private void buttonCreateFolder_Click(object sender, EventArgs e)
        {
            createFolder();
        }

        //删除内容
        public void delete(string name, int type)
        {
            if (type == FCB.TXTFILE)  //删除文件
            {
                FCB deleteFCB = category.search(rootNode, name, type).fcb;
                category.deleteFile(name);
                category.delete(category.search(rootNode, name, FCB.TXTFILE
));
                MyDisk.deleteFileContent(deleteFCB.start, deleteFCB.size);
                fileFormInit(currentRoot);
            }
            else  //删除文件夹
            {
                category.deleteFolder(name);
                fileFormInit(currentRoot);
            }
            //目录树更新
            treeView.Nodes.Clear();
            createTreeView(rootNode.firstChild);
        }

        //返回上一级目录
        private void buttonBack_Click(object sender, EventArgs e)
        {
            nowPath = nowPath.Replace("> " + currentRoot.fcb.fileName, "");
            currentRoot = currentRoot.parent;
            fileFormInit(currentRoot);
        }

        //格式化
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定清空磁盘？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                category.freeCategory(category.root);
                for (int i = 0; i < MyDisk.blockSize; i++)
                {
                    MyDisk.memory[i] = "";
                    MyDisk.bitMap[i] = -1;
                    MyDisk.remain = MyDisk.blockNum;
                }
                MessageBox.Show("磁盘已清空。");
                fileFormInit(rootNode);

                nowPath = "";
                textBoxSearch.Text = "";
                treeView.Nodes.Clear();

                updateLog();        //清空所有日志文件
            }
           
        }
        

        //创建目录树
        public void createTreeView(Category.Node pNode)
        {
            if (pNode == null)      //目录为空, 不需要创建目录树
                return;

            /*文件夹和文本文件分别创建目录树结点*/
            TreeNode tn = new TreeNode();
            if (pNode.fcb.type == FCB.FOLDER)
            {
                tn.Name = pNode.fcb.fileName;
                tn.Text = pNode.fcb.fileName;
                tn.Tag = 1;
                tn.ImageIndex = 1;
                tn.SelectedImageIndex = 1;
            }
            else if (pNode.fcb.type == FCB.TXTFILE)
            {
                tn.Name = pNode.fcb.fileName + ".txt";
                tn.Text = pNode.fcb.fileName + ".txt";
                tn.Tag = 0;
                tn.ImageIndex = 0;
                tn.SelectedImageIndex = 0;
            }

            /*只需按照一个左孩子一个右兄弟建立目录树*/
            if (pNode.parent == rootNode)
            {
                treeView.Nodes.Add(tn);
            }
            else
            {
                CallAddNode(treeView, pNode.parent.fcb.fileName, tn);
            }
            createTreeView(pNode.firstChild);
            createTreeView(pNode.nextBrother);
        }
        //双击目录树结点打开文件或者文件夹
        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeNode tn = treeView.SelectedNode;

            if (tn.Tag.ToString() == "1") //打开文件夹
            {
                Stack<TreeNode> s = new Stack<TreeNode>();
                s.Push(tn);
                currentRoot = category.search(rootNode, tn.Text, FCB.FOLDER);

                //更新路径
                nowPath = "";
                while (tn.Parent != null)
                {
                    s.Push(tn.Parent);
                    tn = tn.Parent;
                }
                nowPath = "";
                while (s.Count() != 0)
                {
                    nowPath += "> " + s.Pop().Text;
                }
                textBoxSearch.Text = nowPath;

                //刷新新目录下的界面
                fileFormInit(currentRoot);
            }
            else if (tn.Tag.ToString() == "0") //打开文件
            {
                NoteForm file = new NoteForm(tn.Text.Replace(".txt", ""), this);
                file.Show();
            }
        }
        //递归查找往目录树中加入结点
        public TreeNode AddNode(TreeNode tnParent, string tnStr, TreeNode newTn)
        {
            if (tnParent == null)
                return null;
            if (tnParent.Name == tnStr)
            {
                tnParent.Nodes.Add(newTn);
            }
                
            TreeNode tnRet = null;
            foreach (TreeNode tn in tnParent.Nodes)
            {
                tnRet = AddNode(tn, tnStr, newTn);
                if (tnRet != null)
                {
                    tnRet.Nodes.Add(newTn);
                    break;
                }
            }
            return tnRet;
        }
        public TreeNode CallAddNode(TreeView tree, string tnStr, TreeNode newTn)
        {
            foreach (TreeNode n in tree.Nodes)
            {
                TreeNode temp = AddNode(n, tnStr, newTn);
                if (temp != null)
                    return temp;
            }
            return null;
        }


        /*
        * CategoryInfo.txt中信息格式
        * 
        * 当前目录下的根节点
        * 文件名称
        * 文件类型
        * 上次修改时间
        * 文件大小
        * 文件起始位置
        * #分隔符
        * 
        */
        /*目录信息*/
        public void readFormDisk()
        {
            //把本地保存的上次的结点信息写回来
            string path = Application.StartupPath + "\\CategoryInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                string parentName = "", Name = "";    //父结点名字，自己的名字
                string lastModify = "";     //最后修改时间
                int type = -1, size = 0, start = -1, infoNum = 1;

                //逐行读取信息
                string str = reader.ReadLine();
                while (str != null)
                {
                    switch (infoNum)
                    {
                        case 1:
                            parentName = str;
                            infoNum++;
                            break;
                        case 2:
                            Name = str;
                            infoNum++;
                            break;
                        case 3:
                            type = int.Parse(str);
                            infoNum++;
                            break;
                        case 4:
                            lastModify = str;
                            infoNum++;
                            break;
                        case 5:
                            size = int.Parse(str);
                            infoNum++;
                            break;
                        case 6:
                            start = int.Parse(str);
                            infoNum++;
                            break;
                        case 7:
                            infoNum = 1;
                            FCB now = new FCB(Name, type, lastModify, size);
                            category.createFile(parentName, now);   //把文件结点的内容加到目录中
                            break;
                        default:
                            break;
                    }
                    str = reader.ReadLine();
                }
                reader.Close();
            }

        }
        public void writeCategory(Category.Node pNode)
        {
            //当前结点的父结点
            Category.Node parentNode = category.currentRootName(rootNode, pNode.fcb.fileName, pNode.fcb.type);
            string InfoPath = Application.StartupPath + "\\CategoryInfo.txt";
            StreamWriter writer = File.AppendText(InfoPath);

            writer.WriteLine(parentNode.fcb.fileName); //写入父结点的名字
            writer.WriteLine(pNode.fcb.fileName);     //写入文件的名字
            writer.WriteLine(pNode.fcb.type);         //写入文件的类型
            writer.WriteLine(pNode.fcb.lastModify);   //写入最后修改的时间
            writer.WriteLine(pNode.fcb.size);         //写入文件的大小
            if (pNode.fcb.type == FCB.TXTFILE)                //写入文件的开始位置
            {
                writer.WriteLine(pNode.fcb.start);
            }
            else if (pNode.fcb.type == FCB.FOLDER)            //若为文件夹则写入-1
            {
                writer.WriteLine(-1);
            }
            writer.WriteLine("#");                    //一个结点写完

            writer.Close();
        }

        /*位图文件*/
        public void readBitMap()
        {
            string path = Application.StartupPath + "\\BitMapInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < MyDisk.blockNum; i++)
                {
                    MyDisk.bitMap[i] = int.Parse(reader.ReadLine());
                }
                reader.Close();
            }
        }
        public void writeBitMap()
        {
            if (File.Exists(Application.StartupPath + "\\BitMapInfo.txt"))
                File.Delete(Application.StartupPath + "\\BitMapInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\BitMapInfo.txt");

            for (int i = 0; i < MyDisk.blockNum; i++)
            {
                writer.WriteLine(MyDisk.bitMap[i]);
            }
            writer.Close();
        }

        /*虚拟磁盘文件*/
        public void readMyDisk()
        {
            string path = Application.StartupPath + "\\MyDiskInfo.txt";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                for (int i = 0; i < MyDisk.blockNum; i++)
                {
                    string str = reader.ReadLine();
                    if (str.IndexOf("()") >= 0)
                        MyDisk.memory[i] = str.Replace("()", "\r\n");
                    else
                    {
                        if (str.IndexOf('(') >= 0)
                            MyDisk.memory[i] = str.Replace('(', '\r');
                        else if (str.IndexOf(')') >= 0)
                            MyDisk.memory[i] = str.Replace(')', '\n');
                        else if (str != "#")
                            MyDisk.memory[i] = str;
                        else if (str == "#")
                            MyDisk.memory[i] = "";
                    }
                }
                reader.Close();
            }
        }
        public void writeMyDisk()
        {
            if (File.Exists(Application.StartupPath + "\\MyDiskInfo.txt"))
                File.Delete(Application.StartupPath + "\\MyDiskInfo.txt");
            StreamWriter writer = new StreamWriter(Application.StartupPath + "\\MyDiskInfo.txt");

            for (int i = 0; i < MyDisk.blockNum; i++)
            {
                if (MyDisk.memory[i].IndexOf("\r\n") >= 0)
                {
                    writer.WriteLine(MyDisk.memory[i].Replace("\r\n", "()"));
                }
                else
                {
                    if (MyDisk.memory[i].IndexOf('\r') >= 0)
                    {
                        writer.WriteLine(MyDisk.memory[i].Replace('\r', '('));
                    }
                    else if (MyDisk.memory[i].IndexOf('\n') >= 0)
                    {
                        writer.WriteLine(MyDisk.memory[i].Replace('\n', ')'));
                    }
                    else if (MyDisk.memory[i] != "")
                        writer.WriteLine(MyDisk.memory[i]);
                    else
                        writer.WriteLine("#");
                }
            }
            writer.Close();
        }



        //主窗体关闭时，把所有的内容写入到本地   
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            updateLog();
        }

        //更新所有日志信息,将所有的内容写入到本地   
        public void updateLog()
        {
            if (File.Exists(Application.StartupPath + "\\CategoryInfo.txt"))
                File.Delete(Application.StartupPath + "\\CategoryInfo.txt");
            Category.Node temp = new Category.Node();
            string path = Application.StartupPath;
            Queue<Category.Node> q = new Queue<Category.Node>();
            q.Enqueue(category.root);

            while (q.Count() != 0)
            {
                temp = q.Dequeue();
                temp = temp.firstChild;
                while (temp != null)
                {
                    q.Enqueue(temp);
                    writeCategory(temp);
                    temp = temp.nextBrother;
                }
            }

            writeBitMap();
            writeMyDisk();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileWindow_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
