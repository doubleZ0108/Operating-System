# 进程管理 - 电梯调度

###### 操作系统第一次课程作业 - 电梯调度

## 目录

- [开发环境](#开发环境)
- [项目结构](#项目结构)
- [操作说明](#操作说明)
- [作者](#作者)

<a name="开发环境"></a>  

## 开发环境

- **开发环境:** Windows 10
- **开发软件:** 
  1. **PyCharm** *2019.1.1.PC-191.6605.12*
  2. **Qt Designer** *v5.11.2.0*
- **开发语言:** python3
- **主要引用块内容:**
  1. PyQt5 (QTimer, QtCore, QtGui, QtWidgets)
  2. pyqt5-tools
  3. threading

<a name="项目结构"></a>  

## 项目结构

│  myElevator.exe   
│  README.md   
│  电梯调度_设计方案报告.md   
│  
├─Resources   
│  ├─Button   
│  │      doordown.png   
│  │      doordown_hover.png   
│  │      doordown_pressed.png   
│  │      doorup.png   
│  │      doorup_hover.png   
│  │      doorup_pressed.png   
│  │      down.png   
│  │      down_hover.png   
│  │      down_pressed.png   
│  │      state.png   
│  │      state_down.png   
│  │      state_up.png   
│  │      up.png   
│  │      up_hover.png   
│  │      up_pressed.png   
│  │      
│  ├─Figure   
│  │      people.png   
│  │      
│  └─Icon   
│          elevator.ico   
│          icon.png   
│          
└─Src   
        dispatch.py   
        myElevator.py   
        myElevatorInterface.py   

<a name="操作说明"></a>  

## 操作说明

- 双击运行`myElevator.exe`, 进入电梯模拟系统如下图

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-9c8f6f1b7547b823.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 点击每部电梯的**功能键**(*开/关键*, *报警器*, *楼层按钮*), 进行**单部电梯内命令处理**模拟

  ![fullsizerender.jpg](https://upload-images.jianshu.io/upload_images/12014150-fc9114292d629599.jpg?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 在上方**下拉框**中选择所在楼层, 并**点击上/下按钮**, 进行**多部电梯外命令处理**模拟

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-84db2daa57e11544.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- **下拉框**中选择所在楼层, 并点击未被禁用**电梯的上/下行开关****(互联结)*, 进行**多部电梯外命令处理**模拟

  ![1557494406326](C:\Users\28718\AppData\Roaming\Typora\typora-user-images\1557494406326.png)

<a name="作者"></a>  

#### 作者

​	**学号**				1754060 

​	**姓名**				张喆

​	**指导老师**		王冬青老师

​	**上课时间**		周三/周五 上午一二节

​	**联系方式**		*email:* doubleZ0108@gmail.com