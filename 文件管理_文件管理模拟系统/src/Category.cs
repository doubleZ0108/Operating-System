using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    public class Category
    {
        //目录节点
        public class Node
        {
            //节点存储的数据
            public FCB fcb = new FCB();

            public Node firstChild = null;      //左孩子
            public Node nextBrother = null;     //右兄弟
            public Node parent = null;          //父结点

            public Node() { }
            public Node(FCB file)
            {
                fcb.fileName = file.fileName;
                fcb.lastModify = file.lastModify;
                fcb.type = file.type;
                fcb.size = file.size;
            }
            public Node(string name, int type)
            {
                fcb.fileName = name;
                fcb.type = type;
            }
        }

        public Node root;    //目录的根节点


        public Category()
        {
            root = null;
        }
        public Category(FCB rootName)
        {
            root = new Node(rootName);
            if (root == null)
            { return; }
        }

        /*清空某目录(当参数为root是为格式化)*/
        public void freeCategory(Node pNode)
        {
            if (pNode == null)
                { return; }

            if (pNode.firstChild != null)   //清空左孩子
            {
                freeCategory(pNode.firstChild);
                pNode.firstChild = null;
            }
            if (pNode.nextBrother != null)  //清空右兄弟
            {
                freeCategory(pNode.nextBrother);
                pNode.nextBrother = null;
            }

            pNode = null;                   //将自己清除
        }

        /*删掉结点*/
        public void delete(Node pNode)
        {
            pNode = null;
        }

        /*搜索文件*/
        public Node search(Node pNode, string fileName, int type)
        {
            if (pNode == null)
                { return null; }
            if (pNode.fcb.fileName == fileName && pNode.fcb.type == type)
                { return pNode; }
            if (pNode.firstChild == null && pNode.nextBrother == null)
                { return null; }
            else
            {
                Node firstChild = search(pNode.firstChild, fileName, type); //递归的搜索左孩子的子树
                if (firstChild != null)
                    { return firstChild; }
                else
                    { return search(pNode.nextBrother, fileName, type); }   //递归的搜索右兄弟的子树
            }
        }

        /*在文件夹中创建文件*/
        public void createFile(string parentFileName, FCB file)
        {
            if (root == null)
                { return; }
            Node parentNode = search(root, parentFileName, FCB.FOLDER);    //找到父文件夹

            if (parentNode == null)
                { return; }
            if (parentNode.firstChild == null)  //该文件夹为空
            {
                parentNode.firstChild = new Node(file);     //新创建的文件为第一个文件, 放到左孩子的位置
                parentNode.firstChild.parent = parentNode;
                return;
            }
            else
            {
                Node temp = parentNode.firstChild;
                while (temp.nextBrother != null)        //顺序找到该文件夹下最后一个文件存储的位置
                    temp = temp.nextBrother;

                temp.nextBrother = new Node(file);
                temp.nextBrother.parent = parentNode;

            }
        }

        /*删除文件夹*/
        public void deleteFolder(string name)
        {
            Node currentNode = search(root, name, FCB.FOLDER);   //找到要删除的结点
            Node parentNode = currentNode.parent;

            if (parentNode.firstChild == currentNode)   //如果要删除的文件夹是父文件夹中的第一项
                { parentNode.firstChild = currentNode.nextBrother; }    //更新父文件夹的左孩子
            else
            {
                Node temp = parentNode.firstChild;
                while (temp.nextBrother != currentNode)
                {
                    temp = temp.nextBrother;
                }

                temp.nextBrother = currentNode.nextBrother;     //找到该文件的哥哥, 让其指向自己的弟弟
            }

            freeCategory(currentNode);        //删除当前结点下的所有文件
        }

        /*删除文件*/
        public void deleteFile(string name)
        {
            Node currentNode = search(root, name, FCB.TXTFILE);   //找到要删除的文件
            Node parentNode = currentNode.parent;

            if (parentNode.firstChild == currentNode)   //如果要删除的文件是父文件夹中的第一项
            { parentNode.firstChild = currentNode.nextBrother; }    //更新父文件夹的左孩子    
            else
            {
                Node temp = parentNode.firstChild;
                while (temp.nextBrother != currentNode)
                {
                    temp = temp.nextBrother;
                }

                temp.nextBrother = currentNode.nextBrother;
            }
            currentNode = null;
        }

        /*判断同级目录下是否重名*/
        public bool noSameName(string name, Node pNode, int type)
        {
            //pNode为该级目录的根节点
            pNode = pNode.firstChild;
            if (pNode == null)  //该目录中无任何文件夹和文件
                { return true; }
            if (pNode.fcb.fileName == name && pNode.fcb.type == type)   //第一个文件夹/文件重名
                { return false; }
            else
            {
                Node temp = pNode.nextBrother;
                while (temp != null)
                {
                    if (temp.fcb.fileName == name && temp.fcb.type == type)
                        { return false; }
                    temp = pNode.nextBrother;
                }
                return true; //不重名
            }
        }

        /*寻找该目录下根目录的名称*/
        public Node currentRootName(Node pNode, string name, int type)
        {
            if (pNode == null)
                { return null; }
            if (pNode.firstChild == null)
                { return null; }
            else
            {
                if (pNode.firstChild.fcb.fileName == name && pNode.firstChild.fcb.type == type)
                    { return pNode; }
                else
                {
                    Node par = pNode;
                    Node temp = pNode.firstChild.nextBrother;
                    while (temp != null)
                    {
                        if (temp.fcb.fileName == name && temp.fcb.type == type)
                            { return par; }
                        else
                            { temp = temp.nextBrother; }
                    }
                    if (currentRootName(par.firstChild, name, type) != null)    //递归地在左孩子子树中寻找
                    {
                        return currentRootName(par.firstChild, name, type);
                    }
                    else
                    {
                        return currentRootName(par.firstChild.nextBrother, name, type); //递归地在右兄弟子树中寻找
                    }
                }
            }
        }
    }
}
