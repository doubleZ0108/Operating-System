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
        public Category category = new Category();
        public Category.Node rootNode = new Category.Node();   //目录的根节点
        public Category.Node currentRoot = new Category.Node(); //目录的当前根节点
        public VirtualDisk MyDisk = new VirtualDisk(1000, 2);      //申请内存空间
        public string nowPath;
        public string searchStr = "";
        public string change;
        public MainForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            FCB root = new FCB("root", 1,"",1);
            rootNode = new Category.Node(root);
            currentRoot = rootNode;
            category.root = rootNode;
            readFormDisk();        //把磁盘中的文件读回来
            readBitMap();
            readMyDisk();
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
                    NoteForm file = new NoteForm(bt.Text, this);
                    file.Show();
                }
                else if (bt.Tag.ToString() == "1")//文件夹
                {
                    currentRoot = category.search(rootNode, bt.Text, 1);
                    nowPath = nowPath + "> " + bt.Text;
                    fileFormInit(currentRoot);
                }
            }
            else
            {
                string filename = fileWindow.contextMenuStrip_FileChoose.SourceControl.Text;
                string type = fileWindow.contextMenuStrip_FileChoose.SourceControl.Tag.ToString();
                if(((ToolStripMenuItem)sender).Name== "打开ToolStripMenuItem")
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
                else if(((ToolStripMenuItem)sender).Name == "删除ToolStripMenuItem")
                {
                    if(type=="0")
                    {
                        delete(filename, 0);
                    }
                    else if(type=="1")
                    {
                        delete(filename, 1);
                    }
                }
            }
        }
        //从本地读文件内容
        public void readFormDisk()
        {
            //把本地保存的上次的结点信息写回来
            string path = Application.StartupPath + "\\CategoryInfo.txt";
            if (File.Exists(path))
            {
                StreamReader sw = new StreamReader(path);
                string parentName="", Name="";    //父结点名字，自己的名字
                int type = -1;              //文件的类型
                string lastModify = "";     //最后修改时间
                int size = 0;              //文件大小
                int start = -1;            //文件开始的位置
                int infoNum = 1;
                string str = sw.ReadLine();
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
                    }
                    str = sw.ReadLine();
                }
                sw.Close();
            }

        }
        //从本地读取位图
        public void readBitMap()
        {
            string path = Application.StartupPath + "\\BitMapInfo.txt";
            if(File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                for(int i=0;i<MyDisk.blockNum;i++)
                {
                    MyDisk.bitMap[i] = int.Parse(sr.ReadLine());
                }
                sr.Close();
            }
        }
        //从本地读内存
        public void readMyDisk()
        {
            string path = Application.StartupPath + "\\MyDiskInfo.txt";
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                for (int i = 0; i < MyDisk.blockNum; i++)
                {
                    string str = sr.ReadLine();
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
                sr.Close();
            }
        }
        //初始化当前结点下的界面
        public void fileFormInit(Category.Node now)
        {
            labelDiskSize.Text = "当前磁盘大小: " + MyDisk.size.ToString() + "B";
            labelBlockSize.Text = "当前盘块大小: " + MyDisk.blockSize.ToString() + "B";
            if (now.fcb.fileName == "root")
                buttonBack.Enabled = false;
            else
                buttonBack.Enabled = true;
            textBoxSearch.Text = nowPath;
            string name = now.fcb.fileName;
            //窗体初始化
            this.fileWindow.Init();
            if (now.firstChild == null)
                return;
            Category.Node temp = new Category.Node();
            temp = now.firstChild; 
            fileWindow.showFiles(temp.fcb.fileName, temp.fcb.lastModify, temp.fcb.type, temp.fcb.size);
            temp = temp.nextBrother;
            while(temp!=null)
            {
                FCB current = temp.fcb;
                fileWindow.showFiles(temp.fcb.fileName, temp.fcb.lastModify, temp.fcb.type, temp.fcb.size);
                temp = temp.nextBrother;
            }
        }
        //界面加载
        private void Form1_Load(object sender, EventArgs e)
        {
            HelpForm helpForm = new HelpForm();
            helpForm.StartPosition = FormStartPosition.CenterScreen;
            helpForm.Show(this);

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
                if (category.noSameName(str, rootNode, 0))
                {
                    string time = DateTime.Now.ToLocalTime().ToString();
                    if (fileWindow.showFiles(str, time, 0, 0))
                        category.createFile(currentRoot.fcb.fileName, new FCB(str, 0, time, 0));  //文件加入到目录中
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
        //新建文件夹
        public void createFolder()
        {
            string str = Interaction.InputBox("请输入文件夹的名称", "字符串", "", 100, 100);
            if (str != "")
            {
                if (category.noSameName(str, rootNode, 1))
                {
                    string time = DateTime.Now.ToLocalTime().ToString();
                    if (fileWindow.showFiles(str, time, 1, 0))
                        category.createFile(currentRoot.fcb.fileName, new FCB(str, 1, time, 0));  //文件夹加入到目录中
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
        //新建文件夹
        private void 新建文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFolder();
        }
        //新建文件
        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createFile();
        }
        //删除内容
        public void delete(string name,int type)
        {
            if(type==0)  //删除文件
            {
                FCB deleteFCB = category.search(rootNode, name, type).fcb;
                category.deleteFile(name);
                category.delete(category.search(rootNode, name, 0));
                MyDisk.deleteFileContent(deleteFCB.start,deleteFCB.size);
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
        //往本地写信息
        public void writeCategory(Category.Node pNode)
        {
            //把当前结点对应的结点信息保存到CategoryInfo文件中
            //保存结点信息，1个结点写6个内容，#表示一个结点全部写完
            //当前结点的父结点
            Category.Node parentNode = category.currentRootName(rootNode, pNode.fcb.fileName, pNode.fcb.type);
            string InfoPath = Application.StartupPath + "\\CategoryInfo.txt";
            StreamWriter SW = File.AppendText(InfoPath);
            SW.WriteLine(parentNode.fcb.fileName); //写入父结点的名字
            SW.WriteLine(pNode.fcb.fileName);     //写入文件的名字
            SW.WriteLine(pNode.fcb.type);         //写入文件的类型
            SW.WriteLine(pNode.fcb.lastModify);   //写入最后修改的时间
            SW.WriteLine(pNode.fcb.size);         //写入文件的大小
            if (pNode.fcb.type == 0)                //写入文件的开始位置
            { 
                SW.WriteLine(pNode.fcb.start);
            }
            else if(pNode.fcb.type==1)            //若为文件夹则写入-1
            {
                SW.WriteLine(-1);
            }
            SW.WriteLine("#");                    //一个结点写完
            SW.Close();
        }
        //把位图写在本地
        public void writeBitMap()
        {
            if (File.Exists(Application.StartupPath + "\\BitMapInfo.txt"))
                File.Delete(Application.StartupPath + "\\BitMapInfo.txt");
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\BitMapInfo.txt");
            for(int i=0;i<MyDisk.blockNum;i++)
            {
                sw.WriteLine(MyDisk.bitMap[i]);
            }
            sw.Close();
        }
        //把内存写在本地
        public void writeMyDisk()
        {
            if (File.Exists(Application.StartupPath + "\\MyDiskInfo.txt"))
                File.Delete(Application.StartupPath + "\\MyDiskInfo.txt");
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\MyDiskInfo.txt");
            for (int i = 0; i < MyDisk.blockNum; i++)
            {
                if(MyDisk.memory[i].IndexOf("\r\n")>=0)
                {
                    sw.WriteLine(MyDisk.memory[i].Replace("\r\n", "()"));
                }
                else
                {
                    if (MyDisk.memory[i].IndexOf('\r') >= 0)
                    {
                        sw.WriteLine(MyDisk.memory[i].Replace('\r', '('));
                    }
                    else if (MyDisk.memory[i].IndexOf('\n') >= 0)
                    {
                        sw.WriteLine(MyDisk.memory[i].Replace('\n', ')'));
                    }
                    else if (MyDisk.memory[i] != "")
                        sw.WriteLine(MyDisk.memory[i]);
                    else
                        sw.WriteLine("#");
                }
            }
            sw.Close();
        }
        //主窗体关闭时，把所有的内容写入到本地   
        protected override void OnFormClosing(FormClosingEventArgs e)
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
        //根据文件名搜索文件
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (searchStr != "" && searchStr != null)
            {
                int end = searchStr.Length - 1;
                int start = searchStr.LastIndexOf('\\') + 1;
                string name = searchStr.Substring(start, end - start + 1);  //包含后缀的名字
                if (name.Length > 4)
                {
                    string folderOrFile = name.Substring(name.Length - 4, 4);
                    if (folderOrFile == ".txt")  //文件
                    {
                        Category.Node cur = category.search(category.root, name.Replace(".txt", ""), 0);
                        if (cur != null)
                        {
                            NoteForm file = new NoteForm(cur.fcb.fileName, this);
                            file.Show();
                        }
                        else
                        {
                            MessageBox.Show("文件" + name + "不存在！请尝试重新输入。");
                        }
                    }
                    else   //文件夹
                    {
                        Category.Node cur = category.search(category.root, name, 1);
                        if (cur != null)
                        {
                            currentRoot = cur;
                            nowPath = "";
                            foreach (var c in searchStr)  //更新当前路径
                            {
                                if (c == '\\')
                                    nowPath += "> ";
                                else
                                    nowPath += c.ToString();
                            }
                            textBoxSearch.Text = "";
                            fileFormInit(cur);
                        }
                        else
                        {
                            MessageBox.Show("文件夹" + name + "不存在！请尝试重新输入。");
                        }
                    }
                }
                else //文件夹
                {
                    Category.Node cur = category.search(category.root, name, 1);
                    if (cur != null)
                    {
                        currentRoot = cur;
                        nowPath = "";
                        foreach (var c in searchStr)  //更新当前路径
                        {
                            if (c == '\\')
                                nowPath += "> ";
                            else
                                nowPath += c.ToString();
                        }
                        textBoxSearch.Text = "";
                        fileFormInit(cur);
                    }
                    else
                        MessageBox.Show("文件夹" + name + "不存在！请尝试重新输入");
                }
            }
        }
        //鼠标进入搜索框时的显示
        private void textBoxSearch_MouseClick(object sender, MouseEventArgs e)
        {
            string newPath = "";
            if(textBoxSearch.Text.ToString()!="")
            {
                foreach (var c in nowPath)
                {
                    if (c != '>' && c != ' ')
                        newPath += c.ToString();
                    else if (c == ' ')
                        newPath += "\\";
                }
                textBoxSearch.Text = newPath;
                //光标设置
                textBoxSearch.Focus();
                textBoxSearch.Select(textBoxSearch.Text.Length, 0);
                textBoxSearch.ScrollToCaret();
            }
        }
        //鼠标离开搜索框时的显示
        private void textBoxSearch_Leave(object sender, EventArgs e)
        {
            searchStr = change;
            textBoxSearch.Text = nowPath;
        }
        //搜索框内容改变
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            change = textBoxSearch.Text;
        }
        //点击清空按钮
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定清空磁盘？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result==DialogResult.OK)
            {
                category.freeCategory(category.root);
                for(int i=0;i<MyDisk.blockSize;i++)
                {
                    MyDisk.memory[i] = "";
                    MyDisk.bitMap[i] = -1;
                    MyDisk.remain = MyDisk.blockNum;
                }
                MessageBox.Show("磁盘已清空。");
                fileFormInit(rootNode);
            }
            nowPath = "";
            textBoxSearch.Text = "";
            treeView.Nodes.Clear();
        }
        //新建文件夹
        private void buttonCreateFolder_Click(object sender, EventArgs e)
        {
            createFolder();
        }
        //新建文件
        private void buttonCreateFile_Click(object sender, EventArgs e)
        {
            createFile();
        }
        //创建目录树
        public void createTreeView(Category.Node pNode)
        {
            if (pNode == null)
                return;

            TreeNode tn = new TreeNode();
            if (pNode.fcb.type == 1)
            {
                tn.Name = pNode.fcb.fileName;
                tn.Text = pNode.fcb.fileName;
                tn.Tag = 1;
                tn.ImageIndex = 1;
                tn.SelectedImageIndex = 1;
            }
            else if (pNode.fcb.type == 0)
            {
                tn.Name = pNode.fcb.fileName + ".txt";
                tn.Text = pNode.fcb.fileName + ".txt";
                tn.Tag = 0;
                tn.ImageIndex = 0;
                tn.SelectedImageIndex = 0;
            }
            if (pNode.parent == rootNode)
            {
                treeView.Nodes.Add(tn);
            }
            else
            {
                CallAddNode(treeView, pNode.parent.fcb.fileName, tn);
            }
            //if (depth == 0)
            //{
            //    treeView.Nodes.Add(tn);
            //}
            //else if (depth == 1)
            //{
            //    foreach (TreeNode node in treeView.Nodes)
            //    {
            //        if (node.Name == pNode.parent.fcb.fileName)
            //        {
            //            node.Nodes.Add(tn);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (TreeNode node in treeView.Nodes[depth - 1].Nodes)
            //    {
            //        if (node.Name == pNode.parent.fcb.fileName)
            //        {
            //            node.Nodes.Add(tn);
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for(int i=0;i<)
            //}

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
                currentRoot = category.search(rootNode, tn.Text, 1);
                nowPath = "";
                while (tn.Parent != null)
                {
                    s.Push(tn.Parent);
                    tn = tn.Parent;
                }
                nowPath = "";
                while(s.Count()!=0)
                {
                    nowPath += "> " + s.Pop().Text;
                }
                textBoxSearch.Text = nowPath;
                fileFormInit(currentRoot);
            }
            else if (tn.Tag.ToString() == "0") //打开文件
            {
                NoteForm file = new NoteForm(tn.Text.Replace(".txt", ""), this);
                file.Show();
            }
        }
        //递归查找往目录树中加入结点，两个函数
        public TreeNode AddNode(TreeNode tnParent,string tnStr,TreeNode newTn)
        {
            if (tnParent == null)
                return null;
            if (tnParent.Name == tnStr)
                tnParent.Nodes.Add(newTn);
            TreeNode tnRet = null;
            foreach(TreeNode tn in tnParent.Nodes)
            {
                tnRet = AddNode(tn, tnStr,newTn);
                if (tnRet != null)
                {
                    tnRet.Nodes.Add(newTn);
                    break;
                }
            }
            return tnRet;
        }
        public TreeNode CallAddNode(TreeView tree,string tnStr,TreeNode newTn)
        {
            foreach(TreeNode n in tree.Nodes)
            {
                TreeNode temp = AddNode(n, tnStr, newTn);
                if (temp != null)
                    return temp;
            }
            return null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileWindow_Load(object sender, EventArgs e)
        {

        }
    }
    
}
