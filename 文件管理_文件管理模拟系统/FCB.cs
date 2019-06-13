using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManageSystem
{
    //目录项
    public class FCB
    {
        public const int TXTFILE = 0;   //文本文件标识
        public const int FOLDER = 1;    //文件夹标识

        public string fileName;       //文件名
        public int start;             //文件在内存中初始存放的位置
        public int type;              //文件类型 => TXTDILE/FOLDER 
        public string lastModify;     //最近修改时间         
        public int size;              //文件大小,文件夹不显示大小

        public FCB() { }
        public FCB(string name,int type, string time, int size)
        {
            this.fileName = name;
            this.type = type;
            this.lastModify = time;
            this.size = size;
        }
        public FCB(string name, int type, string time, int size,int start)
        {
            this.fileName = name;
            this.type = type;
            this.lastModify = time;
            this.size = size;
            this.start = start;
        }
    }
}
