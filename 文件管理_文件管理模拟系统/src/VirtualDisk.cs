using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    public class VirtualDisk
    {
        public const int EMPTY = -1;        //当前块为空
        public const int END = -2;          //结束标识

        public int size;                             //磁盘容量
        public int remain;                           //磁盘剩余空间
        public int blockSize;                        //块大小
        public int blockNum;                         //磁盘块数
        public string[] memory = new string[] { };   //内存空间
        public int[] bitMap = new int[] { };         //位图表


        public VirtualDisk(int size, int blocksize)
        {
            this.size = size;
            this.blockSize = blocksize;
            this.blockNum = size / blockSize;
            this.remain = blockNum;

            this.memory = new string[blockNum]; 
            this.bitMap = new int[blockNum];    
            for (int i = 0; i < blockNum; i++)
            {
                this.bitMap[i] = EMPTY;     //初始化位图表为全部可用
                this.memory[i] = "";     //初始化内存内容为空
            }
                
        }

        /*给文件分配空间并添加内容*/
        public bool giveSpace(FCB fcb, string content)
        {
            int blocks = getBlockSize(fcb.size);

            if (blocks <= remain)
            {
                /*找到该文件开始存放的位置*/
                int start = 0;
                for (int i = 0; i < blockNum; i++) 
                {
                    if (bitMap[i] == EMPTY)
                    {
                        remain--;
                        start = i;
                        fcb.start = i;
                        memory[i] = content.Substring(0, blockSize);

                        break;
                    }
                }

                /*从该位置往后开始存放内容*/
                for (int j = 1, i = start + 1; j < blocks && i < blockNum; i++)
                {
                    if (bitMap[i] == EMPTY)
                    {
                        remain--;

                        bitMap[start] = i;  //以链接的方式存储每位数据
                        start = i;

                        if (blocks != 1)
                        {
                            if (j != blocks - 1)
                            {
                                memory[i] = content.Substring(j * blockSize, blockSize);
                            }
                            else
                            {
                                bitMap[i] = END;    //文件尾
                                if (fcb.size % blockSize != 0)
                                {
                                    memory[i] = content.Substring(j * blockSize, content.Length - j * blockSize);
                                }
                                else
                                {
                                    memory[i] = content.Substring(j * blockSize, blockSize);
                                }
                            }

                        }
                        else
                            { memory[i] = content; }

                        j++;   //找到一个位置
                    }
                }
                return true;
            }
            else
            { return false; }

        }

        /*读取文件内容*/
        public string  getFileContent(FCB fcb)
        {
            if (fcb.start == EMPTY)
                { return ""; }
            else
            {
                string content = "";
                int start = fcb.start;
                int blocks = getBlockSize(fcb.size);

                int count = 0, i = start;
                while(i < blockNum)
                {
                    if (count == blocks)
                    {
                        break;
                    }
                    else
                    {
                        content += memory[i];       //内容拼接内存的一个单元的数据
                        i = bitMap[i];              //跳转到位图指向的下一个存储单元
                        count++;
                    }
                }

                return content;
            }
        }

        /*删除文件内容*/
        public void deleteFileContent(int start,int size)
        {
            int blocks = getBlockSize(size);

            int count = 0, i = start;
            while(i < blockNum)
            {
                if (count == blocks)
                {
                    break;
                }
                else
                {
                    memory[i] = "";             //逐内存单元的清空
                    remain++;

                    int next = bitMap[i];       //先记录即将跳转的位置
                    bitMap[i] = EMPTY;          //清空该位
                    i = next;

                    count++;
                }
            }
        }

        /*更新文件内容*/
        public void fileUpdate(int oldStart,int oldSize, FCB newFcb, string newContent)
        {
            deleteFileContent(oldStart, oldSize);   //删除原内容
            giveSpace(newFcb, newContent);          //开辟新的块并添加内容
        }


        /*获取所占块数*/
        private int getBlockSize(int size)
        {
            return size / blockSize + (size % blockSize != 0 ? 1 : 0);
        }
    }
}
