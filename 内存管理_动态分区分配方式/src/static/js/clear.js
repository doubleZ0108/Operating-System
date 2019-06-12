function clearbtnClick(){
    console.log('clear...')
    var Mem = document.getElementById('memory');
    while(Mem.hasChildNodes()){
        Mem.removeChild(Mem.firstChild)
    }

    var Board = document.getElementById('board');
    while(Board.hasChildNodes()){
        Board.removeChild(Board.firstChild)
    }

    now = 0;        //重置作业列表(从头开始作业调度)
    occupyMem.length = 0;   //清空占用列表
    useableMem.length = 0;  //清空可用列表
    $("#memory-size").removeClass("disable")    //重置滑动条为可用
}