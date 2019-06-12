/**
 * 作业类
 * @param {作业名称} name 
 * @param {申请/释放的空间大小} data 
 */
function Proj(name, data) {
    this.name = name;
    this.data = data;
    this.getname = function () {
        return this.name
    }
    this.getdata = function () {
        return this.data
    }
}

/**
 * 标记类
 * @param {起始地址} start 
 * @param {长度} last 
 */
function Mark(start, last) {
    this.start = start;
    this.last = last;
    this.getstart = function () {
        return this.start;
    }
    this.getlast = function () {
        return this.last;
    }
}

/**
 * 作业列表(后期可考虑动态扩充)
 */
const projList = [
    new Proj("作业1", 130),
    new Proj("作业2", 60),
    new Proj("作业3", 100),
    new Proj("作业2", -60),
    new Proj("作业4", 200),
    new Proj("作业3", -100),
    new Proj("作业1", -130),
    new Proj("作业5", 140),
    new Proj("作业6", 60),
    new Proj("作业7", 50),
    new Proj("作业6", -60)
]

var now = 0;    //当前执行到的指令位置

occupyMem = [] //被占用标记表
useableMem = [] //空闲空间标记表

const ADDSUCCESS = 0;       //添加作业成功
const ADDFAILED = 1;        //添加作业失败
const REMOVESUCCESS = 2;    //释放作业成功

/**Mark对象排序 => 依据start的大小 */
var compareStart = function (x, y) {
    if (x.getstart() < y.getstart()) {
        return -1;
    } else if (x.getstart() > y.getstart()) {
        return 1;
    } else {
        return 0;
    }
}
/**Mark对象排序 => 依据last的大小 */
var compareLast = function (x, y) {
    if (x.getlast() < y.getlast()) {
        return -1;
    } else if (x.getlast() > y.getlast()) {
        return 1;
    } else {
        return 0;
    }
}




/**
 * 内存中添加作业
 * 告示板中显示相应的信息
 */
function nextAssignment() {
    $("#memory-size").addClass("disable") //禁用滑动条

    var algorithm = $("#box").attr("data-value"); //获取所选的算法
    var memSize = document.getElementById("slider").value; //获取当前内存大小(清空之前不会变化)
    var Mem = document.getElementById('memory'); //获取内存实体
    var Board = document.getElementById('board'); //获取告示板实体

    if (occupyMem == false) { //内存没有任何作业
        useableMem.push(new Mark(0, memSize));
    }

    if (now < projList.length) {
        if(Adapt(memSize,Mem,Board,algorithm)){//可以装入作业
            now++;
        }else{
            ;
        }
    } else {
        var mess = document.createElement("mess");
        mess.type = "div";
        mess.innerText = "作业已全部完成!\n";
        mess.style.color = "red";
        Board.appendChild(mess);
    }



    //滚动条自动滚到最底部
    Board.scrollTop = Board.scrollHeight;
}


/**适配算法 */
function Adapt(memSize, Mem, Board, algorithm) {
    var name = projList[now].getname();
    var data = projList[now].getdata();

    if (data > 0) { //申请空间
        if(algorithm == "首次适应算法"){
            useableMem.sort(compareStart)
        }else if(algorithm == "最佳适应算法"){
            useableMem.sort(compareLast);
        }
        console.log("after algorithm filter...", useableMem)

        for (var i = 0; i < useableMem.length; ++i) {
            if (useableMem[i].getlast() > data) { //第一个能放下的位置
                start = useableMem[i].getstart();
                last = useableMem[i].getlast();

                occupyMem.push(new Mark(start, data));
                occupyMem.sort(compareStart)

                useableMem[i].start += data;
                useableMem[i].last -= data;

                addProj(Mem, name, start, data);
                addMess(Board, name, data, start, ADDSUCCESS);

                return true;
            }
        }
    } else { //释放空间
        var proj = document.getElementById("proj" + name[2]);
        document.getElementById("memory").removeChild(proj); //清除作业块

        /*获取起始位置 */
        var start = proj.style.marginLeft;
        start = Number(start.slice(0, -2));
        console.log(start)

        for (var i = 0; i < occupyMem.length; ++i) {
            if (occupyMem[i].getstart() == start) { //清除占用表项
                last = occupyMem[i].getlast();
                occupyMem.splice(i, 1);
                break;
            }
        }

        useableMem.push(new Mark(start, last)); //先在末尾添加一块新的可以使用的空间
        update(); //对可用内存重新整理

        addMess(Board, name, data, start, REMOVESUCCESS);

        return true;
    }

    addMess(Board, name, data, -1, ADDFAILED);
    return false;
}

/**
 * 添加一个作业块(不检测)
 * @param {内存实体} Mem 
 * @param {作业名称} name 
 * @param {起始位置} start 
 * @param {作业数据信息} data 
 */
function addProj(Mem, name, start, data) {
    var proj = document.createElement("proj");
    proj.type = "div";
    proj.classname = "memory-proj";
    proj.id = "proj" + name[2];
    proj.innerText = "\n" + name + "\n" + data + "K" + "\n"; //作业块内部显示作业名和作业大小
    proj.style.marginTop = "1px";
    proj.style.background = randomHexColor(); //随机配色
    proj.style.height = "98px";
    proj.style.width = String(data) + "px"; //作业块的宽度为作业大小

    /*实现以内存左端点为基准定位 */
    proj.style.position = "absolute";
    proj.style.marginLeft = String(start) + "px";

    Mem.appendChild(proj);
}

/**
 * 添加一行日志信息
 * @param {告示板实体} Board 
 * @param {作业名称} name 
 * @param {作业数据信息} data 
 * @param {存放的起始位置} start 
 */
function addMess(Board, name, data, start, flag) {
    var mess = document.createElement("mess");
    mess.type = "div";
    if (flag == ADDSUCCESS) {
        mess.innerText = name + "申请" + String(data) + "K内存空间成功\n" + "起始位置是" + String(start) + "\n\n";
        mess.style.color = "black";
    } else if (flag == ADDFAILED) {
        mess.innerText = name + "要申请" + String(data) + "K内存空间\n当前内存空间不足!\n\n";
        mess.style.color = "red";
    } else if (flag == REMOVESUCCESS) {
        mess.innerText = name + "释放" + String(-data) + "K内存空间成功\n\n";
        mess.style.color = "blue";
    }
    Board.appendChild(mess);
}

/**重新整理useableMem */
function update() {
    useableMem.sort(compareStart)
    console.log("before update", useableMem)

    var i = 0;
    while (i < useableMem.length) {
        while (i + 1 < useableMem.length &&             //当前空闲块和后面的空闲块可以合并,持续循环合并
            (useableMem[i].getstart() + useableMem[i].getlast() == useableMem[i + 1].getstart())) {
            useableMem[i].last += useableMem[i + 1].getlast();
            useableMem.splice(i + 1, 1);    //合并后删除后面的空闲块
        }
        ++i;
    }
    console.log("after update", useableMem)
}