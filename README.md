# 操作系统课程项目

* [项目说明](#项目说明)
   * [进程管理-电梯调度](#进程管理-电梯调度)
   * [内存管理](#内存管理)
      * [动态分区分配方式](#动态分区分配方式)
      * [请求分区分配方式](#请求分区分配方式)
   * [文件管理-文件管理模拟系统](#文件管理-文件管理模拟系统)
* [项目结构](#项目结构)
* [作者](#作者)

------

## 项目说明

### 进程管理-电梯调度

1. 每个电梯里面设置必要功能键：如**数字键**、**关门键**、**开门键**、**上行键**、**下行键**、**报警键**、当前电梯的**楼层数**、**上升及下降状态**等。
2. 每层楼的每部电梯门口，应该有**上行和下行按钮**和当前**电梯状态的数码显示器**。
3. 五部电梯门口的**按钮是互联结的**，即当一个电梯按钮按下去时，其他电梯的相应按钮也就同时点亮，表示也按下去了。
4. 所有电梯初始状态都在第一层。每个电梯如果在它的上层或者下层没有相应请求情况下，则应该**在原地保持不动**。

 <img src="https://upload-images.jianshu.io/upload_images/12014150-9c8f6f1b7547b823.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<img src="https://upload-images.jianshu.io/upload_images/12014150-3ee5d372211bfe25.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<br/>

### 内存管理

#### 动态分区分配方式

假设初始态下，可用内存空间为640K，并有下列请求序列，请分别用首次适应算法和最佳适应算法进行内存块的分配和回收，并显示出每次分配和回收后的空闲分区链的情况来。

| 作业1申请130K |
| :-----------: |
| 作业2申请 60K |
| 作业3申请100k |
| 作业2释放 60K |
| 作业4申请200K |
| 作业3释放100K |
| 作业1释放130K |
| 作业5申请140K |
| 作业6申请 60K |
| 作业7申请 50K |
| 作业6释放 60K |

<img src="https://upload-images.jianshu.io/upload_images/12014150-e9e2c23eb0309973.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<img src="https://upload-images.jianshu.io/upload_images/12014150-0b336d12d32d0cd8.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

#### 请求分区分配方式

- 在模拟过程中，如果所访问指令在内存中，则显示其物理地址，并转到下一条指令；如果没有在内存中，则发生缺页，此时需要记录缺页次数，并将其调入内存。如果4个内存块中已装入作业，则需进行页面置换。

- 所有320条指令执行完成后，计算并显示作业执行过程中发生的缺页率。

- 置换算法可以选用FIFO或者LRU算法

- 作业中指令访问次序可以按照下面原则形成：  

      50%的指令是顺序执行的，25%是均匀分布在前地址部分，25％是均匀分布在后地址部分

<img src="https://upload-images.jianshu.io/upload_images/12014150-1060f1b40ab787fc.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<br/>

### 文件管理-文件管理模拟系统

- 文件存储空间管理可采取显式链接（如FAT）或者其他方法。（即自选一种方法）

- 空闲空间管理可采用位图或者其他方法。如果采用了位图，可将位图和FAT表合二为一。

- 文件目录采用多级目录结构。至于是否采用索引节点结构，自选。目录项目中应包含：文件名、物理地址、长度等信息。同学可在这里增加一些其他信息。

- 文件系统提供的操作：

  - 格式化
  - 创建子目录
  - 删除子目录
  - 显示目录
  - 更改当前目录
  - 创建文件
  - 打开文件
  - 关闭文件
  - 写文件
  - 读文件
  - 删除文件

<img src="https://upload-images.jianshu.io/upload_images/12014150-f936ef8c2fb62a75.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<img src="https://upload-images.jianshu.io/upload_images/12014150-c31f741e16c737a3.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240" alt="image.png" style="zoom:50%;" />

<br/>

## 项目结构

<details>
	<summary>项目结构树</summary>

```
├── README.md
├── this.md
├── 进程管理_电梯调度
│   ├── README.md
│   ├── Resources
│   │   ├── Button
│   │   │   ├── doordown.png
│   │   │   ├── doordown_hover.png
│   │   │   ├── doordown_pressed.png
│   │   │   ├── doorup.png
│   │   │   ├── doorup_hover.png
│   │   │   ├── doorup_pressed.png
│   │   │   ├── down.png
│   │   │   ├── down_hover.png
│   │   │   ├── down_pressed.png
│   │   │   ├── state.png
│   │   │   ├── state_down.png
│   │   │   ├── state_up.png
│   │   │   ├── up.png
│   │   │   ├── up_hover.png
│   │   │   └── up_pressed.png
│   │   ├── Figure
│   │   │   └── people.png
│   │   └── Icon
│   │       ├── elevator.ico
│   │       └── icon.png
│   ├── Src
│   │   ├── dispatch.py
│   │   ├── myElevator.py
│   │   └── myElevatorInterface.py
│   ├── 电梯调度_设计方案报告.md
│   └── 电梯调度_设计方案报告.pdf
├── 内存管理_动态分区分配方式
│   ├── README.md
│   ├── Resource
│   │   └── memory.png
│   ├── src
│   │   ├── Dynamic partition allocation.html
│   │   └── static
│   │       ├── css
│   │       │   ├── range.css
│   │       │   └── style.css
│   │       └── js
│   │           ├── RangeSlider.js
│   │           ├── clear.js
│   │           ├── nextAssingment.js
│   │           ├── randColor.js
│   │           └── select.js
│   ├── 动态分区分配方式模拟_设计方案报告.md
│   └── 动态分区分配方式模拟_设计方案报告.pdf
├── 内存管理_请求分区分配方式
│   ├── DemandPaging.exe
│   ├── README.md
│   ├── src
│   │   └── DemandPaging.cpp
│   ├── 请求分区分配方式模拟_设计方案报告.md
│   └── 请求分区分配方式模拟_设计方案报告.pdf
└── 文件管理_文件管理模拟系统
    ├── BitMapInfo.txt
    ├── CategoryInfo.txt
    ├── FileManageSystem.exe
    ├── FileManageSystem.exe.config
    ├── FileManageSystem.pdb
    ├── MyControl.dll
    ├── MyControl.pdb
    ├── MyDiskInfo.txt
    ├── README.md
    ├── Resources
    │   ├── file18.png
    │   ├── file25.png
    │   ├── fileopen48.ico
    │   ├── folder18.png
    │   ├── folder25.png
    │   └── icon
    │       ├── help.ico
    │       ├── icon.ico
    │       └── note.ico
    ├── src
    │   ├── Category.cs
    │   ├── FCB.cs
    │   ├── Form
    │   │   ├── HelpForm
    │   │   │   ├── HelpForm.Designer.cs
    │   │   │   ├── HelpForm.cs
    │   │   │   └── HelpForm.resx
    │   │   ├── MainForm
    │   │   │   ├── MainForm.Designer.cs
    │   │   │   ├── MainForm.cs
    │   │   │   └── MainForm.resx
    │   │   └── NoteForm
    │   │       ├── NoteForm.Designer.cs
    │   │       ├── NoteForm.cs
    │   │       └── NoteForm.resx
    │   ├── Program.cs
    │   └── VirtualDisk.cs
    ├── 工程文件整体(防丢失).rar
    ├── 文件管理系统_设计方案报告.md
    └── 文件管理系统_设计方案报告.pdf
```
</details>

<br/>

## 作者

| Name     | 张喆                                                |
| -------- | --------------------------------------------------- |
| 学号     | 1754060                                             |
| 指导老师 | 王冬青老师                                          |
| 课程名称 | 计算机视觉                                          |
| 上课时间 | 周三/周五 上午一二节                                |
| 联系方式 | [dbzdbz@tongji.edu.cn](mailto:dbzdbz@tongji.edu.cn) |
