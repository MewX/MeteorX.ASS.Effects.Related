// MyGraphLibTest.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <Windows.h>
#include <assert.h>
#include <iostream>
#include <string>

using namespace std;

// TODO: �ڴ˴����ó�����Ҫ������ͷ�ļ�

#pragma comment( lib, "GraphLib.lib" )
extern "C" __declspec(dllimport) bool MeasureString(int* width, int* height, WCHAR* fontName, BYTE fontCharset, int fontHeight, int fontSpace, WCHAR* str)


int _tmain(int argc, _TCHAR* argv[])
{

	int width = 0, height = 0, fontHeight = 0, fontSpace = 0;
	char  str[100];
	while ( 1 ) {
		cout << "fontHeight, fontSpace, str;" << endl;
		cin >> width >> height >> str;
		MeasureString( &width, &height, L"΢���ź�", GB2312_CHARSET, fontHeight, fontSpace, (wchar_t *) str);
		cout << "Height: " << height << endl << "Width: " << width << endl;
	}
	return 0;
}

