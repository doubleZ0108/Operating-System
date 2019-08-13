# 内存管理 - 请求分区分配方式模拟

###### 操作系统第二次课程作业 - 请求分区分配方式模拟

## 目录

- [开发环境](#开发环境)
- [操作说明](#操作说明)
- [项目结构](#项目结构)
- [作者](#作者)

<a name="开发环境"></a>  

## 开发环境

- **开发环境:** Windows 10

- **开发软件:** 

  **Visual Studio 2017** *15.9.28307.665*
  
- **开发语言:** C++
  

<a name="操作说明"></a>  

## 操作说明

- 双击目录下`DemandPaing.exe`可执行文件进入模拟界面

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-7d111908c6efaf59.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 选择置换算法:

  - 键入`a`或`A`代表选择LRU算法

  - 键入`b`或`B`代表选择FIFO算法

  - 输入无效算法将受到提示, 并允许重新选择

    ![image.png](https://upload-images.jianshu.io/upload_images/12014150-275579270c081a82.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 选择执行模式:

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-c73639046c1737d1.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

  - 键入`a`或`A`代表只执行前320条指令(指令可能重复)

  - 键入`b`或`B`代表执行完所有指令(知道所有指令都被执行为止)

  - 输入无效执行模式将受到提示, 并允许重新选择

    ![image.png](https://upload-images.jianshu.io/upload_images/12014150-1e79d723ec59064f.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 查看对应算法和对应执行模式下的模拟结果

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-2cdd504726ed7371.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 选择功能:

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-245ef610c1d1ad40.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

  - 键入`a`或`A`代表初始化内存(可再次进行模拟)
  - 键入`b`或`B`代表结束程序

- 初始化

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-1060f1b40ab787fc.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 结束程序

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-18d343035dfe0878.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)
  

<a name="项目结构"></a>  

## 项目结构
```
│  list.txt   
│  README.md   
│  请求分区分配方式模拟_设计方案报告.md   
│  请求分区分配方式模拟_设计方案报告.pdf   
│  
└─src   
        DemandPaging.cpp   
```  

<a name="作者"></a>  

#### 作者

​	**学号**				1754060 

​	**姓名**				张喆

​	**指导老师**		王冬青老师

​	**上课时间**		周三/周五 上午一二节

​	**联系方式**		*email:* doubleZ0108@gmail.com
