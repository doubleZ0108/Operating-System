# 内存管理 - 动态分区分配方式模拟

###### 操作系统第二次课程作业 - 动态分区分配方式模拟

## 目录

- [开发环境](#开发环境)
- [项目结构](#项目结构)
- [操作说明](#操作说明)
- [作者](#作者)

<a name="开发环境"></a>  

## 开发环境

- **开发环境:** Windows 10

- **开发软件:** 

  1. **Visual Studio Code** *1.34.0*
  2. **WebStorm** *2019.1.1.WS-191.6707.60*

- **开发语言:** html, javascript, css, jQuery

- **主要引用块内容:**

  ```html
  <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js"></script>
  ```

  

<a name="项目结构"></a>  

## 项目结构

│  README.md   
│  动态分区分配方式模拟_设计方案报告.md   
│  动态分区分配方式模拟_设计方案报告.pdf   
│  
├─Resource   
│      memory.png   
│      
└─src   
    │  Dynamic partition allocation.html   
    │  
    └─static  
        ├─css   
        │      range.css   
        │      style.css   
        │      
        └─js   
                clear.js   
                nextAssingment.js   
                randColor.js   
                RangeSlider.js   
                select.js   

<a name="操作说明"></a>  

## 操作说明

- 双击目录`src`下的`Dynamic partition allocation.html`文件, 并在浏览器中打开,打开后界面如下图所示

![image.png](https://upload-images.jianshu.io/upload_images/12014150-22558ddc5899d061.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 选择希望进行模拟的动态分区分配算法*(首次适应算法/最佳适应算法)*

![image.png](https://upload-images.jianshu.io/upload_images/12014150-7d9372a86b1fe872.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 调节滑动条改变当前内存大小*(上方模拟链式空间长度会随着滑动条滑动而动态变化)*

![image.png](https://upload-images.jianshu.io/upload_images/12014150-37acbe2ad52f2b02.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 点击**下一步**进行作业调度

![image.png](https://upload-images.jianshu.io/upload_images/12014150-e9e2c23eb0309973.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 上方的模拟内存会显示每次分配和回收后的空闲分区链的情况*(不同作业的颜色不同, 以区分不同作业在内存中的位置分布情况)*

![image.png](https://upload-images.jianshu.io/upload_images/12014150-0b336d12d32d0cd8.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 下方的日志信息会显示作业*申请/释放*等信息

  ![image.png](https://upload-images.jianshu.io/upload_images/12014150-0914b96b65d60558.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

- 点击**清空内存**会清空内存中的作业以及日志信息全部内容, 此时可再次调整内存空间大小, 并再次进行动态分区分配方式模拟

<a name="作者"></a>  

#### 作者

​	**学号**				1754060 

​	**姓名**				张喆

​	**指导老师**		王冬青老师

​	**上课时间**		周三/周五 上午一二节

​	**联系方式**		*email:* doubleZ0108@gmail.com