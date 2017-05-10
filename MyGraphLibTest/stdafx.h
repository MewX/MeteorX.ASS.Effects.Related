// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件
//

#pragma once

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
