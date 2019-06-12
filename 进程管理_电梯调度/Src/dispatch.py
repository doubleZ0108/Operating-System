from PyQt5 import QtCore, QtGui, QtWidgets
from PyQt5.QtCore import *
from PyQt5.QtCore import QTimer

from myElevatorInterface import *
import numpy as np
import time, threading

INFINITE = 100  # 定义"无穷大量"

OPEN = 0  # 开门装填
CLOSED = 1  # 关门状态

STANDSTILL = 0  # 静止状态
RUNNING_UP = 1  # 电梯上行状态
RUNNING_DOWN = 2  # 电梯下行状态

NOPE = 0  # 空动画
READYSTART = 1  # 电梯即将运动
READYSTOP = 2  # 电梯即将停止

GOUP = 1  # 用户要上行
GODOWN = 2  # 用户要下行


class Controler(object):
    def __init__(self, Elev):
        # 与界面文件建立连接
        self.elev = Elev

        # 创建定时器, 1s中更新一次电梯状态
        self.timer = QTimer()
        self.timer.timeout.connect(self.updateElevState)
        self.timer.start(1000)

        # 5个电梯内部消息列表(用列表代替队列)
        self.messQueue = []
        for i in range(0, 5):
            self.messQueue.append([])
        # 5个电梯内部不顺路消息列表
        self.messQueue_reverse = []
        for i in range(0, 5):
            self.messQueue_reverse.append([])

    # 警报器槽函数
    def warnCtrl(self, whichelev):
        self.elev.elevEnabled[whichelev] = False  # 该电梯禁用

        self.elev.warnbtn[whichelev].setEnabled(False)  # 报警键禁用
        self.elev.gridLayoutWidget[whichelev].setEnabled(False)  # 楼层按键禁用
        self.elev.openbtn[whichelev].setEnabled(False)  # 开门键禁用
        self.elev.closebtn[whichelev].setEnabled(False)  # 关门键禁用
        self.elev.elevator_back[whichelev].setEnabled(False)  # 电梯背景禁用
        self.elev.elevator_front[2 * whichelev].setEnabled(False)  # 电梯前门禁用
        self.elev.elevator_front[2 * whichelev + 1].setEnabled(False)  # 电梯前门禁用
        self.elev.elevator_Anim[2 * whichelev].stop()  # 停止动画
        self.elev.elevator_Anim[2 * whichelev + 1].stop()  # 停止动画
        self.elev.label[whichelev].setEnabled(False)  # 电梯文字禁用
        self.elev.lcdNumber[whichelev].setEnabled(False)  # 数码管禁用
        self.elev.stateshow[whichelev].setEnabled(False)  # 上下行标志禁用
        self.elev.updoorbtn[whichelev].setEnabled(False)  # 门口上行按钮禁用
        self.elev.downdoorbtn[whichelev].setEnabled(False)  # 门口下行按钮禁用

        # 五部电梯全部禁用
        arr = np.array(self.elev.elevEnabled)
        if ((arr == False).all()):
            self.elev.comboBox.setEnabled(False)  # 下拉框禁用
            self.elev.chooselabel.setEnabled(False)  # 文字禁用
            self.elev.upbtn.setEnabled(False)  # 上行按钮禁用
            self.elev.downbtn.setEnabled(False)  # 下行按钮禁用

            time.sleep(0.5)
            self.MessBox = QtWidgets.QMessageBox.information(self.elev, "警告", "所有电梯已损坏!")

    # 开关门槽函数
    def doorCtrl(self, whichelev, whichcommand):
        if whichcommand == 0:  # 如果用户要开门
            if self.elev.doorState[whichelev] == CLOSED and self.elev.elevState[
                whichelev] == STANDSTILL:  # 如果当前门是关闭状态并且电梯是静止的
                self.elev.doorState[whichelev] = OPEN  # 先将门状态更新为打开
                self.elev.elevEnabled[whichelev] = False

                self.openDoor_Anim(whichelev)

        else:  # 如果用户要关门
            if self.elev.doorState[whichelev] == OPEN and self.elev.elevState[
                whichelev] == STANDSTILL:  # 如果当前门是打开状态并且电梯是静止的
                self.elev.doorState[whichelev] = CLOSED  # 先将门状态更新为关闭
                self.elev.elevEnabled[whichelev] = True

                #将电梯门前的上下行按键熄灭
                for i in range(0, 5):
                    if self.elev.elevEnabled[i]:
                        self.elev.updoorbtn[i].setStyleSheet("QPushButton{border-image: url(Resources/Button/doorup.png)}"
                                                        "QPushButton:hover{border-image: url(Resources/Button/doorup_hover.png)}"
                                                        "QPushButton:pressed{border-image: url(Resources/Button/doorup_pressed.png)}")
                        self.elev.downdoorbtn[i].setStyleSheet("QPushButton{border-image: url(Resources/Button/doordown.png)}"
                                                          "QPushButton:hover{border-image: url(Resources/Button/doordown_hover.png)}"
                                                          "QPushButton:pressed{border-image: url(Resources/Button/doordown_pressed.png)}")
                        self.elev.updoorbtn[i].setEnabled(True)
                        self.elev.downdoorbtn[i].setEnabled(True)
                self.closeDoor_Anim(whichelev)

    # 开门动画
    def openDoor_Anim(self, whichelev):
        self.elev.elevator_Anim[2 * whichelev].setDirection(QAbstractAnimation.Forward)  # 正向设定动画
        self.elev.elevator_Anim[2 * whichelev + 1].setDirection(QAbstractAnimation.Forward)
        self.elev.elevator_Anim[2 * whichelev].start()  # 开始播放
        self.elev.elevator_Anim[2 * whichelev + 1].start()

    # 关门动画
    def closeDoor_Anim(self, whichelev):
        self.elev.elevator_Anim[2 * whichelev].setDirection(QAbstractAnimation.Backward)  # 反向设定动画
        self.elev.elevator_Anim[2 * whichelev + 1].setDirection(QAbstractAnimation.Backward)
        self.elev.elevator_Anim[2 * whichelev].start()  # 开始播放
        self.elev.elevator_Anim[2 * whichelev + 1].start()

    # 小人进电梯动画
    def figureIn_Anim(self, whichelev):
        self.elev.figure[whichelev].setVisible(True)
        self.elev.figure_Anim[whichelev].setDirection(QAbstractAnimation.Forward)
        self.elev.figure_Anim[whichelev].start()

        s = threading.Timer(1.5, self.setDoorTop, (whichelev,))  # 1.5秒之后把门至于顶层
        s.start()

    # 小人出电梯动画
    def figureOut_Anim(self, whichelev):
        self.elev.figure[whichelev].setVisible(True)
        self.elev.figure_Anim[whichelev].setDirection(QAbstractAnimation.Backward)
        self.elev.figure_Anim[whichelev].start()

        s = threading.Timer(1, self.setFigureTop, (whichelev,))  # 1s之后将人至于顶层
        s.start()

    # 将门至于顶层
    def setDoorTop(self, whichelev):
        self.elev.elevator_front[2 * whichelev].raise_()
        self.elev.elevator_front[2 * whichelev + 1].raise_()

    # 将小人至于顶层
    def setFigureTop(self, whichelev):
        self.elev.figure[whichelev].raise_()
        self.elev.figure[whichelev].setVisible(False)

    # 内命令电梯运动
    def elevMove(self, whichelev, dest):

        nowFloor = self.elev.elevNow[whichelev]  # 获取当前电梯位置

        if nowFloor < dest:  # 如果按键大于当前楼层
            if self.elev.elevState[whichelev] == STANDSTILL:  # 电梯处于静止状态
                self.messQueue[whichelev].append(dest)  # 将目标楼层加入 消息队列

            else:
                if self.elev.elevState[whichelev] == RUNNING_UP:  # 电梯正在向上运行
                    self.messQueue[whichelev].append(dest)  # 将目标楼层加入 消息队列并排序
                    self.messQueue[whichelev].sort()
                elif self.elev.elevState[whichelev] == RUNNING_DOWN:  # 电梯正在向下运行
                    self.messQueue_reverse[whichelev].append(dest)  # 将目标楼层加入 不顺路消息队列并排序
                    self.messQueue_reverse[whichelev].sort()

        elif nowFloor > dest:
            if self.elev.elevState[whichelev] == STANDSTILL:
                self.messQueue[whichelev].append(dest)  # 将目标楼层加入 消息队列

            else:
                if self.elev.elevState[whichelev] == RUNNING_DOWN:
                    self.messQueue[whichelev].append(dest)  # 将目标楼层加入 消息队列并反向排序
                    self.messQueue[whichelev].sort()
                    self.messQueue[whichelev].reverse()
                elif self.elev.elevState[whichelev] == RUNNING_UP:
                    self.messQueue_reverse[whichelev].append(dest)  # 将目标楼层加入 不顺路消息队列并反向排序
                    self.messQueue_reverse[whichelev].sort()
                    self.messQueue_reverse[whichelev].reverse()

        else:  # 如果按键就为当前楼层
            if self.elev.elevState[whichelev] == STANDSTILL:  # 电梯静止 => 打开门(并等待用户自行关闭)
                self.elev.doorState[whichelev] = OPEN
                self.openDoor_Anim(whichelev)

            button = self.elev.findChild(QtWidgets.QPushButton,
                                         "button {0} {1}".format(whichelev, nowFloor))  # 恢复按键背景并重新允许点击
            button.setStyleSheet("")
            button.setEnabled(True)

    # 外命令电梯调度
    def chooseCtrl(self, whichfloor, choice):

        # region 初步筛选没损坏的电梯
        EnabledList = []
        for i in range(0, 5):
            if self.elev.elevEnabled[i]:
                EnabledList.append(i)
        print(EnabledList)
        # endregion

        # region 计算每部可用电梯的"可调度性"
        dist = [INFINITE] * 5  # 可使用电梯距离用户的距离
        for EnabledElev in EnabledList:
            if self.elev.elevState[EnabledElev] == RUNNING_UP and choice == GOUP and whichfloor > self.elev.elevNow[
                EnabledElev]:  # 向上顺路
                dist[EnabledElev] = whichfloor - self.elev.elevNow[EnabledElev]

            elif self.elev.elevState[EnabledElev] == RUNNING_DOWN and choice == GODOWN and whichfloor < \
                    self.elev.elevNow[EnabledElev]:  # 向下顺路
                dist[EnabledElev] = self.elev.elevNow[EnabledElev] - whichfloor

            elif self.elev.elevState[EnabledElev] == STANDSTILL:  # 该电梯此时静止
                dist[EnabledElev] = abs(self.elev.elevNow[EnabledElev] - whichfloor)
        # endregion

        BestElev = dist.index(min(dist))  # 选择可调度性最好的电梯作为最佳电梯
        if dist[BestElev] == 0:  # 如果最佳电梯就在用户选择的楼层
            self.elev.doorState[BestElev] = OPEN  # 打开门并等待用户自行关闭
            self.openDoor_Anim(BestElev)

        else:
            self.messQueue[BestElev].append(whichfloor)  # 加入该最佳电梯的消息队列
            button = self.elev.findChild(QtWidgets.QPushButton,
                                         "button {0} {1}".format(BestElev, whichfloor))  # 将用户的目标楼层设定为特殊颜色
            button.setStyleSheet("background-color: rgb(11, 15, 255);")
            button.setEnabled(False)

    # 更新电梯状态
    def updateElevState(self):
        # print('timer clock......')

        for i in range(0, len(self.messQueue)):  # 遍历五部电梯
            if len(self.messQueue[i]):  # 某个电梯的消息队列不为空

                if self.elev.doorState[i] == OPEN:  # 如果电梯门是打开的 => 等待电梯关门
                    continue

                elif self.elev.elevState[i] == STANDSTILL:  # 电梯处于静止状态
                    self.openDoor_Anim(i)
                    self.figureIn_Anim(i)

                    if self.elev.elevNow[i] < self.messQueue[i][0]:  # 根据即将运行的方向更新电梯状态
                        self.elev.elevState[i] = RUNNING_UP
                    elif self.elev.elevNow[i] > self.messQueue[i][0]:
                        self.elev.elevState[i] = RUNNING_DOWN

                    self.elev.animState[i] = READYSTART  # 动画变为就绪运行状态

                elif self.elev.animState[i] == READYSTART:  # 动画处于就绪运行状态
                    self.closeDoor_Anim(i)
                    self.elev.animState[i] = NOPE  # 动画变为空状态

                elif self.elev.animState[i] == READYSTOP:  # 动画处于就绪停止状态
                    self.messQueue[i].pop(0)  # 结束该命令的处理
                    self.closeDoor_Anim(i)
                    self.elev.animState[i] = NOPE       # 动画变为空状态
                    self.elev.elevState[i] = STANDSTILL  # 电梯变为静止状态
                    self.elev.stateshow[i].setStyleSheet("QGraphicsView{border-image: url(Resources/Button/state.png)}")

                else:
                    destFloor = self.messQueue[i][0]  # 获取第一个目标楼层

                    if self.elev.elevNow[i] < destFloor:  # 向上运动
                        self.elev.elevState[i] = RUNNING_UP
                        self.elev.stateshow[i].setStyleSheet(
                            "QGraphicsView{border-image: url(Resources/Button/state_up.png)}")
                        self.elev.elevNow[i] = self.elev.elevNow[i] + 1  # 将当前楼层加一并设置数码管显示
                        self.elev.lcdNumber[i].setProperty("value", self.elev.elevNow[i])

                    elif self.elev.elevNow[i] > destFloor:  # 向下运动
                        self.elev.elevState[i] = RUNNING_DOWN
                        self.elev.stateshow[i].setStyleSheet(
                            "QGraphicsView{border-image: url(Resources/Button/state_down.png)}")
                        self.elev.elevNow[i] = self.elev.elevNow[i] - 1  # 将当前楼层减一并设置数码管显示
                        self.elev.lcdNumber[i].setProperty("value", self.elev.elevNow[i])

                    else:  # 电梯到达目的地
                        self.openDoor_Anim(i)
                        self.figureOut_Anim(i)
                        self.elev.animState[i] = READYSTOP  # 到达目的地 => 动画变为就绪停止状态

                        button = self.elev.findChild(QtWidgets.QPushButton,
                                                     "button {0} {1}".format(i, self.elev.elevNow[i]))  # 恢复该按钮的状态
                        button.setStyleSheet("")
                        button.setEnabled(True)

            elif len(self.messQueue_reverse[i]):  # 如果消息队列为空 & 不顺路消息队列不为空
                self.messQueue[i] = self.messQueue_reverse[i].copy()  # 交替两个队列
                self.messQueue_reverse[i].clear()

        # 电梯在运行过程中禁止点击报警键
        for i in range(0, 5):
            if self.elev.gridLayoutWidget[i].isEnabled():  # 如果这个电梯没被禁用
                if self.elev.elevState[i] == STANDSTILL:  # 如果电梯是静止的
                    self.elev.warnbtn[i].setEnabled(True)
                else:
                    self.elev.warnbtn[i].setEnabled(False)
