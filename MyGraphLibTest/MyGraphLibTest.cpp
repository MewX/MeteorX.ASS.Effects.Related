// MyGraphLibTest.cpp : 定义控制台应用程序的入口点。
//

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <Windows.h>
#include <assert.h>
#include <iostream>
#include <string>

using namespace std;

// TODO: 在此处引用程序需要的其他头文件

#pragma comment( lib, "GraphLib.lib" )
extern "C" __declspec(dllimport) bool MeasureString(int* width, int* height, WCHAR* fontName, BYTE fontCharset, int fontHeight, int fontSpace, WCHAR* str)


int _tmain(int argc, _TCHAR* argv[])
{

	int width = 0, height = 0, fontHeight = 0, fontSpace = 0;
	char  str[100];
	while ( 1 ) {
		cout << "fontHeight, fontSpace, str;" << endl;
		cin >> width >> height >> str;
		MeasureString( &width, &height, L"微软雅黑", GB2312_CHARSET, fontHeight, fontSpace, (wchar_t *) str);
		cout << "Height: " << height << endl << "Width: " << width << endl;
	}
	return 0;
}

