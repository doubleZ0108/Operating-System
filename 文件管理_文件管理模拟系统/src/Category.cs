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

        //清空某一个目录,清空全目录时要使用root
        public void freeCategory(Node pNode)
        {
            if (pNode == null)
                return;
            if (pNode.firstChild != null)
            {
                freeCategory(pNode.firstChild);
                pNode.firstChild = null;
            }
            if (pNode.nextBrother != null)
            {
                freeCategory(pNode.nextBrother);
                pNode.nextBrother = null;
            }
            pNode = null;
        }
        //删掉结点
        public void delete(Node pNode)
        {
            pNode = null;
        }
        //寻找文件
        public Node search(Node pNode,string fileName,int type)
        {
            //fileName为要寻找的文件的名称
            if (pNode == null)
                return null;
            if (pNode.fcb.fileName == fileName && pNode.fcb.type == type)
                return pNode;
            if (pNode.firstChild == null && pNode.nextBrother == null)
            {
                return null;
            }
            else
            {
                Node temp = search(pNode.firstChild, fileName, type);
                if (temp != null)
                    return temp;
                else
                    return search(pNode.nextBrother, fileName, type);
            }
        }
        //根据根目录创建文件
        public void createFile(string parentFileName, FCB file)
        {
            if (root == null)
                return;
            Node parentNode = search(root, parentFileName, 1);    //找到父文件
            
            if (parentNode == null)
                return;
            if(parentNode.firstChild==null)
            {
                parentNode.firstChild = new Node(file);
                parentNode.firstChild.parent = parentNode;
                return;
            }
            else
            {
                Node temp = parentNode.firstChild;
                while (temp.nextBrother != null)
                    temp = temp.nextBrother;
                temp.nextBrother = new Node(file);
                temp.nextBrother.parent = parentNode;
            }
        }
        //删除文件夹
        public void deleteFolder(string name)
        {

            Node currentNode = search(root, name, 1);   //找到要删除的结点
            Node parentNode = currentNode.parent;
            if (parentNode.firstChild == currentNode)
                parentNode.firstChild = currentNode.nextBrother;
            else
            {
                parentNode = parentNode.firstChild;
                while (parentNode.nextBrother != currentNode)
                    parentNode = parentNode.nextBrother;
                parentNode.nextBrother = currentNode.nextBrother;
            }

            freeCategory(currentNode);        //删除当前结点下的所有文件
        }
        //删除文件
        public void deleteFile(string name)
        {
            Node currentNode = search(root, name, 0);   //找到要删除的文件
            Node parentNode = currentNode.parent;
            if (parentNode.firstChild == currentNode)  //如果是左子结点
                parentNode.firstChild = currentNode.nextBrother;
            else
            {
                Node temp = parentNode.firstChild;
                while(temp.nextBrother!=currentNode)
                {
                    temp = temp.nextBrother;
                }
                temp.nextBrother = currentNode.nextBrother;
            }
            currentNode = null; 
        }
        //查找同级目录下是否重名
        public bool noSameName(string name,Node pNode,int type)
        {
            //pNode为该级目录的根节点
            pNode = pNode.firstChild;
            if (pNode == null)  //不重名
                return true; 
            if (pNode.fcb.fileName == name && pNode.fcb.type == type)
                return false;
            else
            {
                pNode = pNode.nextBrother;
                while(pNode!=null)
                {
                    if (pNode.fcb.fileName == name && pNode.fcb.type == type)
                        return false;
                    pNode = pNode.nextBrother;
                }
                return true; //不重名
            }
        }
        //寻找根目录的名称
        public Node currentRootName(Node pNode, string name, int type)
        {
            if (pNode == null)
                return null;
            if (pNode.firstChild == null)
                return null;
            else
            {
                if (pNode.firstChild.fcb.fileName == name && pNode.firstChild.fcb.type == type)
                    return pNode;
                else
                {
                    Node par = pNode;
                    pNode = pNode.firstChild.nextBrother;
                    while (pNode != null)
                    {
                        if (pNode.fcb.fileName == name && pNode.fcb.type == type)
                            return par;
                        else
                            pNode = pNode.nextBrother;
                    }
                    if (currentRootName(par.firstChild, name, type) != null)
                        return currentRootName(par.firstChild, name, type);
                    else
                        return currentRootName(par.firstChild.nextBrother, name, type);
                }
            }
        }
    }
}
