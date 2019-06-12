#include <iostream>
#include <cstdlib>
#include <ctime>
#include <vector>
#include <string>
#include <queue>
#include <algorithm>
#include <iomanip>
using namespace std;

#define MaxSize 4		//分配给作业的总内存块数
#define EMPTY -1		//内存块为空标识
#define TOTALNUM 320	//指令总条数

typedef int InstNum;	//指令号
typedef int PageNum;	//页号
typedef int BlockNum;	//块号


/*返回[low, high]间的随机指令*/
InstNum getRand(InstNum low, InstNum high)
{
	if (high - low == -1) { return high; }		//消除作业中指令访问次序产生high比low小1的问题
	return (rand() % (high - low + 1) + low);
}


class Memory
{
private:
	vector<PageNum> block;		//内存块
	vector<bool> visited;		//是否执行过该指令
	queue<BlockNum> LRU_Queue;	//最近最少使用队列

	int runTime = 0;					//运行次数
	int adjustTime = 0;					//调页次数
	int restInst = TOTALNUM;			//剩余未执行指令

	void execute(string algorithm, InstNum aim);			//按照算法执行一条指令
	PageNum adjust(string algorithm, BlockNum &pos);		//页面置换

	void displayPosMess(InstNum aim) {									//打印指令地址信息
		cout << "物理地址为:" << setw(3)<<aim
			 << ", 地址空间页号为:" <<setw(2)<< aim / 10
			<< ", 页内第" << setw(2) << aim % 10 << "条指令.";
	}
	void displayLoadMess(PageNum fresh, BlockNum pos, bool flag) {		//打印未发生调页的信息
		cout << endl;
		if (flag) {		//已经在内存块中
			cout << fresh << "号页已经在内存中第" << pos << "号块中了, 未发生调页." << endl << endl;
		}
		else {			//没在内存块中, 但是内存块没满
			cout << fresh << "号页放在内存中第" << pos << "号块中, 未发生调页." << endl << endl;
		}
	}
	void displayLoadMess(PageNum old, PageNum fresh, BlockNum pos) {	//打印发生调页的信息
		cout << "  || 调出内存中第" << setw(2)<<pos 
			<< "块中第" <<setw(2)<< old 
			<< "号页, 调入第" << setw(2) << fresh << "号页." << endl << endl;
	}

public:
	Memory() = default;
	~Memory() = default;

	void Init();		//初始化内存
	void Simulate(string algorithm, char type);			//按照算法和执行模式执行指令 

	int getRunTime() { return this->runTime; }			//返回运行次数
	int getAdjustTime() { return this->adjustTime; }	//返回调页次数
	double getAdjustRate(){ return (1.0*this->adjustTime / this->runTime); }	//返回缺页率
};

int main(void)
{
	char method, type, operate;	//置换算法, 执行模式, 功能

	do
	{
		/*选择置换算法*/
		cout <<
			"*********************************************************************************\n"
			"**                                                                             **\n"
			"**                             请选择置换算法：                                **\n"
			"**                             A.       LRU                                    **\n"
			"**                             B.       FIFO                                   **\n"
			"**                                                                             **\n"
			"*********************************************************************************\n" << endl;
		cout << "你想选择: ";
		do
		{
			cin >> method;
			if (method != 'A' && method != 'a' && method != 'B' && method != 'b')
			{
				cout << "您输入的置换算法有误, 请重新输入: ";
			}
		} while (method != 'A' && method != 'a' && method != 'B' && method != 'b');

		string algorithm = (method == 'A' || method == 'a' ? string("LRU") : string("FIFO"));

		/*选择执行模式*/
		cout <<
			"*********************************************************************************\n"
			"**                                                                             **\n"
			"**                             请选择执行模式：                                **\n"
			"**                             A.执行前" << TOTALNUM << "条指令                               **\n"
			"**                             B.执行完所有指令                                **\n"
			"**                                                                             **\n"
			"*********************************************************************************\n" << endl;
		cout << "你想选择: ";
		do
		{
			cin >> type;
			if (type != 'A' && type != 'a' && type != 'B' && type != 'b')
			{
				cout << "您输入的执行模式有误, 请重新输入: ";
			}
		} while (type != 'A' && type != 'a' && type != 'B' && type != 'b');

		/*模拟*/
		Memory myMemory;						//创建内存对象
		myMemory.Init();						//初始化内存
		srand((unsigned)time(NULL));			//获取随机数种子
		myMemory.Simulate(algorithm, type);		//按照该算法和该执行模式进行模拟
		
		cout << algorithm << "算法, ";
		if (type == 'A' || type == 'a') { cout << "执行前" << TOTALNUM << "条指令"; }
		else { cout << "执行完所有指令"; }
		cout << "模拟结果如下: " << endl;
		cout << "======================================" << endl
			<< "共执行" << myMemory.getRunTime() << "条指令" << endl
			<< "调页次数为" << myMemory.getAdjustTime() << "次" << endl
			<< "缺页率为" << myMemory.getAdjustRate() << endl
			<< "=======================================" << endl;


		/*选择功能*/
		cout <<
			"*********************************************************************************\n"
			"**                                                                             **\n"
			"**                             请选择功能：                                    **\n"
			"**                             A.初始化                                        **\n"
			"**                             B.结束程序                                      **\n"
			"**                                                                             **\n"
			"*********************************************************************************\n" << endl;
		cout << "你想选择: ";
		do
		{
			cin >> operate;
			if (operate != 'A' && operate != 'a' && operate != 'B' && operate != 'b')
			{
				cout << "您输入的功能有误, 请重新输入: ";
			}
		} while (operate != 'A' && operate != 'a' && operate != 'B' && operate != 'b');

		if (operate == 'B' || operate == 'b') { break; }

	} while (operate!='B' && operate!='b');

		
	cout << endl << endl
		<< "********************************" << endl
		<< "* 请求调页存储管理方式模拟结束 * " << endl
		<< "********************************" << endl
		<< endl;

	system("pause");
	return 0;
}

