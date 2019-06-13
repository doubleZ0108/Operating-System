# 文件管理 - 文件系统

###### 操作系统第三次课程作业 - 文件系统

❗❗❗ 请勿删除可执行程序目录下的`BitMapInfo.txt` ,`CategoryInfo.txt `, `MyDiskInfo.txt`, 及其他配置文件, 否则可能导致文件系统无法正常运行❗❗❗ 

## 目录

- [开发环境](#开发环境)
- [项目结构](#项目结构)
- [操作说明](#操作说明)
- [作者](#作者)

<a name="开发环境"></a>  

## 开发环境

- **开发环境:** Windows 10

- **开发软件:** 

  **Visual Studio 2017** *15.9.28307.665*
  
- **开发语言:** C#

<a name="项目结构"></a>  

## 项目结构

│  BitMapInfo.txt   
│  CategoryInfo.txt   
│  FileManageSystem.exe   
│  FileManageSystem.exe.config   
│  FileManageSystem.pdb   
│  MyControl.dll   
│  MyControl.pdb   
│  MyDiskInfo.txt   
│  README.md   
│  文件管理系统_设计方案报告.md   
│  文件管理系统_设计方案报告.pdf   
│  
├─Resources   
│  │  file18.png   
│  │  file25.png   
│  │  fileopen48.ico   
│  │  folder18.png  
│  │  folder25.png   
│  │  
│  └─icon   
│          help.ico   
│          icon.ico   
│          note.ico   
│          
└─src
    │  Category.cs   
    │  FCB.cs   
    │  Program.cs   
    │  VirtualDisk.cs   
    │  
    └─Form   
        ├─HelpForm   
        │      HelpForm.cs   
        │      HelpForm.Designer.cs   
        │      HelpForm.resx   
        │      
        ├─MainForm   
        │      MainForm.cs   
        │      MainForm.Designer.cs   
        │      MainForm.resx   
        │      
        └─NoteForm   
                NoteForm.cs   
                NoteForm.Designer.cs   
                NoteForm.resx   
               

<a name="操作说明"></a>  

## 操作说明

- 双击目录下`FileManagementSystem.exe`可执行文件进入文件系统模拟界面

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-f936ef8c2fb62a75.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 请详细阅读**操作帮助**了解模拟器功能, 点击`我知道了`关闭**帮助信息窗口**

- 单击**鼠标右键**, 新建文件夹/新建文件

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-34878e51c17b5e94.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 您也可以点击**右侧按钮**进行创建文件夹/文本文件

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-6261b5c003ac4441.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 输入文件夹名或文件名, 即可在目录中查看到创建的文件夹/文件

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-cf472e4135fc764b.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-a378aa1980c9e33f.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **单击文件夹**可进入下一级文件夹, 上方可查看**当前文件路径**

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-38c1e4affdd75451.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **单击文件**可打开**编辑窗口**, 编辑完成后点击右上角的`X`, *确定以保存, 取消则直接退出*

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-c31f741e16c737a3.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 可查看**文件相应信息**(文件名, 上次修改日期, 文件大小*(自定义单位)*)

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-c530df2f3d520d4e.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 左侧的**目录树**可点击展开, 以查看当前目录结构

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-6c7ac708bf412f4b.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **双击目录树**中的文件夹打开文件夹, 双击目录树中的文件可打开文件进行浏览和编辑

- 点击**返回上级目录**可跳转到上级文件夹, 在root文件夹时无法返回

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-791d9eaafa9e28cd.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 点击右侧**格式化**并再次确定可清空磁盘, 系统会清空所有文件夹和文本文件, 并清空目录树

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-7d650e0e128ea446.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- ❗❗❗受展示限制, 本文件系统模拟器在一个目录下最多可创建8个子项目, 超过8个时会受到系统提醒*<u>(只是受展示方式限制, 物理和逻辑上的存储理论上都允许创建无限多的子项目)</u>*

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-e2418b7652502b4e.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)



<a name="作者"></a>  

#### 作者

​	**学号**				1754060 

​	**姓名**				张喆

​	**指导老师**		王冬青老师

​	**上课时间**		周三/周五 上午一二节

​	**联系方式**		*email:* doubleZ0108@gmail.com