/* 执行一条指令
 * @param {置换算法} algorithm
 * @param {待执行指令} aim
*/
void Memory::execute(string algorithm, InstNum aim)
{
	this->runTime++;		//更新运行次数

	PageNum page = aim / 10;	//计算页号
	BlockNum pos = 0;

	displayPosMess(aim);

	/*检测该页是否已经在内存中*/
	for (pos = 0; pos < MaxSize; ++pos)
	{
		if (block[pos] == page)
		{
			displayLoadMess(page, pos, true);

			return;
		}
	}
	/*检测内存中有无空闲块*/
	for (pos = 0; pos < MaxSize; ++pos)
	{
		if (block[pos] == EMPTY)
		{
			block[pos] = page;
			displayLoadMess(page, pos, false);

			if (algorithm == string("LRU"))
			{
				LRU_Queue.push(pos);		//将其压入最近最少使用队列
			}

			return;
		}
	}

	//执行到这说明: 1.内存块是满的 2.要进行调页
	PageNum old = adjust(algorithm, pos);
	block[pos] = page;
	displayLoadMess(old, page, pos);
}

/* 请求调页
 * @returnValue {要被替换掉的页号}
 * @param {置换算法} algorithm
 * @param {调入调出的位置} pos 
*/
PageNum Memory::adjust(string algorithm, BlockNum &pos)
{
	this->adjustTime++;		//更新调页次数

	PageNum old;
	if (algorithm == "FIFO")
	{
		pos = (this->adjustTime-1) % 4;	//缺页次数为1, 则将0号内存的页调出, 将当前指令调入0 号内存中...以此类推
		old = block[pos];
	}
	else if (algorithm == "LRU")
	{
		pos = LRU_Queue.front();		//取队列头元素 => 最近最少使用的页面
		LRU_Queue.pop();
		LRU_Queue.push(pos);			//将其压入队尾

		old = block[pos];
	}

	return old;
}

void Memory::Init()
{
	this->block.resize(MaxSize, EMPTY);
	this->visited.resize(TOTALNUM, false);
	while (!this->LRU_Queue.empty()) { this->LRU_Queue.pop(); }

	this->runTime = 0;
	this->adjustTime = 0;
	this->restInst = TOTALNUM;
}

/* 请求调页存储管理方式模拟
 * @param {置换算法} algorithm
 * @param {用户选择的执行类型} type
*/
void Memory::Simulate(string algorithm, char type)
{
	InstNum aim;
	if (type == 'A' || type == 'a')
	{
		int cnt = 0;

		//随机选取一个起始指令
		aim = getRand(0, TOTALNUM - 1);
		execute(algorithm, aim); cnt++;
		//顺序执行下一条指令
		aim++;
		execute(algorithm, aim); cnt++;
		while (true)
		{
			if (cnt == TOTALNUM) { break; }
			//跳转到前地址部分
			aim = getRand(0, aim - 1);
			execute(algorithm, aim); cnt++;

			if (cnt == TOTALNUM) { break; }
			//顺序执行下一条指令
			aim++;
			execute(algorithm, aim); cnt++;

			if (cnt == TOTALNUM) { break; }
			//跳转到后地址部分
			aim = getRand(aim + 1, TOTALNUM - 1); 
			execute(algorithm, aim); cnt++;

			if (cnt == TOTALNUM) { break; }
			//顺序执行下一条指令
			aim++;
			execute(algorithm, aim); cnt++;
		}
	}
	else if (type == 'B' || type == 'b')
	{
		//随机选取一个起始指令
		aim = getRand(0, TOTALNUM - 1);
		execute(algorithm, aim); 
		restInst--; visited[aim] = true;
		//顺序执行下一条指令
		aim++;
		execute(algorithm, aim);
		restInst--; visited[aim] = true;

		while (true)
		{
			if (!restInst) { break; }
			//跳转到前地址部分
			aim = getRand(0, aim - 1);
			execute(algorithm, aim);
			if (aim!=TOTALNUM && !visited[aim]) { restInst--; visited[aim] = true; }

			if (!restInst) { break; }
			//顺序执行下一条指令
			aim++;
			execute(algorithm, aim); 
			if (aim != TOTALNUM && !visited[aim]) { restInst--; visited[aim] = true; }

			if (!restInst) { break; }
			//跳转到后地址部分
			aim = getRand(aim + 1, TOTALNUM - 1);
			execute(algorithm, aim);
			if (aim != TOTALNUM && !visited[aim]) { restInst--; visited[aim] = true; }

			if (!restInst) { break; }
			//顺序执行下一条指令
			aim++;
			execute(algorithm, aim);
			if (aim != TOTALNUM && !visited[aim]) { restInst--; visited[aim] = true; }
		}
	}
}